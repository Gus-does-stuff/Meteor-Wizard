using Godot;
using System;
using System.ComponentModel;

public partial class Goblin : Mob
{
	ProgressBar healthbar;
	public override void Entry()
	{
		health = 10;
		damage = 3;
		speed = 100;
		acceletation = 1000;
		spawn_chance = 1f;

		healthbar = GetNode<ProgressBar>("health");
	}

    public override void Damage(int damage, Vector2 velocity)
    {
        health -= damage;
		if(health <= 0)
		{
			this.Free();
		}
		healthbar.Value -= damage*10;
		LinearVelocity += velocity*1.7f;
    }

    public override void Behaviour()
    {
        
    }

}
