using Godot;
using LD52.Scripts;
public enum InternalState
{
    Idle,
    ChasingSkritek,
    WalkingToTree,
    CuttingTree,
    IsConfused
}

public class Woodcutter : Node2D
{
    private GameService gameService;
    public void Initialize(GameService gameService)
    {
        this.gameService = gameService;
    }

    private AnimatedSprite animatedSprite;
    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    public override void _Process(float delta)
    {
        // if sees skritek -> move there
        if (gameService.CanSeeSkritek(GlobalPosition))
        {
            Position += gameService.GetDirectionToSkritek(GlobalPosition);
            Rotation = gameService.GetDirectionToSkritek(GlobalPosition).Angle();
            animatedSprite.Play("run");
        }
        if (gameService.ReachedSkritek(GlobalPosition))
        {
            animatedSprite.Stop();
        }
    }
}