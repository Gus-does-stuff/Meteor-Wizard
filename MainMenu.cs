using Godot;
using System;

public partial class MainMenu : Control
{

	public void _on_run_it_button_up()
	{
		Global.Instance.wave = 0;
		Global.Instance.current_items = [];
		Global.Instance.space_ability = "None";
		Global.Instance.shift_ability = "None";
		GetParent<Menu>().fade_out("Cutscenes/first_blorbo_encounter.tscn");
	}
	public void _on_arena_button_up()
	{
		GetParent<Menu>().fade_out("res://Arena.tscn");
	}
}
