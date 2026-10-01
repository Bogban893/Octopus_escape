using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace OctopusEscape.Network
{
    /// <summary>
    /// Временное меню для запуска хоста и подключения клиента.
    /// Заменится настоящим главным меню на этапе 9.
    /// </summary>
    public class NetworkDebugMenu : MonoBehaviour
    {
        // Хост слушает все сетевые интерфейсы, чтобы к нему можно было подключиться из локальной сети.
        private const string ListenAddress = "0.0.0.0";

        [SerializeField] private string _address = "127.0.0.1";
        [SerializeField] private ushort _port = 7777;

        private void Start()
        {
            Debug.Log($"[NetworkDebugMenu] Меню запущено. NetworkManager найден: {NetworkManager.Singleton != null}", this);
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 240, 200), GUI.skin.box);

            NetworkManager manager = NetworkManager.Singleton;
            if (manager == null)
            {
                GUILayout.Label("NetworkManager не найден на сцене");
            }
            else if (!manager.IsClient && !manager.IsServer)
            {
                DrawStartButtons(manager);
            }
            else
            {
                DrawStatus(manager);
            }

            GUILayout.EndArea();
        }

        private void DrawStartButtons(NetworkManager manager)
        {
            GUILayout.Label("Адрес хоста:");
            _address = GUILayout.TextField(_address);

            if (GUILayout.Button("Host"))
            {
                ApplyConnectionData(manager);
                bool started = manager.StartHost();
                Debug.Log($"[NetworkDebugMenu] StartHost на порту {_port}: {(started ? "успешно" : "ОШИБКА")}", this);
            }

            if (GUILayout.Button("Join"))
            {
                ApplyConnectionData(manager);
                bool started = manager.StartClient();
                Debug.Log($"[NetworkDebugMenu] StartClient к {_address}:{_port}: {(started ? "подключаюсь..." : "ОШИБКА")}", this);
            }
        }

        private static void DrawStatus(NetworkManager manager)
        {
            GUILayout.Label(manager.IsHost ? "Режим: Host" : "Режим: Client");
            GUILayout.Label($"Мой ID: {manager.LocalClientId}");

            if (GUILayout.Button("Disconnect"))
            {
                manager.Shutdown();
            }
        }

        private void ApplyConnectionData(NetworkManager manager)
        {
            var transport = manager.GetComponent<UnityTransport>();
            transport.SetConnectionData(_address, _port, ListenAddress);
        }
    }
}
