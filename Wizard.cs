using Godot;
using Godot.Collections;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

public partial class Wizard : RigidBody2D, Alive
{
	[Export]
	public float Speed = 600.0f;
	public float Acceleration = 3000f;
	[Export]
	public Array<String> powers;
	public String orb_type = "Normal";
	public float health = 100;
	private RigidBody2D Orb;
	private Rope rope;
	public Control UI;
	private TextureProgressBar shift_ability;
	private TextureProgressBar space_ability;

	private float shake_decay_rate = 200f;
	private float shake_speed = 100f;
	private float shake_strength = 100f;
	private float noise_i = 0.0f;
	private float shake_velocity = 0.0f;
	private Camera2D camera;
	private FastNoiseLite noise = new FastNoiseLite();


	public void Damage(float damage)
	{
		Global.Instance.health -= damage;
		UI.GetNode<ProgressBar>("Health").Value = Global.Instance.health;
		if (Global.Instance.health <= 0)
		{
			die();
		}
	}

    public override void _Ready()
    {
        base._Ready(); //Maybe best to leave this in here
		Orb = GetParent().GetNode<RigidBody2D>("ORB");
		rope = GetParent().GetNode<Rope>("Rope");
		rope.create_rope(10, this.GetPath(), Orb.GetPath(), Position, Orb.Position);
		UI = GetNode("CanvasLayer").GetNode<Control>("UI");
		UI.GetNode<ProgressBar>("Health").Value = Global.Instance.health;
		shift_ability = UI.GetNode("Abilities").GetNode<TextureProgressBar>("shift_ability");
		space_ability = UI.GetNode("Abilities").GetNode<TextureProgressBar>("space_ability");
		shift_ability.TextureUnder = ResourceLoader.Load<Texture2D>("Icons/Shift_Abilities/" + Global.Instance.shift_ability + ".png");
		space_ability.TextureUnder = ResourceLoader.Load<Texture2D>("Icons/Space_Abilities/" + Global.Instance.space_ability + ".png");
		shift_ability.MaxValue = 5 - Global.Instance.current_items.Count(o => o == "More Magic!");
		space_ability.MaxValue = 5 - Global.Instance.current_items.Count(o => o == "Magickify Orb");
		shift_ability.TooltipText = Global.Instance.shift_ability;
		space_ability.TooltipText = Global.Instance.space_ability;
		if (Global.Instance.space_ability == "Attract Orb") {Orb.GetNode<Sprite2D>("AreaVisual").Visible = true;}
		for (int i = 0; i < Global.Instance.current_items.Count; i++)
		{
			TextureRect item_icon = new TextureRect();
			item_icon.Texture = ResourceLoader.Load<Texture2D>("Icons/Items/" + Global.Instance.current_items[i] + ".png");
			item_icon.TooltipText = Global.Instance.current_items[i] + "\n" + Global.Instance.item_descriptions[Global.Instance.items.IndexOf(Global.Instance.current_items[i])];
			UI.GetNode<Control>("Items").AddChild(item_icon);
		}
		camera = GetNode<Camera2D>("Camera2D");
		
	}

	public async void die()
	{
		Tween fade_out_tween = CreateTween();
		fade_out_tween.TweenProperty(UI.GetNode<ColorRect>("Vignette"), "color:a", 1f, 1f);

		PackedScene next = GD.Load<PackedScene>("death.tscn");

		await ToSignal(fade_out_tween, Tween.SignalName.Finished);

		GetTree().ChangeSceneToPacked(next);
	}

	public void shake_screen()
	{
		shake_velocity = shake_strength;
	}

    public override void _Process(double delta) // For visual stuff
    {
        base._Process(delta);
		UI.GetNode<Label>("Money").Text = Global.Instance.money.ToString() + " Dollaridoos";
		camera.Offset = new Vector2(noise.GetNoise2D(1, noise_i), noise.GetNoise2D(100, noise_i)) * shake_velocity;
		noise_i += (float)delta*shake_speed;
		shake_velocity = Mathf.MoveToward(shake_velocity, 0, shake_decay_rate * (float)delta);
    }

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("left", "right", "up", "down");

		ApplyCentralForce(direction*Acceleration*Mass);

		if(LinearVelocity.Length() >= Speed)
		{
			LinearVelocity -= LinearVelocity.Normalized() * Acceleration * (float)delta;
		}
		if (Global.Instance.shift_ability != "None")
		{
			shift_ability.Value -= delta;
			space_ability.Value -= delta;
		}
		//GD.Print(Input.IsActionPressed("shift"));
		if (shift_ability.Value <= 0 && Input.IsActionPressed("shift"))
		{
			shift_ability.Value = shift_ability.MaxValue;
			switch (Global.Instance.shift_ability)
			{
				case "Dash Away":
				ApplyCentralImpulse(-Mass*Speed*4*((Orb.Position - Position).Normalized()));
				break;
				case "Dash Toward":
				ApplyCentralImpulse(Mass*Speed*8*((Orb.Position - Position).Normalized()));
				break;
				case "Shield":
				Array<Node2D> entities = GetNode<Area2D>("Shield").GetOverlappingBodies();
				GetNode<Sprite2D>("Shield Visual").Modulate = Colors.White;
				Tween tween = CreateTween();
				tween.TweenProperty(GetNode<Sprite2D>("Shield Visual"), "modulate", new Color(1,1,1,0), 1);
				{
					
				};
				for (int i = 0; i < entities.Count; i++)
				{
					if (entities[i] is RigidBody2D entity && ! (entities[i] is Wizard))
					{
						entity.LinearVelocity = (entity.Position - this.Position).Normalized() * 1000;
					}
				}
				break;
			}
		}
		if (space_ability.Value <= 0 && Input.IsActionPressed("space"))
		{
			space_ability.Value = space_ability.MaxValue;
			switch (Global.Instance.space_ability)
			{
				case "Attract Orb":
				Array<Node2D> entities = Orb.GetNode<Area2D>("Attract").GetOverlappingBodies();
				for (int i = 0; i < entities.Count; i++)
				{
					if (entities[i] is RigidBody2D entity && ! (entities[i] is Orb))
					{
						entity.LinearVelocity = -(entity.Position - Orb.Position).Normalized() * 1000;
					}
				}
				break;
				case "Spin Orb":
				Orb.LinearVelocity += (Orb.Position - Position).Rotated(Mathf.Pi/2).Normalized() * 1000;
				break;
			}
		}
	}
}
