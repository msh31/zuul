namespace Zuul;

class Player
{
    // auto property
    public Room CurrentRoom { get; set; }
    public int Health { get; private set; }
    
    
    // constructor
    public Player()
    {
        CurrentRoom = null;
        this.Health = 100;
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
}