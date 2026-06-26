using Godot;
using Godot.Collections;
using System;

public partial class Global : Node
{
	public String next_scene = "Arena.tscn";
	public String current_orb = "Bouncy";
	public Array<String> orbs = ["Bouncy", "Massive", "Fast"];
	public Array<String> current_items = ["Lead Tipped Boots"]; // For testing
	public Array<String> items = 
		[
			"Prickly Orb", // Done!
			"Magickify Orb",// Done!
			"More Magic!", // Done!
			"Aerodynamics", // Done!
			"Lead Tipped Boots", // Done! (I think. Requires testing.)
			"Big Orb", // Done probably.
			"Hemonomics"//, // Done!
			//"Healing Potion" // Has to wait until shop is done.
		];
	public Array<String> item_descriptions = 	
		[
			"+20% Orb Damage", 
			"-20% Orb Ability Cooldown", 
			"-20% Wizard Ability Cooldown",
			"+20% Max Orb Speed",
			"Walk into the orb to kick it. Also kickspeed stacks.",
			"20% Bigger Orb",
			"+1 dollaridoos dropped on kill",
			"Heal 20% Health"
		];
	public String space_ability = "None";
	public Array<String> space_abilities = ["Attract Orb", "Spin Orb"];
	public String shift_ability = "None";
	public Array<String> shift_abilities = ["Dash Away", "Dash Toward", "Shield"];
	public float health = 100;
	public int money = 1000;

	public static Global Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}
}
