using Godot;
using Godot.Collections;
using System;

public partial class Global : Node
{
	public String next_scene = "Arena.tscn";
	public String current_orb = "Normal";
	public Array<String> orbs = ["Bouncy", "Massive", "Fast"];
	public Array<String> current_items;
	public Array<String> items = [];
	public String space_ability = "Spin Orb";
	public Array<String> space_abilities = ["Attract Orb", "Spin Orb"];
	public String shift_ability = "Shield";
	public Array<String> shift_abilities = ["Dash Away", "Dash Toward", "Shield"];
	public float health = 100;
	public int money;

	public static Global Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}
}
