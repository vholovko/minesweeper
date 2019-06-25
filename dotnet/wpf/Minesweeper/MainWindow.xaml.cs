using System.Windows;
using Minesweeper.Mechanics;
using Minesweeper.SWidgets;

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
//            const int columns = 8;
//            const int rows = 8;
//            const int mines = 10;

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
            var squares = new Game(columns, rows, mines).Squares;

            foreach( var square in squares )
            {
                var sButton = new SButton( square ) { Width = buttonSize, Height = buttonSize };
                Container.Children.Add( sButton );
            }
        }
    }
}
