namespace Airport.Domain;

public partial class Catalog
{
    public readonly List<Airplanes> planes = new();

    public readonly Dictionary<int, Airplanes> dictPlanes = new();

    public int Count => planes.Count;

    public bool IsEmpty => planes.Count == 0;

    public void Add(Airplanes plane)
    {
        planes.Add(plane);
        dictPlanes.Add(plane.Id, plane);
    }

    public bool Remove(Airplanes plane)
    {
        planes.Remove(plane);
        dictPlanes.Remove(plane.Id);
        return true;
    }
}
    