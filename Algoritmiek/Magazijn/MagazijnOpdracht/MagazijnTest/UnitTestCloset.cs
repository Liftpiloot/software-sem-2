using MagazijnOpdracht;

namespace MagazijnTest;

public class Tests
{
    private Closet _closet;
    
    [SetUp]
    public void Setup()
    {
        _closet = new Closet();
    }
    
    [Test]
    public void Init_ShouldCreateClosetWithOneShelf()
    {
        // Arrange
        var closet = new Closet();
        
        // Act
        var result = closet.Shelves.Count;
        
        // Assert
        Assert.That(result, Is.EqualTo(1));
        Assert.That(closet.Shelves[0].Height, Is.EqualTo(1));
    }

    [Test]
    public void AddShelf_ShouldAddShelfToCloset()
    {
        // Arrange
        var shelf = new Shelf(2);

        // Act
        var result = _closet.AddShelf(shelf);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(_closet.Shelves.Count, Is.EqualTo(2));
        Assert.That(_closet.Shelves[1], Is.EqualTo(shelf));
    }

    [Test]
    public void TopShelf_ShouldReturnTopShelf()
    {
        // Arrange
        var shelf1 = new Shelf(1);
        var shelf2 = new Shelf(2);
        _closet.AddShelf(shelf1);
        _closet.AddShelf(shelf2);

        // Act
        var result = _closet.TopShelf();

        // Assert
        Assert.AreEqual(shelf2, result);
    }
    
    
}