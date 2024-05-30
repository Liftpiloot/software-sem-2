namespace MagazijnTest;
using MagazijnOpdracht;

[TestFixture]
public class UnitTestRack
{
    [Test]
    public void AddProduct_SuccessfullyAddsProductToCloset()
    {
        // Arrange
        Rack rack = new Rack(1, 3);
        Product product = new Product(Width.Small, Height.Small, Speed.Fast);

        // Act
        bool added = rack.AddProduct(product, 2);

        // Assert
        Assert.IsTrue(added);
        Assert.AreEqual(1, rack.Closets[0].Shelves[0].Products.Count);
    }

    [Test]
    public void AddProduct_NoSpaceInAnyCloset_ReturnsFalse()
    {
        // Arrange
        Rack rack = new Rack(1, 1);
        Product product = new Product(Width.Small, Height.Small, Speed.Fast);
        Product product2 = new Product(Width.Medium, Height.Medium, Speed.Fast);
        rack.AddProduct(product, 1); // Fills the only available closet

        // Act
        bool added = rack.AddProduct(product2, 1);

        // Assert
        Assert.IsFalse(added);
    }

    [Test]
    public void AddProduct_SpaceInAnotherCloset_SuccessfullyAddsProduct()
    {
        // Arrange
        Rack rack = new Rack(2, 2); // Two closets
        Product product = new Product(Width.Small, Height.Small, Speed.Fast);
        Product product2 = new Product(Width.Medium, Height.Medium, Speed.Fast);
        rack.AddProduct(product, 1); // Fills the first closet

        // Act
        bool added = rack.AddProduct(product2, 1);

        // Assert
        Assert.IsTrue(added);
        Assert.AreEqual(1, rack.Closets[1].Shelves[0].Products.Count);
    }
}