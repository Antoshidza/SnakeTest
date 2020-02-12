using UnityEngine;
using System;

namespace SnakeGameFirst
{
    [Serializable]
    struct CellSpritePresentation
    {
        public CellContentType contentType;
        public Sprite sprite;
    }
    /// <summary>
    /// Используется для инициализации спрайтов для отображения карты
    /// </summary>
    public class CellSpriteInitializator : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Спрайты для визуализации карты")]
        private CellSpritePresentation[] presentationCollectionFromEditor;

        private static Sprite[] spriteCollection;

        public void InitializePresentationCollection()
        {
            spriteCollection = new Sprite[presentationCollectionFromEditor.Length];
            for (int i = 0; i < presentationCollectionFromEditor.Length; i++)
            {
                var currentCellPresentation = presentationCollectionFromEditor[i];
                spriteCollection[(int)currentCellPresentation.contentType] = currentCellPresentation.sprite;
            }
        }
        public static Sprite GetSprite(CellContentType contentType) => spriteCollection[(int)contentType];
    }
}