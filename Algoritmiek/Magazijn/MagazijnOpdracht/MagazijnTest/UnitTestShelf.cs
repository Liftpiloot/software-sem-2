using MagazijnOpdracht;

namespace MagazijnTest;

public class UnitTestShelf
{
    private Shelf _shelf;
    
    [SetUp]
    public void Setup()
    {
        _shelf = new Shelf(1);
    }
    
    [Test]
    public void Init_ShouldCreateShelfWithHeightAndEmptyProductsList()
    {
        // Arrange
        var shelf = new Shelf(1);
        
        // Act
        var result = shelf.Height;
        
        // Assert
        Assert.That(result, Is.EqualTo(1));
        Assert.That(shelf.Products.Count, Is.EqualTo(0));
    }
    
    [Test]
    public void AddProduct_ShouldAddProductToShelf()
    {
        // Arrange
        var product = new Product { Size = Size.Small, Speed = Speed.Fast };

        // Act
        var result = _shelf.AddProduct(product);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(1, _shelf.Products.Count);
    }

    [Test]
    public void IsEmpty_ShouldReturnTrueWhenShelfIsEmpty()
    {
        // Assert
        Assert.IsTrue(_shelf.IsEmpty());
    }

    [Test]
    public void IsFull_ShouldReturnFalseWhenShelfIsNotFull()
    {
        // Assert
        Assert.IsFalse(_shelf.IsFull());
    }

    [Test]
    public void AddProduct_ShouldNotAddLargeProductToNonFirstOrFifthShelf()
    {
        // Arrange
        _shelf = new Shelf(2);
        var product = new Product { Size = Size.Large, Speed = Speed.Fast };

        // Act
        var result = _shelf.AddProduct(product);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void AddProduct_ShouldNotAddMediumProductToEighthShelf()
    {
        // Arrange
        _shelf = new Shelf(8);
        var product = new Product { Size = Size.Medium, Speed = Speed.Fast };

        // Act
        var result = _shelf.AddProduct(product);

        // Assert
        Assert.IsFalse(result);
    }
    
}