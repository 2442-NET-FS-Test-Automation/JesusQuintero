namespace Airport.Domain;

public partial class Catalog
{
    public List<Airplanes> Planes = new();

    public readonly Dictionary<int, Airplanes> dictPlanes = new();

    public int Count => Planes.Count;

    public bool IsEmpty => Planes.Count == 0;

    public void Add(Airplanes plane)
    {
        Planes.Add(plane);
        dictPlanes.Add(plane.Id, plane);
    }

    public bool Remove(Airplanes plane)
    {
        Planes.Remove(plane);
        dictPlanes.Remove(plane.Id);
        return true;
    }

    public void SetList(List<Airplanes> newPlanes)
    {
        Planes = newPlanes;
    }
}
    