using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace OctopusEscape.EditorTools
{
    /// <summary>
    /// Пункт меню «Octopus Escape → Place In Scene» (правый клик по префабу или модели в окне Project).
    /// Если открыт префаб (Prefab Mode) — добавляет ассет дочерним объектом его корня, иначе — на сцену.
    /// Нужен, потому что под Wayland перетаскивание из Project в сцену часто не работает.
    /// </summary>
    internal static class PlaceInSceneMenu
    {
        private const string MenuPath = "Assets/Octopus Escape/Place In Scene";

        [MenuItem(MenuPath, false, 0)]
        private static void Place()
        {
            var asset = Selection.activeObject as GameObject;
            if (asset == null)
            {
                return;
            }

            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            Transform parent = prefabStage != null ? prefabStage.prefabContentsRoot.transform : null;

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(asset, parent);
            Undo.RegisterCreatedObjectUndo(instance, $"Place {asset.name}");
            Selection.activeGameObject = instance;
        }

        [MenuItem(MenuPath, true)]
        private static bool CanPlace()
        {
            return Selection.activeObject is GameObject go && EditorUtility.IsPersistent(go);
        }
    }
}
