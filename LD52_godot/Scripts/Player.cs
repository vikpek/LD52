using Godot;
public class Player : Node2D
{
    private PackedScene projectileScene;
    public override void _Ready() => projectileScene = GD.Load<PackedScene>("res://Scenes/Projectile.tscn");
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

        Position += moveDirection.Normalized() * 3;
        Rotation = (GetGlobalMousePosition() - GlobalPosition).Angle();
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