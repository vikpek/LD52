using Godot;
namespace LD52.Scripts
{
    public class GameService
    {
        private readonly GameData data;
        public GameService(GameData data)
        {
            this.data = data;
        }
        public Vector2 lastKnownSkritekPosition { get; set; }
        public bool IsSkritekHidden { get; set; }

        public bool CanSeeSkritek(Vector2 position)
        {
            if (IsSkritekHidden)
                return false;

            return lastKnownSkritekPosition.DistanceTo(position) < GameConfig.SightingDistance;
        }

        public bool ReachedSkritek(Vector2 position)
        {
            return lastKnownSkritekPosition.DistanceTo(position) < GameConfig.HitDistance;
        }

        public Vector2 GetDirectionToSkritek(Vector2 position)
        {
            return (lastKnownSkritekPosition - position).Normalized();
        }

        public Vector2 GetDirectionToClosestTree(Vector2 globalPosition)
        {
            float closestDistance = float.MaxValue;
            ActualTree closestTree = null;

            foreach (ActualTree actualTree in data.Trees)
            {
                if (closestDistance < actualTree.GlobalPosition.DistanceTo(globalPosition))
                    continue;

                closestDistance = actualTree.GlobalPosition.DistanceTo(globalPosition);
                closestTree = actualTree;
            }

            if(closestTree is null)
                return Vector2.Zero;

            return (closestTree.GlobalPosition - globalPosition).Normalized();
        }
    }
}