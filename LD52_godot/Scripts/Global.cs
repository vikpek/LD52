using System;
using System.Collections.Generic;
using Godot;
using LD52.Scripts;
public class Global : Node2D
{
    public enum GameOutcome
    {
        None,
        ForestCutDown,
        SkritekCaught,
        SkritekWon
    }


    private CheckButton musicCheckButton;
    private AudioStreamPlayer2D audioPlayer;
    private AudioStreamPlayer musicPlayer;
    private List<int> ShownTrivias = new List<int>();

    public override void _Ready()
    {
        this.ZIndex = 1;
        audioPlayer = GetNode<AudioStreamPlayer2D>("AudioPlayer");
        musicPlayer = GetNode<AudioStreamPlayer>("MusicPlayer");
        musicCheckButton = GetNode<CheckButton>("CheckButton");
        InitializeTrivias();

        musicPlayer.Stream = GD.Load(GameConfig.MusicGame) as AudioStream;
        musicPlayer.Play();

        musicCheckButton.Connect("pressed", this, "OnPressed");
        musicCheckButton.Pressed = true;
    }

    private void OnPressed()
    {
        if (musicCheckButton.Pressed)
            musicPlayer.Play();
        else
            musicPlayer.Stop();
    }


    private void InitializeTrivias()
    {
        trivias.Add($"We skriteks let grow tasty mushrooms to lure gatherers from their path and watch them drown in a swamp!");
        trivias.Add($"Skritek is a czech word. And really hard to pronounce. It goes like screeechtkkk. Never mind.");
        trivias.Add($"Rumour has it that we are just squirrels confused by drunk humans. Obviously this is nonsense.");
        trivias.Add($"Mushroom gatherers sometimes lay out their first three mushrooms on a stump in the shape of a triangle. We like that and leave them alone then.");
        trivias.Add($"Rarely we rip humans apart. Hi hi hi.");
        trivias.Add($"We can turn invisible whenever we like!");
        trivias.Add($"We can turn into chickens! And dragons! Anything we want! Muhahaha.");
        trivias.Add($"We exist in many landscapes and forms. Mountains, plains, fields, houses, forests, ...");
        trivias.Add($"Humans sometimes make shrines for their dead. We take them over and to show our gratitude we protect the house.");
        trivias.Add($"Some say that we are born as black chickens and turn into a Skritek when a black cat meows at us.");
        trivias.Add($"Some humans put red painted metal snakes on their doors to protect themselves from us. That doesn't work.");
        trivias.Add($"You can win our hearts by gifting us tailored clothes!");
    }



    private Random rnd = new Random();
    public GameOutcome lastOutcome { get; set; } = GameOutcome.None;
    public int LastScore { get; set; } = 0;
    public int HighScore { get; set; } = 0;

    private List<string> trivias = new List<string>();

    public string GetRandomTriviaMessage()
    {
        int rndIndx = rnd.Next(0, trivias.Count - 1);
        for (int i = 0; i < 10; i++)
        {
            if (ShownTrivias.Contains(rndIndx))
                rndIndx = rnd.Next(0, trivias.Count - 1);
            else
                break;
        }
        ShownTrivias.Add(rndIndx);
        return $"\"" + trivias[rndIndx] + $"\"\n" +
               $"(Trivia: {rndIndx + 1}/{trivias.Count})";
    }

    public void PlaySound(string path)
    {
        audioPlayer.Stream = GD.Load(path) as AudioStream;
        audioPlayer.Play();
    }

}