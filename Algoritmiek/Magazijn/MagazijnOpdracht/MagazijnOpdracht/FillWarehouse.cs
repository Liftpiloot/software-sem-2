namespace MagazijnOpdracht;

public class FillWarehouse
{
    private static readonly int NumberOfRacks = 3;
    private static readonly int NumberOfClosets = 6;
    private static Warehouse _warehouse;
    private static List<Product> Products { get; set; }
    private static List<Product> OverFlowProducts { get; set; }
    
    
    // import products from csv file
    private static void ImportProducts(string path, int limit = 10000)
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
                Height = (Height)Enum.Parse(typeof(Height), values[0], true),
                Width = (Width)Enum.Parse(typeof(Width), values[0], true),
                Speed = (Speed)Enum.Parse(typeof(Speed), values[1], true)
            };
            Products.Add(product);
            if (Products.Count >= limit)
            {
                break;
            }
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
                    
                    var groupedProducts = shelf.Products.GroupBy(p => new{p.Width, p.Speed});

                    foreach (var group in groupedProducts)
                    {
                        Console.WriteLine($"        {group.Count()} X Size: {group.Key.Width}, Speed: {group.Key.Speed}");
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
        OverFlowProducts = new List<Product>();
        Warehouse warehouse = new Warehouse(NumberOfRacks, NumberOfClosets);
        foreach (var product in Products)
        {
            if (!warehouse.AddProduct(product))
            {
                OverFlowProducts.Add(product);
            }
        }
        _warehouse = warehouse;
    }


    public static void Main()
    {
        Fill("C:\\Users\\Abel\\OneDrive\\ICT-1\\Sem-2\\Opdrachten\\Magazijn\\Product_mock_data.csv", 100);
        PrintRacks(_warehouse.Racks);
        PrintOverflow();
    }

    private static void PrintOverflow()
    {
        var groupedProducts = OverFlowProducts.GroupBy(p => new{p.Width, p.Speed});
        Console.WriteLine($"Overflow count: {OverFlowProducts.Count}");
        foreach (var group in groupedProducts)
        {
            Console.WriteLine($"    {group.Count()} X Size: {group.Key.Width}, Speed: {group.Key.Speed}");
        }
    }

    private static void SortProducts()
    {
        // Sort products large to small, fast to slow
        Products = Products.OrderByDescending(product => product.Speed).ThenBy(product => product.Width).ToList();
    }
}