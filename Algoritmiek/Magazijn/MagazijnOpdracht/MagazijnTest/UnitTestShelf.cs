using MagazijnOpdracht;

namespace MagazijnTest;

public class UnitTestShelf
{
[Test]
        public void AddProduct_SuccessfullyAddsProduct()
        {
            // Arrange
            Shelf shelf = new Shelf(1);
            Product product = new Product(Width.Small, Height.Small, Speed.Fast);

            // Act
            bool added = shelf.AddProduct(product, 3); // Closet height is not relevant for Shelf

            // Assert
            Assert.IsTrue(added);
            Assert.AreEqual(1, shelf.Products.Count);
        }

        [Test]
        public void AddProduct_ShelfIsEmpty_SuccessfullyAddsProduct()
        {
            // Arrange
            Shelf shelf = new Shelf(1);
            Product product = new Product(Width.Small, Height.Small, Speed.Fast);

            // Act
            bool added = shelf.AddProduct(product, 3); // Closet height is not relevant for Shelf

            // Assert
            Assert.IsTrue(added);
            Assert.AreEqual(1, shelf.Products.Count);
        }

        [Test]
        public void AddProduct_ShelfIsFull_ReturnsFalse()
        {
            // Arrange
            Shelf shelf = new Shelf(1);
            Product product1 = new Product(Width.Large, Height.Large, Speed.Fast);
            Product product2 = new Product(Width.Large, Height.Large, Speed.Medium);
            Product product3 = new Product(Width.Large, Height.Large, Speed.Slow);
            Product product4 = new Product(Width.Large, Height.Large, Speed.Fast);
            Product product5 = new Product(Width.Large, Height.Large, Speed.Medium);
            
            shelf.AddProduct(product1, 3); // Closet height is not relevant for Shelf
            shelf.AddProduct(product2, 3); 
            shelf.AddProduct(product3, 3); 
            shelf.AddProduct(product4, 3); 
            
            
            shelf.AddProduct(product5, 8);

            // Act
            bool added = shelf.AddProduct(product5, 3); // Closet height is not relevant for Shelf

            // Assert
            Assert.IsFalse(added);
            Assert.AreEqual(4, shelf.Products.Count);
        }

        [Test]
        public void IsEmpty_EmptyShelf_ReturnsTrue()
        {
            // Arrange
            Shelf shelf = new Shelf(1);

            // Act & Assert
            Assert.IsTrue(shelf.IsEmpty());
        }

        [Test]
        public void IsEmpty_NonEmptyShelf_ReturnsFalse()
        {
            // Arrange
            Shelf shelf = new Shelf(1);
            shelf.AddProduct(new Product(Width.Small, Height.Small, Speed.Fast), 3); // Closet height is not relevant for Shelf

            // Act & Assert
            Assert.IsFalse(shelf.IsEmpty());
        }

        [Test]
        public void IsFull_FullShelf_ReturnsTrue()
        {
            // Arrange
            Shelf shelf = new Shelf(1);
            for (int i=0; i<(int)Width.Small; i++)
            {
                shelf.AddProduct(new Product(Width.Small, Height.Small, Speed.Fast), 3); // Closet height is not relevant for Shelf
            }
            
            Shelf shelf2 = new Shelf(1);
            for (int i=0; i<(int)Width.Medium; i++)
            {
                shelf2.AddProduct(new Product(Width.Medium, Height.Medium, Speed.Fast), 3); // Closet height is not relevant for Shelf
            }
            
            Shelf shelf3 = new Shelf(1);
            for (int i=0; i<(int)Width.Large; i++)
            {
                shelf3.AddProduct(new Product(Width.Large, Height.Large, Speed.Fast), 3); // Closet height is not relevant for Shelf
            }

            // Act & Assert
            Assert.IsTrue(shelf.IsFull());
            Assert.IsTrue(shelf2.IsFull());
            Assert.IsTrue(shelf3.IsFull());
        }

        [Test]
        public void IsFull_NotFullShelf_ReturnsFalse()
        {
            // Arrange
            Shelf shelf = new Shelf(1);

            // Act & Assert
            Assert.IsFalse(shelf.IsFull());
        }
    
}