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
    [Export]
    public Color[] deathColors;
    
    public BlobData data;

    private Sprite sprite;
    private Particles2D particles;
    private Particles2D halo;
    private Tween tween;

    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
        particles = GetNode<Particles2D>("Particles");
        halo = GetNode<Particles2D>("Halo");
        tween = GetNode<Tween>("Tween");
        ChangeElement(data.element);
        Position = data.position * 16;
    }

    public void ChangeElement(BlobElement element)
    {
        data.element = element;

        switch (element)
        {
            case BlobElement.BOX_BURNING_INIT:
            case BlobElement.BOX_BURNING:
                SetTexture((int)BlobElement.BOX);
                SetParticles((int)BlobElement.FIRE);
                break;
            case BlobElement.GRASS_BURNING_INIT:
            case BlobElement.GRASS_BURNING:
                SetTexture((int)BlobElement.GRASS);
                SetParticles((int)BlobElement.FIRE);
                break;
            case BlobElement.NEW_ICE:
            case BlobElement.ICE:
                data.isPlayer = false;
                SetTexture((int)BlobElement.ICE);
                SetParticles((int)BlobElement.ICE);
                break;
            case BlobElement.PRE_WATER:
                SetTexture((int)BlobElement.WATER);
                SetParticles((int)BlobElement.WATER);
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
        tween.InterpolateProperty(this, "position", Position, data.position * 16, 0.1f, (Godot.Tween.TransitionType)1);
        tween.Start();
        if(data.level.getTile(data.position).type == TileType.VICTORY_PLAYER && data.isPlayer)
        {
            halo.Emitting = true;
        }
        else if(data.level.getTile(data.position).type == TileType.VICTORY && data.isPlayer == false)
        {
            halo.Emitting = true;
        }
        else
        {
            halo.Emitting = false;
        }
    }

    public void die()
    {
        Particles2D dp = GetNode<Particles2D>("DeathParticles");
        dp.Modulate = getParticleColor(data.element);
        dp.Emitting = true;
        particles.Emitting = false;
        GetNode<Sprite>("Sprite").Visible = false;
        GetNode<Timer>("Timer").Start();
    }

    private Color getParticleColor(BlobElement element)
    {
        switch(element)
        {
            case BlobElement.BOX_BURNING:
            case BlobElement.BOX_BURNING_INIT:
                return deathColors[(int)BlobElement.BOX];
            case BlobElement.GRASS_BURNING:
            case BlobElement.GRASS_BURNING_INIT:
                return deathColors[(int)BlobElement.GRASS];
            case BlobElement.NEW_ICE:
                return deathColors[(int)BlobElement.ICE];
            default:
                return deathColors[(int)element];
        }
    }

    public void selfDelete()
    {
        QueueFree();
    }
}

