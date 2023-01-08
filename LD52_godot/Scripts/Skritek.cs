using System;
using Godot;
using LD52.Scripts;
public class Skritek : Node2D
{
    public event Action<Vector2> OnSkritekMoved = delegate { };
    public event Action OnSkritekCaught = delegate { };
    public event Action<bool> OnSkritekHide = delegate { };

    private PackedScene projectileScene;
    private AnimatedSprite animatedSprite;
    private Label shout;
    private bool setBack = false;

    private AmmoService ammoService;
    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        projectileScene = GD.Load<PackedScene>("res://Scenes/Projectile.tscn");
        shout = GetNode<Label>("Shout");

        var area = GetNode<Area2D>("CollisionArea");
        if (area is null)
            return;

        area.Connect("area_entered", this, "OnAreaEntered");
        area.Connect("area_exited", this, "OnAreaExit");
    }

    public void Initialize(AmmoService ammoServ)
    {
        this.ammoService = ammoServ;
    }
    public void OnAreaEntered(Node collider)
    {
        if (collider?.GetParent() is Bush)
            OnSkritekHide(true);

        if (collider?.GetParent() is Woodcutter)
            OnSkritekCaught();

        if (collider?.GetParent() is ActualTree ||
            collider?.GetParent() is Game ||
            collider?.GetParent() is Hut)
        {
            RanIntoObstacle();
        }
    }

    public void OnAreaExit(Node collider)
    {
        if (collider?.GetParent() is Bush)
            OnSkritekHide(false);
    }



    private Vector2 moveDirection;
    private Vector2 velocity;
    private Vector2 previousPosition;
    public override void _Process(float delta)
    {
        if (setBack)
        {
            Position += -velocity.Normalized();
            return;
        }

        moveDirection = new Vector2(0, 0);
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
        velocity = Position - previousPosition;
        previousPosition = Position;
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
        if(ammoService.CurrentAmmo <=0)
            return;

        ammoService.Used();
        projectile.Rotation = Rotation;
        projectile.Position = Position;
        GetParent()?.AddChild(projectile);
        GetTree()?.SetInputAsHandled();
    }

    private Timer timer;
    private void RanIntoObstacle()
    {
        setBack = true;
        shout.Text = "Ouch!";
        shout.RectPosition.Rotated(0);
        timer = new Timer();
        animatedSprite.Stop();
        AddChild(timer);
        timer.Autostart = true;
        timer.WaitTime = GameConfig.CollisionDuration;
        timer.Connect("timeout", this, "OnTimeout");
        timer.Start();
    }

    public void OnTimeout()
    {
        setBack = false;
        if (timer == null)
            return;
        shout.Text = "";
        timer.Stop();
        timer.Disconnect("timeout", this, "OnTimeout");
        timer = null;
    }
}