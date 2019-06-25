using System.Windows;
using Sodium.Frp;

namespace Minesweeper.Mechanics.Squares
{
    public class Mine
    {
        public Mine( int target, Stream<int> hitStream )
        {
            SClip = hitStream.Filter( target.Equals ).Map( t => HitSquare );
        }

        private static Square HitSquare => new Square(
            "*",
            FontWeights.Bold,
            SystemColors.ControlTextBrush,
            SystemColors.ControlBrush,
            false );

        public Stream<Square> SClip { get; }
    }
}
