namespace LD52.Scripts
{
    public static class GameConfig
    {
        public static readonly float SightingDistance = 1000;
        public static readonly float HitDistance = 10;

        public static readonly int maxConcurrentTrees = 6;
        public static readonly int maxConcurrentBushes = 3;
        public static readonly int maxConcurrentWoodcutters = 3;

        public static float StunDuration = 2f;
        public static float CollisionDuration = 0.2f;
        public static float UpdateInterval = 1f;
        public static int maxAmmo = 5;
        public static int projectileDamage = 250;
    }
}