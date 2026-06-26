using Godot;
using Godot.Collections;
using System;

public partial class Global : Node
{
	public String next_scene = "Arena.tscn";
	public String current_orb = "Normal";
	public Array<String> orbs = ["Bouncy", "Massive", "Fast"];
	public Array<String> current_items = []; // For testing
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
	public int money = 0;
	public int wave = 0;
	public Array<Array<int>> goblin_waves = [
		[0],
		[1],
		[1,0,0,0,0,0],
		[2],
		[1,1,0,0,0,0,0,0,0,0,0],
		[1,1,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0],
		[1,1,1,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]
	];

	public AudioStreamWav button_hover;
	public AudioStreamWav button_click;

	public RandomNumberGenerator rng = new RandomNumberGenerator();


	public static Global Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
		button_hover = ResourceLoader.Load<AudioStreamWav>("Music and Sound/Button Hover.wav");
		button_click = ResourceLoader.Load<AudioStreamWav>("Music and Sound/Button Click.wav");
	}

	public async void emphasis_shake(SceneTree scene_tree)
    {
        Engine.TimeScale = 0.4f;
        await ToSignal(scene_tree.CreateTimer(0.1f, true, false, true), SceneTreeTimer.SignalName.Timeout);
		Engine.TimeScale = 1.0f;
    }

	public void connect_button_sounds()
	{
		Array<Node> buttons = GetTree().GetNodesInGroup("Buttons");
		for(int i = 0; i < buttons.Count; i++)
		{
			if (buttons[i] is Button button)
			{
				button.MouseEntered += _on_button_hover;
				button.ButtonDown += _on_button_click;
				button.ButtonUp += _on_button_click;
			}
		}
	}

	public void _on_button_hover()
	{
		AudioStreamPlayer sound = new AudioStreamPlayer();
		sound.Stream = button_hover;
		sound.PitchScale = rng.RandfRange(0.75f, 1.25f);
		GetParent().AddChild(sound);
		sound.Play();
		sound.Finished += ()=>sound.QueueFree();
	}

	public void _on_button_click()
	{
		AudioStreamPlayer sound = new AudioStreamPlayer();
		sound.Stream = button_click;
		sound.PitchScale = rng.RandfRange(0.75f, 1.25f);
		GetParent().AddChild(sound);
		sound.Play();
		sound.Finished += ()=>sound.QueueFree();
	}
}
