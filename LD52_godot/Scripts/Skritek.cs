using System;
using Godot;
public class Skritek : Node2D
{
    public event Action<Vector2> OnSkritekMoved = delegate { };
    public event Action OnSkritekCaught = delegate { };
    public event Action<bool> OnSkritekHide = delegate { };

    private PackedScene projectileScene;
    private AnimatedSprite animatedSprite;
    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        projectileScene = GD.Load<PackedScene>("res://Scenes/Projectile.tscn");

        var area = GetNode<Area2D>("CollisionArea");
        if (area is null)
            return;

        area.Connect("area_entered", this, "OnAreaEntered");
        area.Connect("area_exited", this, "OnAreaExit");
    }

    public void OnAreaEntered(Node collider)
    {
        if (collider?.GetParent() is Bush)
            OnSkritekHide(true);

        if (collider?.GetParent() is Woodcutter)
            OnSkritekCaught();

    }

    public void OnAreaExit(Node collider)
    {
        if (collider?.GetParent() is Bush)
            OnSkritekHide(false);
    }



    public override void _Process(float delta)
    {
        Vector2 moveDirection = new Vector2(0, 0);
        if (Input.IsKeyPressed((int)KeyList.W))
            moveDirection.y--;
        if (Input.IsKeyPressed((int)KeyList.S))
            moveDirection.y++;
        if (Input.IsKeyPressed((int)KeyList.A))
            moveDirection.x--;
        if (Input.IsKeyPressed((int)KeyList.D))
            moveDirection.x++;

        Rotation = (GetGlobalMousePosition() - GlobalPosition).Angle();
        if (moveDirection.x == 0 && moveDirection.y == 0)
        {
            animatedSprite.Stop();
            return;
        }

        Position += moveDirection.Normalized() * 3;

        OnSkritekMoved(Position);
        animatedSprite.Play("run");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!(@event is InputEventMouseButton mouseEvent))
            return;
        if (mouseEvent.ButtonIndex != (int)ButtonList.Left || !mouseEvent.Pressed)
            return;
        if (projectileScene is null)
            return;
        Projectile projectile = (Projectile)projectileScene.Instance();
        if (projectile is null)
            return;

        projectile.Rotation = Rotation;
        projectile.Position = Position;
        GetParent()?.AddChild(projectile);
        GetTree()?.SetInputAsHandled();
    }
}