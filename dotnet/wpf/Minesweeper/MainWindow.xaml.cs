using System.Linq;
using System.Windows;
using System.Windows.Media;
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
            //const int columns = 8;
            //const int rows = 8;
            // const int mines = 10;

            // Intermediate
            // const int columns = 16;
            // const int rows = 16;
            // const int mines = 40;

            // Expert
            const int columns = 30;
            const int rows = 16;
            // const int mines = 99;

            const int total = columns * rows;

            Container.MaxWidth = buttonSize * columns;
            Container.MaxHeight = buttonSize * rows;

            foreach( var index in Enumerable.Range( 0, total ) )
            {
                Transaction.RunVoid( () =>
                {
                    CellLoop<SolidColorBrush> background = new CellLoop<SolidColorBrush>();
                    CellLoop<bool> enabled = new CellLoop<bool>();

                    var sButton = new SButton( background, enabled ) { Width = buttonSize, Height = buttonSize };

                    Stream<SolidColorBrush> sUpdateBackground = sButton.SClicked.Map( u => SystemColors.ControlDarkBrush );
                    background.Loop( sUpdateBackground.Hold( SystemColors.ControlLightBrush ) );

                    Stream<bool> sUpdateEnabled = sButton.SClicked.Map( u => false );
                    enabled.Loop( sUpdateEnabled.Hold( true ) );

                    Container.Children.Add( sButton );
                } );
            }
        }
    }
}
