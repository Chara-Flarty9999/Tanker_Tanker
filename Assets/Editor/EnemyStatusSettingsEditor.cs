using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyStatusSettings))]
public class EnemyStatusSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        try
        {
            DrawDefaultInspector();

            EnemyStatusSettings settings = (EnemyStatusSettings)target;
            if (settings == null) return;

            if (GUILayout.Button("全EnemyParamのIDを自動生成"))
            {
                if (settings.enemyParamList == null || settings.enemyParamList.Count == 0)
                {
                    Debug.LogWarning("enemyParamListが空です。IDの自動生成を中止します。");
                    return;
                }

                foreach (var param in settings.enemyParamList)
                {
                    param.GenerateID();
                }

                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"エラー\n{ex.Message}");
        }
    }
}