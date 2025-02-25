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
    private void Damage(int damage)
    {
        this.Health -= damage;
    }

    private void Heal(int heal)
    {
        this.Health += heal;
    }

    private bool IsAlive()
    {
        if (this.Health <= 0)
        {
            return true;
        }

        return false;
    }
}