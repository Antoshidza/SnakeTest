using UnityEngine;
using UnityEngine.Events;

namespace SnakeGameFirst
{
    /// <summary>
    /// Медиатор, предоставляющий:
    /// <list type="bullet">
    /// <item>
    /// <description>Обработку пользовательского Input</description>
    /// </item>
    /// <item>
    /// <description>Событие проигрыша</description>
    /// </item>
    /// <item>
    /// <description>Инструменты управления данными игрока</description>
    /// </item>
    /// </list>
    /// </summary>
    public class Player : MonoBehaviour
    {
        private Snake snake;
        public Score Score { get; private set; }

        [SerializeField]
        private UnityEvent OnLose;

        private void Update() => HandleInput();
        ///<summary>Обрабатывает пользовательский ввод: меняет направление движение змейки по нажатию на клавиши стрелок.</summary>
        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                snake.TryChangeMoveVector(new Vector2Int(0, 1));
            else if(Input.GetKeyDown(KeyCode.DownArrow))
                snake.TryChangeMoveVector(new Vector2Int(0, -1));
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                snake.TryChangeMoveVector(new Vector2Int(-1, 0));
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                snake.TryChangeMoveVector(new Vector2Int(1, 0));
        }
        /// <param name="map">Инстанс карты</param>
        /// <param name="spawnPosition">Стартовая позиция змейки</param>
        /// <param name="snakeStepInterval">Временой интервал одного шага змейки</param>
        /// <param name="snakeLength">Стартовая длина змейки</param>
        public void SetPlayer(Map map, Vector2Int spawnPosition, float snakeStepInterval = 1f, int snakeLength = 3)
        {
            snake = new Snake(map, spawnPosition, snakeStepInterval, snakeLength != 0 ? snakeLength : 1);
            snake.OnDie += Lose;
            Score = new Score(snake, 10f); //Hardcoded bonus reset interval
        }
        /// <param name="map">Инстанс карты</param>
        /// <param name="spawnPosition">Стартовая позиция змейки</param>
        /// <param name="configurationData">Данные настройки игрока</param>
        public void SetPlayer(Map map, Vector2Int spawnPosition, PlayerConfigurationData configurationData) =>
            SetPlayer(map, spawnPosition, configurationData.snakeStepInterval, configurationData.startSnakeLength);
        private void Lose() => OnLose?.Invoke();
        /// <summary>Сбрасывает данные игрока: сбрасывает данные змейки и данные очков игрока.</summary>
        /// <param name="spawnIndexPosition">Новая позиция змейки</param>
        public void Reset(int spawnIndexPosition)
        {
            snake.Reset(spawnIndexPosition, 3);
            Score.Reset();
        }
    }
}