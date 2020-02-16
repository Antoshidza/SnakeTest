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
        private SnakeConfigurationData snakeConfigurationData;
        
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

        public Map Map { get; private set; }
        public Snake Snake { get; private set; }
        public Score Score { get; private set; }

        private void Start() => StartGame();
        private void Update() => UpdateGame(Time.deltaTime);
        private void UpdateGame(float deltaTime)
        {
            Map.Update(deltaTime);
            Snake.Update(deltaTime);
            Score.Update(deltaTime);
        }
        /// <summary>Запускает игру.</summary>
        public void StartGame()
        {
            SetGame();
            OnGameReady?.Invoke();
            OnGamePlay?.Invoke();
        }
        ///<summary>Начинает игру занового.</summary>
        public void ResetGame()
        {
            //gameMediator.ResetGame();
            Map.Reset();
            Snake.Reset(Map.CenterIndexPosition);
            Score.Reset();

            OnGamePlay?.Invoke();
        }
        ///<summary>Заканчивает игру.</summary>
        public void GameOver() => OnGameOver?.Invoke();
        /// <summary>Настраивает игру</summary>
        public void SetGame()
        {
            Map = new Map(mapConfigurationData);
            Snake = new Snake(Map, snakeConfigurationData, Map.CenterIndexPosition);
            Score = new Score();

            Snake.OnFeed += Score.IncreaseCount;
            Snake.OnFeed += Score.IncreaseBonus;
            Snake.OnDie += player.Lose;
            player.OnSetDirection += Snake.TryChangeMoveVector;
        }
    }
}