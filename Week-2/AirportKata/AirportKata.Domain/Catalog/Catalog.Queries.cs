using System.Collections;

namespace Airport.Domain;

public partial class Catalog : IEnumerable<Airplanes>
{
    public IEnumerator<Airplanes> GetEnumerator()
    {
        foreach( Airplanes plane in Planes)
        {
            // We wanto to lazily returns items one a time, we don't want to return a second list
            // or anything like that. We will use "yield" with out return
            yield return plane;
        }    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerable<Airplanes> Boardable()
    {
        
        foreach (Airplanes plane in Planes)
        {
            // Checking for type via "is"
            if (plane is IBoardable)
            {
                yield return plane;
            }
        }
    }
}