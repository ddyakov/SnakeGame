using Utilities;

namespace GameEngine
{
    public interface IEngine
    {
        void Start();

        void PrintSnakeAndFood();

        void FoodProperties();

        void SnakeProperties(Position position);

        void GameOverView();

        void MainLogic();
    }
}