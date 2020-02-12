using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SnakeGameFirst
{
    /// <summary>Перечисление для представления содержимого клетки: пусто/еда/змея.</summary>
    public enum CellContentType
    {
        [InspectorName("Пустая клетка")]
        empty,
        [InspectorName("Клетка с едой")]
        food,
        [InspectorName("Клетка с частью тела змеи")]
        snake
    }
    /// <summary>Используется для представления игровой карты.</summary>
    public class Map
    {
        /// <summary>Содержимое клеток. Генерируется при создании карты.</summary>
        private readonly CellContentType[] cellContents;
        /// <summary>Задается при создании карты.</summary>
        public readonly Vector2Int mapSize;
        /// <summary>
        /// Представляет текущее кол-во не пустых клеток.
        /// Увеличивается, когда empty клетка меняется на другой тип <see cref="CellContentType">CellContentType</see>.
        /// Уменьшается в обратной ситуации. Используется для посика пустых клеток.
        /// </summary>
        private int filledCellCount;
        /// <summary>Генерируется при создании карты.</summary>
        private readonly FoodGenerator foodGenerator;

        public int CellCount { get => mapSize.x * mapSize.y; }
        /// <summary>
        /// Возвращает координаты центральной клетки на карте
        /// (расчитывается как половина размера карты типа <see cref="Vector2Int">Vector2Int</see>).
        /// </summary>
        public Vector2Int CenterPosition { get => mapSize / 2; }
        /// <summary>Возвращает индекс центральной клетки на карте.</summary>
        public int CenterIndexPosition { get => ConvertVector2IntToIndex(CenterPosition); }

        /// <summary>
        /// Вызывается, когда содержимое клетки меняется.
        /// Сигнатура включает индекс измененной клетки и новое значение <see cref="CellContentType">CellContentType</see> содержимого клетки.
        /// </summary>
        public event Action<int, CellContentType> OnChangeCellContent;

        /// <param name="mapSize">Размер карты</param>
        /// <param name="maxFoodCount">Максимальное кол-во еды на карте</param>
        /// <param name="foodGenerateInterval">Временной интервал между одной генерацией еды</param>
        public Map(Vector2Int mapSize, int maxFoodCount = 1, float foodGenerateInterval = 1f)
        {
            this.mapSize = mapSize;
            //Создается массив содержимого клеток. Изначально все значения соответствуют CellContentType.empty, что соответствует пустой карте.
            cellContents = new CellContentType[CellCount];
            foodGenerator = new FoodGenerator(this, maxFoodCount, foodGenerateInterval);
        }
        /// <summary>Изменяет содержимое клетки.</summary>
        /// <param name="cellIndex">Индекс клетки</param>
        /// <param name="contentType">Новое содержимое клетки</param>
        public void SetCellContentType(int cellIndex, CellContentType contentType)
        {
            var prevContentType = GetCellContentType(cellIndex);
            if (prevContentType != contentType)
            {
                cellContents[cellIndex] = contentType;

                //При увеличение кол-ва возможных событий, вызываемых сменой одного типа содержимого клетки на другое,
                //необходимо будет вынести логику в отдельную абстракцию.

                if (contentType == CellContentType.empty)
                    filledCellCount--;
                else if (prevContentType == CellContentType.empty)
                    filledCellCount++;
                if (prevContentType == CellContentType.food)
                    foodGenerator.foodCount--;
                OnChangeCellContent?.Invoke(cellIndex, contentType);
            }
        }
        /// <summary>Изменяет содержимое клетки.</summary>
        /// <param name="cellCoordinates">Координаты клетки</param>
        /// <param name="contentType">Новое содержимое клетки</param>
        public void SetCellContentType(Vector2Int cellCoordinates, CellContentType contentType) => SetCellContentType(ConvertVector2IntToIndex(cellCoordinates), contentType);
        /// <summary>Возвращает содержимое клетки.</summary>
        /// <param name="cellIndex">Индекс клетки</param>
        public CellContentType GetCellContentType(int cellIndex) => cellContents[cellIndex];
        /// <summary>Возвращает содержимое клетки.</summary>
        /// <param name="cellCoordinates">Координаты клетки</param>
        public CellContentType GetCellContentType(Vector2Int cellCoordinates) => cellContents[ConvertVector2IntToIndex(cellCoordinates)];
        /// <summary>Возвращает массив индексов всех пустых клеток.</summary>
        public int[] GetAllEmptyCellIndexes()
        {
            var emptyCellCount = CellCount - filledCellCount;
            var emptyCellIndexes = new int[emptyCellCount];
            var counter = 0;
            while (emptyCellCount != 0)
            {
                counter++;
                if (GetCellContentType(counter) == CellContentType.empty)
                {
                    emptyCellIndexes[emptyCellIndexes.Length - emptyCellCount] = counter;
                    emptyCellCount--;
                }
            }
            return emptyCellIndexes;
        }
        /// <summary>Возвращает истину, если есть хотя бы одна пустая клетка. Передает индекс случайной пустой клетки.</summary>
        /// <param name="emptyCellIndex">Индекс случайной пустой клетки</param>
        public bool GetRandomEmptyCell(out int emptyCellIndex)
        {
            emptyCellIndex = -1;
            var emptyCellCount = CellCount - filledCellCount;
            if (emptyCellCount != 0)
            {
                var randomOffset = Random.Range(0, emptyCellCount);
                var indexPosition = 0;

                //Поиск пустых клеток, пока randomOffset не уменьшится до 0
                do
                {
                    if (GetCellContentType(indexPosition) == CellContentType.empty)
                    {
                        randomOffset--;
                        emptyCellIndex = indexPosition;
                    }
                    indexPosition++;
                } while (randomOffset >= 0);

                return true;
            }
            return false;
        }
        /// <summary>Сбросывает карту: освобождает все клетки.</summary>
        public void Reset()
        {
            //Освобождение всех клеток автоматически приводит к "обновлению" данных FoodGenerator
            for (int i = 0; i < cellContents.Length; i++)
                SetCellContentType(i, CellContentType.empty);
        }

        #region Position Utils
        /// <summary>Переводит 2D координаты клетки в индекс.</summary>
        /// <param name="coordinates">Координаты клетки</param>
        public int ConvertVector2IntToIndex(Vector2Int coordinates) => coordinates.y * mapSize.x + coordinates.x;
        /// <summary>Переводит индекс клетки в 2D координаты.</summary>
        /// <param name="index">Индекс клетки</param>
        public Vector2Int ConvertIndexToVector2Int(int index) => new Vector2Int(index % mapSize.x, index / mapSize.y);
        /// <summary>Возвращает истину, если переданные координаты находятся в пределах карты.</summary>
        /// <param name="coordinates">Координаты клетки</param>
        public bool IsInMapBounds(Vector2Int coordinates) => coordinates.x >= 0 && coordinates.x < mapSize.x && coordinates.y >= 0 && coordinates.y < mapSize.y;
        /// <summary>Возвращает истину, если переданный индекс не меньше 0 и не больше или равен кол-ву клеток.</summary>
        /// <param name="index">Индекс клетки</param>
        public bool IsInMapBounds(int index) => index >= 0 && index < CellCount;
        #endregion
    }
}