using System.Windows;
using System.Windows.Media;

namespace Minesweeper.Mechanics
{
    public class Square
    {
        public Square( object content, FontWeight fontWeight, SolidColorBrush foreground, SolidColorBrush background, bool isEnabled )
        {
            Content = content;
            FontWeight = fontWeight;
            Foreground = foreground;
            Background = background;
            IsEnabled = isEnabled;
        }

        public SolidColorBrush Background { get; }
        public object Content { get; }
        public FontWeight FontWeight { get; }
        public SolidColorBrush Foreground { get; }
        public bool IsEnabled { get; }
    }
}
