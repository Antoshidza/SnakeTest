using UnityEngine;
using UnityEngine.UI;

namespace SnakeGameFirst
{
    /// <summary>
    /// Отображает данные об очках игрока в конце игры
    /// </summary>
    public class PlayerFinalScoreDisplay : MonoBehaviour
    {
        [SerializeField]
        private Text scoreText;
        [SerializeField]
        private GameController gameController;

        public void UpdateScoreText() => scoreText.text = $"score: {gameController.Player.Score.Count}";
    }
}