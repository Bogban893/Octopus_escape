using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OctopusEscape.Prototype
{
    /// <summary>
    /// Временный игрок-заглушка для проверки сети: двигается на WASD и красится в свой цвет.
    /// Удаляется на этапе 2, когда появится настоящий осьминог.
    /// </summary>
    /// <remarks>
    /// Позицию синхронизирует NetworkTransform (Authority Mode = Owner),
    /// поэтому скрипту достаточно двигать объект только у его владельца.
    /// </remarks>
    public class PlayerStub : NetworkBehaviour
    {
        private static readonly Color[] PlayerColors =
        {
            new Color(0.62f, 0.40f, 0.85f), // фиолетовый
            new Color(0.95f, 0.55f, 0.70f), // розовый
            new Color(0.40f, 0.75f, 0.85f),
            new Color(0.95f, 0.75f, 0.35f),
        };

        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _spawnSpacing = 2f;
        [SerializeField] private Renderer _renderer;

        // Вызывается редактором при добавлении компонента (или по «Reset» в меню компонента):
        // сам находит Renderer, чтобы не перетаскивать его вручную.
        private void Reset()
        {
            _renderer = GetComponentInChildren<Renderer>();
        }

        public override void OnNetworkSpawn()
        {
            // OwnerClientId одинаков у всех участников, поэтому каждый сам вычислит один и тот же цвет:
            // синхронизировать его по сети не нужно.
            _renderer.material.color = PlayerColors[OwnerClientId % (ulong)PlayerColors.Length];

            // Позицией управляет владелец, поэтому только он расставляет себя, чтобы игроки не появились друг в друге.
            if (IsOwner)
            {
                transform.position += Vector3.right * (OwnerClientId * _spawnSpacing);
            }
        }

        private void Update()
        {
            // Чужими игроками управляет их владелец, мы их только видим.
            if (!IsOwner)
            {
                return;
            }

            Vector2 input = ReadMoveInput();
            var direction = new Vector3(input.x, 0f, input.y);
            transform.position += direction * (_speed * Time.deltaTime);
        }

        // Прямое чтение клавиатуры допустимо только в заглушке.
        // В настоящем осьминоге ввод пойдёт через IOctopusInput (см. docs/ARCHITECTURE.md).
        private static Vector2 ReadMoveInput()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return Vector2.zero;
            }

            var input = new Vector2(
                (keyboard.dKey.isPressed ? 1f : 0f) - (keyboard.aKey.isPressed ? 1f : 0f),
                (keyboard.wKey.isPressed ? 1f : 0f) - (keyboard.sKey.isPressed ? 1f : 0f));

            return Vector2.ClampMagnitude(input, 1f);
        }
    }
}
