using Godot;
namespace LD52.Scripts
{
    public class Main : Node2D
    {
        private PackedScene gameScene;
        private PackedScene tutorial;

        private Button startGameButton;
        private Button showTutorialButton;
        private Label triviaMessage;
        private Global global;
        public override void _Ready()
        {

            global = (Global)GetNode("/root/Global");
            gameScene = GD.Load<PackedScene>("res://Scenes/Game.tscn");
            tutorial = GD.Load<PackedScene>("res://Scenes/Tutorial.tscn");

            startGameButton = GetNode<Button>("StartGame");
            startGameButton.Connect("pressed", this, "OnStartGamePressed");

            showTutorialButton = GetNode<Button>("ShowTutorial");
            showTutorialButton.Connect("pressed", this, "OnShowTutorial");

            triviaMessage = GetNode<Label>("TriviaMessage");
            triviaMessage.Text = global.GetRandomTriviaMessage();
        }

        public void OnStartGamePressed()
        {
            GetTree().ChangeSceneTo(gameScene);
        }

        public void OnShowTutorial()
        {
            GetTree().ChangeSceneTo(tutorial);
        }
    }
}