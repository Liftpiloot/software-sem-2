namespace MagazijnOpdracht;

public class Closet
{
    public List <Shelf> Shelves { get; set; }
    
    public Closet()
    {
        Shelves = new List<Shelf>();
        Shelves.Add(new Shelf(0));
    }
    
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
    
    public Shelf TopShelf()
    {
        return Shelves.OrderByDescending(shelf => shelf.Height).First();
    }
    
    public int NumberOfShelves()
    {
        return Shelves.Count;
    }

    public bool AddShelf(Shelf shelf)
    {
        // If the top shelf is full, return false
        if (TopShelf().Height == 8)
        {
            return false;
        }
        
        // if the top shelf has large products, return true if there is space for a new shelf
        if (TopShelf().Products[0].Size == Size.Large)
        {
            if (Shelves.Count < 2)
            {
                if (shelf.Height == 5)
                {
                    Shelves.Add(shelf);
                    return true;
                }
            }
        }
        
        // if the top shelf has medium products, return true if there is space for a new shelf
        if (TopShelf().Products[0].Size == Size.Medium)
        {
            if (Shelves.Count < 4)
            {
                if (shelf.Height - TopShelf().Height >= 2)
                {
                    Shelves.Add(shelf);
                    return true;
                }

            }
        }
        
        // if the top shelf has small products, return true if there is space for a new shelf
        if (TopShelf().Products[0].Size == Size.Small)
        {
            if (Shelves.Count < 6)
            {
                if (shelf.Height - TopShelf().Height >= 1)
                {
                    Shelves.Add(shelf);
                    return true;
                }
            }
        }
        return false;
    }

}