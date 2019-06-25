using System.Windows;
using Sodium.Frp;
using Sodium.Functional;

namespace Minesweeper.Mechanics.Squares
{
    public class Void
    {
        public Void( Stream<Unit> sClicked )
        {
            SClip = sClicked.Map( u => Square );
        }

        public static Square Square => new Square(
            null,
            FontWeights.Normal,
            SystemColors.ControlTextBrush,
            SystemColors.ControlBrush,
            false );

        public Stream<Square> SClip { get; }
    }
}
