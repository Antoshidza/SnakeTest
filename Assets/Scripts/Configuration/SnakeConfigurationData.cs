using UnityEngine;

namespace SnakeGameFirst
{
    /// <summary>Содержит данные для настройки игрока.</summary>
    [CreateAssetMenu(fileName = "new player configuration", menuName = "PlayerConfigurationData")]
    public class SnakeConfigurationData : ScriptableObject
    {
        [Header("Snake settings")]
        [Tooltip("Стартовая длина змейки")]
        public int startLength;
        [Tooltip("Временной интервал шага змейки")]
        public float stepInterval;
    }
}
