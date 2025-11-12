using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnData1", menuName = "Scriptable Objects/SpawnData1")]
public class SpawnData : ScriptableObject
{
    [Tooltip("ゲーム内でウェーブごとに登場する敵を入力。召喚位置と敵IDをリストに登録する。")]
    public List<SpawnParam> SpawnParamList = new List<SpawnParam>();
}

[System.Serializable]
public class SpawnParam
{
    [Header("基本情報")]

    [Tooltip("召喚する敵のIDを設定")] public string spawnTankID;
    [SerializeField, Tooltip("そのタンクの召喚場所を座標で設定")] Vector3 spawnArea;

    

    #region Properties
    public Vector3 SpawnArea => spawnArea;

    #endregion
}
