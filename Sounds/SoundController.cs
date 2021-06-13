using Godot;
using System;
using System.Collections.Generic;

public class SoundController : AudioStreamPlayer
{
    public static SoundController instance;
    public Dictionary<string,AudioStreamPlayer> sounds;

    public override void _Ready()
    {
        instance = this;
        sounds = new Dictionary<string, AudioStreamPlayer>();
        sounds["theme"] = this;
        foreach(AudioStreamPlayer child in GetChildren())
        {
            sounds[child.Name] = child;
        }
    }

    public void play(string sound)
    {
        sounds[sound].Playing = true;
    }
}
