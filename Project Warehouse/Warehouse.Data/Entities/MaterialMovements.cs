namespace Warehouse.Data;

public class MaterialMovements
{
    public int MaterialMovement_Id {get; set; }
    public int Movement_Id {get; set; }
    public Movements Movement {get; set; } = default!;
    public int Material_Id {get; set; }
    public Materials Material {get; set; } = default!;
    public byte[] RowVersion {get; set; } = default!;
}