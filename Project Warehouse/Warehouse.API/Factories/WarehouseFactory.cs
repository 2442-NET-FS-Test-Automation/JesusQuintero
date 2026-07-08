namespace Warehouse.Data.Entities;

public interface IWarehouseFactory
{
    public Movements MakeMovement(MovementTypes movement, 
                                            IEnumerable<(int idMaterial, int quantity)> materials,
                                            int userId,
                                            int fromBin = 0, 
                                            int toBin = 0);
    public Movements CreateEntryMovement(IEnumerable<(int idMaterial, int quantity)> materials,int userId, int toBin);
    public Movements CreateInternalMovement(IEnumerable<(int idMaterial, int quantity)> materials,int userId, int fromBin, int toBin);
    public Movements CreateShipmentMovement(IEnumerable<(int idMaterial, int quantity)> materials,int userId, int fromBin);
}

public class WarehouseFactory : IWarehouseFactory
{
    public Movements MakeMovement(MovementTypes movement, 
                                            IEnumerable<(int idMaterial, int quantity)> materials,
                                            int userId,
                                            int fromBin = 0, 
                                            int toBin = 0)
    {
        switch (movement)
        {
            case MovementTypes.entry:  return CreateEntryMovement(materials, userId, toBin);
            case MovementTypes.Movement: return CreateInternalMovement(materials, userId, fromBin, toBin);
            case MovementTypes.Shipment: return CreateShipmentMovement(materials, userId, fromBin);
            default:
                throw new ArgumentException($"Unknown order kind: {movement}");
        }
    }

    public Movements CreateEntryMovement(IEnumerable<(int idMaterial, int quantity)> materials,int userId, int toBin)
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

    public Movements CreateInternalMovement(IEnumerable<(int idMaterial, int quantity)> materials,int userId, int fromBin, int toBin)
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

    public Movements CreateShipmentMovement(IEnumerable<(int idMaterial, int quantity)> materials,int userId, int fromBin)
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
}