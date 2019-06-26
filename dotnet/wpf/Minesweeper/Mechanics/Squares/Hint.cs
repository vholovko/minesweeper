using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Sodium.Frp;

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

        public Hint( int minesAround )
        {
            Square = Cell.Constant( new Square(
                minesAround,
                FontWeights.Bold,
                Colors[minesAround],
                SystemColors.ControlBrush,
                false ) );
        }

        public Cell<Square> Square { get; }
    }
}
