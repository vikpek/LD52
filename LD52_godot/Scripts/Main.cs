using Godot;
namespace LD52.Scripts
{
    public class Main : Node2D
    {
        private PackedScene gameScene;
        private PackedScene gameOverScene;
        private PackedScene gameVictory;
        private PackedScene tutorial;

        private Button startGameButton;
        public override void _Ready()
        {
            gameScene = GD.Load<PackedScene>("res://Scenes/Game.tscn");
            gameOverScene = GD.Load<PackedScene>("res://Scenes/GameOver.tscn");
            gameVictory = GD.Load<PackedScene>("res://Scenes/GameVictory.tscn");
            tutorial = GD.Load<PackedScene>("res://Scenes/Tutorial.tscn");

            startGameButton = GetNode<Button>("PanelContainer/VBoxContainer/StartGame");
            startGameButton.Connect("pressed", this, "OnStartGamePressed");
        }

        public void OnStartGamePressed()
        {
            GetTree().ChangeSceneTo(gameScene);
        }
    }
}