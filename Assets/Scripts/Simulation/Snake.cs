using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGameFirst
{
    public class Snake
    {
        private Map map;
        /// <summary>Очередь, содержащая индексы частей тела змейки.</summary>
        private Queue<int> tailIndexesQueue;
        /// <summary>Индекс клетки, на которой расположена голова змейки.</summary>
        private int headPositionIndex;
        /// <summary>Индекс клетки, на которой расположена голова змейки на предыдущем шаге.</summary>
        private int prevHeadPositionIndex;
        /// <summary>Направление движение змейки.</summary>
        private Vector2Int moveVector;
        /// <summary>Таймер движения змейки.</summary>
        private Timer moveTimer;

        /// <summary>Вызывается, когда змейка становится на клетку, которая содержит змейку или находится за пределами карты.</summary>
        public event Action OnDie;
        /// <summary>Вызывается, когда змейка становится на клетку с едой.</summary>
        public event Action OnFeed;

        /// <param name="map">Инстанс карты</param>
        /// <param name="spawnCellIndex">Индекс стартовой клетки змейки</param>
        /// <param name="stepInterval">Временой интервал одного шага змейки</param>
        /// <param name="snakeLength">Стартовая длина змейки</param>
        public Snake(Map map, int spawnCellIndex = 0, float stepInterval = 1f, int snakeLength = 3)
        {
            this.map = map;
            tailIndexesQueue = new Queue<int>();
            Spawn(snakeLength, spawnCellIndex);
            moveTimer = new Timer(stepInterval);
            moveTimer.OnTime += Move;
        }
        /// <param name="map">Инстанс карты</param>
        /// <param name="coordinates">Стартовая позиция змейки</param>
        /// <param name="stepInterval">Временой интервал одного шага змейки</param>
        /// <param name="snakeLength">Стартовая длина змейки</param>
        public Snake(Map map, Vector2Int coordinates, float stepInterval = 1f, int snakeLength = 3)
            : this(map, map.ConvertVector2IntToIndex(coordinates), stepInterval, snakeLength) { }
        /// <summary>Создает змейку, помещая её на карту</summary>
        /// <param name="length">Стартовая длина змейки</param>
        /// <param name="spawnIndexPosition">Индекс стартовой клетки змейки</param>
        private void Spawn(int length, int spawnIndexPosition)
        {
            for (int indexPosition = Math.Max(0, spawnIndexPosition - length) + 1; indexPosition <= spawnIndexPosition; indexPosition++)
                TakeCell(indexPosition);
            moveVector = new Vector2Int(0, 1);
        }
        /// <summary>
        /// Двигает змейку в текущем направлении, изменяя клетку карты. Освобождает клетку на карте с индексом из очереди tailIndexesQueue,
        /// кроме случаев, когда змейка становится на клетку с едой. Вызывает OnFeed, когда змейка становится на клетку с едой.
        /// Вызывает Die(), если змейка становится на клетку со змейкой или на клетку, которая находится за пределами карты.
        /// </summary>
        private void Move()
        {
            var nextCellPosition = map.ConvertIndexToVector2Int(headPositionIndex) + moveVector;
            if (map.IsInMapBounds(nextCellPosition))
            {
                var nextCellIndex = map.ConvertVector2IntToIndex(nextCellPosition);
                var nextCellContent = map.GetCellContentType(nextCellIndex);
                if (nextCellContent != CellContentType.snake)
                {
                    TakeCell(nextCellIndex);
                    if (nextCellContent == CellContentType.food)
                        OnFeed?.Invoke();
                    else
                        RemoveTail();
                    return;
                }
            }
            Die();
        }
        /// <summary>Занимает клетку на карте, изменяя ее содержимое на CellContentType.snake. Обновляет значние позиции головы змейки, добавляет новую позицию в очередь.</summary>
        /// <param name="cellIndex">Индекс клетки</param>
        private void TakeCell(int cellIndex)
        {
            prevHeadPositionIndex = headPositionIndex;
            headPositionIndex = cellIndex;
            tailIndexesQueue.Enqueue(cellIndex);
            map.SetCellContentType(cellIndex, CellContentType.snake);
        }
        private void RemoveTail() => map.SetCellContentType(tailIndexesQueue.Dequeue(), CellContentType.empty);
        private void Die()
        {
            Destroy();
            OnDie?.Invoke();
        }
        private void Destroy()
        {
            while (tailIndexesQueue.Count != 0)
                map.SetCellContentType(tailIndexesQueue.Dequeue(), CellContentType.empty);
            moveTimer.OnTime -= Move;
        }
        /// <summary>
        /// Меняет направление движение змейки, кроме случаев когда направление меняется на противоположное. 
        /// </summary>
        /// <param name="moveVector">Направление движения</param>
        /// <returns>При успешной смене направления возвращает true</returns>
        public bool TryChangeMoveVector(Vector2Int moveVector)
        {
            if (moveVector + map.ConvertIndexToVector2Int(headPositionIndex) != map.ConvertIndexToVector2Int(prevHeadPositionIndex))
            {
                this.moveVector = moveVector;
                return true;
            }
            return false;
        }
        /// <summary>Убирает текущую змейку с карты. Создает новую с заданной длиной и позицией. Начинает движение.</summary>
        /// <param name="spawnCellIndex">Индекс стартовой клетки</param>
        /// <param name="snakeLength">Стартовая длина змейки</param>
        public void Reset(int spawnCellIndex = 0, int snakeLength = 3)
        {
            Destroy();
            Spawn(snakeLength, spawnCellIndex);
            moveTimer.OnTime += Move;
        }
        public void Update(float deltatTime) => moveTimer.Update(deltatTime);
    }
}
