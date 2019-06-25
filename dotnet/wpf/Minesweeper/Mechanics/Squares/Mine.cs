using System.Windows;
using Sodium.Frp;
using Sodium.Functional;

namespace Minesweeper.Mechanics.Squares
{
    public class Mine
    {
        public Mine( Stream<Unit> sClicked )
        {
            SClip = sClicked.Map( u => Square );
        }

        private static Square Square => new Square(
            "*",
            FontWeights.Bold,
            SystemColors.ControlTextBrush,
            SystemColors.ControlBrush,
            false );

        public Stream<Square> SClip { get; }
    }
}
