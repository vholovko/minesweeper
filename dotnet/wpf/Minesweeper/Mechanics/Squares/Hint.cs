using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Sodium.Frp;
using Sodium.Functional;

namespace Minesweeper.Mechanics.Squares
{
    public class Hint
    {
        private static readonly IReadOnlyDictionary<int, SolidColorBrush> Colors = new Dictionary<int, SolidColorBrush>
        {
            { 1, Brushes.Blue },
            { 2, Brushes.Green },
            { 3, Brushes.Red },
            { 4, Brushes.Magenta },
            { 5, Brushes.Orange },
            { 6, Brushes.Brown },
            { 7, Brushes.Black },
            { 8, Brushes.DarkRed }
        };

        public Hint( int minesAround, Stream<Unit> sClicked )
        {
            MinesAround = minesAround;
            SClip = sClicked.Map( u => Square );
        }

        private int MinesAround { get; }

        private Square Square => new Square(
            MinesAround,
            FontWeights.Bold,
            Colors[MinesAround],
            SystemColors.ControlBrush,
            false );

        public Stream<Square> SClip { get; }
    }
}
