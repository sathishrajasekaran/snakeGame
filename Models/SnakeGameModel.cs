namespace snakeGame.Models;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class SnakeGameModel
{
    private const int GridSize = 20;
    private Direction _currentDirection = Direction.Right;

    public List<Point> SnakePositions { get; set; } = new();
    public Point? FoodPosition { get; set; }
    public int Score { get; set; }
    public bool IsGameOver { get; private set; }

    public void InitializeGame()
    {
        IsGameOver = false;
        Score = 3;
        _currentDirection = Direction.Right;
        SnakePositions = new List<Point>
        {
            new(10, 10),
            new(9, 10),
            new(8, 10)
        };
        SpawnFood();
    }

    public void SetDirection(Direction direction)
    {
        var oppositeDirections = new Dictionary<Direction, Direction>
        {
            [Direction.Up] = Direction.Down,
            [Direction.Down] = Direction.Up,
            [Direction.Left] = Direction.Right,
            [Direction.Right] = Direction.Left
        };

        if (oppositeDirections[_currentDirection] == direction)
        {
            return;
        }

        _currentDirection = direction;
    }

    public void Move()
    {
        if (IsGameOver)
        {
            return;
        }

        var head = SnakePositions[0];
        var nextPosition = _currentDirection switch
        {
            Direction.Up => new Point(head.X, head.Y - 1),
            Direction.Down => new Point(head.X, head.Y + 1),
            Direction.Left => new Point(head.X - 1, head.Y),
            Direction.Right => new Point(head.X + 1, head.Y),
            _ => throw new InvalidOperationException("Unknown direction")
        };

        if (IsCollision(nextPosition))
        {
            IsGameOver = true;
            return;
        }

        SnakePositions.Insert(0, nextPosition);

        if (FoodPosition != null && nextPosition.X == FoodPosition.X && nextPosition.Y == FoodPosition.Y)
        {
            Score++;
            SpawnFood();
        }
        else
        {
            SnakePositions.RemoveAt(SnakePositions.Count - 1);
        }
    }

    private bool IsCollision(Point nextPosition)
    {
        if (nextPosition.X < 0 || nextPosition.Y < 0 || nextPosition.X >= GridSize || nextPosition.Y >= GridSize)
        {
            return true;
        }

        return SnakePositions.Any(segment => segment.X == nextPosition.X && segment.Y == nextPosition.Y);
    }

    private void SpawnFood()
    {
        var random = new Random();
        Point food;
        do
        {
            food = new Point(random.Next(0, GridSize), random.Next(0, GridSize));
        }
        while (SnakePositions.Any(segment => segment.X == food.X && segment.Y == food.Y));

        FoodPosition = food;
    }
}
