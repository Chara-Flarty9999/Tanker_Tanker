using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatusSettings", menuName = "Scriptable Objects/EnemyStatusSettings")]
public class EnemyStatusSettings : ScriptableObject
{
    [Tooltip("ゲームに登場する敵をすべて個別で作成する。武器や特性が違うものは違う個体と判断する")]public List<EnemyParam> EnemyParamList = new List<EnemyParam>();

    public EnemyParam GetParamByID(string id)
    {
        return EnemyParamList.FirstOrDefault(p => p.tankID == id);
    }
}

[System.Serializable]
public class EnemyParam
{
    [Header("基本情報")]

    [Tooltip("その敵の固有IDを設定")] public string tankID;
    [Tooltip("その敵の名前を設定。画面に表示する")] public string tankName;
    [SerializeField, Tooltip("そのタンクのプレハブを設定する")] GameObject prefab;
    [SerializeField, Tooltip("最大体力を設定")] int maxHP;
    [SerializeField, Tooltip("戦車の移動速度を設定")] float maxMoveSpeed;
    [SerializeField, Tooltip("装備している武器種を設定")] WeaponType weaponType;
    [SerializeField, Tooltip("攻撃頻度を設定")] float weaponFireRate;
    [SerializeField, Tooltip("攻撃範囲を設定")] float attackRange;
    [SerializeField, Tooltip("行動パターンを設定")] BehaviorType behaviorType;
    [SerializeField, Tooltip("倒したときのスコアを設定")] int rewardScore;

    [Header("特性設定")]
    [SerializeField, Tooltip("プレイヤーに近づいて自爆を行うかどうか")] bool isSelfDestruct;
    [SerializeField, Tooltip("trueの場合移動しなくなる")] bool isStationary;
    [SerializeField, Tooltip("その他、特殊な設定")] SpecialFlags specialFlags;

    

    #region Properties

    public int MaxHP => maxHP;
    public float MaxMoveSpeed => maxMoveSpeed;
    public WeaponType WeaponType => weaponType;
    public float WeaponFireRate => weaponFireRate;
    public float AttackRange => attackRange;
    public BehaviorType BehaviorType => behaviorType;
    public bool IsSelfDestruct => isSelfDestruct;
    public bool IsStationary => isStationary;
    public int RewardScore => rewardScore;
    public GameObject Prefab => prefab;
    public SpecialFlags SpecialFlags => specialFlags;

    #endregion
}
public enum WeaponType
{
    None,
    Cannon,
    MachineGun,
    TwinCannon
}

public enum BehaviorType
{
    Assault,      // 突撃
    Turret,   // 固定砲台
    Tactical,     // 中距離
    Mouse,        // チョロチョロ(高機動)
    Dreadnought   // 巨大ボス
}

[System.Flags]
public enum SpecialFlags
{
    None = 0,
    Stealth = 1 << 0,
    WallWalker = 1 << 1,
    ExplodesOnDeath = 1 << 2,
    Shiny = 1 << 3 // GoldTank向け
}