using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class Funcs : Node
{
    public static void damage_number(Node parent, Vector2 position, int damage)
    {
        PackedScene effect_scene = ResourceLoader.Load<PackedScene>("number_effect.tscn");
        NumberEffect effect = effect_scene.Instantiate<NumberEffect>();
        effect.Duration = 1.0f;
        effect.Text = damage.ToString();
        effect.Position = position;
        parent.AddChild(effect);
    }

}