using Godot;
using System;
using System.ComponentModel;
using System.Linq;

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
    public int money_drop = 1;
	public Wizard wizard;
	// Called when the node enters the scene tree for the first time.
    public abstract void Behaviour();
    public abstract void Entry();
    public abstract void Damage(float damage, Vector2 velocity);
    public void hit(Node body)
    {
		if (body is Wizard)
		{
			wizard.Damage(damage);
			LinearVelocity = (wizard.Position - Position).Normalized() * -500;
		}
    }

    public void die()
    {
        this.QueueFree(); // Add potential animation and sound here
        Global.Instance.money += money_drop + Global.Instance.current_items.Count(o=>o=="Hemonomics");
    }
	public override void _Ready()
	{
		wizard = GetParent().GetNode<Wizard>("Wizard");
        BodyEntered += hit;
        ContactMonitor = true;
        MaxContactsReported = 3;
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
