using UnityEngine;

namespace SnakeGameFirst
{
    /// <summary>
    /// Используется для отображение карты с помощью спрайтов
    /// </summary>
    public class MapPresentation : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Префаб для отображения клетки")]
        private GameObject cellPrefab;
        [SerializeField]
        [Tooltip("Расстояние между центрами клеток")]
        private Vector2 cellMargin;
        [SerializeField]
        [Tooltip("Инициализатор спрайтов для отображения клеток")]
        CellSpriteInitializator cellPresentation;
        [SerializeField]
        [Tooltip("Камера для отображения клеток")]
        private Camera camera;
        [SerializeField]
        [Tooltip("Monobehaviour GameController на сцене")]
        private GameController gameController;

        private SpriteRenderer[] cellRenderers;

        private void DisplayMap(Map map)
        {
            for (int i = 0; i < cellRenderers.Length; i++)
                ChangeCellSprite(i, map.GetCellContentType(i));
            FitGridInCameraView();
        }
        public void StartPresentation()
        {
            cellPresentation.InitializePresentationCollection();
            Destroy(cellPresentation);

            SpawnGrid(gameController.Map);
            gameController.Map.OnChangeCellContent += ChangeCellSprite;
            DisplayMap(gameController.Map);
        }
        private void SpawnGrid(Map map)
        {
            cellRenderers = new SpriteRenderer[map.CellCount];
            var index = 0;
            for (int y = 0; y < map.mapSize.y; y++)
            {
                for (int x = 0; x < map.mapSize.x; x++)
                {
                    var newCell = Instantiate(cellPrefab, new Vector3(x * cellMargin.x, y * cellMargin.y, 0), Quaternion.identity, transform);
                    cellRenderers[index] = newCell.GetComponent<SpriteRenderer>();
                    newCell.name = $"({x};{y})";
                    index++;
                }
            }
        }
        private void ChangeCellSprite(int indexPosition, CellContentType contentType) => cellRenderers[indexPosition].sprite = CellSpriteInitializator.GetSprite(contentType);
        /// <summary>
        /// Настроивает камеру под обзор игровой карты
        /// </summary>
        private void FitGridInCameraView()
        {
            if (cellRenderers.Length != 0)
            {
                
                var bounds = cellRenderers[0].bounds;
                for (int i = 1; i < cellRenderers.Length; i++)
                    bounds.Encapsulate(cellRenderers[i].bounds);
                var boundsCenter = bounds.center;
                camera.transform.position = new Vector3(boundsCenter.x, boundsCenter.y, camera.transform.position.z); 
                camera.orthographicSize = Mathf.Max(bounds.size.x * Screen.height / Screen.width * 0.5f, bounds.size.y * Screen.width / Screen.height * 0.3f) ;
            }
        }
    }
}