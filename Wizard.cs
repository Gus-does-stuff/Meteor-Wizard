using Godot;
using System;
using System.ComponentModel;
using System.Diagnostics;

public partial class Wizard : CharacterBody2D
{
	[Export]
	public float Speed = 300.0f;
	private RigidBody2D Orb;
	private Rope rope;
	private Control UI;

    public override void _Ready()
    {
        base._Ready(); //Maybe best to leave this in here
		Orb = GetParent().GetNode<RigidBody2D>("ORB");
		rope = GetParent().GetNode<Rope>("Rope");
		rope.create_rope(10, this.GetPath(), Orb.GetPath(), Position, Orb.Position);
		UI = GetParent().GetNode("CanvasLayer").GetNode<Control>("UI");
	}

    public override void _Process(double delta) // For visual stuff
    {
        base._Process(delta);
		
    }

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		Vector2 direction = Input.GetVector("left", "right", "up", "down");

		if (direction != Vector2.Zero)
		{
			velocity = direction * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
