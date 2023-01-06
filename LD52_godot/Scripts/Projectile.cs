using Godot;
public abstract class Projectile : Node2D
{
    private float distanceTravelled;
    public override void _Ready()
    {
        var area = GetNode<Area2D>("CollisionArea");
        if (area is null)
            return;

        area.Connect("area_entered", this, "OnCollision");
        area.Connect("body_entered", this, "OnCollision");
    }

    public override void _Process(float delta)
    {
        float moveAmount = delta * 500;
        Position += Transform.x.Normalized() * moveAmount;
        distanceTravelled += moveAmount;
        if (distanceTravelled > 10000)
            QueueFree();
    }

    public void OnCollision(Node collider) => QueueFree();
}