using System.Windows;
using Sodium.Frp;

namespace Minesweeper.Mechanics.Squares
{
    public class Void
    {
        public Void()
        {
            Square = Cell.Constant( new Square(
                null,
                FontWeights.Normal,
                SystemColors.ControlTextBrush,
                SystemColors.ControlBrush,
                false ) );
        }

        public Cell<Square> Square { get; }
    }
}
