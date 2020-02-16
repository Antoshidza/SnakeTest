using System;

namespace SnakeGameFirst
{
    /// <summary>Используется для представления победных очков игрока.</summary>
    public class Score
    {
        private int count;
        /// <summary>При увеличении счета игрока, счет увеличивается на величину bonus.</summary>
        private int bonus;
        /// <summary>Таймер сброса значения bonus.</summary>
        private Timer bonusResetTimer;

        public int Count
        {
            get => count;
            private set
            {
                count = value;
                OnCountChange?.Invoke(value);
            }
        }
        public int Bonus
        {
            get => bonus;
            private set
            {
                bonus = value;
                OnBonusChange?.Invoke(value);
            }
        }

        public event Action<int> OnCountChange;
        public event Action<int> OnBonusChange;

        /// <param name="snake">Инстанс змейки, за событиями которой следит Score.</param>
        /// <param name="bonusResetInterval">Временой интервал сброса множителя очков.</param>
        public Score(float bonusResetInterval = 5f)
        {
            bonus = 1;
            bonusResetTimer = new Timer(bonusResetInterval);
            bonusResetTimer.OnTime += ResetBonus;
        }
        private void ResetBonus() => Bonus = 1;
        public void IncreaseCount()
        {
            Count += bonus;
            OnCountChange?.Invoke(count);
        }
        public void IncreaseBonus()
        {
            Bonus++;
            bonusResetTimer.Reset();
        }
        /// <summary>Обнуляет очки, сбрасывает множитель до 1</summary>
        public void Reset()
        {
            Count = 0;
            ResetBonus();
        }
        public void Update(float deltaTime) => bonusResetTimer.Update(deltaTime);
    }
}
