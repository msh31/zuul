namespace Zuul;

class Inventory
{
    // fields
    private int maxWeight;
    private Dictionary<string, Item> items;

    // constructor
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        items = new Dictionary<string, Item>();
    }
    
    // methods
    public bool Put(string itemName, Item item)
    {
        if (item.Weight > maxWeight)
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
            items.Remove(itemName);
            return items[itemName];
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
        return maxWeight - TotalWeight();
    }
}