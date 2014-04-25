using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WP8TwosUnlimited
{
    public class GameState : IEnumerable<GameTile>
    {
        private readonly int size;

        private IReadOnlyDictionary<GameTile, GameTileState> tileStates;

        public GameState(int size, IDictionary<GameTile, GameTileState> tileStates)
        {
            this.size = size;

            this.tileStates = new ReadOnlyDictionary<GameTile, GameTileState>(new Dictionary<GameTile, GameTileState>(tileStates));
        }

        public int GetSize()
        {
            return size;
        }

        public bool ContainsTile(GameTile tile)
        {
            return tileStates.ContainsKey(tile);
        }

        public GameTileState GetTileState(GameTile tile)
        {
            return tileStates[tile];
        }

        public IEnumerator<GameTile> GetEnumerator()
        {
            return tileStates.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
