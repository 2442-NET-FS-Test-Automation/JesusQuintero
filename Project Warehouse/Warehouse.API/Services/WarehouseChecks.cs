using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Warehouse.Data.Entities;
using Warehouse.Data.Records;

namespace Warehouse.Data.Services;

public interface IBinInventoryService
{
    public Task ReduceBinInventory(WarehouseDBContext db , int fromBin, List<MaterialMovementDto> materials);
    public Task AddBinInventory(WarehouseDBContext db , int toBin, List<MaterialMovementDto> materials);
    public Task AddBinInventorySequential(WarehouseDBContext db , int toBin, List<MaterialMovementDto> materials);
}

public class BinInventoryService : IBinInventoryService
{
    

    public async Task ReduceBinInventory(WarehouseDBContext db , int fromBin, List<MaterialMovementDto> materials)
    {
        // Get the located materials by bin to check if has enough stock
        List<LocatedMaterials> fromBinLocated = await db.LocatedMaterials.Where(l => l.Bin_Id == fromBin).ToListAsync();
        ConcurrentDictionary<int, LocatedMaterials> fromDict = new ConcurrentDictionary<int, LocatedMaterials>
                                                            (fromBinLocated.ToDictionary(l=>l.Material_Id));


        // Checking if have enough stock
        Parallel.ForEach(materials, mat =>
        {
            if(!fromDict.TryGetValue(mat.idMaterial, out _) || fromDict[mat.idMaterial].Quantity < mat.quantity)
            {
                Log.Error("Not found enough stock if {material} material in bin {bin}", mat.idMaterial, fromBin);
                throw new InsufficientStockException(fromBin);
            } 
        });

        foreach (var mat in materials)
        {
            LocatedMaterials loc = fromDict[mat.idMaterial];
            loc.Quantity -= mat.quantity;

            if (loc.Quantity == 0)  db.LocatedMaterials.Remove(loc); 
            
        }
    }

    public async Task AddBinInventory(WarehouseDBContext db , int toBin, List<MaterialMovementDto> materials)
    {

        // Get the located materials to the new bin to add stock or create new entities
        List<LocatedMaterials> toBinLocated = await db.LocatedMaterials.Where(l => l.Bin_Id == toBin).ToListAsync();
        List<LocatedMaterials> localMaterials = db.LocatedMaterials.Local.Where(l => l.Bin_Id == toBin).ToList();
        IEnumerable<LocatedMaterials> combinedMaterials = toBinLocated.Concat(localMaterials)
                                        .GroupBy(l => l.Material_Id)
                                        .Select(g => g.First());
        ConcurrentDictionary<int, LocatedMaterials> toBinDict = new ConcurrentDictionary<int, LocatedMaterials>
                                                            (combinedMaterials.ToDictionary(l=>l.Material_Id));
            
        Parallel.ForEach(materials, mat =>
        {
            toBinDict.AddOrUpdate(mat.idMaterial,
                // Creating no existing entities to store values
                addValueFactory: (key) => new LocatedMaterials
                {
                    Bin_Id = toBin,
                    Material_Id = key,
                    Quantity = mat.quantity
                },
                // Modify existing entities to update the new quantities
                updateValueFactory: (key, currentBin) =>
                {
                    lock (currentBin)
                    {
                        currentBin.Quantity +=  mat.quantity;
                    }
                    return currentBin;
                });
        });


        // Store new entities generated before in the parallel foreach
        foreach (var newLocatedMATERIAL in toBinDict.Values)
        {
            if (db.Entry(newLocatedMATERIAL).State == EntityState.Detached)
            {
                db.LocatedMaterials.Add(newLocatedMATERIAL);
            }
        }
    }

    public async Task AddBinInventorySequential(WarehouseDBContext db , int toBin, List<MaterialMovementDto> materials)
    {
        List<LocatedMaterials> toBinLocated = await db.LocatedMaterials.Where(l => l.Bin_Id == toBin).ToListAsync();
        List<LocatedMaterials> localMaterials = db.LocatedMaterials.Local.Where(l => l.Bin_Id == toBin).ToList();
        IEnumerable<LocatedMaterials> combinedMaterials = toBinLocated.Concat(localMaterials)
                                        .GroupBy(l => l.Material_Id)
                                        .Select(g => g.First());


        Dictionary<int, LocatedMaterials> toBinDict = combinedMaterials.ToDictionary(l=>l.Material_Id);

        foreach(MaterialMovementDto mat in materials)
        {
            
            if (!toBinDict.TryGetValue(mat.idMaterial, out _))
            {
                toBinDict.Add(mat.idMaterial,new LocatedMaterials { Bin_Id =  toBin, Material_Id = mat.idMaterial, Quantity = mat.quantity});
                db.LocatedMaterials.Add(toBinDict[mat.idMaterial]);
            }
            else
            {
                toBinDict[mat.idMaterial].Quantity += mat.quantity;
            }
        }
    }


}