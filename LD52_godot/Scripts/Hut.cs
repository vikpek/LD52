using Godot;
using LD52.Scripts;
public class Hut : Node2D
{
    private int health = 1000;
    private ProgressBar progressBar;
    private Sprite sprite;

    private bool destroyed = false;
    public bool Destroyed => destroyed;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        progressBar = GetNode<ProgressBar>("ProgressBar");
        sprite = GetNode<Sprite>("Sprite");
        progressBar.PercentVisible = false;
        progressBar.MaxValue = health;
        progressBar.MinValue = 0;

    }

    public override void _Process(float delta)
    {
        progressBar.Value = health;
        if (health <= 0)
        {
            sprite.Texture = ResourceLoader.Load("res://Art/stump_01.png") as Texture;
            destroyed = true;
        }
    }
    public void AddDamage()
    {
        health -= GameConfig.projectileDamage;
    }
}