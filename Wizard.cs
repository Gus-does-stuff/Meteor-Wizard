using Godot;
using Godot.Collections;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

public partial class Wizard : CharacterBody2D, Alive
{
	[Export]
	public float Speed = 300.0f;
	[Export]
	public Array<String> powers;
	public String orb_type = "Normal";
	public float health = 100;
	private RigidBody2D Orb;
	private Rope rope;
	private Control UI;
	private TextureProgressBar shift_ability;
	private TextureProgressBar space_ability;

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
		UI.GetNode("Abilities").GetNode<TextureProgressBar>("shift_ability").TextureUnder = ResourceLoader.Load<Texture2D>("Icons/Shift_Abilities/" + Global.Instance.shift_ability + ".png");
		UI.GetNode("Abilities").GetNode<TextureProgressBar>("space_ability").TextureUnder = ResourceLoader.Load<Texture2D>("Icons/Space_Abilities/" + Global.Instance.space_ability + ".png");
		shift_ability.MaxValue = 5 - Global.Instance.current_items.Count(o => o == "More Magic!");
		space_ability.MaxValue = 5 - Global.Instance.current_items.Count(o => o == "Magickify Orb");
		for (int i = 0; i < Global.Instance.current_items.Count; i++)
		{
			TextureRect item_icon = new TextureRect();
			item_icon.Texture = ResourceLoader.Load<Texture2D>("Icons/Items/" + Global.Instance.current_items[i] + ".png");
			item_icon.TooltipText = Global.Instance.current_items[i] + "\n" + Global.Instance.item_descriptions[Global.Instance.items.IndexOf(Global.Instance.current_items[i])];
			UI.GetNode<Control>("Items").AddChild(item_icon);
		}
		
	}

	public async void die()
	{
		Tween fade_out_tween = CreateTween();
		fade_out_tween.TweenProperty(UI.GetNode<ColorRect>("Vignette"), "color:a", 1f, 1f);

		PackedScene next = GD.Load<PackedScene>("death.tscn");

		await ToSignal(fade_out_tween, Tween.SignalName.Finished);

		GetTree().ChangeSceneToPacked(next);
	}

    public override void _Process(double delta) // For visual stuff
    {
        base._Process(delta);
		UI.GetNode<Label>("Money").Text = Global.Instance.money.ToString() + " Dollaridoos";
    }

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		Vector2 direction = Input.GetVector("left", "right", "up", "down");

		if (velocity.Length() > Speed * Mathf.Sqrt2)
		{
			velocity = direction*Speed - velocity.Normalized() * Speed * (float)delta;
		}
		else if (direction != Vector2.Zero)
		{
			velocity = direction * Speed;
		}
		else
		{
			velocity -= velocity.Normalized() * Speed * (float)delta;
		}

		Velocity = velocity;
		
		shift_ability.Value -= delta;
		space_ability.Value -= delta;
		//GD.Print(Input.IsActionPressed("shift"));
		if (shift_ability.Value <= 0 && Input.IsActionPressed("shift"))
		{
			shift_ability.Value = shift_ability.MaxValue;
			switch (Global.Instance.shift_ability)
			{
				case "Dash Away":
				Velocity = -Speed*4*((Orb.Position - Position).Normalized());
				break;
				case "Dash Toward":
				Velocity = Speed*8*((Orb.Position - Position).Normalized());
				break;
				case "Shield":
				Array<Node2D> entities = GetNode<Area2D>("Shield").GetOverlappingBodies();
				for (int i = 0; i < entities.Count; i++)
				{
					if (entities[i] is RigidBody2D entity)
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
					if (entities[i] is RigidBody2D entity)
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
		MoveAndSlide();
	}
}
