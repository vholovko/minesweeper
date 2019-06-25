using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Minesweeper.Mechanics;
using Sodium.Frp;
using Sodium.Functional;

namespace Minesweeper.SWidgets
{
    public class SButton : Button, IDisposable
    {
        private readonly IReadOnlyList<IListener> listeners;

        public SButton( Stream<Square> setSquare )
        {
            StreamSink<Unit> sClickedSink = Stream.CreateSink<Unit>();
            SClicked = sClickedSink;
            Click += ( sender, args ) => sClickedSink.Send( Unit.Value );

            // ReSharper disable once UseObjectOrCollectionInitializer
            List<IListener> listeners = new List<IListener>();
            listeners.Add( setSquare.Listen( square =>
            {
                Dispatcher.InvokeAsync( () =>
                {
                    Content = square.Content;
                    FontWeight = square.FontWeight;
                    Foreground = square.Foreground;
                    Background = square.Background;
                    IsEnabled = square.IsEnabled;
                } );
            } ) );
            this.listeners = listeners;
        }

        public Stream<Unit> SClicked { get; }

        public void Dispose()
        {
            foreach( IListener l in listeners )
            {
                l.Unlisten();
            }
        }
    }
}
