using UnityEngine;
using UnityEngine.Events;

namespace SnakeGameFirst
{
    ///<summary>Медиатор для управления этапами игры начало/сброс/конец. Предоставляет доступ к данным игры.</summary>
    public class GameController : MonoBehaviour //Автоматически создает и запускает игру в Start()
    {
        #region Editor fields
        [SerializeField]
        [Tooltip("Monobehaviour Player на сцене")]
        private Player player;
        [SerializeField]
        [Tooltip("Настройки змейки")]
        private MapConfigurationData mapConfigurationData;
        [SerializeField]
        [Tooltip("Настройки игрока")]
        private PlayerConfigurationData playerConfigurationData;
        
        ///<summary>Вызывается, когда игра готова к старту (иницилизируется карта/игрок и т.д.).</summary>
        [SerializeField]
        private UnityEvent OnGameReady;
        ///<summary>Вызывается, когда начинается игровой процесс.</summary>
        [SerializeField]
        private UnityEvent OnGamePlay;
        ///<summary>Вызывается в момент проигрыша.</summary>
        [SerializeField]
        private UnityEvent OnGameOver;
        #endregion

        private GameMediator gameMediator;
        public Player Player { get => gameMediator.player; }
        public Map Map { get => gameMediator.map; }

        private void Start() => StartGame();
        /// <summary>Запускает игру.</summary>
        public void StartGame()
        {
            gameMediator = new GameMediator(mapConfigurationData, player, playerConfigurationData);
            OnGameReady?.Invoke();
            OnGamePlay?.Invoke();
        }
        ///<summary>Начинает игру занового.</summary>
        public void ResetGame()
        {
            gameMediator.ResetGame();
            OnGamePlay?.Invoke();
        }
        ///<summary>Заканчивает игру.</summary>
        public void GameOver() => OnGameOver?.Invoke();
    }
}