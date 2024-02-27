// See https://aka.ms/new-console-template for more information

using CircusTrein;

// Define constants
int pointsPerWagon = 10;

// Create a fake list of animals
List<Animal> animals = new List<Animal>();
AddAnimal("Chicken", Animal.Size.Small, Animal.Diet.Herbivore); // Chicken
AddAnimal("Elephant", Animal.Size.Large, Animal.Diet.Herbivore); // Elephant
AddAnimal("Lion", Animal.Size.Large, Animal.Diet.Carnivore); // Lion
AddAnimal("Giraffe", Animal.Size.Large, Animal.Diet.Herbivore); // Giraffe
AddAnimal("Zebra", Animal.Size.Medium, Animal.Diet.Herbivore); // Zebra
AddAnimal("Tiger", Animal.Size.Large, Animal.Diet.Carnivore); // Tiger
AddAnimal("Rabbit", Animal.Size.Small, Animal.Diet.Herbivore); // Rabbit
AddAnimal("Pig", Animal.Size.Medium, Animal.Diet.Herbivore); // Pig
AddAnimal("Cow", Animal.Size.Large, Animal.Diet.Herbivore); // Cow
AddAnimal("Horse", Animal.Size.Large, Animal.Diet.Herbivore); // Horse
AddAnimal("Guinea Pig", Animal.Size.Small, Animal.Diet.Herbivore); // Guinea Pig
AddAnimal("Goose", Animal.Size.Small, Animal.Diet.Herbivore); // Goose


// Get the animals from the user
//List<Animal> animals = GetAnimals();

// Calculate the number of points
int points = animals.Sum(a => (int) a.AnimalSize);
Console.WriteLine("points: " + points);

// Sort the animals by size
animals.Sort((a,b) => -1 * a.AnimalSize.CompareTo(b.AnimalSize));
List<List<Animal>> wagons = new List<List<Animal>>();

// Add the first wagon
wagons.Add(new List<Animal>());

// Put the animals in the wagons
PutAnimmalsInWagons(animals, wagons, pointsPerWagon);

PrintWagons(wagons);


List<Animal> GetAnimals()
{
    bool adding = true;
    List<Animal> list = new List<Animal>();
    while (adding)
    {
        Console.WriteLine("Enter the name of the animal: ");
        string name = Console.ReadLine();
        Console.WriteLine("Enter the size of the animal (s, m, l): ");
        string size = Console.ReadLine();
        switch (size)
        {
            case "s":
                size = "Small";
                break;
            case "m":
                size = "Medium";
                break;
            case "l":
                size = "Large";
                break;
        }
        Console.WriteLine("Enter the diet of the animal (carnivore (c), herbivore (h)): ");
        string diet = Console.ReadLine();
        switch (diet)
        {
            case "c":
                diet = "Carnivore";
                break;
            case "h":
                diet = "Herbivore";
                break;
        }
        list.Add(new Animal
        {
            Name = name,
            AnimalSize = (Animal.Size)Enum.Parse(typeof(Animal.Size), size, true),
            AnimalDiet = (Animal.Diet)Enum.Parse(typeof(Animal.Diet), diet, true)
        });
        Console.WriteLine("Do you want to add another animal? (y/n)");
        adding = Console.ReadLine().ToLower() == "y";
    }

    return list;
}

void PutAnimmalsInWagons(List<Animal> list, List<List<Animal>> wagons1, int pointsPerWagon1)
{
    foreach (Animal animal in list)
    {
        bool placed = false;

        foreach (List<Animal> wagon in wagons1.OrderByDescending(w => w.Sum(a => (int)a.AnimalSize)))
        {
            // Check if there's a carnivore in the wagon that's equal to or larger than the current animal
            if (wagon.Any(a => a.AnimalDiet == Animal.Diet.Carnivore && a.AnimalSize >= animal.AnimalSize))
            {
                continue; // Skip this wagon
            }
            // check if the animal is a carnivore and there is an animal in the wagon that's the same size or smaller
            if (animal.AnimalDiet == Animal.Diet.Carnivore && wagon.Any(a => a.AnimalSize <= animal.AnimalSize))
            {
                continue; // Skip this wagon
            }
            // Check if the animal fits in the wagon
            if (wagon.Sum(a => (int)a.AnimalSize) + (int)animal.AnimalSize <= pointsPerWagon1)
            {
                wagon.Add(animal);
                placed = true;
                break;
            }
        }

        // If the animal doesn't fit in any wagon, add a new wagon
        if (!placed)
        {
            List<Animal> newWagon = new List<Animal> { animal };
            wagons1.Add(newWagon);
        }
    }
}

void PrintWagons(List<List<Animal>> list1)
{
    // print the wagons
    Console.WriteLine("Number of wagons needed: " + list1.Count);
    for (int i = 0; i < list1.Count; i++)
    {
        Console.WriteLine("Wagon " + (i + 1) + ": " + string.Join(", ", list1[i].Select(a => a.AnimalSize + " " + a.AnimalDiet)));
    }
}

void AddAnimal(string name, Animal.Size size, Animal.Diet diet)
{
    animals.Add(new Animal
    {
        Name = name,
        AnimalSize = size,
        AnimalDiet = diet
    });
}
