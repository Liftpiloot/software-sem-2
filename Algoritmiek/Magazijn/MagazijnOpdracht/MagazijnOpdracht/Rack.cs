namespace MagazijnOpdracht;

public class Rack
{
    public List<Closet> Closets { get; set; }
    
    public Rack(int numberOfClosets)
    {
        Closets = new List<Closet>();
        for (int i = 0; i < numberOfClosets; i++)
        {
            if (i % 2 == 0) // TODO remove if statement
            {
                Closets.Add(new Closet(6));
                continue;
            }
            Closets.Add(new Closet(8));
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