namespace MagazijnOpdracht;

public class Closet
{
    public List<Shelf> Shelves { get; set; }

    public Closet()
    {
        Shelves = new List<Shelf>();
        Shelves.Add(new Shelf(1));
    }

    public Shelf TopShelf()
    {
        return Shelves.OrderByDescending(shelf => shelf.Height).First();
    }

    public bool AddShelf(Shelf newshelf)
    {
        // if shelf at height already exists, return false
        if (Shelves.Any(shelf => shelf.Height == newshelf.Height))
        {
            return false;
        }

        Shelf topShelf = TopShelf();
        // if the top shelf has large products, return true if there is space for a new shelf
        if (topShelf.IsEmpty() || topShelf.Products[0].Size == Size.Large)
        {
            if (newshelf.Height - TopShelf().Height >= 4)
            {
                Shelves.Add(newshelf);
                return true;
            }
        }

        // if the top shelf has medium products, return true if there is space for a new shelf
        if (topShelf.IsEmpty() || topShelf.Products[0].Size == Size.Medium)
        {
            if (newshelf.Height - TopShelf().Height >= 2)
            {
                Shelves.Add(newshelf);
                return true;
            }
        }

        // if the top shelf has small products, return true if there is space for a new shelf
        if (topShelf.IsEmpty() || TopShelf().Products[0].Size == Size.Small)
        {
            if (Shelves.Count < 6)
            {
                if (newshelf.Height - TopShelf().Height >= 1)
                {
                    Shelves.Add(newshelf);
                    return true;
                }
            }
        }

        return false;
    }
}