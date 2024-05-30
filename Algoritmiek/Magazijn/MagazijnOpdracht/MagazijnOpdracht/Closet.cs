namespace MagazijnOpdracht;

public class Closet
{
    public List<Shelf> Shelves { get; set; }

    public List<int> Layers { get; set; }

    public Closet(int layers)
    {
        Shelves = new List<Shelf>();
        Layers = new List<int>();
        for (int i = 1; i <= layers; i++)
        {
            Layers.Add(i);
        }
    }

    public Shelf TopShelf()
    {
        return Shelves.OrderByDescending(shelf => shelf.Height).First();
    }

    public bool AddShelf(Shelf newshelf)
    {
        // if new shelf is higher than the highest layer, return false
        if (newshelf.Height > Layers.Max())
        {
            return false;
        }
        
        // if shelf at height already exists, return false
        if (Shelves.Any(shelf => shelf.Height == newshelf.Height))
        {
            return false;
        }
        
        // if the shelf is the first shelf, add shelf
        if (Shelves.Count == 0)
        {
            Shelves.Add(newshelf);
            return true;
        }
        
        // if new shelf is higher than top shelf including product, and lower than the highest layer, add shelf
        if (newshelf.Height - (int)TopShelf().Products[0].Height >= TopShelf().Height && newshelf.Height <= Layers.Max())
        {
            Shelves.Add(newshelf);
            return true;
        }

        return false;
    }

    public bool AddProduct(Product product, int layer)
    {
        if (Shelves.Count != 0 && TopShelf().Height == layer)
        {
            if (TopShelf().AddProduct(product))
            {
                return true; // Product added
            }
        }
        if (Shelves.Count == 0 || TopShelf().Height < layer)
        {
            Shelf newShelf = new Shelf(layer);
            if (AddShelf(newShelf))
            {
                if (newShelf.AddProduct(product))
                {
                    return true; // Product added
                }
            }
        }

        return false;
    }
}