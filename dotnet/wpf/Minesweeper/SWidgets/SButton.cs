using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Sodium.Frp;
using Sodium.Functional;

namespace Minesweeper.SWidgets
{
    public class SButton : Button, IDisposable
    {
        private readonly IReadOnlyList<IListener> listeners;

        public SButton()
            : this( Cell.Constant( SystemColors.ControlLightBrush ), Cell.Constant(true) )
        {
        }

        public SButton( Cell<SolidColorBrush> background, Cell<bool> enabled )
        {
            StreamSink<Unit> sClickedSink = Stream.CreateSink<Unit>();
            SClicked = sClickedSink;
            Click += ( sender, args ) => sClickedSink.Send( Unit.Value );

            // Set the initial value at the end of the transaction so it works with CellLoops.
            Transaction.Post( () => Background = background.Sample() );
            Transaction.Post( () => IsEnabled = enabled.Sample());

            // ReSharper disable once UseObjectOrCollectionInitializer
            List<IListener> listeners = new List<IListener>();
            listeners.Add( background.Updates().Listen( e => Dispatcher.InvokeIfNecessary( () => Background = e ) ) );
            listeners.Add( enabled.Updates().Listen( e => Dispatcher.InvokeIfNecessary( () => IsEnabled = e ) ) );
            this.listeners = listeners;
        }

        public Stream<Unit> SClicked { get; }

        public void Dispose()
        {
            foreach( IListener l in this.listeners )
            {
                l.Unlisten();
            }
        }
    }
}
