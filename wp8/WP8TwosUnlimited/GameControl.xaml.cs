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
        public static readonly int TileSize = 40;
        public static readonly int TilePadding = 8;
        public static readonly int AnimationStepsA = 6;
        public static readonly int AnimationStepsB = 6;
        public static readonly int SwipeDelta = 10;

        private GameState gameState;

        private Dictionary<GameTile, GameTileControl> tileControls;
        private Dictionary<GameTile, GameTileControl> oldTileControls;

        private DispatcherTimer timer;

        public GameControl()
        {
            InitializeComponent();

            gameState = GameLogic.Start();

            tileControls = new Dictionary<GameTile, GameTileControl>();
            oldTileControls = new Dictionary<GameTile, GameTileControl>();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            update(gameState);

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

        private void update(GameState state)
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

                tileControl.Update(state.GetOldTileState(tile));

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
        }

        private void UserControl_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            double x = e.TotalManipulation.Translation.X;
            double y = e.TotalManipulation.Translation.Y;

            if (Math.Abs(y) > Math.Abs(x) && Math.Abs(y) > SwipeDelta)
            {
                gameState = GameLogic.Step(gameState, y < 0.0 ? GameMove.Up : GameMove.Down);

                update(gameState);
            }
            else if (Math.Abs(x) > SwipeDelta)
            {
                gameState = GameLogic.Step(gameState, x < 0.0 ? GameMove.Left : GameMove.Right);

                update(gameState);
            }
        }
    }
}
