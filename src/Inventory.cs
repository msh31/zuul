namespace Zuul;

class Inventory
{
    // fields
    private Dictionary<string, Item> items;

    // auto property
    public int MaxWeight { get; private set; }
    
    // constructor
    public Inventory(int maxWeight)
    {
        this.MaxWeight = maxWeight;
        items = new Dictionary<string, Item>();
    }
    
    // methods
    public bool Put(string itemName, Item item)
    {
        if (item.Weight > FreeWeight())
        {
            return false;
        }
        
        items.Add(itemName, item);
        return true;
    }

    public Item Get(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            Item item = items[itemName];
            items.Remove(itemName);
            return item;
        }
        
        return null;
    }

    public int TotalWeight()
    {
        int total = 0;

        foreach (var kvp in items)
        {
            Item item = kvp.Value;
            total += item.Weight;
        }

        return total;
    }

    public int FreeWeight()
    {
        return MaxWeight - TotalWeight();
    }

    public string Show()
    {
        if (items.Count == 0)
        {
            return "Nothing";
        }

        string inventory = "";
        
        foreach (var kvp in items)
        {
            string itemName = kvp.Key;
            Item item = kvp.Value;
            
            if (inventory.Length > 0)
                inventory += ", ";

            inventory += $"{itemName} ({item.Weight} kg)";
        }

        return inventory;
    }
}