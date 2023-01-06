using Godot;
public class Player : Node2D
{
    float speed = 150;

    public override void _Ready()
    {

    }


    public override void _Process(float delta)
    {
        float moveDistance = speed * delta;
        Vector2 moveDirection = new Vector2(0, 0);
        if (Input.IsKeyPressed((int)KeyList.W))
            moveDirection.y = -moveDistance;
        if (Input.IsKeyPressed((int)KeyList.S))
            moveDirection.y = moveDistance;
        if (Input.IsKeyPressed((int)KeyList.A))
            moveDirection.x = -moveDistance;
        if (Input.IsKeyPressed((int)KeyList.D))
            moveDirection.x = moveDistance;

        Position += moveDirection;
        Rotation = (GetGlobalMousePosition() - GlobalPosition).Angle();
    }
}