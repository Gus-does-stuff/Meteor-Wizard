using Godot;
using Godot.NativeInterop;
using System;
using System.ComponentModel;

public partial class Goblin : Mob
{
	ProgressBar healthbar;
	
	public float speed = 100;
	public float Friction = 1000;
	public float Acceleration = 1000;
    public override void _Ready()
    {
        base._Ready();
		health = 10;
		damage = 3;

		healthbar = GetNode<ProgressBar>("health");
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