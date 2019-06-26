using System.Windows;
using System.Collections.Generic;
using Sodium.Frp;
using Sodium.Functional;

namespace Minesweeper.Mechanics.Squares
{
    public class Void
    {
        public Void( int target, Stream<int> sClicked, Stream<ISet<int>> sFlip )
        {
            var signal = sClicked.Filter( target.Equals ).Map( _ => Unit.Value ).OrElse( sFlip.Filter( targets => targets.Contains( target ) ).Map( _ => Unit.Value ) );

            var square = new CellLoop<Square>();
            square.Loop( signal.Map( _ => new Square(
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
