namespace Airport.Domain;


public interface IBoardable
{
    void Board(int[] passengers);

    void BoardStatus();
}