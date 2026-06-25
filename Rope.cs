using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class Rope : Node2D
{
	public Array<NodePath> segments;
	public Line2D line;
	public void create_rope(int length, NodePath body1, NodePath body2, Vector2 start, Vector2 end)
	{
		line = GetNode<Line2D>("Line");
		Shape2D segment_shape = ResourceLoader.Load<Shape2D>("rope-segment.tres");
		
		Vector2 segment_size = (end-start)/length;

		segments = [body1];

		for (int i = 0; i < length; i++)
		{
			RigidBody2D segment_i = new RigidBody2D();
			segment_i.Position = start + segment_size*i + segment_size/2;
			segment_i.GravityScale = 0;
			segment_i.Name = "Segment " + i.ToString();
			segment_i.CollisionLayer = 2;
			segment_i.CollisionMask = 2;
			segment_i.Mass = 0.1f;
			AddChild(segment_i);
			segments.Add(segment_i.GetPath());
			CollisionShape2D segment_i_shape = new CollisionShape2D();
			segment_i_shape.Shape = segment_shape;
			segment_i.AddChild(segment_i_shape);
			//Line2D segment_i_line = new Line2D();
			//segment_i_line.Points = [-segment_size/2, segment_size/2];
			//egment_i.AddChild(segment_i_line);
		}
		
		segments.Add(body2);


		line.AddPoint(GetNode<Node2D>(segments[0]).Position);
		for(int i = 0; i < segments.Count - 1; i++)
		{
			line.AddPoint(GetNode<Node2D>(segments[i+1]).Position);
			PinJoint2D joint = new PinJoint2D();
			joint.Position = start + i * segment_size;
			joint.NodeA = segments[i];
			joint.NodeB = segments[i+1];
			AddChild(joint);
		}
	}

    public override void _Process(double delta)
    {
        base._Process(delta);
		line.Points = [];
		line.AddPoint(GetNode<Node2D>(segments[0]).Position);
		for(int i = 0; i < segments.Count - 1; i++)
		{
			line.AddPoint(GetNode<Node2D>(segments[i+1]).Position);
		}
    }

}
