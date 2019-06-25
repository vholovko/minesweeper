﻿using System;
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
		private static Cell<List<T>> Sequence<T>(IEnumerable<Cell<T>> input)
		{
			return input.Aggregate(Cell.Constant(new List<T>()), (current, c) => current.Lift(c, (list0, a) => new List<T>(list0) { a }));
		}

		public Game(int columns, int rows, int mines, Stream<int> sClicked)
		{
			Mines = LayMines(mines, columns * rows);
			Adjacent = GetAdjacent(columns, rows);
			Hints = GatherHints(columns, rows, Mines, Adjacent);
			Voids = Hints.Keys.Where(target => Hints[target] == 0).ToHashSet();
			Field = GetField(columns, rows, sClicked);
		}

		private ISet<int> Mines { get; }
		private IReadOnlyDictionary<int, ISet<int>> Adjacent { get; }
		private IReadOnlyDictionary<int, int> Hints { get; }
		private ISet<int> Voids { get; }
		public IReadOnlyDictionary<int, Cell<Square>> Field { get; }

		private IReadOnlyDictionary<int, Cell<Square>> GetField(int columns, int rows, Stream<int> sClicked)
		{
			return Enumerable
				.Range(0, columns * rows)
				.ToDictionary(target => target, target => GetSquare(target, sClicked));
		}

		private Cell<Square> GetSquare(int target, Stream<int> sClicked)
		{
			var sFlip = Stream.Never<ISet<int>>();

			if (Mines.Contains(target))
			{
				return new Mine(target, sClicked).Square;
			}

			if (Voids.Contains(target))
			{
				// Adjacent[target].Where(Voids.Contains).ToHashSet()
				return new Void(target, sClicked, sFlip).Square;
			}

			if (Hints.ContainsKey(target))
			{
				return new Hint(target, Hints[target], sClicked).Square;
			}

			return Cell.Constant(Square.Default);
		}

		private static ISet<int> LayMines(int amount, int total)
		{
			var random = new Random();
			var mines = new HashSet<int>();
			var laid = 0;

			while (laid < amount)
			{
				var target = random.Next(0, total);
				if (mines.Contains(target))
				{
					continue;
				}

				mines.Add(target);
				laid++;
			}

			return mines;
		}

		private static IReadOnlyDictionary<int, ISet<int>> GetAdjacent(int columns, int rows)
		{
			return Enumerable
				.Range(0, columns * rows)
				.ToDictionary(target => target, target => GetAdjacentToTarget(target, columns, rows));
		}

		private static ISet<int> GetAdjacentToTarget(int target, int columns, int rows)
		{
			int north = target - columns,
				south = target + columns,
				west = target - 1,
				east = target + 1,
				northWest = north - 1,
				northEast = north + 1,
				southWest = south - 1,
				southEast = south + 1;

			if (target / rows == 0)
			{
				northWest = north = northEast = -1;
			}

			if (target / rows == rows - 1)
			{
				southWest = south = southEast = -1;
			}

			if (target % columns == 0)
			{
				northWest = west = southWest = -1;
			}

			if (target % columns == columns - 1)
			{
				northEast = east = southEast = -1;
			}

			var adjacent = new[] { north, south, east, west, northEast, northWest, southEast, southWest };

			return new HashSet<int>(adjacent.Where(a => a >= 0));
		}

		private static IReadOnlyDictionary<int, int> GatherHints(int columns, int rows, ISet<int> mines, IReadOnlyDictionary<int, ISet<int>> adjacent)
		{
			return Enumerable
				.Range(0, columns * rows)
				.Where(target => !mines.Contains(target))
				.ToDictionary(target => target, target => adjacent[target].Where(mines.Contains).Count());
		}
	}
}
