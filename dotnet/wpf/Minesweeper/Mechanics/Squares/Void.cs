using System.Windows;
using Sodium.Frp;

namespace Minesweeper.Mechanics.Squares
{
    public class Void
    {
        public Void( int target, Stream<int> hitStream)
        {
            SClip = hitStream.Filter( target.Equals ).Map( t => HitSquare );
        }

        public static Square HitSquare => new Square(
            null,
            FontWeights.Normal,
            SystemColors.ControlTextBrush,
            SystemColors.ControlBrush,
            false);

        public Stream<Square> SClip { get; }
    }
}
