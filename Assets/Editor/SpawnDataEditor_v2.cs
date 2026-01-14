using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnData))]
public class SpawnDataEditor_v2 : Editor
{
    SpawnData _data;
    EnemyStatusSettings _autoFoundSettings;
    EnemyStatusSettings _overrideSettings;

    void OnEnable()
    {
        _data = (SpawnData)target;
        AutoFindSettings();
    }

    void AutoFindSettings()
    {
        var guids = AssetDatabase.FindAssets("t:EnemyStatusSettings");
        if (guids != null && guids.Length > 0)
        {
            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            _autoFoundSettings = AssetDatabase.LoadAssetAtPath<EnemyStatusSettings>(path);
        }
        else
        {
            _autoFoundSettings = null;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (_data == null) return;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("SpawnData エディタ", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        _overrideSettings = (EnemyStatusSettings)EditorGUILayout.ObjectField("Enemy Settings", _overrideSettings, typeof(EnemyStatusSettings), false);
        if (GUILayout.Button("Auto Find", GUILayout.Width(80)))
        {
            AutoFindSettings();
            _overrideSettings = null;
            Repaint();
        }
        EditorGUILayout.EndHorizontal();

        var settings = GetSettings();
        if (settings == null)
        {
            EditorGUILayout.HelpBox("EnemyStatusSettings がプロジェクトに見つかりません。アセットを割り当てしてください。", MessageType.Warning);
        }

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Spawn"))
        {
            Undo.RecordObject(_data, "Add SpawnParam");
            _data.SpawnParamList.Add(new SpawnParam());
            EditorUtility.SetDirty(_data);
            AssetDatabase.SaveAssets();
        }
        if (GUILayout.Button("Clear All"))
        {
            if (EditorUtility.DisplayDialog("Confirm", "SpawnParamList を全てクリアしますか?", "Yes", "No"))
            {
                Undo.RecordObject(_data, "Clear SpawnParamList");
                _data.SpawnParamList.Clear();
                EditorUtility.SetDirty(_data);
                AssetDatabase.SaveAssets();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        for (int i = 0; i < _data.SpawnParamList.Count; i++)
        {
            var item = _data.SpawnParamList[i];
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"[{i}]", GUILayout.Width(30));

            string currentId = item.spawnTankID ?? "";
            EditorGUILayout.LabelField("ID:", GUILayout.Width(20));
            EditorGUILayout.SelectableLabel(currentId, GUILayout.Height(16));

            if (GUILayout.Button("Select", GUILayout.Width(60)))
            {
                ShowSelectionMenu(i);
            }

            if (GUILayout.Button("Clear", GUILayout.Width(50)))
            {
                Undo.RecordObject(_data, "Clear Spawn ID");
                _data.SpawnParamList[i].spawnTankID = "";
                EditorUtility.SetDirty(_data);
                AssetDatabase.SaveAssets();
            }

            if (GUILayout.Button("Remove", GUILayout.Width(70)))
            {
                Undo.RecordObject(_data, "Remove SpawnParam");
                _data.SpawnParamList.RemoveAt(i);
                EditorUtility.SetDirty(_data);
                AssetDatabase.SaveAssets();
                i--;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                continue;
            }

            EditorGUILayout.EndHorizontal();

            if (settings != null)
            {
                var param = settings.GetParamByID(currentId);
                if (param != null)
                {
                    EditorGUILayout.LabelField($"Name: {param._tankName}   Behavior: {param.BehaviorType}   Weapon: {param.WeaponType}   HP: {param.MaxHP}");
                }
                else if (!string.IsNullOrEmpty(currentId))
                {
                    EditorGUILayout.LabelField($"ID '{currentId}' が見つかりません。", EditorStyles.helpBox);
                }
            }

            EditorGUILayout.EndVertical();
        }
    }

    EnemyStatusSettings GetSettings()
    {
        return _overrideSettings != null ? _overrideSettings : _autoFoundSettings;
    }

    void ShowSelectionMenu(int index)
    {
        var settings = GetSettings();
        var menu = new GenericMenu();
        if (settings == null || settings.enemyParamList == null || settings.enemyParamList.Count == 0)
        {
            menu.AddDisabledItem(new GUIContent("EnemyStatusSettings が見つからないかリストが空です"));
            menu.ShowAsContext();
            return;
        }

        var groups = settings.enemyParamList.GroupBy(p => p.BehaviorType).OrderBy(g => (int)g.Key);
        foreach (var group in groups)
        {
            // Header
            menu.AddDisabledItem(new GUIContent(group.Key.ToString()));
            foreach (var p in group)
            {
                string label = $"  {(p.Prefab != null ? p.Prefab.name : p._tankName)} ({p._tankID})";
                string id = p._tankID; // capture
                menu.AddItem(new GUIContent(label), false, () => OnSelect(index, id));
            }
        }

        menu.AddSeparator("");
        menu.AddItem(new GUIContent("(None)"), false, () => OnSelect(index, ""));
        menu.ShowAsContext();
    }

    void OnSelect(int index, string id)
    {
        if (_data == null) return;
        if (index < 0 || index >= _data.SpawnParamList.Count) return;
        Undo.RecordObject(_data, "Set Spawn ID");
        _data.SpawnParamList[index].spawnTankID = id;
        EditorUtility.SetDirty(_data);
        AssetDatabase.SaveAssets();
        Repaint();
    }
}
