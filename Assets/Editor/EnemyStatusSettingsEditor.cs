using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyStatusSettings))]
public class EnemyStatusSettingsEditor : Editor
{
    enum SortField
    {
        BehaviorType,
        TankName,
        MaxHP,
        MaxMoveSpeed,
        RewardScore
    }

    static SortField s_selectedField = SortField.BehaviorType;
    static bool s_sortAscending = true;

    public override void OnInspectorGUI()
    {
        try
        {
            DrawDefaultInspector();

            EnemyStatusSettings settings = (EnemyStatusSettings)target;
            if (settings == null) return;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("エディタ操作", EditorStyles.boldLabel);

            // ID自動生成ボタン（既存）
            if (GUILayout.Button("全EnemyParamのIDを自動生成（現在のリスト順）"))
            {
                if (settings.enemyParamList == null || settings.enemyParamList.Count == 0)
                {
                    Debug.LogWarning("enemyParamListが空です。IDの自動生成を中止します。");
                    return;
                }

                Undo.RecordObject(settings, "Generate EnemyParam IDs");
                // ナンバリングでIDを生成 (Tank_001, Tank_002, ...)
                for (int i = 0; i < settings.enemyParamList.Count; i++)
                {
                    var param = settings.enemyParamList[i];
                    if (param == null) continue;
                    param.GenerateID(i + 1);
                }

                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
            }

            // 新: マルチキーでソートしてIDを割り当てるボタン
            if (GUILayout.Button("BehaviorType→SpecialFlags→WeaponTypeでソートしてID割り当て"))
            {
                if (settings.enemyParamList == null || settings.enemyParamList.Count == 0)
                {
                    Debug.LogWarning("enemyParamListが空です。処理を中止します。");
                }
                else
                {
                    Undo.RecordObject(settings, "Sort By Behavior/Special/Weapon and Assign IDs");

                    // 安定的に複数キーでソート（BehaviorType, SpecialFlags, WeaponType, TankName）
                    settings.enemyParamList.Sort((a, b) =>
                    {
                        if (a == null && b == null) return 0;
                        if (a == null) return -1;
                        if (b == null) return 1;

                        int cmp = ((int)a.BehaviorType).CompareTo((int)b.BehaviorType);
                        if (cmp != 0) return cmp;

                        cmp = ((int)a.SpecialFlags).CompareTo((int)b.SpecialFlags);
                        if (cmp != 0) return cmp;

                        cmp = ((int)a.WeaponType).CompareTo((int)b.WeaponType);
                        if (cmp != 0) return cmp;

                        return string.Compare(a?._tankName, b?._tankName, System.StringComparison.Ordinal);
                    });

                    // ソート方向が降順の場合は逆順にする
                    if (!s_sortAscending)
                    {
                        settings.enemyParamList.Reverse();
                    }

                    // ソート後にナンバリングIDを付与
                    for (int i = 0; i < settings.enemyParamList.Count; i++)
                    {
                        var param = settings.enemyParamList[i];
                        if (param == null) continue;
                        param.GenerateID(i + 1);
                    }

                    EditorUtility.SetDirty(settings);
                    AssetDatabase.SaveAssets();
                }
            }

            EditorGUILayout.Space();

            // ソートUI
            EditorGUILayout.BeginHorizontal();
            s_selectedField = (SortField)EditorGUILayout.EnumPopup("ソート項目", s_selectedField);
            s_sortAscending = EditorGUILayout.ToggleLeft("昇順", s_sortAscending, GUILayout.Width(80));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("ソート実行"))
            {
                if (settings.enemyParamList == null || settings.enemyParamList.Count == 0)
                {
                    Debug.LogWarning("enemyParamListが空です。ソートを中止します。");
                }
                else
                {
                    Undo.RecordObject(settings, "Sort EnemyParamList");
                    SortEnemyParamList(settings.enemyParamList, s_selectedField, s_sortAscending);
                    EditorUtility.SetDirty(settings);
                    AssetDatabase.SaveAssets();
                }
            }

            // クイックソートボタン（よく使いそうな項目）
            if (GUILayout.Button("HPで昇順"))
            {
                if (settings.enemyParamList != null && settings.enemyParamList.Count > 0)
                {
                    Undo.RecordObject(settings, "Sort EnemyParamList by HP Asc");
                    SortEnemyParamList(settings.enemyParamList, SortField.MaxHP, true);
                    EditorUtility.SetDirty(settings);
                    AssetDatabase.SaveAssets();
                }
            }
            if (GUILayout.Button("移動速度で降順"))
            {
                if (settings.enemyParamList != null && settings.enemyParamList.Count > 0)
                {
                    Undo.RecordObject(settings, "Sort EnemyParamList by MoveSpeed Desc");
                    SortEnemyParamList(settings.enemyParamList, SortField.MaxMoveSpeed, false);
                    EditorUtility.SetDirty(settings);
                    AssetDatabase.SaveAssets();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"エラー\n{ex.Message}");
        }
    }

    static void SortEnemyParamList(System.Collections.Generic.List<EnemyParam> list, SortField field, bool ascending)
    {
        if (list == null) return;

        System.Comparison<EnemyParam> comparison = null;
        switch (field)
        {
            case SortField.BehaviorType:
                comparison = (a, b) =>
                {
                    if (a == null || b == null) return 0;
                    return ((int)a.BehaviorType).CompareTo((int)b.BehaviorType);
                };
                break;
            case SortField.TankName:
                comparison = (a, b) => string.Compare(a?._tankName, b?._tankName, System.StringComparison.Ordinal);
                break;
            case SortField.MaxHP:
                comparison = (a, b) => a == null || b == null ? 0 : a.MaxHP.CompareTo(b.MaxHP);
                break;
            case SortField.MaxMoveSpeed:
                comparison = (a, b) => a == null || b == null ? 0 : a.MaxMoveSpeed.CompareTo(b.MaxMoveSpeed);
                break;
            case SortField.RewardScore:
                comparison = (a, b) => a == null || b == null ? 0 : a.RewardScore.CompareTo(b.RewardScore);
                break;
        }

        if (comparison == null) return;

        if (ascending)
            list.Sort(comparison);
        else
            list.Sort((a, b) => -comparison(a, b));
    }
}