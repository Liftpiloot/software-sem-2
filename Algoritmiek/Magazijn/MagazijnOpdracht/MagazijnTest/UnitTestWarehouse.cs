using System.IO.Enumeration;
using MagazijnOpdracht;

namespace MagazijnTest;

public class UnitTestWarehouse
{
    [Test]
    public void AddProduct_SuccessfullyAddsProduct()
    {
        // Arrange
        Warehouse warehouse = new Warehouse(1, 1, 3);
        Product product = new Product(Width.Medium, Height.Medium, Speed.Fast);

        // Act
        bool added = warehouse.AddProduct(product);

        // Assert
        Assert.IsTrue(added);
    }

    [Test]
    public void AddProduct_NoSpaceForProduct_ReturnsFalse()
    {
        // Arrange
        Warehouse warehouse = new Warehouse(1, 1, 1);
        Product product = new Product(Width.Medium, Height.Medium, Speed.Fast);

        // Act
        bool added = warehouse.AddProduct(product);

        // Assert
        Assert.IsFalse(added);
    }

    [Test]
    public void AddProduct_MultipleRacks_SuccessfullyAddsProduct()
    {
        // Arrange
        Warehouse warehouse = new Warehouse(3, 1, 3);
        Product product = new Product(Width.Medium, Height.Medium, Speed.Fast);
        
        // Act
        bool added = warehouse.AddProduct(product);

        // Assert
        Assert.IsTrue(added);
    }
    

    [Test]
    public void AddProduct_EmptyWarehouse_ReturnsFalse()
    {
        // Arrange
        Warehouse warehouse = new Warehouse(0, 0, 0);
        Product product = new Product(Width.Large, Height.Large, Speed.Fast);

        // Act
        bool added = warehouse.AddProduct(product);

        // Assert
        Assert.IsFalse(added);
    }
}