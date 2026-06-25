using Godot;
using System;
using System.Linq;

public partial class Orb : RigidBody2D
{

	private Wizard wizard;
	public float base_damage = 1;
	public float base_speed = 1000;
	public float knockback_multiplier = 1;
	public float speed = 1000;
	public Line2D trail;

    public override void _Ready()
    {
        base._Ready();
		wizard = GetParent().GetNode<Wizard>("Wizard");
		set_type(Global.Instance.current_orb);
		speed = base_speed * (1 + 0.2f*Global.Instance.current_items.Count(o => o == "Aerodynamics"));
		((CircleShape2D)GetNode<CollisionShape2D>("Hitbox").Shape).Radius = 75*(1 + 0.2f*Global.Instance.current_items.Count(o=>o=="Big Orb"));
		GetNode<Sprite2D>("Sprite2D").Scale = Vector2.One * 1.3f * (1 + 0.2f*Global.Instance.current_items.Count(o=>o=="Big Orb"));
		trail = GetNode<Line2D>("Trail");
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		if(LinearVelocity.Length() >= speed)
		{
			LinearVelocity = LinearVelocity.Normalized() * speed;
		}
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
		trail.Position = -GlobalPosition.Rotated(-Rotation);
		trail.Rotation = -Rotation;
		trail.AddPoint(Position);
		if (trail.Points.Length > 100)
		{
			trail.RemovePoint(0);
		}
    }

	public void _on_slam(Node body)
	{
		if (body is Mob mob)
		{
			mob.Damage((1 + Global.Instance.current_items.Count(o => o == "Prickly Orb")*0.2f)*base_damage*Mathf.Pow((LinearVelocity - mob.LinearVelocity).Length(), 2)*Mass/50000, LinearVelocity);
			
		}
		if (body is Wizard)
		{
			ApplyCentralImpulse((Position - wizard.Position).Normalized() * 1000 * Global.Instance.current_items.Count(o=>o=="Lead Tipped Boots"));
		}
	}

	public void set_type(String orb_type)
	{
		if(orb_type == "Normal")
		{
			PhysicsMaterialOverride.Bounce = 2f;
			Mass = 3f;
			base_speed = 500f;
			knockback_multiplier = 1;
			GetNode<Sprite2D>("Sprite2D").Texture = ResourceLoader.Load<Texture2D>("Assets/Normal Orb.png");
		}
		if(orb_type == "Bouncy")
		{
			PhysicsMaterialOverride.Bounce = 20f;
			Mass = 3f;
			base_speed = 500f;
			knockback_multiplier = 1;
			GetNode<Sprite2D>("Sprite2D").Texture = ResourceLoader.Load<Texture2D>("Assets/Boundy Orb.png");

		}
		if(orb_type == "Massive")
		{
			PhysicsMaterialOverride.Bounce = 2f;
			Mass = 20f;
			base_speed = 500f;
			knockback_multiplier = 1f;
			GetNode<Sprite2D>("Sprite2D").Texture = ResourceLoader.Load<Texture2D>("Assets/Massive Orb.png");
		}
		if(orb_type == "Fast")
		{
			PhysicsMaterialOverride.Bounce = 2f;
			Mass = 3f;
			base_speed = 5000f;
			knockback_multiplier = 1f;
			GetNode<Sprite2D>("Sprite2D").Texture = ResourceLoader.Load<Texture2D>("Assets/Fast Orb.png");
		}
		else
		{
			GD.Print("Orb type does not exist lmao");
		}
	}

}
