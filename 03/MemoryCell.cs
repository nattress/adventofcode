using System;
using System.Collections.Generic;

namespace day3
{
    enum AdjacentDirection
    {
        Up,
        Right,
        Down,
        Left
    }

    class PositionalMemoryCell
    {
        public int Index { get; }
        public Point Point { get; }

        public int Value { get; }
        public PositionalMemoryCell(Point point, int index, int value)
        {
            Point = point;
            Index = index;
            Value = value;
        }
    }

    class Point
    {
        public int X {get;}
        public int Y {get;}

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point TranslatePoint(AdjacentDirection direction)
        {
            switch (direction)
            {
                case AdjacentDirection.Down:
                    return new Point(X, Y - 1);
                case AdjacentDirection.Left:
                    return new Point(X - 1, Y);
                case AdjacentDirection.Up:
                    return new Point(X, Y + 1);
                case AdjacentDirection.Right:
                    return new Point(X + 1, Y);
            }

            throw new Exception("Uh oh");
        }

        public override bool Equals(object obj)
        {
            Point other = obj as Point;
            if (other == null)
                return false;

            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X ^ 13 + Y;
        }
    }

    /// <summary>
    /// Provides new positional memory cells, building the spiral shape
    /// </summary>
    class PositionalMemoryCellBank
    {
        AdjacentDirection _currentSearchDirection = AdjacentDirection.Down;
        Dictionary<Point, PositionalMemoryCell> _pointToCellMap = new Dictionary<Point, PositionalMemoryCell>();
        Dictionary<int, PositionalMemoryCell> _indexToCellMap = new Dictionary<int, PositionalMemoryCell>();        
        int nextIndex = 1;
        Point _currentPoint = new Point(0, 0);

        public PositionalMemoryCell GetCellAtIndex(int index)
        {
            // If we've already expanded the spiral past the input index, early out
            if (_indexToCellMap.ContainsKey(index))
            {
                return _indexToCellMap[index];
            }

            while (nextIndex <= index)
            {
                int newValue = 0;
                // Explore neighbourhood of the new cell and sum the adjacent values
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if ( x == 0 && y == 0)
                            continue;

                        Point adjacentPoint = new Point(_currentPoint.X + x, _currentPoint.Y + y);
                        if (_pointToCellMap.ContainsKey(adjacentPoint))
                        {
                            newValue += _pointToCellMap[adjacentPoint].Value;
                        }
                    }
                }

                // Base case where we have no filled in squares yet. Origin is initialized to 1.
                if (newValue == 0)
                    newValue = 1;

                // Create the next cell in the spiral
                var newCell = new PositionalMemoryCell(_currentPoint, nextIndex, newValue);

                _pointToCellMap.Add(_currentPoint, newCell);
                _indexToCellMap.Add(nextIndex, newCell);

                // Translate point
                AdjacentDirection lookLeftForEdgeOfSpiralDirection = RotateDirectionCounterClockwise(_currentSearchDirection);
                Point lookLeftForEdgeOfSpiralPoint = _currentPoint.TranslatePoint(lookLeftForEdgeOfSpiralDirection);
                if (_pointToCellMap.ContainsKey(lookLeftForEdgeOfSpiralPoint))
                {
                    // Continue expanding in the current direction
                    _currentPoint = _currentPoint.TranslatePoint(_currentSearchDirection);
                }
                else
                {
                    // Turn left after passing the inner spiral edge to create the counter-clockwise
                    // expansion.
                    _currentSearchDirection = lookLeftForEdgeOfSpiralDirection;
                    _currentPoint = lookLeftForEdgeOfSpiralPoint;
                }

                nextIndex++;
            }

            return _indexToCellMap[index];
        }

        private AdjacentDirection RotateDirectionCounterClockwise(AdjacentDirection direction)
        {
            switch (direction)
            {
                case AdjacentDirection.Down:
                    return AdjacentDirection.Right;
                case AdjacentDirection.Left:
                    return AdjacentDirection.Down;
                case AdjacentDirection.Up:
                    return AdjacentDirection.Left;
                case AdjacentDirection.Right:
                    return AdjacentDirection.Up;
            }

            throw new Exception("Uh oh");
        }
    }
}