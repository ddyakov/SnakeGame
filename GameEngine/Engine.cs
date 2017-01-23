namespace GameEngine
{
    using Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class Engine : IEngine
    {
        private static Random randomNumbersGenerator = new Random();
        private const string foodSymbol = "@";
        private const string snakeSymbol = "#";
        private const string leftArrow = "<";
        private const string rightArrow = ">";
        private const string upArrow = "^";
        private const string downArrow = "v";
        private const int initialSnakeLength = 5;

        int lastFoodTime = 0;
        int foodDisappearTime = 8000;
        int negativePoints = 0;
        int direction = (int)Directions.Right;
        double sleepTime = 100;

        Queue<Position> snakeElements = new Queue<Position>();
        Position food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                randomNumbersGenerator.Next(0, Console.WindowWidth));

        Position[] directions = new Position[]
        {
            new Position(0, 1), //right
            new Position(0, -1), //left
            new Position(1, 0), //down
            new Position(-1, 0), //up
        };

        public void PrintSnakeAndFood()
        {
            for (int i = 0; i <= initialSnakeLength; i++)
            {
                snakeElements.Enqueue(new Position(0, i));
            }

            foreach (var position in snakeElements)
            {
                SnakeProperties(position);
            }

            FoodProperties();
        }
        

        public void MainLogic()
        {
            while (true)
            {
                negativePoints++;

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();

                    if (userInput.Key == ConsoleKey.RightArrow)
                    {
                        if (direction != (int)Directions.Left) direction = (int)Directions.Right;
                    }

                    if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        if (direction != (int)Directions.Right) direction = (int)Directions.Left;
                    }

                    if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        if (direction != (int)Directions.Up) direction = (int)Directions.Down;
                    }

                    if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        if (direction != (int)Directions.Down) direction = (int)Directions.Up;
                    }
                }

                var snakeHead = snakeElements.Last();
                var nextDirection = directions[direction];
                var snakeNewHead = new Position(snakeHead.Row + nextDirection.Row,
                    snakeHead.Col + nextDirection.Col);

                if (snakeNewHead.Col < 0) snakeNewHead.Col = Console.WindowWidth - 1;
                if (snakeNewHead.Row < 0) snakeNewHead.Row = Console.WindowHeight - 1;
                if (snakeNewHead.Col >= Console.WindowWidth) snakeNewHead.Col = 0;
                if (snakeNewHead.Row >= Console.WindowHeight) snakeNewHead.Row = 0;

                if (snakeElements.Contains(snakeNewHead))
                {
                    GameOverView();

                    return;
                }

                Console.SetCursorPosition(snakeHead.Col, snakeHead.Row);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(snakeSymbol);

                snakeElements.Enqueue(snakeNewHead);
                Console.SetCursorPosition(snakeNewHead.Col, snakeNewHead.Row);
                Console.ForegroundColor = ConsoleColor.Magenta;

                if (direction == (int)Directions.Right) Console.Write(rightArrow);
                if (direction == (int)Directions.Left) Console.Write(leftArrow);
                if (direction == (int)Directions.Up) Console.Write(downArrow);
                if (direction == (int)Directions.Down) Console.Write(upArrow);

                if (snakeNewHead.Col == food.Col && snakeNewHead.Row == food.Row)
                {
                    //feeding the snake
                    do
                    {
                        food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                           randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(food));

                    lastFoodTime = Environment.TickCount;
                    FoodProperties();

                    sleepTime--;
                }
                else
                {
                    //moving...
                    var last = snakeElements.Dequeue();
                    Console.SetCursorPosition(last.Col, last.Row);
                    Console.Write(" ");
                }

                if ((Environment.TickCount - lastFoodTime) >= foodDisappearTime)
                {
                    negativePoints += 50;

                    Console.SetCursorPosition(food.Col, food.Row);
                    Console.Write(" ");

                    do
                    {
                        food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                           randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(food));

                    lastFoodTime = Environment.TickCount;
                }

                FoodProperties();

                sleepTime -= 0.01;
                Thread.Sleep((int)sleepTime);
            }
        }

        public void Start()
        {
            this.PrintSnakeAndFood();
            this.MainLogic();
        }

        public void FoodProperties()
        {
            Console.SetCursorPosition(food.Col, food.Row);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(foodSymbol);
        }

        public void SnakeProperties(Position position)
        {
            Console.SetCursorPosition(position.Col, position.Row);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(snakeSymbol);
        }

        public void GameOverView()
        {
            Console.SetCursorPosition(0, 0);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Resources.GameOver);

            var userPoints = (snakeElements.Count - 6) * 100 - negativePoints;
            userPoints = Math.Max(userPoints, 0);

            Console.WriteLine($"{Resources.UserScore} {userPoints}");
        }
    }
}
