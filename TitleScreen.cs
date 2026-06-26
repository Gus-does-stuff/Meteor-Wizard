using Godot;
using System;

public partial class TitleScreen : Control
{
	// Called when the node enters the scene tree for the first time.
	public void _on_start_game()
	{
		Global.Instance.wave = 0;
		Global.Instance.current_items = [];
		Global.Instance.space_ability = "None";
		Global.Instance.shift_ability = "None";
		GetParent<Menu>().fade_out("Cutscenes/first_blorbo_encounter.tscn");
	}
}
