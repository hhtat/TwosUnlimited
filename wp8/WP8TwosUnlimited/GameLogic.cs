using System;
using System.Collections.Generic;
using System.Linq;

namespace WP8TwosUnlimited
{
    public static class GameLogic
    {
        public static GameState Start()
        {
            PoweredTile[,] board = new PoweredTile[2, 2];

            SpawnTile(board);
            SpawnTile(board);

            return GetGameState(board);
        }

        public static GameState Step(GameState state, GameMove move, out IDictionary<GameTile, GameTileState> oldTileStates)
        {
            PoweredTile[,] board = GetBoard(state);

            for (int i = 0; i < GetRotation(move); i++)
            {
                board = RotateClockwise(board);
            }

            int highestPower = GetHighestPower(board);

            oldTileStates = new Dictionary<GameTile, GameTileState>();

            if (FloatTiles(board, oldTileStates))
            {
                SpawnTile(board);
            }

            int newHighestPower = GetHighestPower(board);

            if (newHighestPower != highestPower)
            {
                if (Checkpoint(newHighestPower))
                {
                    board = Promote(board);
                }
            }

            for (int i = 0; i < 4 - GetRotation(move); i++)
            {
                foreach (GameTile tile in oldTileStates.Keys.ToList())
                {
                    oldTileStates[tile] = RotateClockwise(oldTileStates[tile], board.GetLength(0));
                }

                board = RotateClockwise(board);
            }

            return GetGameState(board);
        }

        private static int GetHighestPower(PoweredTile[,] board)
        {
            int max = int.MinValue;

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    PoweredTile current = board[i, j];

                    if (current != null && current.power > max)
                    {
                        max = current.power;
                    }
                }
            }

            return max;
        }

        private static PoweredTile[,] GetBoard(GameState state)
        {
            PoweredTile[,] board = new PoweredTile[state.GetSize(), state.GetSize()];

            foreach (GameTile tile in state)
            {
                GameTileState tileState = state.GetTileState(tile);

                board[tileState.GetRow(), tileState.GetColumn()] = new PoweredTile(tile, tileState.GetPower());
            }

            return board;
        }

        private static GameState GetGameState(PoweredTile[,] board)
        {
            Dictionary<GameTile, GameTileState> tileStates = new Dictionary<GameTile, GameTileState>();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    PoweredTile current = board[i, j];

                    if (current != null)
                    {
                        tileStates[current.tile] = new GameTileState(current.power, i, j);
                    }
                }
            }

            return new GameState(board.GetLength(0), tileStates);
        }

        private static int GetRotation(GameMove move)
        {
            switch (move)
            {
                case GameMove.Up:
                    return 0;
                case GameMove.Right:
                    return 1;
                case GameMove.Down:
                    return 2;
                case GameMove.Left:
                    return 3;
            }

            throw new ArgumentOutOfRangeException();
        }

        private static PoweredTile[,] RotateClockwise(PoweredTile[,] board)
        {
            PoweredTile[,] rotated = new PoweredTile[board.GetLength(0), board.GetLength(0)];

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    rotated[board.GetLength(0) - 1 - j, i] = board[i, j];
                }
            }

            return rotated;
        }

        private static GameTileState RotateClockwise(GameTileState tileState, int size)
        {
            return new GameTileState(tileState.GetPower(), size - 1 - tileState.GetColumn(), tileState.GetRow());
        }

        private static bool FloatTiles(PoweredTile[,] board, IDictionary<GameTile, GameTileState> oldTileStates)
        {
            bool floated = false;

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    PoweredTile current = board[i, j];

                    if (current != null)
                    {
                        int k;

                        for (k = i; k > 0; k--)
                        {
                            PoweredTile upper = board[k - 1, j];

                            if (upper != null)
                            {
                                if (!upper.upped && upper.power == current.power)
                                {
                                    current.power++;
                                    current.upped = true;

                                    oldTileStates[upper.tile] = new GameTileState(upper.power, k - 1, j);

                                    k--;
                                }

                                break;
                            }
                        }

                        if (i != k)
                        {
                            board[i, j] = null;
                            board[k, j] = current;

                            floated = true;
                        }
                    }
                }
            }

            return floated;
        }

        private static void SpawnTile(PoweredTile[,] board)
        {
            List<GameTileState> holes = new List<GameTileState>();

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    if (board[i, j] == null)
                    {
                        holes.Add(new GameTileState(0, i, j));
                    }
                }
            }

            if (holes.Count > 0)
            {
                Random random = new Random();

                GameTileState hole = holes[random.Next(holes.Count)];

                board[hole.GetRow(), hole.GetColumn()] = new PoweredTile(new GameTile(), random.NextDouble() < 0.9 ? 1 : 2);
            }
        }

        private static bool Checkpoint(int power)
        {
            switch (power)
            {
                case 4:
                case 7:
                case 11:
                    return true;
            }

            return false;
        }

        private static PoweredTile[,] Promote(PoweredTile[,] board)
        {
            PoweredTile[,] promoted = new PoweredTile[board.GetLength(0) + 1, board.GetLength(0) + 1];

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    promoted[i, j] = board[i, j];
                }
            }

            return promoted;
        }

        private class PoweredTile
        {
            public readonly GameTile tile;
            public int power;
            public bool upped;

            public PoweredTile(GameTile tile, int power)
            {
                this.tile = tile;
                this.power = power;
            }
        }
    }
}
