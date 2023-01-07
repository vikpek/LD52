using Godot;
namespace LD52.Scripts
{
    public class GameService
    {
        public Vector2 lastKnownSkritekPosition { get; set; }

        public bool CanSeeSkritek(Vector2 position)
        {
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

    }
}