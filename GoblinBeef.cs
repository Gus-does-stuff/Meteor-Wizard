using Godot;
using Godot.Collections;
using Godot.NativeInterop;
using System;
using System.ComponentModel;

public partial class GoblinBeef : Mob
{
	ProgressBar healthbar;
	
	public float speed = 200;
	public float Friction = 5000;
	public float Acceleration = 1000;
    public Array<AudioStreamWav> goblin_sounds = [];
	public RandomNumberGenerator rng;

	public override void _Ready()	
    {
        base._Ready();
		health = 75;
		damage = 10;

		healthbar = GetNode<ProgressBar>("health");
		for (int i = 1; i <= 3; i++)
		{
			goblin_sounds.Add(ResourceLoader.Load<AudioStreamWav>("Music and Sound/Goblin Hit " + i.ToString() + ".wav"));
		}
		rng = new RandomNumberGenerator();
	}

    public override void Damage(float damage, Vector2 velocity)
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
		sound.PitchScale = rng.RandfRange(0.25f, 0.50f);
		GetParent().AddChild(sound);
		sound.Play();
		sound.Finished += ()=>sound.QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		ApplyCentralForce((wizard.Position - Position).Normalized() * Acceleration);
		if(LinearVelocity.Length() >= speed)
		{
			LinearVelocity -= LinearVelocity.Normalized() * Friction * (float)delta;
		}
    }

}