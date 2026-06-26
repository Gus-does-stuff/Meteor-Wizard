using Godot;
using System;

public partial class Border : Area2D
{
	public void _on_body_exited(Node2D body)
	{
		if (body is Alive alive)
		{
			alive.die();
		}
	}
	
}
