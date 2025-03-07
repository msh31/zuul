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
		Room outside = new Room("outside the main entrance of the university");
		Room theatre = new Room("in a lecture theatre");
		Room pub = new Room("in the campus pub");
		Room lab = new Room("in a computing lab");
		Room office = new Room("in the computing admin office");

		// Initialise room exits
		outside.AddExit("east", theatre);
		outside.AddExit("south", lab);
		outside.AddExit("west", pub);

		theatre.AddExit("west", outside);
		theatre.AddExit("up", pub);

		pub.AddExit("east", outside);

		lab.AddExit("north", outside);
		lab.AddExit("east", office);
		lab.AddExit("down", office);

		office.AddExit("west", lab);
		office.AddExit("down", outside);

		// Create your Items here
		Item crowbar = new(5, "crowbar");
		Item chainsaw = new(15, "chainsaw");
		Item medkit = new(25, "medkit");
		Item key = new(10, "key");
		
		// And add them to the Rooms
		outside.Chest.Put("crowbar", crowbar);
		pub.Chest.Put("medkit", medkit);

		// Start game outside
		player.CurrentRoom = outside;
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
		ColorfulTextWrapper.HighlightWordInText($"Welcome to Zuul, {Environment.UserName}!", ConsoleColor.Cyan, "Zuul", true, true);
		ColorfulTextWrapper.HighlightWordInText("Zuul is a new, incredibly boring adventure game.", ConsoleColor.Gray, "adventure", true, true);
		ColorfulTextWrapper.HighlightWordInText("Type 'help' if you need help.\n", ConsoleColor.Yellow, "help", true, true);
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
		Console.WriteLine("You are lost. You are alone.");
		Console.WriteLine("You wander around at the university.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}
	
	// Print out player status information
	private void PrintStatus()
	{
		ColorfulTextWrapper.HighlightWordInText($"Health: {player.Health} / 100", ConsoleColor.Green, "Health", true, false);
		ColorfulTextWrapper.HighlightWordInText($"Weight: {player.backpack.TotalWeight()} / {player.backpack.MaxWeight} kg", ConsoleColor.Blue, "Weight", true, false);
		
		ColorfulTextWrapper.WriteTextWithColor(player.ShowBackpack(), ConsoleColor.Gray, false, false);
		// TODO: Add inventory (max)weight > weight / maxWeight
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
		string result = player.Use(itemName);
		
		ColorfulTextWrapper.WriteFormattedTextByType(result, "inf", true, false);
	}
}
