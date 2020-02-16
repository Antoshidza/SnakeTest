using UnityEngine;
using UnityEngine.Events;
using System;

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
        public Snake Snake { get; private set; }
        public Score Score { get; private set; }

        [SerializeField]
        public UnityEvent OnLose;

        public Predicate<Vector2Int> OnSetDirection;

        private void Update() => HandleInput();
        ///<summary>Обрабатывает пользовательский ввод: меняет направление движение змейки по нажатию на клавиши стрелок.</summary>
        private void HandleInput()
        {
            //if (Input.GetKeyDown(KeyCode.UpArrow))
            //    Snake.TryChangeMoveVector(new Vector2Int(0, 1));
            //else if(Input.GetKeyDown(KeyCode.DownArrow))
            //    Snake.TryChangeMoveVector(new Vector2Int(0, -1));
            //else if (Input.GetKeyDown(KeyCode.LeftArrow))
            //    Snake.TryChangeMoveVector(new Vector2Int(-1, 0));
            //else if (Input.GetKeyDown(KeyCode.RightArrow))
            //    Snake.TryChangeMoveVector(new Vector2Int(1, 0));

            if (Input.GetKeyDown(KeyCode.UpArrow))
                OnSetDirection.Invoke(new Vector2Int(0, 1));
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                OnSetDirection.Invoke(new Vector2Int(0, -1));
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                OnSetDirection.Invoke(new Vector2Int(-1, 0));
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                OnSetDirection.Invoke(new Vector2Int(1, 0));
        }
        /// <param name="map">Инстанс карты</param>
        /// <param name="spawnPosition">Стартовая позиция змейки</param>
        /// <param name="snakeStepInterval">Временой интервал одного шага змейки</param>
        /// <param name="snakeLength">Стартовая длина змейки</param>
        public void SetPlayer(Map map, Vector2Int spawnPosition, float snakeStepInterval = 1f, int snakeLength = 3)
        {
            Snake = new Snake(map, spawnPosition, snakeStepInterval, snakeLength != 0 ? snakeLength : 1);
            Score = new Score(10f); //Hardcoded bonus reset interval
            Snake.OnFeed += Score.IncreaseCount;
            Snake.OnFeed += Score.IncreaseBonus;
        }
        /// <param name="map">Инстанс карты</param>
        /// <param name="spawnPosition">Стартовая позиция змейки</param>
        /// <param name="configurationData">Данные настройки игрока</param>
        public void SetPlayer(Map map, Vector2Int spawnPosition, SnakeConfigurationData configurationData) =>
            SetPlayer(map, spawnPosition, configurationData.stepInterval, configurationData.startLength);
        public void Lose() => OnLose?.Invoke();
        /// <summary>Сбрасывает данные игрока: сбрасывает данные змейки и данные очков игрока.</summary>
        /// <param name="spawnIndexPosition">Новая позиция змейки</param>
        public void Reset(int spawnIndexPosition)
        {
            Snake.Reset(spawnIndexPosition, 3);
            Score.Reset();
        }
        /// <summary>Вызывает <see cref="SnakeGameFirst.Snake">Snake</see>.Update() и <see cref="Score">Score</see>.Update()</summary>
        public void Update(float deltaTime)
        {
            Snake.Update(deltaTime);
            Score.Update(deltaTime);
        }
    }
}