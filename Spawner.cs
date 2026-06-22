using Godot;
using System;
using System.Linq;

public partial class Spawner : Timer
{
	[Export]
	public Godot.Collections.Array<PackedScene> mobs;
	[Export]
	public int spawn_radius = 1000;
	private RandomNumberGenerator rng;
	private Node2D world;
	private Wizard wizard;

    public override void _Ready()
    {
        base._Ready();
		rng = new RandomNumberGenerator();
		world = GetParent<Node2D>();
		wizard = world.GetNode<Wizard>("Wizard");
    }

	public void every_second()
	{
		for(int i=0; i < mobs.Count; i++)
		{
			Mob mob = mobs[i].Instantiate<Mob>();
			if(rng.RandfRange(0, 1) <= mob.spawn_chance)
			{
				float angle = rng.RandfRange(0, 3.14f);
				mob.Position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawn_radius + wizard.Position;
				world.AddChild(mob);
			}
		}
	}
}
