using System.Collections.Generic;
using Zuul;

class Room
{
	
	// Private fields
	private string description;
	private Inventory chest;
	private Dictionary<string, Room> exits; // stores exits of this room.
	private Dictionary<string, bool> lockedExits;
	private Dictionary<string, string> keyTypes;

	// properties
	public Inventory Chest
	{
		get { return chest; }
	}
	
	// Create a room described "description". Initially, it has no exits.
	// "description" is something like "in a kitchen" or "in a court yard".
	public Room(string desc)
	{
		description = desc;
		exits = new Dictionary<string, Room>();
		lockedExits = new Dictionary<string, bool>();
		keyTypes = new Dictionary<string, string>();
		
		chest = new Inventory(999999);
	}
	
	// Define an exit for this room.
	public void AddExit(string direction, Room neighbor, bool isLocked = false, string keyType = "key")
	{
		exits.Add(direction, neighbor);
		lockedExits.Add(direction, isLocked);
		keyTypes.Add(direction, keyType);
	}

	// Return the description of the room.
	public string GetShortDescription()
	{
		return description;
	}

	// Return a long description of this room, in the form:
	//     You are in the kitchen.
	//     Exits: north, west
	public string GetLongDescription()
	{
		string str = "You are ";
		str += description;
		str += ".\n";
		str += GetExitString();
		return str;
	}

	// Return the room that is reached if we go from this room in direction
	// "direction". If there is no room in that direction, return null.
	public Room GetExit(string direction)
	{
		if (exits.ContainsKey(direction))
		{
			return exits[direction];
		}
		return null;
	}

	// Return a string describing the room's exits, for example
	// "Exits: north, west".
	private string GetExitString()
	{
		string str = "Exits: ";
    
		List<string> exitStrings = new List<string>();
		foreach (var exit in exits.Keys)
		{
			if (lockedExits[exit])
			{
				exitStrings.Add(exit + " (locked)");
			}
			else
			{
				exitStrings.Add(exit);
			}
		}
    
		str += String.Join(", ", exitStrings);
		return str;
	}
	
	// helper method
	public string ShowItems()
	{
		ColorfulTextWrapper.WriteTextWithColor("You see the following items: \n", ConsoleColor.Yellow, true, false);
		return chest.Show();
	}
	
	public void LockExit(string direction)
	{
		if (exits.ContainsKey(direction))
		{
			lockedExits[direction] = true;
		}
	}
	
	public void UnlockExit(string direction)
	{
		if (lockedExits.ContainsKey(direction))
		{
			lockedExits[direction] = false;
		}
	}
	
	public bool IsExitLocked(string direction)
	{
		return lockedExits.ContainsKey(direction) && lockedExits[direction];
	}
	
	public void SetKeyType(string direction, string keyType)
	{
		if (exits.ContainsKey(direction))
		{
			keyTypes[direction] = keyType;
		}
	}
	
	public string GetKeyType(string direction)
	{
		if (keyTypes.ContainsKey(direction))
		{
			return keyTypes[direction];
		}
		return "key";
	}
}
