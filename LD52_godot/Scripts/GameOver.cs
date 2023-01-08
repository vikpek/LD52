using System;
using Godot;
using LD52.Scripts;
public class GameOver : Node2D
{

    private PackedScene gameScene;
    private PackedScene tutorial;
    private PackedScene mainScene;

    private Label mainMessage;
    private Label triviaMessage;
    private Label lastScore;
    private Label highScore;

    private Button startGameButton;
    private Button showTutorialButton;
    private Button backToMainButton;

    private Global global;
    public override void _Ready()
    {
        global = (Global)GetNode("/root/Global");
        gameScene = GD.Load<PackedScene>("res://Scenes/Game.tscn");
        tutorial = GD.Load<PackedScene>("res://Scenes/Tutorial.tscn");

        mainScene = GD.Load<PackedScene>("res://Scenes/Main.tscn");
        backToMainButton = GetNode<Button>("ButtonMain");
        backToMainButton.Connect("pressed", this, "OnPressed");

        startGameButton = GetNode<Button>("StartGame");
        startGameButton.Connect("pressed", this, "OnStartGamePressed");

        showTutorialButton = GetNode<Button>("ShowTutorial");
        showTutorialButton.Connect("pressed", this, "OnShowTutorial");


        mainMessage = GetNode<Label>("MainMessage");
        triviaMessage = GetNode<Label>("TriviaMessage");
        lastScore = GetNode<Label>("LastScore");
        highScore = GetNode<Label>("HighScore");

        lastScore.Text = $"Score: {global.LastScore.ToString()}";
        highScore.Text = $"Highest Score: {global.HighScore.ToString()}";


        switch (global.lastOutcome)
        {
            case Global.GameOutcome.None:
                break;
            case Global.GameOutcome.ForestCutDown:
                global.PlaySound(GameConfig.SFXDefeat);
                mainMessage.Text = GameOverMessage_ForestCutDown;
                break;
            case Global.GameOutcome.SkritekCaught:
                global.PlaySound(GameConfig.SFXCaught);
                mainMessage.Text = GameOverMessage_Caught;
                break;
            case Global.GameOutcome.SkritekWon:
                global.PlaySound(GameConfig.SFXVictory);
                mainMessage.Text = GameOverMessage_Victory;
                break;
        }

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

    public void OnPressed()
    {
        GetTree().ChangeSceneTo(mainScene);
    }
    public static string GameOverMessage_Victory = $"The Skritek managed to make wood harvesting so annoying that the woodcutters left for the tavern. " + "\n" +
                                                   $"The trees are safe for today. " + "\n" +
                                                   $"Good job! ";

    public static string GameOverMessage_ForestCutDown = $"They cut down the whole forest! Who are you going to annoy now? Your days will be long and boring...";

    public static string GameOverMessage_Caught = $"You got caught! The other Skriteks will laugh about you now while the villagers will keep you as a pet! " +
                                                  $"The shame is unbearable!" +
                                                  $"Check the Tutorial for some hints so it doesn't happen again.";

}