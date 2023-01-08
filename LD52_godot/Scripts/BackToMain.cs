using Godot;
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

    public override void _Process(float delta)
    {
        if (Input.IsKeyPressed((int)KeyList.Escape))
            OnPressed();
    }

    public void OnPressed()
    {
        GetTree().ChangeSceneTo(mainScene);
    }
}