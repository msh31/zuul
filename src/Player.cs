// ReSharper disable InconsistentNaming 

namespace Zuul;

class Player
{
    // auto property
    public Room CurrentRoom { get; set; }
    public int Health { get; private set; }
    private int MaxHealth { get; }
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

    private void Heal(int heal)
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

        if (item == null)
        {
            ColorfulTextWrapper.WriteFormattedTextByType($"You don't have a {itemName}!", "err", true, false);
            return false;
        }
        
        if (CurrentRoom.Chest.Put(itemName, item))
        {
            ColorfulTextWrapper.WriteTextWithColor($"Dropped {itemName}!", ConsoleColor.DarkGreen, true, false);
            return true;
        }

        backpack.Put(itemName, item);
        return false;
    }
    
    public string ShowBackpack()
    {
        ColorfulTextWrapper.WriteTextWithColor("Your backpack contains: ", ConsoleColor.Yellow, false, false);
        return backpack.Show();
    }

    public string Use(string itemName, string direction = null)
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
            case "card":
            case "masterkey":
                if (direction != null)
                {
                    // handled by the UnlockDoor method
                    result = "";
                }
                else
                {
                    result += "but you need to specify a direction. Try 'use key north'.";
                }
                break;
            case "charm":
                result += "and feel a sense of protection wash over you.";
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