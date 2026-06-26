using Godot;
using Godot.Collections;
using Godot.NativeInterop;
using System;
using System.ComponentModel;
using System.Numerics;

public partial class Goblin : Mob
{
	ProgressBar healthbar;
	
	public String state = "Recovering";
	public float engage_distance = 500;
	public float wait_time = 0.5f;
	private float wait_timer = 0f;
	public float charge_time = 2f;
	private float charge_timer = 0f;
	private Godot.Vector2 charge_direction;
	public float speed = 250;
	public float Friction = 1000;
	public float Acceleration = 1000;
    public Array<AudioStreamWav> goblin_sounds = [];
	public RandomNumberGenerator rng;

	public override void _Ready()	
    {
        base._Ready();
		money_drop = 5;
		health = 10;
		damage = 3;

		healthbar = GetNode<ProgressBar>("health");
		for (int i = 1; i <= 3; i++)
		{
			goblin_sounds.Add(ResourceLoader.Load<AudioStreamWav>("Music and Sound/Goblin Hit " + i.ToString() + ".wav"));
		}
		rng = new RandomNumberGenerator();
	}

    public override void Damage(float damage, Godot.Vector2 velocity)
    {
        health -= damage;
		if(health <= 0)
		{
			this.die();
		}
		healthbar.Value -= damage;
		ApplyCentralImpulse(velocity);
		Funcs.damage_number(GetParent(), Position, Mathf.RoundToInt(damage));
		AudioStreamPlayer sound = new AudioStreamPlayer();
		sound.Stream = goblin_sounds[rng.RandiRange(0,2)];
		sound.PitchScale = rng.RandfRange(0.75f, 1.25f);
		GetParent().AddChild(sound);
		sound.Play();
		sound.Finished += ()=>sound.QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
		base._PhysicsProcess(delta);
		Rotation = 0;
		switch(state)
		{
			case "Recovering":
				ApplyCentralForce((wizard.Position - Position).Normalized() * Acceleration);
				if(LinearVelocity.Length() >= speed)
				{
					LinearVelocity -= LinearVelocity.Normalized() * Friction * (float)delta;
				}
				if ((wizard.Position - Position).Length() <= engage_distance)
				{
					state = "Engaged";
					LinearVelocity = Godot.Vector2.Zero;
				}
			break;
			case "Engaged":
				wait_timer += (float)delta;
				if (wait_timer >= wait_time)
				{
					wait_timer = 0;
					state = "Charging";
					charge_direction = (wizard.Position - Position).Normalized();
					GetNode<Line2D>("Charge Indicator").Points = [Godot.Vector2.Zero, (wizard.Position - Position)];
				}
			break;
			case "Charging":
				charge_timer += (float)delta;
				if (charge_timer >= charge_time)
				{
					charge_timer = 0;
					state = "Recovering";
					GetNode<Line2D>("Charge Indicator").Points = [];
				}
				ApplyCentralForce(charge_direction * Acceleration);
			break;
		}
		
        
		
    }

}