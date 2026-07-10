using Warehouse.Data.Records;
using Warehouse.Data.Entities;
using System.Collections.Concurrent;
using System.Data.Common;

namespace Warehouse.Data.Services;

public interface IWarehouseFactory
{
    public Movements MakeMovement(MovementTypes movement, 
                List<MaterialMovementDto> materials, int userId, int fromBin = 0, int toBin = 0);
    public Movements CreateEntryMovement(List<MaterialMovementDto> materials,int userId, int toBin);
    public Movements CreateInternalMovement(List<MaterialMovementDto> materials,int userId, int fromBin, int toBin);
    public Movements CreateShipmentMovement(List<MaterialMovementDto> materials,int userId, int fromBin);
    public Task MakeShipment(WarehouseDBContext db, List<MaterialMovementDto> materials, int customer, decimal price);
    public Task<PriorityQueue<GenericMaterialMovementDto, int>> GenerateBurst(int n);

    public Task<BurstResult> RunSecuentialBurst(WarehouseDBContext db, IWarehouseFactory factory, IBinInventoryService invService,
                                                    PriorityQueue<GenericMaterialMovementDto, int> myMovements);
}

public class WarehouseFactory : IWarehouseFactory
{
    public Movements MakeMovement(MovementTypes movement, 
                                            List<MaterialMovementDto> materials,
                                            int userId,
                                            int fromBin = 0, 
                                            int toBin = 0)
    {
        switch (movement)
        {
            case MovementTypes.Entry:  return CreateEntryMovement(materials, userId, toBin);
            case MovementTypes.Movement: return CreateInternalMovement(materials, userId, fromBin, toBin);
            case MovementTypes.Shipment: return CreateShipmentMovement(materials, userId, fromBin);
            default:
                throw new ArgumentException($"Unknown order kind: {movement}");
        }
    }

    public Movements CreateEntryMovement(List<MaterialMovementDto> materials,int userId, int toBin)
    {
        return new Movements
        {
            User_Id = userId,
            NewBinLocation_Id = toBin,
            Movement_Time = DateTime.Now,
            MaterialMovements = materials.Select(m => new MaterialMovements
            {
                Material_Id = m.idMaterial,
                Quantity = m.quantity
            }).ToList()
        };
    }

    public Movements CreateInternalMovement(List<MaterialMovementDto> materials,int userId, int fromBin, int toBin)
    {
        return new Movements
        {
            User_Id = userId,
            LastBinLocation_Id = fromBin,
            NewBinLocation_Id = toBin,
            Movement_Time = DateTime.Now,
            MaterialMovements = materials.Select(m => new MaterialMovements
            {
                Material_Id = m.idMaterial,
                Quantity = m.quantity
            }).ToList()

        };
    }

    public Movements CreateShipmentMovement(List<MaterialMovementDto> materials,int userId, int fromBin)
    {
        return new Movements
        {
            User_Id = userId,
            LastBinLocation_Id = fromBin,
            Movement_Time = DateTime.Now,
            MaterialMovements = materials.Select(m => new MaterialMovements
            {
                Material_Id = m.idMaterial,
                Quantity = m.quantity
            }).ToList()
        };
    }

    public async Task MakeShipment(WarehouseDBContext db, List<MaterialMovementDto> materials, int customer, decimal price)
    {
        Shipments newShipment = new Shipments
        {
            Customer_Id = customer,
            Shipment_Date = DateOnly.FromDateTime(DateTime.Now),
            Sale_Price = price,
            MaterialsByShipments = materials.Select(m => new MaterialsByShipments
            {
                Material_Id = m.idMaterial,
                Quantity = m.quantity
            }).ToList()
        };

        db.Shipments.Add(newShipment);
    }

    public async Task<PriorityQueue<GenericMaterialMovementDto, int>> GenerateBurst(int n)
    {
        Random r = new Random();
        ConcurrentBag<(GenericMaterialMovementDto element, int p)> temp = new();
        Parallel.For(0, n, i =>
        {
            List<MaterialMovementDto> mm = new List<MaterialMovementDto> { new MaterialMovementDto(r.Next(1,4), r.Next(1,11))};
            switch (r.Next(1, 4))
            {
                case 1:
                    GenericMaterialMovementDto newEntry = 
                        new GenericMaterialMovementDto(MovementTypes.Entry, mm, r.Next(1,3), toBin:r.Next(1,6));
                    temp.Add(( newEntry,1));
                break;
                case 2:
                    GenericMaterialMovementDto newMov = 
                            new GenericMaterialMovementDto(MovementTypes.Entry, mm, r.Next(1,3), r.Next(1,6), r.Next(1,6));
                        temp.Add(( newMov,2));
                break;
                case 3:
                GenericMaterialMovementDto newShip = 
                            new GenericMaterialMovementDto(MovementTypes.Entry, mm, r.Next(1,3), r.Next(1,6));
                        temp.Add(( newShip,3));
                break;
                default:
                break;
            }
        });

        return new PriorityQueue<GenericMaterialMovementDto, int>(temp);
    }


    public async Task<BurstResult> RunSecuentialBurst(WarehouseDBContext db, IWarehouseFactory factory, IBinInventoryService invService,
                                                    PriorityQueue<GenericMaterialMovementDto, int> myMovements)
    {
        Random r = new Random();
        int cEntry=0, cMov=0, cShip=0, fMov=0, fShip=0;
        GenericMaterialMovementDto Mov;
        while(myMovements.Count > 0)
        {
            Mov = myMovements.Dequeue();
            try
            {
                switch (Mov.Movement)
                {
                    case MovementTypes.Entry:
                        Movements newEntry = factory.MakeMovement(MovementTypes.Entry, 
                            Mov.materials, 
                            Mov.userId, 
                            toBin:Mov.toBin);
                        db.Movements.Add(newEntry);
                        await invService.AddBinInventory(db, Mov.toBin, Mov.materials);
                        cEntry += 1;
                    break;
                    case MovementTypes.Movement:
                        await invService.ReduceBinInventory(db,Mov.fromBin, Mov.materials);
                        await invService.AddBinInventory(db,Mov.toBin,Mov.materials);

                        Movements newMov = factory.MakeMovement(MovementTypes.Movement, 
                            Mov.materials, 
                            Mov.userId, 
                            Mov.fromBin,
                            Mov.toBin);

                        db.Movements.Add(newMov);
                        cMov += 1;
                    break;
                    case MovementTypes.Shipment:
                        await invService.ReduceBinInventory(db,Mov.fromBin, Mov.materials);

                        Movements newShip = factory.MakeMovement(MovementTypes.Shipment, 
                            Mov.materials, Mov.userId, Mov.fromBin);
                        db.Movements.Add(newShip);
                        await factory.MakeShipment(db, Mov.materials, r.Next(1, 2), Math.Round((decimal)(r.NextDouble() * 100), 2));
                        cShip += 1;
                    break;
                }
            }
            catch(InsufficientStockException)
            {
                if(Mov.Movement == MovementTypes.Movement) fMov += 1;
                else if (Mov.Movement == MovementTypes.Shipment) fShip += 1;
            }
        }

        return new BurstResult(cEntry, cMov, cShip, fMov, fShip);
    }

    
}