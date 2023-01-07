using Godot;
using LD52.Scripts;

public class Woodcutter : Node2D
{
    private ActualTree IsCutting = null;
    private GameService gameService;
    public void Initialize(GameService gameService)
    {
        this.gameService = gameService;
    }

    private AnimatedSprite animatedSprite;
    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

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
            tree.RegisterWoodcutter();
            IsCutting = tree;
        }
    }

    public void OnAreaExit(Node collider)
    {
        if (collider?.GetParent() is ActualTree tree)
        {
            tree.UnregisterWoodcutter();
            IsCutting = null;
        }
    }

    public override void _Process(float delta)
    {
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

        if(IsCutting == null || IsCutting.CutDown)
            Position += gameService.GetDirectionToClosestTree(GlobalPosition);

        // todo: if collides with tree area -> start cuttingds
    }
}