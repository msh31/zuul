using System;
using System.Drawing;
using Zuul;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;

	// Constructor
	public Game()
	{
		parser = new Parser();
		player = new Player();
		CreateRooms();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		// Create the rooms
		//Room outside = new Room("in a rain-drenched parking lot. Your car is dead.");
		Room mainHall = new Room("in a decaying entrance hall. Dust covers the reception desk.");
		Room eastCorridor = new Room("in a dark hallway. Patient doors line the walls. Something scratched the paint.");
		Room westWing = new Room("in an abandoned medical wing. Rusted equipment remains on the floors.");
		Room cafeteria = new Room("in the patient cafeteria. Moldy trays still sit on tables.");
		Room staffRoom = new Room("in the staff break room. A bloody hand print marks the wall.");
		Room basement = new Room("in a flooded basement. Water drips from pipes. Rats scatter at your approach.");
		Room morgue = new Room("in the morgue. Body drawers hang open. Recent footprints cross the dusty floor.");
		Room medicalStorage = new Room("in a ransacked supply room. Most cabinets are empty or broken.");
		Room secondFloor = new Room("in the upper ward hallway. More secure rooms line the corridor.");
		Room recordsRoom = new Room("among shelves of patient files. Someone was searching for something here.");
		//Room secOffice = new Room("in the guard station. Monitors still show static images of empty halls.");
		Room directorsOffice = new Room("in a once-lavish office. Papers about \"Project Echo\" scatter the floor.");
		Room laboratory = new Room("in a hidden research lab. Strange symbols mark the walls. Equipment hums.");
		Room ritualChamber = new Room("in a circular room beneath the lab. Candles, symbols, and a strange altar.");
		
		// Initialise room exits
		mainHall.AddExit("east", eastCorridor);
		mainHall.AddExit("west", westWing, true);
		mainHall.AddExit("south", cafeteria);

		eastCorridor.AddExit("west", mainHall);
		eastCorridor.AddExit("south", staffRoom);

		westWing.AddExit("east", mainHall);
		westWing.AddExit("south", medicalStorage);
		westWing.AddExit("down", basement);

		cafeteria.AddExit("north", mainHall);
		cafeteria.AddExit("west", westWing);

		staffRoom.AddExit("north", eastCorridor);
		staffRoom.AddExit("west", cafeteria);

		medicalStorage.AddExit("north", westWing);
		medicalStorage.AddExit("up", secondFloor);

		secondFloor.AddExit("down", medicalStorage);
		secondFloor.AddExit("east", recordsRoom);

		recordsRoom.AddExit("west", secondFloor);
		recordsRoom.AddExit("east", directorsOffice);

		directorsOffice.AddExit("west", recordsRoom);
		directorsOffice.AddExit("down", laboratory);

		laboratory.AddExit("up", directorsOffice);
		laboratory.AddExit("east", ritualChamber);

		ritualChamber.AddExit("west", laboratory);

		basement.AddExit("up", westWing);
		basement.AddExit("east", morgue);

		morgue.AddExit("west", basement);

		// Create your Items here
		Item flashlight = new Item(3, "flashlight");
		Item medkit = new Item(5, "medkit");
		Item baseKey = new Item(1, "key");         // Regular key for basic doors
		Item cardKey = new Item(1, "card");        // Keycard for security doors
		Item masterKey = new Item(1, "masterkey"); // Master key for the ritual chamber
		Item sisterNecklace = new Item(1, "necklace");
		Item ancientBook = new Item(4, "book");
		Item ritualDagger = new Item(2, "dagger");
		Item protectiveCharm = new Item(1, "charm");
		
		// And add them to the Rooms
		mainHall.Chest.Put("flashlight", flashlight);
		westWing.Chest.Put("medkit", medkit);
		cafeteria.Chest.Put("key", baseKey); 
		staffRoom.Chest.Put("card", cardKey);
		morgue.Chest.Put("necklace", sisterNecklace);
		recordsRoom.Chest.Put("book", ancientBook);
		laboratory.Chest.Put("masterkey", masterKey);
		laboratory.Chest.Put("dagger", ritualDagger);
		secondFloor.Chest.Put("charm", protectiveCharm);

		// Start game outside
		player.CurrentRoom = mainHall;
	}

	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		// execute them until the player wants to quit.
		bool finished = false;
		while (!finished)
		{
			Command command = parser.GetCommand();
			finished = ProcessCommand(command);

			if (!player.IsAlive())
			{
				ColorfulTextWrapper.HighlightWordInText("You died.", ConsoleColor.Red, "died", true, false);
				finished = true;
			}
		}
		ColorfulTextWrapper.HighlightWordInText("Thank you for playing! Press [Enter] to continue.", ConsoleColor.Magenta, "Enter", true, true);
		Console.ReadLine();
	}

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		ColorfulTextWrapper.WriteTextWithColor("╔══════════════════════════════════════════════════════════════╗", ConsoleColor.DarkRed, true, true);
		ColorfulTextWrapper.WriteTextWithColor("║                       Z U U L                               ║", ConsoleColor.Red, true, true);
		ColorfulTextWrapper.WriteTextWithColor("╚══════════════════════════════════════════════════════════════╝", ConsoleColor.DarkRed, true, true);
    
		ColorfulTextWrapper.WriteAnimatedTextWithColor("The voices in your head grow louder as you wake up...", ConsoleColor.Gray, true);
		Console.WriteLine();
    
		ColorfulTextWrapper.WriteTextWithColor("You are Geert, a patient who has been confined to this asylum for over a decade.", ConsoleColor.Gray, true, false);
		ColorfulTextWrapper.HighlightWordInText("\"The medicine is poison! They're experimenting on you! You need to escape NOW!\"", ConsoleColor.Yellow, "poison", true, false);
    
		Console.WriteLine();
		ColorfulTextWrapper.WriteTextWithColor("You find yourself in the main hall. The asylum seems strangely empty, but there are signs of recent activity.", ConsoleColor.Gray, true, false);
		ColorfulTextWrapper.WriteTextWithColor("Today is the day you finally break free...", ConsoleColor.DarkRed, true, false);
    
		Console.WriteLine();
		ColorfulTextWrapper.HighlightWordInText("Type 'help' if you need assistance.\n", ConsoleColor.Cyan, "help", true, false);
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}

	// Given a command, process (that is: execute) the command.
	// If this command ends the game, it returns true.
	// Otherwise false is returned.
	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;

		if(command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...");
			return wantToQuit; // false
		}

		switch (command.CommandWord)
		{
			case "help":
				PrintHelp();
				break;
			case "go":
				GoRoom(command);
				break;
			case "look":
				Look();
				break;
			case "status":
				PrintStatus();
				break;
			case "take":
				Take(command);
				break;
			case "drop":
				Drop(command);
				break;
			case "use":
				Use(command);
				break;
			case "quit":
				wantToQuit = true;
				break;
			case "unlock":
				UnlockDoor(command);
				break;
		}

		return wantToQuit;
	}

	// ######################################
	// implementations of user commands:
	// ######################################
	
	// Print out some help information.
	// Here we print the mission and a list of the command words.
	private void PrintHelp()
	{
		Console.WriteLine("You are lost. You are alone. You need to escape.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}
	
	// Print out player status information
	private void PrintStatus()
	{
		ColorfulTextWrapper.HighlightWordInText($"Health: {player.Health} / 100", ConsoleColor.Green, "Health", true, false);
		ColorfulTextWrapper.HighlightWordInText($"Weight: {player.backpack.TotalWeight()} / {player.backpack.MaxWeight} kg", ConsoleColor.Blue, "Weight", true, false);
		
		ColorfulTextWrapper.WriteTextWithColor(player.ShowBackpack() + "\n", ConsoleColor.Gray, false, false);
	}
	
	// Show items in the room
	private void Look()
	{
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
		Console.WriteLine(player.CurrentRoom.ShowItems());
	}

	// Try to go to one direction. If there is an exit, enter the new
	// room, otherwise print an error message.
	private void GoRoom(Command command)
	{
		if(!command.HasSecondWord())
		{
			// if there is no second word, we don't know where to go...
			Console.WriteLine("Go where?");
			return;
		}

		string direction = command.SecondWord;

		// Try to go to the next room.
		Room nextRoom = player.CurrentRoom.GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to "+direction+"!");
			return;
		}

		if (player.CurrentRoom.IsExitLocked(direction))
		{
			ColorfulTextWrapper.HighlightWordInText($"The door to the {direction} is locked. You need to unlock it first.", ConsoleColor.Yellow, $"{direction}", true, false);
			return;
		}
		
		player.Damage(8);
		
		if (!player.IsAlive())
		{
			//ColorfulTextWrapper.HighlightWordInText("You are dead.", ConsoleColor.Red, "dead", true, true);
			return;
		}
		
		player.CurrentRoom = nextRoom;
		
		Console.WriteLine(player.CurrentRoom.GetLongDescription());
	}

	private void Take(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Take what?");
			return;
		}
    
		string itemName = command.SecondWord;
		
		player.TakeFromChest(itemName); //includes error checking
	}
	
	private void Drop(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Drop what?");
			return;
		}
		
		string itemName = command.SecondWord;
		
		if (player.DropToChest(itemName))
		{
			// message handling already done in takefromchest method
		}
		else
		{
			ColorfulTextWrapper.HighlightWordInText($"You don't have a {itemName} in your backpack!", ConsoleColor.Yellow, $"{itemName}", true, false);
		}
	}

	private void Use(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Use what?");
			return;
		}
    
		string itemName = command.SecondWord;
		string direction = null;
    
		// Check if there's a direction specified (like 'use key west')
		if (command.HasThirdWord())
		{
			direction = command.ThirdWord;
			
			if (itemName == "key" || itemName == "card" || itemName == "masterkey")
			{
				UnlockDoor(command);
				return;
			}
		}
    
		string result = player.Use(itemName, direction);
    
		if (!string.IsNullOrEmpty(result))
		{
			ColorfulTextWrapper.WriteFormattedTextByType(result, "inf", true, false);
		}
	}
	
	private void UnlockDoor(Command command)
	{
		if (!command.HasSecondWord())
		{
			Console.WriteLine("Unlock what?");
			return;
		}
    
		if (!command.HasThirdWord())
		{
			Console.WriteLine("Unlock what direction? Try 'unlock key north'");
			return;
		}
    
		string itemName = command.SecondWord;
		string direction = command.ThirdWord;
    
		// Check if player has key
		Item item = player.backpack.Get(itemName);
    
		if (item == null)
		{
			ColorfulTextWrapper.HighlightWordInText($"You don't have a {itemName} in your backpack!", ConsoleColor.Yellow, $"{itemName}", true, false);
			return;
		}
		
		player.backpack.Put(itemName, item);
		
		if (player.CurrentRoom.GetExit(direction) == null)
		{
			Console.WriteLine("There is no door in that direction!");
			return;
		}
    
		if (!player.CurrentRoom.IsExitLocked(direction))
		{
			Console.WriteLine("That door isn't locked!");
			return;
		}
    
		// Check if this is the right key type for this door
		string requiredKeyType = player.CurrentRoom.GetKeyType(direction);
    
		if (itemName.ToLower() == requiredKeyType.ToLower())
		{
			player.CurrentRoom.UnlockExit(direction);
			ColorfulTextWrapper.WriteTextWithColor($"You unlock the door to the {direction} with the {itemName}.", ConsoleColor.Green, true, false);
		}
		else
		{
			ColorfulTextWrapper.HighlightWordInText($"The {itemName} doesn't work on this lock. You need a {requiredKeyType}.", ConsoleColor.Red, requiredKeyType, true, false);
		}
	}
}
