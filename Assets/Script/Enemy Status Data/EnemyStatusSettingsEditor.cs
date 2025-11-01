using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyStatusSettings))]
public class EnemyStatusSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnemyStatusSettings settings = (EnemyStatusSettings)target;

        if (GUILayout.Button("‘SEnemyParam‚ÌID‚ğ©“®¶¬"))
        {
            foreach (var param in settings.enemyParamList)
            {
                param.GenerateID();
            }

            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
        }
    }
}