namespace SystemOfStrangerThings
{
    public class ShulkerBoxColection
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
}
