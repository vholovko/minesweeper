using System.Windows;
using System.Windows.Media;
using Sodium.Frp;

namespace Minesweeper.Mechanics.Squares
{
    public class Flag
    {
        public Flag()
        {
            Square = Cell.Constant( new Square(
                "!",
                FontWeights.Bold,
                Brushes.Red,
                SystemColors.ControlLightLightBrush,
                true ) );
        }

        public Cell<Square> Square { get; }
    }
}
