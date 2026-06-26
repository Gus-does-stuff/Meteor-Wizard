using Godot;
using Godot.Collections;
using Godot.NativeInterop;
using System;
using System.ComponentModel;
using System.Numerics;

public partial class RollingGoblin : Mob
{
	ProgressBar healthbar;
	
	public String state = "Rolling";
	public float sprawl_time = 6f;
	private float sprawl_timer = 0f;
	private float intangible_time = 0.5f;
	private float intangible_timer = 0;
	private Godot.Vector2 charge_direction;
	public float speed = 250;
	public float Friction = 1000;
	public float Acceleration = 1000;
    public Array<AudioStreamWav> goblin_sounds = [];
	public RandomNumberGenerator rng;
	private Sprite2D sprite;
	private Texture2D sprawled_sprite;
	private Texture2D rolling_sprite;

	public override void _Ready()	
    {
        base._Ready();
		money_drop = 20;
		health = 2;
		damage = 3;

		healthbar = GetNode<ProgressBar>("health");
		for (int i = 1; i <= 3; i++)
		{
			goblin_sounds.Add(ResourceLoader.Load<AudioStreamWav>("Music and Sound/Goblin Hit " + i.ToString() + ".wav"));
		}
		rng = new RandomNumberGenerator();
		sprawled_sprite = ResourceLoader.Load<Texture2D>("Mobs/Sprawled Goblin.png");
		rolling_sprite = ResourceLoader.Load<Texture2D>("Mobs/Rolling Goblin.png");
		sprite = GetNode<Sprite2D>("Sprite2D");
		ApplyCentralImpulse((wizard.Position - Position).Rotated(Mathf.Pi/2));
	}

    public override void Damage(float damage, Godot.Vector2 velocity)
    {
		if (state == "Rolling")
		{
			Funcs.damage_number(GetParent(), Position, 0);
			state = "Intangible";
			LinearDamp = 10;
			sprite.Texture = sprawled_sprite;
			Rotation = 0;
			CollisionLayer = 4;
		}
		if (state == "Sprawled")
		{
			health -= damage;
			if(health <= 0)
			{
				this.die();
			}
			healthbar.Value -= damage;
			ApplyCentralImpulse(velocity);
			Funcs.damage_number(GetParent(), Position, Mathf.RoundToInt(damage));
		}
        
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
		switch(state)
		{
			case "Rolling":
				ApplyCentralForce(LinearVelocity.Normalized() * Acceleration);
				if(LinearVelocity.Length() >= speed)
				{
					LinearVelocity -= LinearVelocity.Normalized() * Friction * (float)delta;
				}
				sprite.Rotation += (float)delta * LinearVelocity.Length()/50;
			break;
			case "Intangible":
				intangible_timer += (float)delta;
				if (intangible_timer >= intangible_time)
				{
					intangible_timer = 0;
					state = "Sprawled";
					CollisionLayer = 1;
				}
			break;
			case "Sprawled":
				Rotation = Mathf.MoveToward(Rotation, 0, (float)delta);
				sprawl_timer += (float)delta;
				if (sprawl_timer >= sprawl_time)
				{
					sprawl_timer = 0;
					state = "Rolling";
					sprite.Texture = rolling_sprite;
					LinearDamp = 0;
					ApplyCentralImpulse(Godot.Vector2.One);
				}
			break;
		}
		
        
		
    }

}