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
        foreach (Closet closet in Closets)
        {
            if (closet.AddProduct(product, layer))
            {
                return true; // Product added
            }
        }
        return false; // No space for product
    }
}