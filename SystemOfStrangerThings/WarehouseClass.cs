namespace SystemOfStrangerThings
{
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
}
