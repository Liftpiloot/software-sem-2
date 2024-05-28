using MagazijnOpdracht;

namespace MagazijnTest;

public class UnitTestWarehouse
{
    private Warehouse _warehouse;

    [SetUp]
    public void Setup()
    {
        _warehouse = new Warehouse(2, 2);
    }
    
    [Test]
    public void Warehouse_ShouldCreateWarehouseWithTwoRacks()
    {
        // Arrange
        var warehouse = new Warehouse(2, 2);
        
        // Act
        var result = warehouse.Racks.Count;
        
        // Assert
        Assert.That(result, Is.EqualTo(2));
    }

    [Test]
    public void AddProduct_ShouldAddProductToWarehouse()
    {
        // Arrange
        var product = new Product { Size = Size.Small, Speed = Speed.Fast };

        // Act
        var result = _warehouse.AddProduct(product);

        // Assert
        Assert.IsTrue(result);
    }
    
    [Test]
    public void AddProduct_ShouldReturnFalseWhenWarehouseIsFull()
    {
        // Arrange
        List<Product> products = new List<Product>();
        for (int i = 0; i < 100; i++)
        {
            var product = new Product {
                Size = (Size)(i % 3), // This will cycle through the Size enum values
                Speed = (Speed)(i % 2) // This will cycle through the Speed enum values
            };
            products.Add(product);
        }

        // Act
        bool result = true;
        foreach (var product in products)
        {
            result = _warehouse.AddProduct(product);
            if (!result)
            {
                break;
            }
        }

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void FourClosetsShouldFitSixhundredSmallProducts()
    {
        // Arrange
        List<Product> products = new List<Product>();
        for (int i = 0; i < 600; i++)
        {
            var product = new Product { Size = Size.Small, Speed = Speed.Fast };
            products.Add(product);
        }
        
        // Act
        bool result = true;
        foreach (var product in products)
        {
            result = _warehouse.AddProduct(product);
            if (!result)
            {
                break;
            }
        }
        
        // Assert
        Assert.IsTrue(result);
    }
}