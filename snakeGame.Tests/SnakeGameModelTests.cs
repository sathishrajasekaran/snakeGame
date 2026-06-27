using snakeGame.Models;

namespace snakeGame.Tests;

public class SnakeGameModelTests
{
    [Fact]
    public void InitializeGame_ShouldCreateSnakeWithThreeSegmentsAndFood()
    {
        var game = new SnakeGameModel();

        game.InitializeGame();

        Assert.Equal(3, game.SnakePositions.Count);
        Assert.Equal(3, game.Score);
        Assert.False(game.IsGameOver);
        Assert.NotNull(game.FoodPosition);
    }

    [Fact]
    public void Move_ShouldAdvanceSnakeAndIncreaseScoreAfterEatingFood()
    {
        var game = new SnakeGameModel();
        game.InitializeGame();
        var initialHead = game.SnakePositions[0];

        game.SetDirection(Direction.Right);
        game.FoodPosition = new Point(initialHead.X + 1, initialHead.Y);

        game.Move();

        Assert.Equal(initialHead.X + 1, game.SnakePositions[0].X);
        Assert.Equal(initialHead.Y, game.SnakePositions[0].Y);
        Assert.Equal(4, game.Score);
    }

    [Fact]
    public void Move_ShouldEndGameWhenSnakeHitsOwnBody()
    {
        var game = new SnakeGameModel();
        game.InitializeGame();
        game.SnakePositions = new List<Point>
        {
            new(2, 2),
            new(1, 2),
            new(1, 1),
            new(2, 1)
        };
        game.FoodPosition = new Point(5, 5);
        game.SetDirection(Direction.Up);

        game.Move();

        Assert.True(game.IsGameOver);
    }
}
