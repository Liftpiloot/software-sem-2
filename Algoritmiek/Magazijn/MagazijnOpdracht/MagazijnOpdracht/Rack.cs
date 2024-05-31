namespace MagazijnOpdracht;

public class Rack
{
    public List<Closet> Closets { get; set; }
    
    public Rack(int numberOfClosets, int numberOfLayers)
    {
        Closets = new List<Closet>();
        for (int i = 0; i < numberOfClosets; i++)
        {
            Closets.Add(new Closet(numberOfLayers));
        }
    }

    public bool AddProduct(Product product, int layer)
    { 
        bool productAdded = false;
        int closetIndex = 0;
        while (!productAdded && closetIndex < Closets.Count)
        {
            productAdded = Closets[closetIndex].AddProduct(product, layer);
            closetIndex++;
        }
        return productAdded;
    }
    
}