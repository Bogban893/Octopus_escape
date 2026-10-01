# Соглашения

## Код C#

### Именование

| Что | Стиль | Пример |
|---|---|---|
| Классы, структуры, enum | PascalCase | `OctopusGrabber`, `AlarmLevel` |
| Интерфейсы | `I` + PascalCase | `IGrabbable` |
| Методы, свойства, события | PascalCase | `TryGrab()`, `IsHolding`, `LevelChanged` |
| Приватные поля | `_camelCase` | `_rigidbody` |
| Поля для Inspector | `[SerializeField] private` + `_camelCase` | `[SerializeField] private float _speed;` |
| Локальные переменные, параметры | camelCase | `targetPosition` |
| Константы | PascalCase | `MaxTentacles` |

### Правила

- **Один класс — один файл**, имя файла = имя класса (Unity этого требует для MonoBehaviour).
- Namespace повторяет путь: `Scripts/Gameplay/AI/` → `OctopusEscape.Gameplay.AI`.
- Поля для Inspector — `[SerializeField] private`, а не `public`. Снаружи — свойство только для чтения.
- Ссылки на компоненты получаем в `Awake`, не в `Update`. `GetComponent` / `FindObjectOfType` в `Update` — нельзя.
- Настройки — в ScriptableObject-конфигах, не магические числа в коде.
- Комментарии объясняют **зачем**, а не **что**.

### Порядок в классе

```csharp
public class Example : MonoBehaviour
{
    // 1. константы
    // 2. [SerializeField] поля
    // 3. приватные поля
    // 4. события и свойства
    // 5. Unity-методы (Awake, OnEnable, Start, Update, FixedUpdate, OnDisable)
    // 6. публичные методы
    // 7. приватные методы
}
```

### Сеть

- Методы RPC заканчиваются на `Rpc`: `RequestOpenRpc()`.
- В начале метода явно проверяем, где он выполняется: `if (!IsServer) return;`.
- Физика — в `FixedUpdate`, ввод — в `Update`.

## Unity

- **Перемещать, переименовывать и удалять ассеты — только внутри Unity.** У каждого файла есть `.meta` с GUID, по которому на него ссылаются сцены и префабы.
- `.meta` файлы **всегда коммитятся** вместе с ассетом.
- Объекты на сцене по возможности — префабы. Правки делаем в префабе, а не в экземпляре на сцене.
- Не редактировать одну сцену вдвоём одновременно — merge сцен почти невозможен.
- Имена ассетов: PascalCase, с префиксом типа, где помогает: `M_OctopusSkin` (материал), `T_OctopusSkin_Albedo` (текстура), `SO_OctopusConfig` (ScriptableObject).

## Git

- Коммиты небольшие, по одной задаче.
- Сообщения в стиле: `feat: хватание предметов`, `fix: камера дёргается у клиента`, `docs: обновил AI`.
- Тяжёлые файлы (модели, текстуры, звук, шрифты, PDF) хранятся в Git LFS — список форматов в `.gitattributes`.
- Переводы строк везде `LF` (задано в `.gitattributes`). Имена файлов не должны отличаться только регистром (Linux vs Windows).
- Перед коммитом: проект компилируется, в консоли Unity нет ошибок.
