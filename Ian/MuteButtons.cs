using Godot;
using System;

public class MuteButtons : HBoxContainer
{
    public void toggleMusicMute(bool val)
    {
        AudioServer.SetBusMute(0, val);
    }

    public void toggleEffectsMute(bool val)
    {
        AudioServer.SetBusMute(1, val);
    }
}
