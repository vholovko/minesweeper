using System.Windows;
using Sodium.Frp;

namespace Minesweeper.Mechanics.Squares
{
    public class Void
    {
        public Void( int target )
        {
            Target = target;
            Square = Cell.Constant( new Square( null, FontWeights.Normal, SystemColors.ControlTextBrush, SystemColors.ControlBrush, false ) );
        }
        public int Target { get; }

        public Cell<Square> Square { get; }
    }
}
