using System.Linq;
using System.Windows;
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
            const int columns = 8;
            const int rows = 8;
            const int mines = 10;

            // Intermediate
            // const int columns = 16;
            // const int rows = 16;
            // const int mines = 40;

            // Expert
//            const int columns = 30;
//            const int rows = 16;
//            const int mines = 99;

            Container.MaxWidth = buttonSize * columns;
            Container.MaxHeight = buttonSize * rows;
            var game = new Game( columns, rows, mines );

            foreach( var target in Enumerable.Range( 0, columns * rows ) )
            {
                Transaction.RunVoid( () =>
                 {
                     var stream = new StreamLoop<Square>();
                     var button = new SButton( stream ) { Width = buttonSize, Height = buttonSize };
                     stream.Loop( game.ToSClip( target, button.SClicked ) );
                     Container.Children.Add( button );
                 } );
            }
        }
    }
}
