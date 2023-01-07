using Godot;
public class ActualTree : Node2D
{

    private int health = 1000;
    private ProgressBar progressBar;
    private Label healthDebug;
    private Label registeredWoodcuttersDebug;

    private bool cutDown = false;
    public bool CutDown => cutDown;

    private int registeredWoodcutters = 0;
    public override void _Ready()
    {
        healthDebug = GetNode<Label>("Health");
        progressBar = GetNode<ProgressBar>("ProgressBar");
        progressBar.PercentVisible = false;
        progressBar.MaxValue = health;
        progressBar.MinValue = 0;
        registeredWoodcuttersDebug = GetNode<Label>("RegisteredWoodcutters");
    }

    public void RegisterWoodcutter() => registeredWoodcutters++;
    public void UnregisterWoodcutter() => registeredWoodcutters--;
    public override void _Process(float delta)
    {
        health -= registeredWoodcutters;
        healthDebug.Text = health.ToString();
        registeredWoodcuttersDebug.Text = registeredWoodcutters.ToString();

        progressBar.Value = health;

        if (health <= 0)
        {
            cutDown = true;

        }
    }
}