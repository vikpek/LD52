using System;
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

        private List<Woodcutter> woodcutters = new List<Woodcutter>();

        private List<Node2D> spawnTreeNodes = new List<Node2D>();
        private List<Node2D> spawnBushNodes = new List<Node2D>();

        private List<ActualTree> trees = new List<ActualTree>();
        private List<Bush> bushes = new List<Bush>();


        private GameService gameService;
        public override void _Ready()
        {
            gameService = new GameService();

            SpawnSkritek();
            SpawnWoodcutters();

            var arr = GetNode("SpawnTreeNodes").GetChildren();
            foreach (Node2D spawnNode in arr)
                spawnTreeNodes.Add(spawnNode);

            arr = GetNode("SpawnBushNodes").GetChildren();
            foreach (Node2D spawnNode in arr)
                spawnBushNodes.Add(spawnNode);


            treeScene = GD.Load<PackedScene>("res://Scenes/ActualTree.tscn");
            while (trees.Count <= GameConfig.maxConcurrentTrees)
                SpawnActualTree();

            bushScene = GD.Load<PackedScene>("res://Scenes/Bush.tscn");
            while (bushes.Count <= GameConfig.maxConcurrentBushes)
                SpawnBushes();
        }
        private void SpawnBushes()
        {
            var instance = bushScene.Instance() as Bush;
            instance.Position = spawnBushNodes[bushes.Count].Position;
            bushes.Add(instance);
            AddChild(instance);
        }
        private void SpawnActualTree()
        {
            var instance = treeScene.Instance() as ActualTree;
            instance.Position = spawnTreeNodes[trees.Count].Position;
            trees.Add(instance);
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
        private void HandleSkritekHide(bool hideStatus)
        {
            GD.Print($"Skritek hidden={hideStatus}");
        }

        private void SpawnWoodcutters()
        {
            woodcutterScene = GD.Load<PackedScene>("res://Scenes/Woodcutter.tscn");
            var instance = woodcutterScene.Instance() as Woodcutter;
            instance.Position = new Vector2(300, 300);
            instance.Initialize(gameService);
            woodcutters.Add(instance);
            AddChild(instance);
        }


        private void HandleSkritekMoved(Vector2 position)
        {
            gameService.lastKnownSkritekPosition = position;
        }
    }
}