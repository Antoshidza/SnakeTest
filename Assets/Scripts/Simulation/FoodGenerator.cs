namespace SnakeGameFirst
{
    /// <summary>Управляет клетками карты, генерируя на них еду.</summary>
    public class FoodGenerator
    {
        private readonly Map map;
        public readonly int maxFoodCount;
        public int foodCount;
        private readonly Timer foodGenerationTimer;

        ///<param name="map">Карта, на которой генерируется еда</param>
        ///<param name="maxFoodCount">Максимальное единовременное кол-во еды</param>
        ///<param name="generateInterval">Временной интервал одной генерации еды</param>
        public FoodGenerator(Map map, int maxFoodCount = 1, float generateInterval = 1f)
        {
            this.map = map;
            this.maxFoodCount = maxFoodCount;
            foodGenerationTimer = new Timer(generateInterval);
            foodGenerationTimer.OnTime += GenerateFood;
        }
        /// <summary>Генерирует единицу еды на карте.</summary>
        private void GenerateFood()
        {
            if (foodCount < maxFoodCount && map.GetRandomEmptyCell(out var randomEmptyCellIndex))
            {
                map.SetCellContentType(randomEmptyCellIndex, CellContentType.food);
                foodCount++;
            }
        }
        public void Update(float deltaTime) => foodGenerationTimer.Update(deltaTime);
    }
}