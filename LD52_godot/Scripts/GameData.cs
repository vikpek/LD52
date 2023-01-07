using System.Collections.Generic;
namespace LD52.Scripts
{
    public class GameData
    {

        public List<Woodcutter> Woodcutters { get; } = new List<Woodcutter>();
        public List<ActualTree> Trees { get; } = new List<ActualTree>();
        public List<Bush> Bushes { get; } = new List<Bush>();
    }
}