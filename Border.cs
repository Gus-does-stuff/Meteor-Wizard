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

	public async void on_shop_entered(Node2D body)
	{
		if (body is Wizard wizard)
		{
			Tween fade_out_tween = CreateTween();
			fade_out_tween.TweenProperty(GetParent().GetNode("Wizard").GetNode("CanvasLayer").GetNode("UI").GetNode<ColorRect>("Vignette"), "color:a", 1f, 1f);

			PackedScene next = GD.Load<PackedScene>("upgrades.tscn");

			await ToSignal(fade_out_tween, Tween.SignalName.Finished);

			GetTree().ChangeSceneToPacked(next);
		}
	}
	
}
