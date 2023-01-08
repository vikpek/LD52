using Godot;
using LD52.Scripts;
public abstract class Projectile : Node2D
{


    private float distanceTravelled;
    private Global global;
    public override void _Ready()
    {
        global = (Global)GetNode("/root/Global");

        var area = GetNode<Area2D>("CollisionArea");
        if (area is null)
            return;

        area.Connect("area_entered", this, "OnCollision");
        area.Connect("body_entered", this, "OnCollision");
    }

    public override void _Process(float delta)
    {
        float moveAmount = delta * 300;
        Position += Transform.x.Normalized() * moveAmount;
        distanceTravelled += moveAmount;
        if (distanceTravelled > 200)
            QueueFree();
    }

    public void OnCollision(Node collider)
    {
        if (collider.GetParent() is Woodcutter woodcutter)
        {
            global.PlaySound(GameConfig.SFXHit);
            QueueFree();
        }

        if (collider.GetParent() is ActualTree actualTree)
        {
            global.PlaySound(GameConfig.SFXHitMiss);
            QueueFree();
        }

        if (collider.GetParent() is Hut hut)
        {
            global.PlaySound(GameConfig.SFXHit);
            hut.AddDamage();
            QueueFree();
        }
    }
}