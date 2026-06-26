using Godot;
using System;

public partial class OrbShop : VBoxContainer
{
	public String orb;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();
		orb = Global.Instance.orbs[rng.RandiRange(0, Global.Instance.orbs.Count-1)];
		GetNode<Label>("Name").Text = orb;
		GetNode<TextureRect>("TextureRect").Texture = ResourceLoader.Load<Texture2D>("res://Icons/Orbs/" + orb + ".png");
		GetNode<TextureRect>("TextureRect").TooltipText = "Replaces current orb with:\n" + orb + " Orb";
		GetNode<Label>("Price").Text = (rng.RandiRange(2, 3) * (1 + Global.Instance.wave)).ToString();
	}

	public void get_orb()
	{
		if (Global.Instance.money >= GetNode<Label>("Price").Text.ToInt())
		{
			Global.Instance.current_orb = orb;
			Global.Instance.money -= GetNode<Label>("Price").Text.ToInt();
			GetNode<Button>("Equip").Disabled = true;
			GetNode<Button>("Equip").Text = "Orb Purchased!";
		}
		else
		{
			GetNode<Button>("Equip").Disabled = true;
			GetNode<Button>("Equip").Text = "CARD DECLINED";
		}
	}
}
