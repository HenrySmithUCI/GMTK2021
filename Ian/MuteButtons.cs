using Godot;
using System;

public class MuteButtons : HBoxContainer
{
    public override void _Ready()
    {
        GetNode<CheckBox>("EffectsMute").Pressed = AudioServer.IsBusMute(1);
    }

    public void toggleMusicMute(bool val)
    {
        AudioServer.SetBusMute(2, val);
    }

    public void toggleEffectsMute(bool val)
    {
        AudioServer.SetBusMute(1, val);
    }
}
