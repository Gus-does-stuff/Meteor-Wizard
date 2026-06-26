using Godot;
using System;
using System.ComponentModel;
using System.Linq;

public abstract partial class Mob : RigidBody2D, Alive
{
	[Export]
	public float health = 1;
	[Export]
	public int damage = 1;
    public int money_drop = 1;
	public Wizard wizard;
    public abstract void Damage(float damage, Vector2 velocity);
    public void hit(Node body)
    {
		if (body is Wizard)
		{
			wizard.Damage(damage);
            ApplyCentralImpulse((wizard.Position - Position).Normalized() * -500);
		}
        if (body is Mob mob)
        {
            mob.Damage(1, LinearVelocity);
            ApplyCentralImpulse((mob.Position - Position).Normalized() * -100);
        }
        Global.Instance.emphasis_shake(GetTree());
        wizard.shake_screen();
    }

    public void die()
    {
        this.QueueFree(); // Add potential animation and sound here
        Global.Instance.money += money_drop + Global.Instance.current_items.Count(o=>o=="Hemonomics");
    }
	public override void _Ready()
	{
		wizard = GetParent().GetNode<Wizard>("Wizard");
        BodyEntered += hit;
        ContactMonitor = true;
        MaxContactsReported = 5;
	}


}
