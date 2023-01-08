using Godot;
using System;
using System.Collections.Generic;

public class Global : Node2D
{
    public enum GameOutcome
    {
        None,
        ForestCutDown,
        SkritekCaught,
        SkritekWon
    }

    private List<int> ShownTrivias = new List<int>();

    public override void _Ready()
    {
        InitializeTrivias();
    }
    private void InitializeTrivias()
    {
        trivias.Add($"Skriteks let grow tasty mushrooms to lure gatherers from their path and watch them drown in a swamp!");
        trivias.Add($"Skritek is a czech word. And really hard to pronounce. It goes like screeechtkkk. Never mind.");
        trivias.Add($"Rumour has it that Skriteks are just squirrels confused by drunk humans. Obviously this is nonsense.");
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
        return $"Did you know?" + "\n" +
               trivias[rndIndx] + "\n" +
               $"From the Secrets of Skriteks ({rndIndx+1}/{trivias.Count})";
    }
}