using Godot;
using System;

public partial class SpaceShop : VBoxContainer
{
	public String ability;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();
		ability = Global.Instance.space_abilities[rng.RandiRange(0, Global.Instance.space_abilities.Count-1)];
		GetNode<Label>("Name").Text = ability;
		GetNode<TextureRect>("TextureRect").Texture = ResourceLoader.Load<Texture2D>("res://Icons/Space_Abilities/" + ability + ".png");
		GetNode<TextureRect>("TextureRect").TooltipText = "Replaces current space ability with:\n" + ability;
		GetNode<Label>("Price").Text = (rng.RandiRange(2, 3) * (1 + Global.Instance.wave)).ToString();
	}

	public void get_ability()
	{
		if (Global.Instance.money >= GetNode<Label>("Price").Text.ToInt())
		{
			Global.Instance.space_ability = ability;
			Global.Instance.money -= GetNode<Label>("Price").Text.ToInt();
			GetNode<Button>("Equip").Disabled = true;
			GetNode<Button>("Equip").Text = "Ability Purchased!";
		}
		else
		{
			GetNode<Button>("Equip").Disabled = true;
			GetNode<Button>("Equip").Text = "CARD DECLINED";
		}
	}
}
