using System;

namespace SnakeGameFirst
{
    /// <summary>Таймер, обновляющийся внутри класса Updater. Вызывает событие по достижению установленного времени.</summary>
    class Timer
    {
        /// <summary>Текуще значение таймера</summary>
        private float value;
        /// <summary>Временой интервал срабатывания таймера</summary>
        private float interval;

        /// <summary>Вызывается, когда значение таймера достигает установленного интервала</summary>
        public event Action OnTime;

        /// <param name="interval">Временой интервал срабатывания таймера</param>
        public Timer(float interval = 1f) => this.interval = interval;
        /// <summary>Увеличивает значение таймера. Если досигнуто значение, установленного интервала, вызывается OnTime</summary>
        public void Update(float time)
        {
            value += time;
            if (value > interval)
            {
                value -= interval;
                OnTime?.Invoke();
            }
        }
        public void Reset() => value = 0;
    }
}
