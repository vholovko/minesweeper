using System.Linq;
using System.Windows;
using System.Collections.Generic;
using Minesweeper.Mechanics;
using Minesweeper.SWidgets;
using Sodium.Frp;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            const int buttonSize = 24;

            // Introduction
            // const int columns = 8;
            // const int rows = 8;
            // const int mines = 10;

            // Intermediate
            // const int columns = 16;
            // const int rows = 16;
            // const int mines = 40;

            // Expert
            const int columns = 30;
            const int rows = 16;
            const int mines = 99;

            Container.MaxWidth = buttonSize * columns;
            Container.MaxHeight = buttonSize * rows;

            Transaction.RunVoid( () =>
             {
                 var sClicked = Stream.Never<int>();
                 var squares = new Dictionary<int, CellLoop<Square>>();
                 foreach( var target in Enumerable.Range( 0, columns * rows ) )
                 {
                     var square = new CellLoop<Square>();
                     squares.Add( target, square );
                     var button = new SButton( square ) { Width = buttonSize, Height = buttonSize };
                     sClicked = button.SClicked.Map( u => target ).OrElse( sClicked );
                     Container.Children.Add( button );
                 }

                 var field = new Game( columns, rows, mines, sClicked ).Field;

                 foreach( var target in Enumerable.Range( 0, columns * rows ) )
                 {
                     squares[target].Loop( field[target] );
                 }
             } );
        }
    }
}
