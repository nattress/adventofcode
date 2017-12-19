using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace _19
{
    class Point
    {
        public int X {get;set;}
        public int Y {get;set;}

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point NextPointInDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Point(X, Y - 1);
                case Direction.Down:
                    return new Point(X, Y + 1);
                case Direction.Left:
                    return new Point(X - 1, Y);
                case Direction.Right:
                    return new Point(X + 1, Y);
            }

            throw new InvalidOperationException();
        }
    }

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    enum MazeLineType
    {
        Vertical,
        Horizontal,
        Cross,
        Letter,
        Nothing
    }

    class Maze
    {
        char[,] _data;

        public Maze(string[] data)
        {
            _data = new char[data[0].Length, data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                char[] dataLine = data[i].ToCharArray();
                for (int j = 0; j < data[0].Length; j++)
                {
                    _data[j, i] = dataLine[j];
                }
            }
        }

        public Point GetEntrance()
        {
            for (int i = 0; i < _data.GetLength(1); i++)
            {
                if (_data[i, 0] == '|')
                    return new Point(i, 0);
            }

            throw new InvalidOperationException();
        }

        public MazeLineType MazeLineTypeAtCoordinate(int x, int y)
        {
            // Out of bounds? Treat as nothing to save bounds checking
            // in consuming algorithms
            if (x < 0 || x >= _data.GetLength(0))
                return MazeLineType.Nothing;

            if (y < 0 || y >= _data.GetLength(1))
                return MazeLineType.Nothing;

            switch (_data[x, y])
            {
                case '|':
                    return MazeLineType.Vertical;
                case '-':
                    return MazeLineType.Horizontal;
                case '+':
                    return MazeLineType.Cross;
                case ' ':
                    return MazeLineType.Nothing;
                default:
                    Debug.Assert("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(_data[x, y].ToString()));
                    return MazeLineType.Letter;
            }
        }

        public MazeLineType MazeLineTypeAtPoint(Point point)
        {
            return MazeLineTypeAtCoordinate(point.X, point.Y);
        }

        public char LetterAtPoint(Point point)
        {
            Debug.Assert(MazeLineTypeAtPoint(point) == MazeLineType.Letter);
            return _data[point.X, point.Y];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputLines = new List<string>();

            using (TextReader tr = File.OpenText(args[0]))
            {
                string line = "";
                while (true)
                {
                    line = tr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;
                    
                    inputLines.Add(line);
                }
            }

            Maze maze = new Maze(inputLines.ToArray());

            FollowPath(maze, maze.GetEntrance(), Direction.Down, out string visitedLetters, out int totalSteps);

            Console.WriteLine($"Letters visited from beginning to end: {visitedLetters}");
            Console.WriteLine($"Total steps: {totalSteps}");
        }

        static Direction RotateClockwise(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Up;
            }

            throw new InvalidDataException();
        }

        static Direction RotateAntiClockwise(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Left;
                case Direction.Right:
                    return Direction.Up;
                case Direction.Down:
                    return Direction.Right;
                case Direction.Left:
                    return Direction.Down;
            }

            throw new InvalidDataException();
        }

        static void FollowPath(Maze maze, Point start, Direction startingDirection, out string visitedLetters, out int totalSteps)
        {
            Point currentPoint = start;
            Direction currentDirection = startingDirection;
            visitedLetters = "";
            totalSteps = 0;

            while (true)
            {
                switch (maze.MazeLineTypeAtPoint(currentPoint))
                {
                    case MazeLineType.Vertical:
                    case MazeLineType.Horizontal:
                        // Do nothing. The algorithm always follows the current direction anyway.
                        break;
                    case MazeLineType.Cross:
                        // Make a 90-degree turn
                        Direction clockwise = RotateClockwise(currentDirection);
                        if (maze.MazeLineTypeAtPoint(currentPoint.NextPointInDirection(clockwise)) != MazeLineType.Nothing)
                        {
                            currentDirection = clockwise;
                            break;
                        }
                        Direction antiClockwise = RotateAntiClockwise(currentDirection);
                        if (maze.MazeLineTypeAtPoint(currentPoint.NextPointInDirection(antiClockwise)) != MazeLineType.Nothing)
                        {
                            currentDirection = antiClockwise;
                            break;
                        }
                        
                        throw new InvalidDataException("Expected a valid line either left or right of current point");
                    case MazeLineType.Letter:
                        visitedLetters += maze.LetterAtPoint(currentPoint);
                        break;
                    case MazeLineType.Nothing:
                        // Reached the end
                        return;
                }
                totalSteps++;
                currentPoint = currentPoint.NextPointInDirection(currentDirection);
            }
        }
    }
}
