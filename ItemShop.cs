using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class ItemShop : HBoxContainer
{
	public Array<String> items;
	public Array<int> item_indices = [0,0,0];
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();
		items = Global.Instance.items.Duplicate();
		items.Shuffle();
		for(int i = 0; i < 3; i++)
		{
			item_indices[i] = Global.Instance.items.IndexOf(items[i]);
			Control item_slot = GetNode<Control>("Item" + (i+1).ToString());
			item_slot.GetNode<Label>("Name").Text = items[i];
			item_slot.GetNode<TextureRect>("TextureRect").TooltipText = Global.Instance.item_descriptions[item_indices[i]];
			item_slot.GetNode<Label>("Price").Text = rng.RandiRange(100, 250).ToString();
			item_slot.GetNode<TextureRect>("TextureRect").Texture = ResourceLoader.Load<Texture2D>("res://Icons/Items/" + items[i] + ".png");
		}
	}

    public override void _Process(double delta)
    {
        base._Process(delta);
		foreach(Node child in GetParent().GetNode("Current Items").GetChildren())
		{
			if (child is TextureRect){child.QueueFree();}
		}
		for (int i = 0; i < Global.Instance.current_items.Count; i++)
		{
			TextureRect item_icon = new TextureRect();
			item_icon.Texture = ResourceLoader.Load<Texture2D>("Icons/Items/" + Global.Instance.current_items[i] + ".png");
			GetParent().GetNode("Current Items").AddChild(item_icon);
		}
    }

	public void buy_item_1()
	{
		Node item = GetNode("Item1");
		if (Global.Instance.money >= GetNode("Item1").GetNode<Label>("Price").Text.ToInt())
		{
			Global.Instance.current_items.Add(item.GetNode<Label>("Name").Text);
			Global.Instance.money -= item.GetNode<Label>("Price").Text.ToInt();
			item.GetNode<Button>("Equip").Disabled = true;
			item.GetNode<Button>("Equip").Text = "Purchased!";
		}
		else
		{
			item.GetNode<Button>("Equip").Disabled = true;
			item.GetNode<Button>("Equip").Text = "CARD DECLINED";
		}
	}

	public void buy_item_2()
	{
		Node item = GetNode("Item2");
		if (Global.Instance.money >= GetNode("Item2").GetNode<Label>("Price").Text.ToInt())
		{
			Global.Instance.current_items.Add(item.GetNode<Label>("Name").Text);
			Global.Instance.money -= item.GetNode<Label>("Price").Text.ToInt();
			item.GetNode<Button>("Equip").Disabled = true;
			item.GetNode<Button>("Equip").Text = "Purchased!";
		}
		else
		{
			item.GetNode<Button>("Equip").Disabled = true;
			item.GetNode<Button>("Equip").Text = "CARD DECLINED";
		}
	}

	public void buy_item_3()
	{
		Node item = GetNode("Item3");
		if (Global.Instance.money >= GetNode("Item3").GetNode<Label>("Price").Text.ToInt())
		{
			Global.Instance.current_items.Add(item.GetNode<Label>("Name").Text);
			Global.Instance.money -= item.GetNode<Label>("Price").Text.ToInt();
			item.GetNode<Button>("Equip").Disabled = true;
			item.GetNode<Button>("Equip").Text = "Purchased!";
		}
		else
		{
			item.GetNode<Button>("Equip").Disabled = true;
			item.GetNode<Button>("Equip").Text = "CARD DECLINED";
		}
	}
}
