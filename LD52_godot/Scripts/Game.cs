using System;
using System.Collections.Generic;
using Godot;
namespace LD52.Scripts
{
    public class Game : Node2D
    {
        private PackedScene skritekScene;
        private PackedScene woodcutterScene;
        private PackedScene actualTreeScene;
        private PackedScene bushScene;
        private PackedScene mainScene;

        private List<Node2D> spawnTreeNodes = new List<Node2D>();
        private List<Node2D> spawnBushNodes = new List<Node2D>();

        private GameService gameService;
        private GameData data;
        private ScoreService scoreService;

        private Label labelScore;

        public event Action<GameResult> OnGameResult = delegate { };
        public override void _Ready()
        {
            labelScore = GetNode<Label>("Score");

            data = new GameData();
            gameService = new GameService(data);
            scoreService = new ScoreService();

            SpawnSkritek();
            SpawnWoodcutters();

            var arr = GetNode("SpawnTreeNodes").GetChildren();
            foreach (Node2D spawnNode in arr)
                spawnTreeNodes.Add(spawnNode);

            arr = GetNode("SpawnBushNodes").GetChildren();
            foreach (Node2D spawnNode in arr)
                spawnBushNodes.Add(spawnNode);


            actualTreeScene = GD.Load<PackedScene>("res://Scenes/ActualTree.tscn");
            while (data.Trees.Count <= GameConfig.maxConcurrentTrees)
                SpawnActualTree();

            bushScene = GD.Load<PackedScene>("res://Scenes/Bush.tscn");
            while (data.Bushes.Count <= GameConfig.maxConcurrentBushes)
                SpawnBushes();

            mainScene = GD.Load<PackedScene>("res://Scenes/Main.tscn");
        }

        public override void _Process(float delta)
        {
            data.Trees.RemoveAll(tree => tree.CutDown);
            scoreService.IncrementScore();
            if (labelScore != null)
                labelScore.Text = scoreService.Score.ToString();
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
            var instance = actualTreeScene.Instance() as ActualTree;
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
            instance.OnSkritekCaught += HandleSkritekCaught;
            AddChild(instance);
        }
        private void HandleSkritekCaught()
        {
            GetTree().ChangeSceneTo(mainScene);
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