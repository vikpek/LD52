using System.Collections.Generic;
using System.Linq;
using Godot;
public class ActualTree : Node2D
{

    private int health = 1000;
    private ProgressBar progressBar;
    private Label healthDebug;
    private Label registeredWoodcuttersDebug;

    [Export]
    private int _bounceDirection = 1;

    private bool cutDown = false;
    public bool CutDown => cutDown;

    private List<Woodcutter> registeredWoodcutters = new List<Woodcutter>();
    public override void _Ready()
    {
        healthDebug = GetNode<Label>("Health");
        progressBar = GetNode<ProgressBar>("ProgressBar");
        progressBar.PercentVisible = false;
        progressBar.MaxValue = health;
        progressBar.MinValue = 0;
        registeredWoodcuttersDebug = GetNode<Label>("RegisteredWoodcutters");
    }

    public void RegisterWoodcutter(Woodcutter woodcutter) => registeredWoodcutters.Add(woodcutter);
    public void UnregisterWoodcutter(Woodcutter woodcutter) => registeredWoodcutters.Remove(woodcutter);
    public override void _Process(float delta)
    {
        foreach (Woodcutter _ in registeredWoodcutters.Where(woodcutter => !woodcutter.IsStunned))
            health -= registeredWoodcutters.Count;

        healthDebug.Text = health.ToString();
        registeredWoodcuttersDebug.Text = registeredWoodcutters.Count.ToString();

        progressBar.Value = health;

        if (health <= 0)
        {
            cutDown = true;

        }
    }
}