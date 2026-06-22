using Godot;
using System;

public partial class Menu : Control
{

	public void switch_menu(String menu_name)
	{
		for(int i = 0; i < GetChildCount(); i++) // to ignore the background color
		{
			if(GetChild(i) is ColorRect) {continue;}
			GetChild<Control>(i).Hide();
		}
		GetNode<Control>(menu_name).Show();
	}
	
	public async void fade_out(String next_scene)
	{
		GD.Print("Fading out");
		Tween fade_out_tween = CreateTween();
		fade_out_tween.TweenProperty(GetNode<ColorRect>("Vignette"), "color:a", 1f, 1f);

		PackedScene next = GD.Load<PackedScene>(next_scene);

		await ToSignal(fade_out_tween, Tween.SignalName.Finished);

		GetTree().ChangeSceneToPacked(next);
	}
}
