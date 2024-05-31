namespace MagazijnOpdracht;

public class Warehouse
{
    public List<Rack> Racks { get; set; }
    private List<int> _layerNumbers;
    
    public Warehouse(int numberOfRacks, int numberOfClosets, int numberOfLayers)
    {
        Racks = new List<Rack>();
        for (int i = 0; i < numberOfRacks; i++)
        {
            Racks.Add(new Rack(numberOfClosets, numberOfLayers));
        }
        _layerNumbers = GetLayerNumbers();
    }

    public bool AddProduct(Product product)
    {
        var productAdded = false;
        var layerIndex = 0;
        while (!productAdded && layerIndex < _layerNumbers.Count)
        {
            var rackIndex = 0;
            while (!productAdded && rackIndex < Racks.Count)
            {
                productAdded = Racks[rackIndex].AddProduct(product, _layerNumbers[layerIndex]);
                rackIndex++;
            }
            layerIndex++;
        }
        return productAdded;
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