using Godot;
using System;

public partial class NumberEffect : Node2D
{
	[Export]
	public String Text = "Placeholder";
	[Export]
	public float Duration = 1.0f;
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		GetNode<Label>("Label").Text = Text;
		RandomNumberGenerator rng = new RandomNumberGenerator();
		Tween floater = CreateTween();
		floater.TweenProperty(this, "position", Position + new Vector2(rng.RandfRange(-2.0f, 2.0f), -2)*Duration*100, Duration);
		
		
		await ToSignal(floater, Tween.SignalName.Finished);

		this.QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
