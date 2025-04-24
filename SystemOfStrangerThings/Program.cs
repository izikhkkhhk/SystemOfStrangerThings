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

}

class ShulkerBoxColection : WareHouse
{
    public string Colection_name { get; set; }
    public ShulkerBoxColection(string colection_name, int capacity, decimal maxTotalWeight) : base(capacity, maxTotalWeight)
    {
        Colection_name = colection_name;
    }
}