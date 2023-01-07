using System.Collections.Generic;
using Godot;
namespace LD52.Scripts
{
    public class Game : Node2D
    {
        private PackedScene skritek;
        private PackedScene woodcutter;

        private List<Woodcutter> woodcutters = new List<Woodcutter>();

        private GameService gameService;
        public override void _Ready()
        {
            gameService = new GameService();
            SpawnSkritek();
            SpawnWoodcutters();
        }



        private void SpawnSkritek()
        {
            skritek = GD.Load<PackedScene>("res://Scenes/Skritek.tscn");
            var instance = skritek.Instance() as Skritek;
            instance.Position = new Vector2(100, 100);
            instance.OnSkritekMoved += OnSkritekMoved;
            AddChild(instance);

        }

        private void SpawnWoodcutters()
        {
            woodcutter = GD.Load<PackedScene>("res://Scenes/Woodcutter.tscn");
            var instance = woodcutter.Instance() as Woodcutter;
            instance.Position = new Vector2(300, 300);
            instance.Initialize(gameService);
            woodcutters.Add(instance);
            AddChild(instance);
        }


        private void OnSkritekMoved(Vector2 position)
        {
            gameService.lastKnownSkritekPosition = position;
            GD.Print(gameService.lastKnownSkritekPosition);
        }
    }
}