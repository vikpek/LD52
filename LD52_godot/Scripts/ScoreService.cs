namespace LD52.Scripts
{
    public class ScoreService
    {
        private int score = 0;
        public int Score => score;


        public void IncrementScore() => score = Score + 1;
        public void AddBonusToScore(int bonus) => score = Score + bonus;
    }
}