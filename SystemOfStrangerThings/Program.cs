using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System;
public class Item
{
    public string Name { get; set; }
    public decimal Weight_kg { get; set; }
    public int Weird_level { get; set; }
    public bool Is_delicate { get; set; }

    public Item(string name, decimal weight_kg, int weird_level, bool is_delicate)
    {
        Name = name;
        Weight_kg = Math.Round(weight_kg, 3);
        Weird_level = weird_level;
        Is_delicate = is_delicate;
    }
}
public class WareHouse
{

}

