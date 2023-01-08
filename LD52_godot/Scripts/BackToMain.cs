using Godot;
using System;

public class BackToMain : Node
{
    private PackedScene mainScene;
    private Button backToMainButton;
    public override void _Ready()
    {
        mainScene = GD.Load<PackedScene>("res://Scenes/Main.tscn");
        backToMainButton = GetNode<Button>("ButtonMain");
        backToMainButton.Connect("pressed", this, "OnPressed");
    }

    public void OnPressed()
    {
        GetTree().ChangeSceneTo(mainScene);
    }
}