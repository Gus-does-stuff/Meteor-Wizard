using Godot;
using System;

public partial class Orb : RigidBody2D
{


	public void _on_slam(Node body)
	{
		GD.Print("Wow we slammed!");
		if (body is Mob mob)
		{
			mob.Damage(5, LinearVelocity);
		}
	}

}
