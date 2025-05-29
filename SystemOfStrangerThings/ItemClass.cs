using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemOfStrangerThings
{
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
            if (weird_level > 10 && weird_level < 1)
            {
                Console.WriteLine("Poziom dziwności musi mieścić się w przedziale od 1 do 10");
                return;
            }
            Is_delicate = is_delicate;

        }
        public string description()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }

    }
}
