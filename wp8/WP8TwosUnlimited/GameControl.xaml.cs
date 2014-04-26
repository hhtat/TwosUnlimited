using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WP8TwosUnlimited
{
    public partial class GameControl : UserControl
    {
        public static readonly int TileSize = 55;
        public static readonly int TilePadding = 11;
        public static readonly int AnimationStepsA = 6;
        public static readonly int AnimationStepsB = 6;

        public bool GameOver { get; private set; }

        private GameState gameState;

        private Dictionary<GameTile, GameTileControl> tileControls;
        private Dictionary<GameTile, GameTileControl> oldTileControls;

        private DispatcherTimer timer;

        public GameControl()
        {
            InitializeComponent();

            newGame();
        }

        public void DoMove(GameMove move)
        {
            if (GameOver)
            {
                throw new InvalidOperationException();
            }

            IDictionary<GameTile, GameTileState> oldTileStates;

            gameState = GameLogic.Move(gameState, move, out oldTileStates);

            GameOver = !GameLogic.CanMove(gameState);

            update(gameState, oldTileStates);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timer = null;
        }

        private void newGame()
        {
            GameOver = false;

            gameState = GameLogic.Start();

            tileControls = new Dictionary<GameTile, GameTileControl>();
            oldTileControls = new Dictionary<GameTile, GameTileControl>();

            update(gameState, new Dictionary<GameTile, GameTileState>());

            gameOverUI.Visibility = Visibility.Collapsed;
            gameOverUI.Opacity = 0.0;
        }

        private void update(GameState state, IDictionary<GameTile, GameTileState> oldTileStates)
        {
            canvas.Children.Clear();

            canvas.Height = TilePadding + (TileSize + TilePadding) * state.GetSize();
            canvas.Width = TilePadding + (TileSize + TilePadding) * state.GetSize();

            for (int i = 0; i < state.GetSize(); i++)
            {
                for (int j = 0; j < state.GetSize(); j++)
                {
                    Border border = new Border()
                    {
                        Background = new SolidColorBrush(Color.FromArgb(255, 204, 192, 179)),
                        CornerRadius = new CornerRadius(2),
                        Width = TileSize,
                        Height = TileSize,
                    };

                    Canvas.SetTop(border, TilePadding + (TileSize + TilePadding) * i);
                    Canvas.SetLeft(border, TilePadding + (TileSize + TilePadding) * j);

                    canvas.Children.Add(border);
                }
            }

            IEnumerable<GameTile> oldTiles = tileControls.Keys.Where(tile => !state.ContainsTile(tile)).ToList();
            IEnumerable<GameTile> continuingTiles = tileControls.Keys.Intersect(state).ToList();
            IEnumerable<GameTile> newTiles = state.Where(tile => !tileControls.ContainsKey(tile)).ToList();

            oldTileControls.Clear();

            foreach (GameTile tile in oldTiles)
            {
                GameTileControl tileControl = tileControls[tile];

                tileControls.Remove(tile);
                oldTileControls[tile] = tileControl;

                tileControl.Update(oldTileStates[tile]);

                canvas.Children.Add(tileControl);
            }

            foreach (GameTile tile in continuingTiles)
            {
                GameTileControl tileControl = tileControls[tile];

                tileControl.Update(state.GetTileState(tile));

                canvas.Children.Add(tileControl);
            }

            foreach (GameTile tile in newTiles)
            {
                GameTileControl tileControl = new GameTileControl();

                tileControls[tile] = tileControl;

                tileControl.Update(state.GetTileState(tile));

                canvas.Children.Add(tileControl);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            foreach (GameTileControl tileControl in tileControls.Values)
            {
                tileControl.Animate();
            }

            foreach (GameTileControl tileControl in oldTileControls.Values)
            {
                tileControl.Animate();
            }

            if (GameOver)
            {
                if (gameOverUI.Opacity < 1.0)
                {
                    gameOverUI.Opacity += 0.01;
                }

                gameOverUI.Visibility = Visibility.Visible;
            }
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            newGame();
        }
    }
}
