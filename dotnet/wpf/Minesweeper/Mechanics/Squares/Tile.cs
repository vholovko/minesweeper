using System.Windows;
using Sodium.Frp;

namespace Minesweeper.Mechanics.Squares
{
    public class Tile
    {
        public Tile()
        {
            Square = Cell.Constant( new Square(
                null,
                FontWeights.Normal,
                SystemColors.ControlTextBrush,
                SystemColors.ControlLightBrush,
                true ) );
        }
        public Cell<Square> Square { get; }
    }
}
