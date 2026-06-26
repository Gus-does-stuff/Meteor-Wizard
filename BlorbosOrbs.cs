using Godot;
using System;
using System.ComponentModel;

public partial class BlorbosOrbs : Area2D
{
	[Export]
	public Vector2 outside_map = Vector2.Zero;
	[Export]
	public Vector2 inside_map = Vector2.Zero;
	// Called when the node enters the scene tree for the first time.
	public async override void _Ready()
	{
		GetNode<RichTextLabel>("Dialogue Box").Text = "[font_size=30][color=#BAFAFA]Bl[wave]orb[/wave]o:[/color] I gotta get outta here!";
		Tween tween = CreateTween();
		tween.TweenProperty(this, "position", outside_map, 5);
		await ToSignal(tween, Tween.SignalName.Finished);

		Monitoring = true;
		GetNode<RichTextLabel>("Dialogue Box").Text = "[font_size=30][color=#BAFAFA]Bl[wave]orb[/wave]o:[/color] Go get 'em! You got this!";
	}	

	public void _on_mobs_killed()
	{
		Tween tween = CreateTween();
		tween.TweenProperty(this, "position", inside_map, 5);
		GetNode<RichTextLabel>("Dialogue Box").Text = "[font_size=30][color=#BAFAFA]Bl[wave]orb[/wave]o:[/color] Phew, that was close. Want to buy some orbs?";
	}

	public async void _on_body_entered(Node2D body)
	{
		GD.Print("Body Entered Received");
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
