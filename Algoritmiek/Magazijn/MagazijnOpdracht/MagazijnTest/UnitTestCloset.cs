using MagazijnOpdracht;

namespace MagazijnTest;

public class Tests
{
    [Test]
        public void AddShelf_SuccessfullyAddsShelf()
        {
            // Arrange
            Closet closet = new Closet(3);
            Shelf shelf = new Shelf(2);

            // Act
            bool added = closet.AddShelf(shelf);

            // Assert
            Assert.IsTrue(added);
            Assert.Contains(shelf, closet.Shelves);
        }

        [Test]
        public void AddShelf_SameHeightShelf_ReturnsFalse()
        {
            // Arrange
            Closet closet = new Closet(3);
            Shelf shelf1 = new Shelf(2);
            Shelf shelf2 = new Shelf(2);

            // Act
            closet.AddShelf(shelf1);
            bool added = closet.AddShelf(shelf2);

            // Assert
            Assert.IsFalse(added);
            Assert.AreEqual(1, closet.Shelves.Count);
        }

        [Test]
        public void AddShelf_TooHighShelf_ReturnsFalse()
        {
            // Arrange
            Closet closet = new Closet(3);
            Shelf shelf = new Shelf(4);

            // Act
            bool added = closet.AddShelf(shelf);

            // Assert
            Assert.IsFalse(added);
            Assert.IsEmpty(closet.Shelves);
        }

        [Test]
        public void AddProduct_SuccessfullyAddsProduct()
        {
            // Arrange
            Closet closet = new Closet(3);
            Product product = new Product(Width.Medium, Height.Medium, Speed.Fast);

            // Act
            bool added = closet.AddProduct(product, 2);

            // Assert
            Assert.IsTrue(added);
            Assert.AreEqual(1, closet.Shelves.Count);
            Assert.AreEqual(1, closet.Shelves[0].Products.Count);
        }

        [Test]
        public void AddProduct_TooHighLayer_ReturnsFalse()
        {
            // Arrange
            Closet closet = new Closet(3);
            Product product = new Product(Width.Medium, Height.Medium, Speed.Fast);

            // Act
            bool added = closet.AddProduct(product, 4);

            // Assert
            Assert.IsFalse(added);
            Assert.IsEmpty(closet.Shelves);
        }

        [Test]
        public void AddProduct_FitsExistingShelf_SuccessfullyAddsProduct()
        {
            // Arrange
            Closet closet = new Closet(3);
            Product product1 = new Product(Width.Medium, Height.Medium, Speed.Fast);
            Product product2 = new Product(Width.Medium, Height.Medium, Speed.Medium);
            closet.AddShelf(new Shelf(1));

            // Act
            closet.AddProduct(product1, 1);
            bool added = closet.AddProduct(product2, 1);

            // Assert
            Assert.IsTrue(added);
            Assert.AreEqual(1, closet.Shelves.Count);
            Assert.AreEqual(2, closet.Shelves[0].Products.Count);
        }

        [Test]
        public void AddProduct_FitsNewShelf_SuccessfullyAddsProduct()
        {
            // Arrange
            Closet closet = new Closet(3);
            Product product1 = new Product(Width.Small, Height.Small, Speed.Fast);
            Product product2 = new Product(Width.Medium, Height.Medium, Speed.Medium);

            // Act
            closet.AddProduct(product1, 1);
            bool added = closet.AddProduct(product2, 2);

            // Assert
            Assert.IsTrue(added);
            Assert.AreEqual(2, closet.Shelves.Count);
            Assert.AreEqual(1, closet.Shelves[1].Products.Count);
        }

        [Test]
        public void AddProduct_NoRoomForNewShelf_ReturnsFalse()
        {
            // Arrange
            Closet closet = new Closet(1);
            Product product = new Product(Width.Medium, Height.Medium, Speed.Fast);

            // Act
            bool added = closet.AddProduct(product, 2);

            // Assert
            Assert.IsFalse(added);
            Assert.IsEmpty(closet.Shelves);
        }
    
    
}