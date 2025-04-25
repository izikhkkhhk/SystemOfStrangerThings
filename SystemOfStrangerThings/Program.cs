using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System;
public class Item
{
    public string Name { get; set; }
    public decimal Weight_kg { get; set; }
    public int Weird_level { get; set; }
    [JsonConverter(typeof(BooleanYesNoConverter))]
    public bool Is_delicate { get; set; }

    public Item(string name, decimal weight_kg, int weird_level, bool is_delicate)
    {
        Name = name;
        Weight_kg = Math.Round(weight_kg, 3);
        Weird_level = weird_level;
        if(weird_level > 10 && weird_level < 1)
        {
            Console.WriteLine("Weird level must be between 1 and 10");
            return;
        }
        Is_delicate = is_delicate;

    }
    public string description()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }

}
public class BooleanYesNoConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString()?.ToLower() == "yes";
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value ? "Tak" : "Nie");
    }
}

public class WareHouse
{
    public int Capacity { get; private set; } // pojemnosc
    public int CurrentItemCount { get; private set; } // aktualna_ilosc_przedmiotow
    public decimal MaxTotalWeight { get; private set; } // maksymalna_laczna_waga
    private List<Item> Items; // Kolekcja przechowująca przedmioty

    public WareHouse(int capacity, decimal maxTotalWeight)
    {
        Capacity = capacity;
        MaxTotalWeight = Math.Round(maxTotalWeight, 3); // Округление до 3 знаков после запятой
        Items = new List<Item>();
        CurrentItemCount = 0;
    }
    private decimal GetCurrentTotalWeight()
    {
        decimal totalWeight = 0;
        foreach (var item in Items)
        {
            totalWeight += item.Weight_kg;
        }
        return totalWeight;
    }
    public (bool, string) AddItem(Item item)
    {
        if (CurrentItemCount >= Capacity)
        {
            return (false, "Bląd:Magazyn jest pełny. Nie można dodać więcej przedmiotów.");
        }

        decimal currentTotalWeight = GetCurrentTotalWeight();
        if (currentTotalWeight + item.Weight_kg > MaxTotalWeight)
        {
            return (false, "Błąd:Dodanie tego przedmiotu przekroczy maksymalną dopuszczalną wagę magazynu.");
        }
        if (CurrentItemCount > Capacity / 2)
        {
            if (item.Weird_level >= 7 && item.Is_delicate == true)
            {
                return (false, "Błąd: Zbyt ryzykowny przedmiot przy obecnym zapełnieniu");
            }
        }

        Items.Add(item);
        CurrentItemCount++;
        return (true, "Przedmiot został pomyślnie dodany do magazynu.");
    }
    public void ListOfAllItems()
    {
        if (Items.Count == 0)
        {
            Console.WriteLine("Magazyn jest pusty");
            return;
        }
        Console.WriteLine("Lista wszystkich przedmiotow:");
        foreach (var item in Items)
        {
            Console.WriteLine(item.description());
        }
    }
    public bool RemoveItem(string itemName)
    {
        var itemToRemove = Items.Find(item => item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        if (itemToRemove != null)
        {
            Items.Remove(itemToRemove);
            CurrentItemCount--;

            Console.WriteLine($"Przedmiot '{itemName}' został usunięty z magazynu.");
            return true;
        }
        else
        {
            Console.WriteLine($"Błąd: Przedmiot o nazwie '{itemName}' nie został znaleziony w magazynie.");
            return false;
        }
    }
    public void list_delicate_or_heavy(decimal weight_threshold)
    {
        if (Items.Count == 0)
        {
            Console.WriteLine("Magazyn jest pusty.");
            return;
        }

        Console.WriteLine($"Przedmioty, które są delikatne lub mają wagę większą niż {weight_threshold} kg:");
        foreach (var item in Items)
        {
            if (item.Is_delicate || item.Weight_kg > weight_threshold)
            {
                Console.WriteLine(item.description());
            }
        }

    }
    public void AverageWeirdLevel()
    {
        if (Items.Count == 0)
        {
            Console.WriteLine("Magazyn jest pusty.");
            return;
        }
        decimal totalWeirdLevel = 0;
        foreach (var item in Items)
        {
            totalWeirdLevel += item.Weird_level;
        }
        decimal averageWeirdLevel = totalWeirdLevel / Items.Count;
        Console.WriteLine($"Średni poziom dziwności przedmiotów w magazynie: {averageWeirdLevel}");
    }   
}

class ShulkerBoxColection
{
    public string Colection_name { get; set; }
    public List<WareHouse> WareHouses;

    public ShulkerBoxColection(string name)
    {
        Colection_name = name;
        WareHouses = new List<WareHouse>();
    }
    public void AddWareHouse(WareHouse wareHouse)
    {
        WareHouses.Add(wareHouse);
    }
    public void ListWareHouses()
    {
        if (WareHouses.Count == 0)
        {
            Console.WriteLine("Brak kolekcji.");
            return;
        }
        Console.WriteLine($"Lista kolekcji '{Colection_name}':");
        foreach (var wareHouse in WareHouses)
        {
            Console.WriteLine($"- Magazyn o pojemności {wareHouse.Capacity} i maksymalnej wadze {wareHouse.MaxTotalWeight} kg");
        }
    }
}
public class Programm
{
    public static void Main(string[] args)
    {
    }
    public static void ManageWarehouse(WareHouse warehouse)
    {
        while (true)
        {
            Console.WriteLine("\nZarządzanie magazynem:");
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
                    Console.WriteLine("Wybierz magazyn który potrzebujesz:");
                    collection.ListWareHouses();
                    int selectedWarehouseIndex;
                    if (int.TryParse(Console.ReadLine(), out selectedWarehouseIndex) && selectedWarehouseIndex > 0 && selectedWarehouseIndex <= collection.WareHouses.Count)
                    {
                        WareHouse selectedWarehouse = collection.WareHouses[selectedWarehouseIndex - 1];
                        ManageWarehouse(selectedWarehouse);
                    }
                    else
                    {
                        Console.WriteLine("Błąd:Nieprawidłowy wybór. Spróbuj ponownie");
                    }
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Błąd:Nieprawidłowa opcja. Spróbuj ponownie.");
                    break;
            }
        }
    } 
}
