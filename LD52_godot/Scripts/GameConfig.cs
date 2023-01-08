using System;
namespace LD52.Scripts
{
    public static class GameConfig
    {
        public static readonly float SightingDistance = 1000;
        public static readonly float HitDistance = 10;

        public static readonly int maxConcurrentTrees = 10;
        public static readonly int maxConcurrentBushes = 2;
        public static readonly int maxConcurrentWoodcutters = 2;

        public static float StunDuration = 2f;
        public static float CollisionDuration = 0.2f;
        public static float UpdateInterval = 1f;
        public static int maxAmmo = 12;
        public static int projectileDamage = 300;

        public static string SFXBounce = "res://SFX/bounce.wav";
        public static string SFXThrow = "res://SFX/throw.wav";
        public static string SFXHit = "res://SFX/hit.wav";
        public static string SFXHitMiss = "res://SFX/hit_miss.wav";
        public static string SFXVictory = "res://SFX/victory.wav";
        public static string SFXDefeat = "res://SFX/defeat.wav";
        public static string SFXHidden = "res://SFX/hidden.wav";
        public static string SFXNewAmmo = "res://SFX/new_ammo.wav";
        public static string SFXCaught = "res://SFX/caught.wav";
        public static string SFXChop => GetRandomChop();
        public static string MusicGame = "res://Music/game.wav";

        private static string GetRandomChop()
        {
            Random rnd = new Random();
            int indx = rnd.Next(1, 4);
            return $"res://SFX/chop0{indx}.wav";
        }
    }
}