using System;
using System.Windows.Controls;

namespace WP8TwosUnlimited
{
    public partial class GameTileControl : UserControl
    {
        private int lastValue;
        private int lastTop;
        private int lastLeft;

        private int nextValue;
        private int nextTop;
        private int nextLeft;

        private int step;

        public GameTileControl()
        {
            InitializeComponent();

            Width = GameControl.TilePadding + GameControl.TileSize + GameControl.TilePadding;
            Height = GameControl.TilePadding + GameControl.TileSize + GameControl.TilePadding;

            border.Width = GameControl.TileSize;
            border.Height = GameControl.TileSize;
        }

        public void Animate()
        {
            if (step < GameControl.AnimationStepsA)
            {
                double progress = (double)step / (double)GameControl.AnimationStepsA;

                Canvas.SetTop(this, lastTop + progress * (nextTop - lastTop));
                Canvas.SetLeft(this, lastLeft + progress * (nextLeft - lastLeft));

                step++;
            }
            else if (step - GameControl.AnimationStepsA < GameControl.AnimationStepsB)
            {
                double progress = (double)(step - GameControl.AnimationStepsA) / (double)GameControl.AnimationStepsB;

                if (lastValue == 0)
                {
                    viewBox.Width = progress * GameControl.TileSize;
                    viewBox.Height = progress * GameControl.TileSize;
                }
                else if (nextValue != lastValue)
                {
                    viewBox.Width = GameControl.TileSize + 0.3 * (0.5 - Math.Abs(progress - 0.5)) * GameControl.TileSize;
                    viewBox.Height = GameControl.TileSize + 0.3 * (0.5 - Math.Abs(progress - 0.5)) * GameControl.TileSize;
                }

                textBlock.Text = nextValue.ToString();

                Canvas.SetTop(this, nextTop);
                Canvas.SetLeft(this, nextLeft);

                step++;
            }
            else
            {
                viewBox.Width = GameControl.TileSize;
                viewBox.Height = GameControl.TileSize;

                textBlock.Text = nextValue.ToString();

                Canvas.SetTop(this, nextTop);
                Canvas.SetLeft(this, nextLeft);
            }
        }

        public void Update(GameTileState state)
        {
            lastValue = nextValue;
            lastTop = nextTop;
            lastLeft = nextLeft;

            if (state == null)
            {
                nextValue = 0;
            }
            else
            {
                nextValue = state.GetValue();

                nextTop = (GameControl.TileSize + GameControl.TilePadding) * state.GetRow();
                nextLeft = (GameControl.TileSize + GameControl.TilePadding) * state.GetColumn();
            }

            step = 0;
        }
    }
}
