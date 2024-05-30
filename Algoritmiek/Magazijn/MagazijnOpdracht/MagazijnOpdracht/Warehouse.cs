namespace MagazijnOpdracht;

public class Warehouse
{
    public List<Rack> Racks { get; set; }
    private List<int> _layerNumbers;
    
    public Warehouse(int numberOfRacks, int numberOfClosets)
    {
        Racks = new List<Rack>();
        for (int i = 0; i < numberOfRacks; i++)
        {
            Racks.Add(new Rack(numberOfClosets));
        }
        _layerNumbers = GetLayerNumbers();
    }

    public bool AddProduct(Product product)
    {
        foreach (int layer in _layerNumbers)
        {
            foreach (Rack rack in Racks)
            {
                if (rack.AddProduct(product, layer))
                {
                    return true; // Product added
                }
            }
        }
        return false; // No space for product
    }
    
    // get the highest layer of all closets
    private List<int> GetLayerNumbers()
    {
        HashSet<int> layerNumbers = new HashSet<int>();
        foreach (Rack rack in Racks)
        {
            foreach (Closet closet in rack.Closets)
            {
                layerNumbers.UnionWith(closet.Layers);
            }
        }
        return layerNumbers.ToList();
    }
    

}