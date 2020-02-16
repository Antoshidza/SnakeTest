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
        [SerializeField]
        public UnityEvent OnLose;

        public Predicate<Vector2Int> OnSetDirection;

        private void Update() => HandleInput();
        ///<summary>Обрабатывает пользовательский ввод: меняет направление движение змейки по нажатию на клавиши стрелок.</summary>
        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                OnSetDirection.Invoke(new Vector2Int(0, 1));
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                OnSetDirection.Invoke(new Vector2Int(0, -1));
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                OnSetDirection.Invoke(new Vector2Int(-1, 0));
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                OnSetDirection.Invoke(new Vector2Int(1, 0));
        }
        public void Lose() => OnLose?.Invoke();
    }
}