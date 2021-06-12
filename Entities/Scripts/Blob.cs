using Godot;
using System;

public class Blob : Node2D
{
    [Export]
    public Texture[] elementTextures;
    [Export]
    public Texture[] elementParticles;
    
    public BlobData data;

    private Sprite sprite;
    private Particles2D particles;

    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
        particles = GetNode<Particles2D>("Particles");
        sprite.Texture = elementTextures[(int)data.element];
        if(data.element != BlobElement.STONE)
        {
            particles.Visible = true;
            particles.Texture = elementParticles[(int)data.element];
        }
        Position = data.position * 16;
    }

    public void Move(Vector2 dir)
    {
        Position = data.position * 16;
    }
}

