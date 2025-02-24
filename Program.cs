class Program
{
	public static void Main(string[] args)
	{
		//Game properties
		Console.Title = "Zuul";
		Console.ForegroundColor = ConsoleColor.Gray;
		
		// Create and play the Game.
		Game game = new Game();
		game.Play();
	}
}
