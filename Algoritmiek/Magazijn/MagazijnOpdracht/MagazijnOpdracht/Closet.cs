namespace MagazijnOpdracht;

public class Closet
{
    public List <Shelf> Shelves { get; set; }
    
    public bool IsEmpty()
    {
        return Shelves.Count == 0;
    }

    public bool IsFull()
    {
        if (Shelves.Count != 0)
        {
            return Shelves.Any(shelf => shelf.Height == 8);
        }
        return false;
    }
    
    public int TopShelfHeight()
    {
        if (Shelves.Count != 0)
        {
            return Shelves.Max(shelf => shelf.Height);
        }
        return 0;
    }
    
    public int NumberOfShelves()
    {
        return Shelves.Count;
    }


}