using UnityEngine;

namespace SnakeGameFirst
{
    /// <summary>Предоставляет инструменты управления игрой и доступ к данным игры.</summary>
    public class GameMediator
    {
        public readonly Player player;
        public readonly Map map;

        /// <summary>Инициализирует новую игру: создает карту и игрока.</summary>
        /// <param name="player">Ссылка на инстанс игрока</param>
        /// <param name="mapSize">Размер карты</param>
        /// <param name="maxFoodCount">Максимальное кол-во еды на карте единовременно</param>
        /// <param name="foodGenerateInterval">Временой интервал одной генерации еды</param>
        /// <param name="snakeLength">Стартовая длина змейки</param>
        /// <param name="snakeStepInterval">Временой интервал одного шага змейки</param>
        public GameMediator(Player player, Vector2Int mapSize, int maxFoodCount = 1, float foodGenerateInterval = 1f, int snakeLength = 3, float snakeStepInterval = 1f)
        {
            map = new Map(mapSize, maxFoodCount, foodGenerateInterval);
            this.player = player;
            player.SetPlayer(map, map.CenterPosition, snakeStepInterval, snakeLength);
        }
        /// <summary>
        /// Инициализирует новую игру: создает карту и игрока.
        /// </summary>
        /// <param name="mapConfigurationData">Данные для создания карты</param>
        /// <param name="player">Ссылка на инстанс игрока</param>
        /// <param name="playerConfigurationData">Данные для настройки игрока</param>
        public GameMediator(MapConfigurationData mapConfigurationData, Player player, PlayerConfigurationData playerConfigurationData) :
            this(player: player,
                    mapSize: mapConfigurationData.mapSize,
                    maxFoodCount: mapConfigurationData.maxFoodCount,
                    foodGenerateInterval: mapConfigurationData.foodGenerateInterval,
                    snakeLength: playerConfigurationData.startSnakeLength,
                    snakeStepInterval: playerConfigurationData.snakeStepInterval) { }
        /// <summary>Сбрасывает игру: очищает карту, обнуляет счет игрока, пересоздает змейку.</summary>
        public void ResetGame()
        {
            map.Reset();
            player.Reset(map.CenterIndexPosition);
        }
    }
}