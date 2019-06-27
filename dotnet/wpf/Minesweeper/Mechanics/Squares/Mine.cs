using System.Windows;
using Sodium.Frp;

namespace Minesweeper.Mechanics.Squares
{
    public class Mine
    {
        public Mine()
        {
            Square = Cell.Constant( new Square(
                "☼",
                FontWeights.Bold,
                SystemColors.ControlTextBrush,
                SystemColors.ControlBrush,
                false ) );
        }

        public Cell<Square> Square { get; }
    }
}
