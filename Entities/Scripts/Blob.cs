using Godot;
using System;

public class Blob : Node2D
{
    [Export]
    public Texture[] elementTextures;
    [Export]
    public Texture[] playerElementTextures;
    [Export]
    public Texture[] elementParticles;
    
    public BlobData data;

    private Sprite sprite;
    private Particles2D particles;

    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
        particles = GetNode<Particles2D>("Particles");
        SetTexture((int)data.element);
        SetParticles((int)data.element);
        Position = data.position * 16;
    }

    public void ChangeElement(BlobElement element)
    {
        data.element = element;

        if (element is BlobElement.ICE)
            data.isPlayer = false;

        if (element != BlobElement.BOX_BURNING)
        {
            SetTexture((int)element);
            SetParticles((int)element);
        }
        else
        {
            SetParticles((int)BlobElement.FIRE);
        }
    }
    
    public void SetTexture(int elementId)
    {
        if(data.isPlayer)
            sprite.Texture = playerElementTextures[(int)data.element];
        else
            sprite.Texture = elementTextures[(int)data.element];
    }

    public void SetParticles(int elementId)
    {
        if (data.element != BlobElement.STONE)
        {
            particles.Visible = true;
            particles.Texture = elementParticles[elementId];
        }
    }

    public void Move(Vector2 dir)
    {
        Position = data.position * 16;
    }

    public void die()
    {

        QueueFree();
    }
}

