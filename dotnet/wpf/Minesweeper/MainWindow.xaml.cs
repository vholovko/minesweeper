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

            foreach( var index in Enumerable.Range( 0, 80 ) )
            {
                Transaction.RunVoid( () =>
                 {
                     CellLoop<SolidColorBrush> background = new CellLoop<SolidColorBrush>();
                     CellLoop<bool> enabled = new CellLoop<bool>();

                     var sButton = new SButton( background, enabled ) { Width = 24, Height = 24 };

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
