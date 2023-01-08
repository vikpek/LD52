using System;
using System.Collections.Generic;
using System.Linq;
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
        private PackedScene gameDefeatScene;

        private Global global;

        private List<Node2D> spawnTreeNodes = new List<Node2D>();
        private List<Node2D> spawnBushNodes = new List<Node2D>();
        private List<Node2D> spawnWoodcutterNodes = new List<Node2D>();

        private GameService gameService;
        private GameData data;
        private ScoreService scoreService;
        private AmmoService ammoService;

        private Label labelScore;
        private Label labelAmmo;
        private Button buttonBackToMain;

        private AudioStreamPlayer2D audioPlayer;

        public event Action<GameResult> OnGameResult = delegate { };
        public override void _Ready()
        {
            global = (Global)GetNode("/root/Global");
            labelScore = GetNode<Label>("Score");
            labelAmmo = GetNode<Label>("Ammo");
            audioPlayer = GetNode<AudioStreamPlayer2D>("AudioPlayer");
            buttonBackToMain = GetNode<Button>("BackToMain");

            buttonBackToMain.Connect("pressed", this, "OnBackToMain");


            data = new GameData();
            gameService = new GameService(data);
            scoreService = new ScoreService();
            ammoService = new AmmoService();

            SpawnSkritek();

            var arr = GetNode("SpawnTreeNodes").GetChildren();
            foreach (Node2D spawnNode in arr)
                spawnTreeNodes.Add(spawnNode);

            arr = GetNode("SpawnBushNodes").GetChildren();
            foreach (Node2D spawnNode in arr)
                spawnBushNodes.Add(spawnNode);

            arr = GetNode("SpawnWoodcutterNodes").GetChildren();
            foreach (Node2D spawnNode in arr)
            {
                spawnWoodcutterNodes.Add(spawnNode);
                data.Huts.Add(spawnNode as Hut);
            }

            actualTreeScene = GD.Load<PackedScene>("res://Scenes/ActualTree.tscn");
            while (data.Trees.Count <= GameConfig.maxConcurrentTrees)
                SpawnActualTree();

            bushScene = GD.Load<PackedScene>("res://Scenes/Bush.tscn");
            while (data.Bushes.Count < GameConfig.maxConcurrentBushes)
                SpawnBushes();

            mainScene = GD.Load<PackedScene>("res://Scenes/Main.tscn");
            gameDefeatScene = GD.Load<PackedScene>("res://Scenes/GameOver.tscn");

            StartSlowUpdate();
        }
        public override void _Process(float delta)
        {
            if (Input.IsKeyPressed((int)KeyList.Escape))
                OnBackToMain();

            List<Hut> destroyedHuts = data.Huts.FindAll(hut => hut.Destroyed);
            foreach (Hut destroyedHut in destroyedHuts)
            {
                var spawnNode = spawnWoodcutterNodes.Find(node => node.Position == destroyedHut.Position);
                spawnWoodcutterNodes.Remove(spawnNode);
            }

            data.Huts.RemoveAll(hut => hut.Destroyed);
            data.Trees.RemoveAll(tree => tree.Destroyed);

            if (data.Trees.Count <= 0)
            {
                global.lastOutcome = Global.GameOutcome.ForestCutDown;
                HandleGameOver();
            }

            if (data.Huts.Count <= 0)
                HandleVictory();

            scoreService.IncrementScore();
            if (labelScore != null)
                labelScore.Text = scoreService.Score.ToString();


            labelAmmo.Text = $"{ammoService.CurrentAmmo}/{ammoService.MaximumAmmo}";
        }

        public void OnBackToMain() => GetTree().ChangeSceneTo(mainScene);

        private Timer timer;
        public void StartSlowUpdate()
        {
            timer = new Timer();
            AddChild(timer);
            timer.Autostart = true;
            timer.WaitTime = GameConfig.UpdateInterval;
            timer.Connect("timeout", this, "OnUpdate");
            timer.Start();
        }

        public void OnUpdate()
        {
            if (data.Trees.Any(tree => tree.RegisteredWoodcutterCount > 0))
            {
                if (audioPlayer.Stream == null)
                    audioPlayer.Stream = GD.Load(GameConfig.SFXChop) as AudioStream;
                audioPlayer.Play();
            }


            if (data.Woodcutters.Count <= GameConfig.maxConcurrentWoodcutters)
                if (data.Woodcutters.Count < 1)
                {
                    SpawnWoodcutter();
                }
                else
                {
                    if (rnd.Next(0, 10) == 3)
                        SpawnWoodcutter();
                }
            if (gameService.IsSkritekHidden)
            {
                if (ammoService.AddAmmo())
                    global.PlaySound(GameConfig.SFXNewAmmo);
            }

        }

        private void SpawnBushes()
        {
            var instance = bushScene.Instance() as Bush;
            data.Bushes.Add(instance);

            int indx = rnd.Next(0, spawnBushNodes.Count - 1);
            instance.Position = spawnBushNodes[indx].Position;
            spawnBushNodes.RemoveAt(indx);

            AddChild(instance);
        }
        private void SpawnActualTree()
        {
            var instance = actualTreeScene.Instance() as ActualTree;
            data.Trees.Add(instance);

            int indx = rnd.Next(0, spawnTreeNodes.Count - 1);
            instance.Position = spawnTreeNodes[indx].Position;
            spawnTreeNodes.RemoveAt(indx);

            AddChild(instance);
        }
        private void SpawnSkritek()
        {
            skritekScene = GD.Load<PackedScene>("res://Scenes/Skritek.tscn");
            var instance = skritekScene.Instance() as Skritek;
            instance.Position = new Vector2(400, 200);
            instance.Initialize(ammoService);
            instance.OnSkritekMoved += HandleSkritekMoved;
            instance.OnSkritekHide += HandleSkritekHide;
            instance.OnSkritekCaught += HandleSkritekCaught;
            AddChild(instance);
        }
        private void HandleSkritekCaught()
        {
            global.lastOutcome = Global.GameOutcome.SkritekCaught;
            HandleGameOver();
        }

        private void HandleVictory()
        {
            global.lastOutcome = Global.GameOutcome.SkritekWon;
            HandleGameOver();
        }
        private void HandleGameOver()
        {
            PersistScore();
            GetTree().ChangeSceneTo(gameDefeatScene);
        }
        private void PersistScore()
        {

            global.LastScore = scoreService.Score;
            if (global.LastScore > global.HighScore)
                global.HighScore = global.LastScore;
        }
        private void HandleSkritekHide(bool hideStatus) => gameService.IsSkritekHidden = hideStatus;


        private Random rnd = new Random();
        private void SpawnWoodcutter()
        {
            woodcutterScene = GD.Load<PackedScene>("res://Scenes/Woodcutter.tscn");
            var instance = woodcutterScene.Instance() as Woodcutter;
            instance.Position = spawnWoodcutterNodes[rnd.Next(0, spawnWoodcutterNodes.Count - 1)].Position;
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