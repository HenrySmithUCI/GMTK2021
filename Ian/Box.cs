using Godot;
using System;

public class Box : Sprite
{
    String[] textures = { "res://Ian/Box_Player.png",
                          "res://Ian/Box.png"};

    public void setTexture(int id)
    {
        Texture = (Texture)GD.Load(textures[id]);
    }
}
