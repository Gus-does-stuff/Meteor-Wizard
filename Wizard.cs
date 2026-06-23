using Godot;
using Godot.Collections;
using System;
using System.ComponentModel;
using System.Diagnostics;

public partial class Wizard : CharacterBody2D, Alive
{
	[Export]
	public float Speed = 300.0f;
	[Export]
	public Array<String> powers;
	public String orb_type = "Normal";
	public float health = 100;
	private RigidBody2D Orb;
	private Rope rope;
	private Control UI;

	public void Damage(float damage)
	{
		Global.Instance.health -= damage;
		UI.GetNode<ProgressBar>("Health").Value = Global.Instance.health;
		if (Global.Instance.health <= 0)
		{
			die();
		}
	}

    public override void _Ready()
    {
        base._Ready(); //Maybe best to leave this in here
		Orb = GetParent().GetNode<RigidBody2D>("ORB");
		rope = GetParent().GetNode<Rope>("Rope");
		rope.create_rope(10, this.GetPath(), Orb.GetPath(), Position, Orb.Position);
		UI = GetNode("CanvasLayer").GetNode<Control>("UI");
		UI.GetNode<ProgressBar>("Health").Value = Global.Instance.health;
	}

	public async void die()
	{
		Tween fade_out_tween = CreateTween();
		fade_out_tween.TweenProperty(UI.GetNode<ColorRect>("Vignette"), "color:a", 1f, 1f);

		PackedScene next = GD.Load<PackedScene>("death.tscn");

		await ToSignal(fade_out_tween, Tween.SignalName.Finished);

		GetTree().ChangeSceneToPacked(next);
	}

    public override void _Process(double delta) // For visual stuff
    {
        base._Process(delta);
		UI.GetNode<Label>("Money").Text = Global.Instance.money.ToString() + " Dollaridoos";
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

	public void update_powers() // None for now. Durable for extra health? Extra speed?
	{
		
	}
}
