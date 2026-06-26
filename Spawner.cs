using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Numerics;

public partial class Spawner : Timer
{
	[Export]
	public Godot.Collections.Array<PackedScene> mobs;
	[Export]
	public int spawn_radius = 1000;
	private RandomNumberGenerator rng;
	private Node2D world;
	private Wizard wizard;
	public Array<int> spawn_queue = [0];
	public Array<PackedScene> mob_list = [];
	public int spawn_batch_size = 10;
	public Godot.Vector2 spawn_location;
	public int base_node_count;
	[Signal]
	public delegate void AllMobsDefeatedEventHandler();

    public override void _Ready()
    {
        base._Ready();
		rng = new RandomNumberGenerator();
		world = GetParent<Node2D>();
		wizard = world.GetNode<Wizard>("Wizard");
		spawn_location = Godot.Vector2.One.Rotated(rng.RandfRange(0, Mathf.Pi*2));
		mob_list.Add(ResourceLoader.Load<PackedScene>("goblin.tscn"));
		base_node_count = world.GetChildCount();
    }

    public override void _Process(double delta) // Uhhh fix or smthn
    {
        base._Process(delta);
		int spawn_batch = spawn_batch_size;
		if (spawn_batch_size > spawn_queue.Count){spawn_batch = spawn_queue.Count;}
		for (int i = 0; i < spawn_batch; i++)
		{
			int j = spawn_queue[spawn_queue.Count-1];
			spawn_queue.RemoveAt(spawn_queue.Count-1);
			Mob mob = mob_list[j].Instantiate<Mob>();
			mob.Position = spawn_location + Godot.Vector2.One * (rng.RandfRange(-spawn_radius, spawn_radius));
			world.AddChild(mob);
			
		}

		if (world.GetChildCount() == base_node_count)
		{
			EmitSignal(SignalName.AllMobsDefeated);
		}
    }
}
