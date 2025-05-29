using System.Text.Json;
using System.Text.Json.Serialization;
using SystemOfStrangerThings;

public class BooleanYesNoConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString()?.ToLower() == "tak";
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value ? "Tak" : "Nie");
    }
}
public class Programm
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Siema Cwaniaczek. Tutaj jest miejsce na wszystkie twoje najdżiwniejsze rzeczy ");

        // Список для хранения всех коллекций Shulker Box
        List<ShulkerBoxColection> shulkerBoxCollections = new List<ShulkerBoxColection>();

        while (true)
        {
            Console.WriteLine("\nGłówne menu:");
            Console.WriteLine("1. Utwórz nową kolekcję ShulkerBox");
            Console.WriteLine("2. Lista koleckji");
            Console.WriteLine("3. Zarządzaj kolekcją  ShulkerBox");
            Console.WriteLine("4. Wychodze stąd");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Wprowadź nazwę kolekcji ShulkerBox:");
                    string collectionName = Console.ReadLine();

                    ShulkerBoxColection newCollection = new ShulkerBoxColection(collectionName);
                    shulkerBoxCollections.Add(newCollection);

                    Console.WriteLine($"Shulker Box Kolekcja '{collectionName}' została poślnie stworzona.");
                    break;

                case "2":
                    if (shulkerBoxCollections.Count == 0)
                    {
                        Console.WriteLine("Nie utworzono jeszcze żadnych kolekcji ShulkerBox");
                    }
                    else
                    {
                        Console.WriteLine("Lista kolekcji Shulker Box:");
                        for (int i = 0; i < shulkerBoxCollections.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {shulkerBoxCollections[i].Colection_name}");
                        }
                    }
                    break;

                case "3":
                    if (shulkerBoxCollections.Count == 0)
                    {
                        Console.WriteLine("Nie utworzono jeszcze żadnych kolekcji ShulkerBoox");
                        break;
                    }

                    Console.WriteLine("Wybierz kolekcję ShulkerBox według numeru :");
                    for (int i = 0; i < shulkerBoxCollections.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {shulkerBoxCollections[i].Colection_name}");
                    }

                    int selectedCollectionIndex;
                    if (int.TryParse(Console.ReadLine(), out selectedCollectionIndex) && selectedCollectionIndex > 0 && selectedCollectionIndex <= shulkerBoxCollections.Count)
                    {
                        ShulkerBoxColection selectedCollection = shulkerBoxCollections[selectedCollectionIndex - 1];
                        ManageShulkerBoxCollection(selectedCollection);
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie");
                    }
                    break;

                case "4":
                    Console.WriteLine("Koniec na dziś...");
                    return;

                default:
                    Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie");
                    break;
            }
        }
    }
    public static void ManageWarehouse(WareHouse warehouse)
    {
        while (true)
        {
            Console.WriteLine("\n\nZarządzanie magazynem:");
            Console.WriteLine("1. Dodaj przedmiot");
            Console.WriteLine("2. Wyświetl wszystkie przedmioty");
            Console.WriteLine("3. Usuń przedmiot");
            Console.WriteLine("4. Wyświetl delikatne lub ciężkie przedmioty");
            Console.WriteLine("5. Pokaż średni poziom dziwności");
            Console.WriteLine("6. Powrót do poprzedniego menu");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Podaj nazwę przedmiotu:");
                    string name = Console.ReadLine();

                    Console.WriteLine("Podaj wagę przedmiotu (w kg):");
                    decimal weight = decimal.Parse(Console.ReadLine());

                    int weirdLevel;
                    while (true)
                    {
                        Console.WriteLine("Podaj poziom dziwności (1-10):");
                        if (int.TryParse(Console.ReadLine(), out weirdLevel) && weirdLevel >= 1 && weirdLevel <= 10)
                        {
                            break;
                        }
                        Console.WriteLine("Błąd: Poziom dziwności musi być pomiędzy 1 a 10. Spróbuj ponownie.");
                    }
                    Console.WriteLine("Czy przedmiot jest delikatny? (true/false):");
                    bool isDelicate = bool.Parse(Console.ReadLine());

                    Item item = new Item(name, weight, weirdLevel, isDelicate);
                    var (success, message) = warehouse.AddItem(item);
                    Console.WriteLine(message);
                    break;

                case "2":
                    warehouse.ListOfAllItems();
                    break;

                case "3":
                    Console.WriteLine("Wprowadź nazwę elementu, który chcesz usunąć:");
                    string itemNameToRemove = Console.ReadLine();
                    warehouse.RemoveItem(itemNameToRemove);
                    break;

                case "4":
                    Console.WriteLine("Wprowadź próg wagowy:");
                    decimal weightThreshold = decimal.Parse(Console.ReadLine());
                    warehouse.list_delicate_or_heavy(weightThreshold);
                    break;

                case "5":
                    warehouse.AverageWeirdLevel();
                    break;

                case "6":
                    return;

                default:
                    Console.WriteLine("Bląd:Nieprawidłowa opcja. Spróbuj ponownie");
                    break;
            }
        }
    }
    private static void ManageShulkerBoxCollection(ShulkerBoxColection collection)
    {
        while (true)
        {
            Console.WriteLine($"\nZarządzanie Shulker Box kolekcją: {collection.Colection_name}");
            Console.WriteLine("1. Dodaj nowy magazyn");
            Console.WriteLine("2. Lista mgazynów");
            Console.WriteLine("3. Zarządzanie magazynami");
            Console.WriteLine("4. Wróć do głównego menu");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Podaj pojemność mamgazynu:");
                    int capacity = int.Parse(Console.ReadLine());

                    Console.WriteLine("Wprowadź maksymalną wagę magazynu (w kg):");
                    decimal maxTotalWeight = decimal.Parse(Console.ReadLine());

                    WareHouse newWarehouse = new WareHouse(capacity, maxTotalWeight);
                    collection.AddWareHouse(newWarehouse);

                    Console.WriteLine("Magazyn został dodany do kolekcji");
                    break;
                case "2":
                    collection.ListWareHouses();
                    break;
                case "3":
                    Console.WriteLine("Wybierz magazyn według numeru:");
                    collection.ListWareHouses();
                    int selectedWarehouseIndex;
                    if (int.TryParse(Console.ReadLine(), out selectedWarehouseIndex) && selectedWarehouseIndex > 0 && selectedWarehouseIndex <= collection.WareHouses.Count)
                    {
                        WareHouse selectedWarehouse = collection.WareHouses[selectedWarehouseIndex - 1];
                        ManageWarehouse(selectedWarehouse);
                    }
                    else
                    {
                        Console.WriteLine("Bląd:Nieprawidłowy wybór. Spróbuj ponownie");
                    }
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Bląd:Nieprawidłowa opcja. Spróbuj ponownie");
                    break;
            }
        }
    }
}
