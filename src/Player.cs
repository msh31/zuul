namespace Zuul;

class Player
{
    // auto property
    public Room CurrentRoom { get; set; }
    public int Health { get; private set; }
    public int MaxHealth { get; }
    public Inventory backpack { get; }
    
    
    // constructor
    public Player()
    {
        CurrentRoom = null;
        this.Health = 100;
        this.MaxHealth = 100;
        this.backpack = new Inventory(25); 
    }
    
    // methods
    public void Damage(int damage)
    {
        this.Health -= damage;
    }

    public void Heal(int heal)
    {
        this.Health += heal;

        if (Health > MaxHealth)
            Health = MaxHealth;
    }

    public bool IsAlive()
    {
        return this.Health > 0;
    }

    public bool TakeFromChest(string itemName)
    {
        Item item = CurrentRoom.Chest.Get(itemName);

        if (item == null)
        {
            ColorfulTextWrapper.HighlightWordInText($"Item: {itemName} does not exist here", ConsoleColor.Yellow, $"{itemName}", true, false);
            return false;
        }

        if (backpack.Put(itemName, item))
        {
            ColorfulTextWrapper.HighlightWordInText($"Added {itemName} to backpack!", ConsoleColor.Gray, $"{itemName}", true, false);
            return true;
        }
        else
        {
            ColorfulTextWrapper.HighlightWordInText($"{itemName} is too heavy!", ConsoleColor.Red, $"{itemName}", true, false);
            CurrentRoom.Chest.Put(itemName, item);
            return false;
        }
    }
    
    public bool DropToChest(string itemName)
    {
        Item item = backpack.Get(itemName);

        if (CurrentRoom.Chest.Put(itemName, item))
        {
            ColorfulTextWrapper.WriteAnimatedTextWithColor($"Dropped {itemName}!", ConsoleColor.Gray, true);
            return true;
        }

        return false;
    }
    
    public string ShowBackpack()
    {
        ColorfulTextWrapper.WriteFormattedTextByType("Your backpack contains: ", "inf", false, false);
        return backpack.Show();
    }

    public string Use(string itemName)
    {
        Item item = backpack.Get(itemName);

        if (item == null)
            return $"{itemName} was not found in the backpack!"; 

        bool consume = false;
        string result = $"You used the {itemName} ";
        
        switch (itemName)
        {
            case "medkit":
                Heal(25);
                result += $"! Current health: {Health}";
                consume = true;
                break;
            case "key":
                result += $"but it doesn't do anything yet..";
                break;
            default:
                result = $"You fiddle with the {itemName}, but nothing special happens.";
                break;
        }
        
        if (!consume)
            backpack.Put(itemName, item);
    
        return result;
    }
}