using UnityEngine;

namespace SnakeGameFirst
{
    /// <summary>Содержит данные для настройки игровой карты.</summary>
    [CreateAssetMenu(fileName ="new map configuration", menuName ="MapConfigurationData")]
    public class MapConfigurationData : ScriptableObject
    {
        [Header("Map settings")]
        [Tooltip("Размер карты")]
        public Vector2Int mapSize;
        [Tooltip("Максимальное кол-во еды на карте единовременно")]
        public int maxFoodCount;
        [Tooltip("Временной интервал генерации еды на карте")]
        public float foodGenerateInterval;
    }
}
