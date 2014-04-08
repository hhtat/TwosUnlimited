
namespace WP8TwosUnlimited
{
    public class GameTileState
    {
        private readonly int power;

        private readonly int row;

        private readonly int column;

        public GameTileState(int power, int row, int column)
        {
            this.power = power;
            this.row = row;
            this.column = column;
        }

        public int GetPower()
        {
            return power;
        }

        public int GetValue()
        {
            return 1 << power;
        }

        public int GetRow()
        {
            return row;
        }

        public int GetColumn()
        {
            return column;
        }
    }
}
