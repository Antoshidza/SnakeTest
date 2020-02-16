# SnakeTest
Версия Unity: 2019.3.0f5

Тестовое задание: Змейка. Программная часть проекта разделена на три составляющих: Simulation/Presentation/Configuration. Вывод данных реализован с помощью SpriteRenderer.

# Simulation
Включает в себя программную часть симуляции игрового процесса:
* Map: Игровая карта определенного размера NxM. Включает в себя список содержимого клеток, событие изменения содержимого клетки.
* Snake: Содержит позиции частей тела змейки, события кормления и смерти змейки. Двигается/увеличивается/умирает.
* Score: Содержит данные иговых очков, события изменения счета и множителя очков. Увеличивает очки/изменяет множитель очков.
* Player: Содержит Snake и Score, событие проигрыша.
* FoodGenerator: Содержит данные о кол-ве еды на карте и генерирует новую еду в течении времени.
* **GameController**: Верхний уровень симуляции. Наиболее пригодный класс для редактирования. Содержит Player/Snake/Score/Map и предоставляет к ним доступ для презентации. Включает логику управления игрой: подготовка игры/начало игры/конец игры. Содержит UnityEvent.
* Прочие вспомогательные классы: Timer.

# Presentation
Включает в себя программную часть графического представления игровых данных:
* MapPresentation: Генерирует сетку из SpriteRenderer. Подписывается на событие изменения клеток Map, изменяя соответствующий SpriteRenderer.
* PlayerStatisticDisplay: Подписывает на события Score. Обновляет текст, отображающий текущие очки игрока и множитель очков.
* PlayerFinalScoreDisplay: Служит для отображения финального счета по окончанию игры.

# Configuration
Включает в себя классы для настройки игры и графического отображения:
* MapConfigurationData: Содержит данные настройки игровой карты.
* SnakeConfigurationData: Содержит данные настройки змейки.
* CellSpriteInitializator: Содержит коллекцию спрайтов для отображения каждого типа содержимого клеток.

# Использование UnityEvents
В данном случае использовал этот инструмент, т.к. при такой простой задаче он является компромиссным решением между удобством программирования и удобством сопровождения. При большой вероятности редактирования кода UnityEvents крайне неудобен, т.к. в Unity Editor не обновляются названия методов, и приходится устанавливать заново вручную.

# Игровой процесс
* Старт новой игры: змейка появляется по центру карты с длиной хвоста 3 блока (можно изменить в GameController).
* Шаг змейки: каждый шаг сопровождается увеличением змейки в сторону её направления и удаления последнего блока в хвосте.
* Попадание на клетку с едой: увеличивает счет игрока и отменяет удаление последнего блока змейки.
* Счет игрока: увеличивается каждый раз, когда змейка ест еду. Счет увеличивается на величину множителя очков. Кроме самих очков на 1 возрастает и множитель счета, однако, если змейка долго не ест, множитель очков сбрасывается до 1.
* Смерть змейки: происходит из-за попадания змейки на саму себя или из-за выхода за пределы карты. После смерти змейки можно начать игру снова.
* Управление змейкой: направление движения змейки изменяется стрелками. Змейка не может изменить направление на противоположное тому, которому она двигалась на предыдущем шаге.
![Gameplay](https://sun9-44.userapi.com/c858024/v858024587/17feb1/zJT57a-e2eA.jpg)
