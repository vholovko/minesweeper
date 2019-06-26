using System.Windows;
using Sodium.Frp;

namespace Minesweeper.Mechanics.Squares
{
    public class Flag
    {
        public Flag()
        {
            Square = Cell.Constant( new Square(
                "ℬ",
                FontWeights.Normal,
                SystemColors.ControlTextBrush,
                SystemColors.ControlDarkBrush,
                true ) );
        }

        public Cell<Square> Square { get; }
    }
}
