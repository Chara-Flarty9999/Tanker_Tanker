using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnData1", menuName = "Scriptable Objects/SpawnData1")]
public class SpawnData : ScriptableObject
{
    [Tooltip("ゲーム内でウェーブごとに登場する敵を入力。召喚位置と敵IDをリストに登録する。")]public List<Spawnparam> SpawnParamList = new List<Spawnparam>();

    public Spawnparam GetParamByID(string id)
    {
        return SpawnParamList.FirstOrDefault(p => p.spawnTankID == id);
    }
}

[System.Serializable]
public class Spawnparam
{
    [Header("基本情報")]

    [Tooltip("召喚する敵のIDを設定")] public string spawnTankID;
    [SerializeField, Tooltip("そのタンクのプレハブを設定する")] Vector3 spawnArea;

    

    #region Properties
    public Vector3 SpawnArea => spawnArea;

    #endregion
}
