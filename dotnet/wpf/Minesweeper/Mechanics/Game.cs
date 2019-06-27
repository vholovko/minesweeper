using System;
using System.Collections.Generic;
using System.Linq;
using Minesweeper.Mechanics.Squares;
using Sodium.Frp;
using Sodium.Functional;
using Void = Minesweeper.Mechanics.Squares.Void;

namespace Minesweeper.Mechanics
{
    public class Game
    {
        public Game( int columns, int rows, int mines, Stream<int> sCheck, Stream<int> sFlag )
        {
            Mines = LayMines( mines, columns * rows );
            Adjacent = GetAdjacent( columns, rows );
            Hints = GatherHints( columns, rows, Mines, Adjacent );
            Voids = Hints.Keys.Where( target => Hints[target] == 0 ).ToHashSet();
            Flags = GetFlags( columns, rows, sFlag );
            Field = GetField( columns, rows, sCheck, sFlag );
        }

        private ISet<int> Mines { get; }
        private IReadOnlyDictionary<int, ISet<int>> Adjacent { get; }
        private IReadOnlyDictionary<int, int> Hints { get; }
        private ISet<int> Voids { get; }
        public IReadOnlyDictionary<int, Cell<Square>> Field { get; }
        public IReadOnlyDictionary<int, Cell<bool>> Flags { get; }

        private IReadOnlyDictionary<int, Cell<bool>> GetFlags( int columns, int rows, Stream<int> sFlag )
        {
            return Enumerable
                .Range( 0, columns * rows )
                .ToDictionary( target => target, target => GetFlag( target, sFlag ) );
        }

        private Cell<bool> GetFlag( int target, Stream<int> sFlag )
        {
            Stream<int> sMark = sFlag.Filter( target.Equals );
            CellLoop<bool> flag = new CellLoop<bool>();
            Stream<bool> sInvert = sMark.Snapshot( flag, ( _, value ) => !value );
            flag.Loop( sInvert.Hold( true ) );

            return flag;
        }

        private IReadOnlyDictionary<int, Cell<Square>> GetField( int columns, int rows, Stream<int> sCheck, Stream<int> sFlag )
        {
            return Enumerable
                .Range( 0, columns * rows )
                .ToDictionary( target => target, target => GetSquare( target, sCheck, sFlag ) );
        }

        private Cell<Square> GetSquare( int target, Stream<int> sCheck, Stream<int> sFlag )
        {
            Cell<Square> square = sFlag.Filter( target.Equals ).Snapshot( Flags[target], ( _, value ) => value ? new Flag().Square : new Tile().Square ).Hold( new Tile().Square ).SwitchC();

            Stream<ISet<int>> sFlipMine = sCheck.Filter( t => Flags[t].Sample() ).Filter( Mines.Contains ).Map( _ => Mines );
            Stream<ISet<int>> sFlipVoid = sCheck.Filter( t => Flags[t].Sample() ).Filter( Voids.Contains ).Map( t => CollectConnectedVoid( t, Voids, Adjacent ) );

            if( Mines.Contains( target ) )
            {
                var sFlip = sFlipMine.Filter( targets => targets.Contains( target ) ).Map( _ => Unit.Value );
                var sTrigger = sCheck.Filter( target.Equals ).Filter( _ => Flags[target].Sample() ).Map( _ => Unit.Value ).OrElse( sFlip );

                return sTrigger.Map( _ => new Mine().Square ).Hold( square ).SwitchC();
            }

            if( Voids.Contains( target ) )
            {
                var sFlip = sFlipVoid.Filter( targets => targets.Contains( target ) ).Map( _ => Unit.Value );
                var sTrigger = sCheck.Filter( target.Equals ).Filter( _ => Flags[target].Sample() ).Map( _ => Unit.Value ).OrElse( sFlip );

                return sTrigger.Map( _ => new Void().Square ).Hold( square ).SwitchC();
            }

            if( Hints.ContainsKey( target ) )
            {
                var sFlip = sFlipVoid.Filter( voids => Adjacent[target].Any( voids.Contains ) ).Map( voids => Unit.Value );
                var sTrigger = sCheck.Filter( target.Equals ).Filter( _ => Flags[target].Sample() ).Map( _ => Unit.Value ).OrElse( sFlip );

                return sTrigger.Map( _ => new Hint( Hints[target] ).Square ).Hold( square ).SwitchC();
            }

            return new Tile().Square;
        }

        private ISet<int> CollectConnectedVoid( int target, ISet<int> voids, IReadOnlyDictionary<int, ISet<int>> adjacent, ISet<int> connectedVoids = null )
        {
            connectedVoids = connectedVoids ?? ( voids.Contains( target ) ? new HashSet<int> { target } : new HashSet<int>() );

            foreach( var adjacentVoid in adjacent[target].Where( voids.Contains ) )
            {
                if( connectedVoids.Contains( adjacentVoid ) )
                {
                    continue;
                }

                connectedVoids.Add( adjacentVoid );
                connectedVoids.UnionWith( CollectConnectedVoid( adjacentVoid, voids, adjacent, connectedVoids ) );
            }

            return connectedVoids;
        }

        private static ISet<int> LayMines( int amount, int total )
        {
            var random = new Random();
            var mines = new HashSet<int>();
            var laid = 0;

            while( laid < amount )
            {
                var target = random.Next( 0, total );
                if( mines.Contains( target ) )
                {
                    continue;
                }

                mines.Add( target );
                laid++;
            }

            return mines;
        }

        private static IReadOnlyDictionary<int, ISet<int>> GetAdjacent( int columns, int rows )
        {
            return Enumerable
                .Range( 0, columns * rows )
                .ToDictionary( target => target, target => GetAdjacentToTarget( target, columns, rows ) );
        }

        private static ISet<int> GetAdjacentToTarget( int target, int columns, int rows )
        {
            int north = target - columns,
                south = target + columns,
                west = target - 1,
                east = target + 1,
                northWest = north - 1,
                northEast = north + 1,
                southWest = south - 1,
                southEast = south + 1;

            if( target / columns == 0 )
            {
                northWest = north = northEast = -1;
            }

            if( target / columns == rows - 1 )
            {
                southWest = south = southEast = -1;
            }

            if( target % columns == 0 )
            {
                northWest = west = southWest = -1;
            }

            if( target % columns == columns - 1 )
            {
                northEast = east = southEast = -1;
            }

            var adjacent = new[] { north, south, east, west, northEast, northWest, southEast, southWest };

            return new HashSet<int>( adjacent.Where( a => a >= 0 ) );
        }

        private static IReadOnlyDictionary<int, int> GatherHints( int columns, int rows, ISet<int> mines, IReadOnlyDictionary<int, ISet<int>> adjacent )
        {
            return Enumerable
                .Range( 0, columns * rows )
                .Where( target => !mines.Contains( target ) )
                .ToDictionary( target => target, target => adjacent[target].Where( mines.Contains ).Count() );
        }
    }
}
