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
	}

	public void get_ability()
	{
		Global.Instance.space_ability = ability;
		GetNode<Button>("Equip").Disabled = true;
		GetNode<Button>("Equip").Text = "Equipped!";
	}
}
