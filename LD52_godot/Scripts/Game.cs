using System.Collections.Generic;
using Godot;
namespace LD52.Scripts
{
    public class Game : Node2D
    {
        private PackedScene skritekScene;
        private PackedScene woodcutterScene;
        private PackedScene treeScene;
        private PackedScene bushScene;

        private List<Node2D> spawnTreeNodes = new List<Node2D>();
        private List<Node2D> spawnBushNodes = new List<Node2D>();

        private GameService gameService;
        private GameData data;
        public override void _Ready()
        {
            data = new GameData();
            gameService = new GameService(data);

            SpawnSkritek();
            SpawnWoodcutters();

            var arr = GetNode("SpawnTreeNodes").GetChildren();
            foreach (Node2D spawnNode in arr)
                spawnTreeNodes.Add(spawnNode);

            arr = GetNode("SpawnBushNodes").GetChildren();
            foreach (Node2D spawnNode in arr)
                spawnBushNodes.Add(spawnNode);


            treeScene = GD.Load<PackedScene>("res://Scenes/ActualTree.tscn");
            while (data.Trees.Count <= GameConfig.maxConcurrentTrees)
                SpawnActualTree();

            bushScene = GD.Load<PackedScene>("res://Scenes/Bush.tscn");
            while (data.Bushes.Count <= GameConfig.maxConcurrentBushes)
                SpawnBushes();
        }
        private void SpawnBushes()
        {
            var instance = bushScene.Instance() as Bush;
            instance.Position = spawnBushNodes[data.Bushes.Count].Position;
            data.Bushes.Add(instance);
            AddChild(instance);
        }
        private void SpawnActualTree()
        {
            var instance = treeScene.Instance() as ActualTree;
            instance.Position = spawnTreeNodes[data.Trees.Count].Position;
            data.Trees.Add(instance);
            AddChild(instance);
        }
        private void SpawnSkritek()
        {
            skritekScene = GD.Load<PackedScene>("res://Scenes/Skritek.tscn");
            var instance = skritekScene.Instance() as Skritek;
            instance.Position = new Vector2(100, 100);
            instance.OnSkritekMoved += HandleSkritekMoved;
            instance.OnSkritekHide += HandleSkritekHide;
            AddChild(instance);
        }
        private void HandleSkritekHide(bool hideStatus) => gameService.IsSkritekHidden = hideStatus;

        private void SpawnWoodcutters()
        {
            woodcutterScene = GD.Load<PackedScene>("res://Scenes/Woodcutter.tscn");
            var instance = woodcutterScene.Instance() as Woodcutter;
            instance.Position = new Vector2(300, 300);
            instance.Initialize(gameService);
            data.Woodcutters.Add(instance);
            AddChild(instance);
        }


        private void HandleSkritekMoved(Vector2 position)
        {
            gameService.lastKnownSkritekPosition = position;
        }
    }
}