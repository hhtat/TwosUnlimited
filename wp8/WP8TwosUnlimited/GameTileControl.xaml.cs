using System;
using System.Windows.Controls;
using System.Windows.Media;

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

                takeNextValues();

                step++;
            }
            else
            {
                viewBox.Width = GameControl.TileSize;
                viewBox.Height = GameControl.TileSize;

                takeNextValues();
            }
        }

        public void Update(GameTileState state)
        {
            lastValue = nextValue;
            lastTop = nextTop;
            lastLeft = nextLeft;

            nextValue = state.GetValue();

            nextTop = (GameControl.TileSize + GameControl.TilePadding) * state.GetRow();
            nextLeft = (GameControl.TileSize + GameControl.TilePadding) * state.GetColumn();

            step = 0;
        }

        private void takeNextValues()
        {
            Canvas.SetTop(this, nextTop);
            Canvas.SetLeft(this, nextLeft);

            Color foregroundColor;
            Color backgroundColor;

            switch (nextValue)
            {
                case 2:
                    foregroundColor = Color.FromArgb(255, 119, 110, 101);
                    backgroundColor = Color.FromArgb(255, 238, 228, 218);
                    break;
                case 4:
                    foregroundColor = Color.FromArgb(255, 119, 110, 101);
                    backgroundColor = Color.FromArgb(255, 237, 224, 200);
                    break;
                case 8:
                    foregroundColor = Color.FromArgb(255, 249, 246, 242);
                    backgroundColor = Color.FromArgb(255, 242, 177, 121);
                    break;
                case 16:
                    foregroundColor = Color.FromArgb(255, 249, 246, 242);
                    backgroundColor = Color.FromArgb(255, 245, 149, 99);
                    break;
                case 32:
                    foregroundColor = Color.FromArgb(255, 249, 246, 242);
                    backgroundColor = Color.FromArgb(255, 246, 124, 95);
                    break;
                case 64:
                    foregroundColor = Color.FromArgb(255, 249, 246, 242);
                    backgroundColor = Color.FromArgb(255, 246, 94, 59);
                    break;
                case 128:
                    foregroundColor = Color.FromArgb(255, 249, 246, 242);
                    backgroundColor = Color.FromArgb(255, 237, 207, 114);
                    break;
                case 256:
                    foregroundColor = Color.FromArgb(255, 249, 246, 242);
                    backgroundColor = Color.FromArgb(255, 237, 204, 97);
                    break;
                case 512:
                    foregroundColor = Color.FromArgb(255, 249, 246, 242);
                    backgroundColor = Color.FromArgb(255, 237, 200, 80);
                    break;
                case 1024:
                    foregroundColor = Color.FromArgb(255, 249, 246, 242);
                    backgroundColor = Color.FromArgb(255, 237, 197, 63);
                    break;
                case 2048:
                    foregroundColor = Color.FromArgb(255, 249, 246, 242);
                    backgroundColor = Color.FromArgb(255, 237, 194, 46);
                    break;
                default:
                    foregroundColor = Color.FromArgb(255, 119, 110, 101);
                    backgroundColor = Color.FromArgb(255, 60, 58, 50);
                    break;
            }

            textBlock.Foreground = new SolidColorBrush(foregroundColor);
            border.Background = new SolidColorBrush(backgroundColor);

            textBlock.Text = nextValue.ToString();
        }
    }
}
