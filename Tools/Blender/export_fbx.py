"""
Экспорт модели из Blender в FBX с настройками для Unity.

Запуск (без открытия Blender):
    blender -b <модель>.blend --python Tools/Blender/export_fbx.py -- <путь>.fbx

Пример:
    blender -b Art/Source/Octopus.blend --python Tools/Blender/export_fbx.py -- Assets/Models/Octopus.fbx

В Unity у импортированной модели должна быть включена галочка
Model → Bake Axis Conversion (иначе модель будет повёрнута на -90°).
Подробности — docs/ASSETS.md.
"""

import os
import sys

import bpy

args = sys.argv[sys.argv.index("--") + 1:] if "--" in sys.argv else []
if len(args) != 1:
    sys.exit("Укажи путь к FBX после '--': ... --python export_fbx.py -- Assets/Models/Model.fbx")

output_path = os.path.abspath(args[0])

bpy.ops.export_scene.fbx(
    filepath=output_path,
    object_types={'ARMATURE', 'MESH'},
    # Масштаб: метры Blender = метры Unity, без x100 на корне.
    apply_unit_scale=True,
    apply_scale_options='FBX_SCALE_ALL',
    # Оси: поворот Z-up → Y-up запекает Unity (Bake Axis Conversion).
    # Экспериментальная опция Blender bake_space_transform ломает анимации скелета, поэтому выключена.
    axis_forward='-Z',
    axis_up='Y',
    bake_space_transform=False,
    use_mesh_modifiers=True,
    mesh_smooth_type='FACE',
    # Кости *_end — кончики щупалец, нужны для хватания.
    add_leaf_bones=True,
    # Все анимации (Actions) попадают в файл отдельными клипами.
    bake_anim=True,
    bake_anim_use_all_actions=True,
    bake_anim_use_nla_strips=False,
    bake_anim_force_startend_keying=True,
)

print(f"FBX экспортирован: {output_path}")
