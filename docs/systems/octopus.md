# Осьминог (игрок)

**Статус:** план
**Этап:** [ROADMAP](../../ROADMAP.md) — этапы 2–4, 10
**Код:** `Assets/Scripts/Gameplay/Octopus/`, `Assets/Scripts/Gameplay/Tentacles/`, `Assets/Scripts/Input/`

## Назначение

Персонаж игрока: передвижение, физические щупальца, хватание, маскировка.

## Как работает

```
OctopusInputReader ──IOctopusInput──┬─→ OctopusLocomotion   (тело: Rigidbody)
  (только у владельца)              ├─→ OctopusGrabber      (выбор щупальца, захват)
                                    └─→ OctopusCamouflage   (окрас)
                                              │
TentacleRig  ←── цели захвата ────────────────┘
  (8 × Tentacle, физика локально)
OctopusAnimator ← скорость/состояние → Animator (Idle, Crawl, Hide, Peek)
```

### Планируемый интерфейс ввода

```csharp
public interface IOctopusInput
{
    Vector2 Move { get; }
    Vector2 Look { get; }
    event Action GrabPressed;
    event Action ReleasePressed;
    event Action InteractPressed;
}
```

### Хватание (идея)

1. ЛКМ → ищем ближайший `IGrabbable` в радиусе перед камерой.
2. Выбираем свободное щупальце, чей кончик ближе всего к цели.
3. Кончик щупальца тянется к точке захвата и фиксируется (joint).
4. ПКМ → отпустить. С движением — бросок.

## Сеть

- Тело: owner authority, синхронизация позиции.
- Щупальца: локальная физика у всех. По сети передаётся только `NetworkVariable` с целью захвата для каждого занятого щупальца.
- Схваченный предмет: владение передаётся хватающему.

## Настройки

`OctopusConfig` (ScriptableObject): скорость, ускорение, радиус хватания, сила броска, параметры джоинтов щупалец.

## Известные проблемы и TODO

- Решить, как смешивать анимацию тела с физикой щупалец (этап 3).
