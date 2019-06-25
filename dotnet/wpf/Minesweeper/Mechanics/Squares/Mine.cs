using System.Windows;
using Sodium.Frp;
using Sodium.Functional;

namespace Minesweeper.Mechanics.Squares
{
    public class Mine
    {
        public Mine( Stream<Unit> sClicked )
        {
            var square = new CellLoop<Square>();
            square.Loop( sClicked.Map( u => new Square(
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
