namespace MagazijnOpdracht;

public class Warehouse
{
    public List<Rack> Racks { get; set; }
    
    public int[] layerNumbers = { 1, 2, 3, 4, 5, 6, 7, 8 };
    
    public Warehouse(int numberOfRacks, int numberOfClosets)
    {
        Racks = new List<Rack>();
        for (int i = 0; i < numberOfRacks; i++)
        {
            Racks.Add(new Rack(numberOfClosets));
        }
    }

    public bool AddProduct(Product product)
    {
        int[] layers = [];

        if (product.Speed == Speed.Slow)
        {
            layers = layerNumbers.Reverse().ToArray();
        }
        else if (product.Size == Size.Medium)
        {
            int middleIndex = layerNumbers.Length / 2;
        
            List<int> firstHalf = layerNumbers.Take(middleIndex+1).Reverse().ToList();
            List<int> secondHalf = layerNumbers.Skip(middleIndex).ToList();
            
            for (int i = 0; i < Math.Min(firstHalf.Count, secondHalf.Count); i++)
            {
                layers.Append(firstHalf[i]);
                layers.Append(secondHalf[i]);
            }
        }
        else
        {
            layers = layerNumbers;
        }        foreach (int layer in layers)
        {
            // return false if product doesn't fit on height
            if (product.Size == Size.Large && layer != 1 && layer != 5)
            {
                continue;
            }
            if (product.Size == Size.Medium && layer == 8)
            {
                continue;
            }
            foreach (Rack rack in Racks)
            {
                foreach (Closet closet in rack.Closets)
                {
                    if (closet.TopShelf().Height < layer)
                    {
                        Shelf newShelf = new Shelf(layer);
                        bool add = closet.AddShelf(newShelf);
                    }
                    foreach (Shelf shelf in closet.Shelves)
                    {
                        if (shelf.Height == layer)
                        {
                            if (shelf.AddProduct(product))
                            {
                                return true; // Product added
                            }
                        }

                    }
                }
            }
        }
        return false; // No space for product
    }

}