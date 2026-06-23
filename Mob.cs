using Godot;
using System;
using System.ComponentModel;

public abstract partial class Mob : RigidBody2D, Alive
{
	[Export]
	public float health = 1;
	[Export]
	public int damage = 1;
	[Export]
	public float speed = 1;
    [Export]
    public float acceletation = 1;
	[Export]
	public float spawn_chance = 0.1f;
	public Wizard wizard;
	// Called when the node enters the scene tree for the first time.
    public abstract void Behaviour();
    public abstract void Entry();
    public abstract void Damage(float damage, Vector2 velocity);
    public abstract void hit(Node body);

    public void die()
    {
        this.Free(); // Add potential animation and sound here
    }
	public override void _Ready()
	{
		wizard = GetParent().GetNode<Wizard>("Wizard");
        Entry();
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Vector2 wizard_direction = wizard.Position - Position;
        LinearVelocity += wizard_direction.Normalized() * speed * (float)delta;
        if(LinearVelocity.Length() >= speed)
        {
            LinearVelocity -= LinearVelocity * (float)delta * 0.7f;
        }
        
    }

	public override void _Process(double delta)
	{
		Vector2 wizard_direction = wizard.Position - Position;
        Behaviour();
	}
}
