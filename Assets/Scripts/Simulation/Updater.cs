using System;
using UnityEngine;

namespace SnakeGameFirst
{
    /// <summary>Используется для покадрового обновления любых сущностей</summary>
    public class Updater : MonoBehaviour
    {
        public static event Action OnUpdate;
        public static event Action<float> OnUpdateWithTime;

        private void Update()
        {
            OnUpdate?.Invoke();
            OnUpdateWithTime?.Invoke(Time.deltaTime);
        }
    }
}