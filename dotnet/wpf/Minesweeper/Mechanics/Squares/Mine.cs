using System.Windows;
using Sodium.Frp;

namespace Minesweeper.Mechanics.Squares
{
    public class Mine
    {
        public Mine( int target )
        {
            Target = target;
            Square = Cell.Constant( new Square( "*", FontWeights.Bold, SystemColors.ControlTextBrush, SystemColors.ControlBrush, false ) );
        }
        public int Target { get; }

        public Cell<Square> Square { get; }
    }
}
