using Godot;
using System;

public partial class Death : Control
{
	public async void Continue()
	{
		Tween fade_out_tween = CreateTween();
		fade_out_tween.TweenProperty(GetNode<ColorRect>("Vignette"), "color:a", 1f, 1f);

		PackedScene next = GD.Load<PackedScene>("menu.tscn");

		await ToSignal(fade_out_tween, Tween.SignalName.Finished);

		GetTree().ChangeSceneToPacked(next);
	}
}
