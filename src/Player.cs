namespace Zuul;

class Player
{
    // fields
    private Inventory backpack;
    
    // auto property
    public Room CurrentRoom { get; set; }
    public int Health { get; private set; }
    
    
    // constructor
    public Player()
    {
        CurrentRoom = null;
        this.Health = 100;
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
    }

    public bool IsAlive()
    {
        return this.Health > 0;
    }

    public bool TakeFromChest(string itemName)
    {
        Item item = CurrentRoom.Chest.Get(itemName);

        if (backpack.Put(itemName, item))
        {
            ColorfulTextWrapper.WriteAnimatedTextWithColor($"Added {itemName} to backpack!", ConsoleColor.Gray, true);
            return true;
        }
        else
        {
            ColorfulTextWrapper.WriteAnimatedTextWithColor($"{itemName} is too heavy!", ConsoleColor.Red, true);
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
        ColorfulTextWrapper.WriteFormattedTextByType("Your backpack contains: ", "inf", true, false);
        return backpack.Show();
    }
}