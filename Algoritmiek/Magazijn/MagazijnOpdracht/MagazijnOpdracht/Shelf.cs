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
        if (Products.Count != 0)
        {
            if (Products[0].Size == Size.Small)
            {
                return Products.Count == (int)Size.Small;
            }

            if (Products[0].Size == Size.Medium)
            {
                return Products.Count == (int)Size.Medium;
            }
            return Products.Count == (int)Size.Large;
        }
        return false;
    }
    
    public bool AddProduct(Product product)
    {
        if (this.IsEmpty())
        {
            Products.Add(product);
            return true;
        }
        
        if (this.IsFull())
        {
            return false;
        }

        if (Products[0].Size != product.Size)
        {
            return false;
        }

        Products.Add(product);
        return true;
    }
}