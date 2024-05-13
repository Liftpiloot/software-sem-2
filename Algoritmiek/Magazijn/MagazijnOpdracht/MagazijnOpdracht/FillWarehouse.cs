using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace MagazijnOpdracht;

public class FillWarehouse
{
    private static readonly int NumberOfRacks = 3;
    private static readonly int NumberOfClosets = 6;
    private static Warehouse _warehouse;
    private static List<Product> Products { get; set; }
    
    
    // import products from csv file
    private static List<Product> ImportProducts(string path, int limit=10000)
    {
        using var reader = new StreamReader(path);
        Products = new List<Product>();
        reader.ReadLine(); // skip header
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line.Split(',');

            var product = new Product
            {
                Size = (Size)Enum.Parse(typeof(Size), values[0], true),
                Speed = (Speed)Enum.Parse(typeof(Speed), values[1], true)
            };
            Products.Add(product);
            if (Products.Count >= limit)
            {
                break;
            }
        }

        return Products;
    }

    private static void PrintProducts(List<Product> products)
    {
        foreach (var product in products)
        {
            Console.WriteLine($"Size: {product.Size}, Speed: {product.Speed}");
        }
    }

    private static void PrintRacks(List<Rack> racks)
    {
        for (int i = 0; i < racks.Count; i++)
        {
            Console.WriteLine($"Rack {i + 1}:");
            foreach (var closet in racks[i].Closets)
            {
                Console.WriteLine($"  Closet {racks[i].Closets.IndexOf(closet) + 1}:");
                foreach (var shelf in closet.Shelves)
                {
                    //if (!shelf.IsEmpty())
                    {
                        Console.WriteLine($"    Shelf {shelf.Height}:");
                    }

                    foreach (var product in shelf.Products)
                    {
                        Console.WriteLine($"      - Size: {product.Size}, Speed: {product.Speed}");
                    }
                }
            }
            Console.WriteLine(); // Add a blank line after each rack
        }
    }

    public static void Fill(string path, int limit)
    {
        ImportProducts(path, limit);
        SortProducts();
        Warehouse warehouse = new Warehouse(NumberOfRacks, NumberOfClosets);
        foreach (var product in Products)
        {
            if (!warehouse.AddProduct(product))
            {
                Console.WriteLine("No space for product" + product.Size +" "+ product.Speed);
            }
        }
        _warehouse = warehouse;
    }


    public static void Main()
    {
        Fill("C:\\Users\\Abel\\OneDrive\\ICT-1\\Sem-2\\Opdrachten\\Magazijn\\Product_mock_data.csv", 1000);

        PrintRacks(_warehouse.Racks);
    }

    private static void SortProducts()
    {
        // Sort products large to small, fast to slow
        Products = Products.OrderByDescending(product => product.Speed).ThenBy(product => product.Size).ToList();
    }
}