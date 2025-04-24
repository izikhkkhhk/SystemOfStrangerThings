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
        if(weird_level> 10&& weird_level<1)
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
        writer.WriteStringValue(value ? "Yes" : "No");
    }
}

public class WareHouse
{

}
