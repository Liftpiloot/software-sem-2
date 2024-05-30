namespace MagazijnOpdracht;

public class Shelf
{
    public List<Product> Products { get; set; }
    public int Height { get; set; }

    public Shelf(int height)
    {
        Products = new List<Product>();
        Height = height;
    }

    public bool IsEmpty()
    {
        return Products.Count == 0;
    }

    public bool IsFull()
    {
        return Products.Count >= (int)Products[0].Width;
    }

    public bool AddProduct(Product product, int closetHeight)
    {
        if (IsEmpty() || Products[0].Width == product.Width && !IsFull())
        {
            Products.Add(product);
            return true;
        }

        return false;
    }
}