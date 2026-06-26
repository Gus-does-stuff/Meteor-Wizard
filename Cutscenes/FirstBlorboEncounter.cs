using Godot;
using System;

public partial class FirstBlorboEncounter : Control
{
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		TextureRect window = GetNode<TextureRect>("VBoxContainer/Control/cutscene_window");
		RichTextLabel subtitles = GetNode<RichTextLabel>("VBoxContainer/subtitle_window");
		TextureRect background = GetNode<TextureRect>("VBoxContainer/Control/cutscene_background");
		Tween tween = CreateTween();
		background.Texture = ResourceLoader.Load<Texture2D>("Assets/The Fall.png");
		window.Modulate = new Color(0,0,0,0);
		background.Modulate = new Color(1,1,1,1);
		subtitles.Text = "You were cast out of Mage Society. They were fed up with you tying ropes around their magic orbs. Bah! To heck with them and their schemes. I'll show them. I'll show them all...";
		tween.TweenProperty(background, "modulate", new Color(0,0,0,1), 5);

		await ToSignal(tween, Tween.SignalName.Finished);
		GD.Print("Past await");

		background.Texture = ResourceLoader.Load<Texture2D>("Assets/Bottom Of Chasm.png");
		subtitles.Text = "At the bottom of the Great Wizard Cliff, you wake. In landing, you exhausted your last orb. A green creature waddles around in the distance. Now if you could only find your hat...";
		GetNode<Button>("VBoxContainer/Leave").Disabled = false;
		tween = CreateTween();
		tween.TweenProperty(background, "modulate", Colors.White, 10);

		await ToSignal(tween, Tween.SignalName.Finished);

		window.Texture = ResourceLoader.Load<Texture2D>("Assets/Blorbo-Shady.png");
		subtitles.Text = "As you look around, a figure approaches...";
		tween = CreateTween();
		tween.TweenProperty(window, "modulate", Colors.Black, 2);
		await ToSignal(tween, Tween.SignalName.Finished);
		tween = CreateTween();
		tween.TweenProperty(window, "modulate", Colors.White, 2);
		await ToSignal(tween, Tween.SignalName.Finished);

		window.Texture = ResourceLoader.Load<Texture2D>("Assets/Blorbo-Happy.png");
		subtitles.Text = "[color=#BAFAFA]Bl[wave]orb[/wave]o:[/color] Why hello there! I'm Bl[wave]orb[/wave]o! I assume this hat I found is yours?";
		tween = CreateTween();
		tween.TweenProperty(window, "modulate", Colors.White, 5);
		await ToSignal(tween, Tween.SignalName.Finished);

		window.Texture = ResourceLoader.Load<Texture2D>("Assets/Oh Shit A Goblin.png");
		subtitles.Text = "[color=#BAFAFA]Bl[wave]orb[/wave]o:[/color] Oh shit a Goblin! You're on your own, pal, I gotta skedaddle!";
		tween = CreateTween();
		tween.TweenProperty(window, "modulate", Colors.White, 5);
		await ToSignal(tween, Tween.SignalName.Finished);

		start_game();

	}

	public async void start_game()
	{
		Tween fade_out_tween = CreateTween();
		fade_out_tween.TweenProperty(GetNode<ColorRect>("Vignette"), "color:a", 1f, 1f);


		PackedScene next = GD.Load<PackedScene>("Arena.tscn");

		await ToSignal(fade_out_tween, Tween.SignalName.Finished);

		GetTree().ChangeSceneToPacked(next);
	}
}
