using UnityEngine;
using UnityEngine.UI;

namespace SnakeGameFirst
{
    /// <summary>Отображает данные о победных очках игрока</summary>
    public class PlayerStatisticDisplay : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Текст для отображения очков игрока")]
        private Text scoreCountText;
        [SerializeField]
        [Tooltip("Текст для отображения бонуса игрока")]
        private Text scoreBonusText;
        [SerializeField]
        [Tooltip("Monobehaviour GameController на сцене")]
        private GameController gameController;

        public void StartDisplay()
        {
            UpdateCountText(gameController.Score.Count);
            UpdateBonusText(gameController.Score.Bonus);
            gameController.Score.OnCountChange += UpdateCountText;
            gameController.Score.OnBonusChange += UpdateBonusText;
        }
        private void UpdateCountText(int value) => scoreCountText.text = $"ОЧКИ: {value}";
        private void UpdateBonusText(int value) => scoreBonusText.text = $"x {value}";
    }
}