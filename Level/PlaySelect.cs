using Godot;
using System;

public class PlaySelect : Node
{
    public void play()
    {
        SoundController.instance.play("Select");
    }
}
