using UnityEditor;
using UnityEngine;

namespace OctopusEscape.EditorTools
{
    /// <summary>
    /// Пункт меню «Octopus Escape → Save As Prefab» (правый клик по объекту в Hierarchy).
    /// Сохраняет объект сцены как префаб в Assets/Prefabs.
    /// Нужен, потому что под Wayland перетаскивание из Hierarchy в Project часто не работает.
    /// </summary>
    internal static class SaveAsPrefabMenu
    {
        private const string MenuPath = "GameObject/Octopus Escape/Save As Prefab";
        private const string PrefabFolder = "Assets/Prefabs";

        // При выделении нескольких объектов Unity вызывает пункт контекстного меню
        // отдельно для каждого из них и передаёт объект в command.context.
        [MenuItem(MenuPath, false, 0)]
        private static void Save(MenuCommand command)
        {
            GameObject target = command.context as GameObject;
            if (target == null)
            {
                target = Selection.activeGameObject;
            }

            if (target == null || EditorUtility.IsPersistent(target))
            {
                return;
            }

            if (!AssetDatabase.IsValidFolder(PrefabFolder))
            {
                AssetDatabase.CreateFolder("Assets", "Prefabs");
            }

            string path = AssetDatabase.GenerateUniqueAssetPath($"{PrefabFolder}/{target.name}.prefab");
            PrefabUtility.SaveAsPrefabAssetAndConnect(target, path, InteractionMode.UserAction);
            Debug.Log($"Префаб сохранён: {path}", target);
        }

        [MenuItem(MenuPath, true)]
        private static bool CanSave()
        {
            GameObject selected = Selection.activeGameObject;
            return selected != null && !EditorUtility.IsPersistent(selected);
        }
    }
}
