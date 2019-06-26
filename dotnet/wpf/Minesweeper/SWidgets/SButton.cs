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

        public SButton( Cell<Square> square )
        {
            StreamSink<Unit> sClickedSink = Stream.CreateSink<Unit>();
            SClicked = sClickedSink;
            Click += ( sender, args ) => sClickedSink.Send( Unit.Value );

            StreamSink<Unit> sClickedRightSink = Stream.CreateSink<Unit>();
            SClickedRight = sClickedRightSink;
            MouseRightButtonDown += ( sender, args ) => sClickedRightSink.Send( Unit.Value );

            // Set the initial value at the end of the transaction so it works with CellLoops.
            Transaction.Post( () =>
            {
                var sample = square.Sample();

                Content = sample.Content;
                FontWeight = sample.FontWeight;
                Foreground = sample.Foreground;
                Background = sample.Background;
                IsEnabled = sample.IsEnabled;
            } );

            // ReSharper disable once UseObjectOrCollectionInitializer
            List<IListener> listeners = new List<IListener>();
            listeners.Add( square.Updates().Listen( e => Dispatcher.InvokeIfNecessary( () =>
            {
                Content = e.Content;
                FontWeight = e.FontWeight;
                Foreground = e.Foreground;
                Background = e.Background;
                IsEnabled = e.IsEnabled;
            } ) ) );
            this.listeners = listeners;
        }

        public Stream<Unit> SClicked { get; }

        public Stream<Unit> SClickedRight { get; }

        public void Dispose()
        {
            foreach( IListener l in this.listeners )
            {
                l.Unlisten();
            }
        }
    }
}
