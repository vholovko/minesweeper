using System.Windows;
using Sodium.Frp;
using Sodium.Functional;

namespace Minesweeper.Mechanics.Squares
{
    public class Void
    {
        public Void( Stream<Unit> sClicked )
        {
            var square = new CellLoop<Square>();
            square.Loop( sClicked.Map( u => new Square(
                null,
                FontWeights.Normal,
                SystemColors.ControlTextBrush,
                SystemColors.ControlBrush,
                false ) ).Hold( Mechanics.Square.Default ) );
            Square = square;
        }

        public Cell<Square> Square { get; }
    }
}
