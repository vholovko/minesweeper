using System.Windows;
using Sodium.Frp;

namespace Minesweeper.Mechanics.Squares
{
    public class Mine
    {
        public Mine( int target, Stream<int> sClicked )
        {
            var square = new CellLoop<Square>();
            square.Loop( sClicked.Filter( target.Equals ).Map( _ => new Square(
                    "*",
                    FontWeights.Bold,
                    SystemColors.ControlTextBrush,
                    SystemColors.ControlBrush,
                    false ) ).Hold( Mechanics.Square.Default ) );
            Square = square;
        }

        public Cell<Square> Square { get; }
    }
}
