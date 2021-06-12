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

        switch (element)
        {
            case BlobElement.BOX_BURNING:
                SetParticles((int)BlobElement.FIRE);
                break;
            case BlobElement.GRASS_BURNING:
                SetParticles((int)BlobElement.FIRE);
                break;
            case BlobElement.NEW_ICE:
            case BlobElement.ICE:
                data.isPlayer = false;
                SetTexture((int)BlobElement.ICE);
                SetParticles((int)BlobElement.ICE);
                break;
            default:
                SetTexture((int)element);
                SetParticles((int)element);
                break;
        }
    }
    
    public void SetTexture(int elementId)
    {
        if(data.isPlayer)
            sprite.Texture = playerElementTextures[elementId];
        else
            sprite.Texture = elementTextures[elementId];
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

