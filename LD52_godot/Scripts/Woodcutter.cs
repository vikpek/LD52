using Godot;
using LD52.Scripts;
public class Woodcutter : Node2D
{
    private ActualTree IsCutting = null;
    private GameService gameService;

    private Label shout;

    private bool isStunned = false;
    public bool IsStunned => isStunned;

    public void Initialize(GameService gameService)
    {
        this.gameService = gameService;
    }

    private AnimatedSprite animatedSprite;
    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        shout = GetNode<Label>("Shout");

        var area = GetNode<Area2D>("CollisionArea");
        if (area is null)
            return;

        area.Connect("area_entered", this, "OnAreaEntered");
        area.Connect("area_exited", this, "OnAreaExit");
    }

    public void OnAreaEntered(Node collider)
    {
        if (collider?.GetParent() is ActualTree tree)
        {
            tree.RegisterWoodcutter(this);
            IsCutting = tree;
        }

        if (collider?.GetParent() is Projectile projectile)
        {
            HitByProjectile();
        }
    }

    public void OnAreaExit(Node collider)
    {
        if (collider?.GetParent() is ActualTree tree)
        {
            tree.UnregisterWoodcutter(this);
            IsCutting = null;
        }
    }

    public override void _Process(float delta)
    {
        if (IsStunned)
            return;

        // if sees skritek -> move there
        if (gameService.CanSeeSkritek(GlobalPosition))
        {
            Position += gameService.GetDirectionToSkritek(GlobalPosition);
            Rotation = gameService.GetDirectionToSkritek(GlobalPosition).Angle();
            animatedSprite.Play("run");
            if (gameService.ReachedSkritek(GlobalPosition))
            {
                animatedSprite.Stop();
            }
            return;
        }

        if (IsCutting == null || IsCutting.CutDown)
        {
            Position += gameService.GetDirectionToClosestTree(GlobalPosition);
            Rotation = gameService.GetDirectionToClosestTree(GlobalPosition).Angle();
        }

        // todo: if collides with tree area -> start cuttingds
    }

    private Timer timer;
    public void HitByProjectile()
    {
        isStunned = true;
        shout.Text = "zzZZzz";
        timer = new Timer();
        animatedSprite.Stop();
        AddChild(timer);
        timer.Autostart = true;
        timer.WaitTime = GameConfig.StunDuration;
        timer.Connect("timeout", this, "OnTimeout");
        timer.Start();
    }

    public void OnTimeout()
    {
        isStunned = false;
        shout.Text = "";
        timer.Stop();
        timer.Disconnect("timeout", this, "OnTimeout");
        timer = null;
    }
}