using Godot;
using Godot.Collections;
using System;

public partial class Border : Area2D
{
	public AudioStreamPlayer audio;
	public Array<AudioStreamWav> tracks = [];
	public AudioStreamWav finish;
	public RandomNumberGenerator rng = new RandomNumberGenerator();
	private bool mobs_defeated = false;
    public override void _Ready()
    {
        base._Ready();
		audio = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		audio.Play();
		tracks.Add(ResourceLoader.Load<AudioStreamWav>("Music and Sound/OhNo!Goblins!A.wav"));
		tracks.Add(ResourceLoader.Load<AudioStreamWav>("Music and Sound/OhNo!Goblins!B.wav"));
		finish = ResourceLoader.Load<AudioStreamWav>("Music and Sound/OhNo!Goblins!Outro.wav");
    }

	public void _on_body_exited(Node2D body)
	{
		if (body is Alive alive)
		{
			alive.die();
		}
	}

	public void _on_all_mobs_defeated()
	{
		mobs_defeated = true;
	}

	public void _on_audio_finished()
	{
		if (mobs_defeated && audio.Stream == finish) {}
		
		else if (mobs_defeated)
		{
			audio.Stream = finish;
			audio.Play();
		}
		else 
		{
			audio.Stream = tracks[rng.RandiRange(0, tracks.Count-1)];
			audio.Play();
		}
	}
	
}
