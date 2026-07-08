namespace Warehouse.Data.Entities;

public interface IWarehouseFactory
{
    public Task<MaterialMovements> MakeMovement(MovementTypes movement, int idMaterial, int quantity, int fromBin = 0, int toBin = 0);
}

public class WarehouseFactory
{
    public async Task<MaterialMovements> MakeMovement(MovementTypes movement, 
                                                            int idMaterial, 
                                                            int quantity, 
                                                            int fromBin = 0, 
                                                            int toBin = 0)
    {
        switch (movement)
        {
            case MovementTypes.entry:
            return new MaterialMovements();
            case MovementTypes.Movement:
            return new MaterialMovements();
            case MovementTypes.Shipment:
            return new MaterialMovements();
            default:
            return new MaterialMovements();
        }
    }
}