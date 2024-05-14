namespace MagazijnOpdracht;

public class Warehouse
{
    public List<Rack> Racks { get; set; }

    private readonly int[] _layerNumbers = { 1, 2, 3, 4, 5, 6, 7, 8 };
    
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
        foreach (int layer in _layerNumbers)
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
                        closet.AddShelf(newShelf);
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