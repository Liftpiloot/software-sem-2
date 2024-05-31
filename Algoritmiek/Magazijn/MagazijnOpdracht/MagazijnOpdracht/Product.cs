namespace MagazijnOpdracht;

public class Product
{
    public Width Width { get; set; }
    public Height Height { get; set; }
    public Speed Speed { get; set; }
    
    public Product(Width width, Height height, Speed speed)
    {
        Width = width;
        Height = height;
        Speed = speed;
    }
}
