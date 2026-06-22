using Godot;
using System;

public partial class TitleScreen : Control
{
	// Called when the node enters the scene tree for the first time.
	public void _on_start_game()
	{
		GetParent<Menu>().switch_menu("Main");
	}
}
