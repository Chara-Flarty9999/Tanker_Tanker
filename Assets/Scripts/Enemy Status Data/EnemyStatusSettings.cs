using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Enums;

[CreateAssetMenu(fileName = "EnemyStatusSettings", menuName = "Scriptable Objects/EnemyStatusSettings")]
public class EnemyStatusSettings : ScriptableObject
{
    [Tooltip("ゲームに登場する敵をすべて個別で作成する。武器や特性が違うものは違う個体と判断する")]public List<EnemyParam> enemyParamList = new List<EnemyParam>();

    public EnemyParam GetParamByID(string id)
    {
        return enemyParamList.FirstOrDefault(p => p._tankID == id);//要素を最初に一致した一つだけ返す。
    }
}

[System.Serializable]
public class EnemyParam
{
    [Header("基本情報")]

    [Tooltip("その敵の固有IDを設定")] public string _tankID;
    [Tooltip("その敵の名前を設定。画面に表示する")] public string _tankName;
    [SerializeField, Tooltip("そのタンクのプレハブを設定する")] GameObject _prefab;
    [SerializeField, Tooltip("最大体力を設定")] int _maxHP;
    [SerializeField, Tooltip("戦車の移動速度を設定")] float _maxMoveSpeed;
    [SerializeField, Tooltip("装備している武器種を設定")] WeaponType _weaponType;
    [SerializeField, Tooltip("攻撃頻度を設定")] float _weaponFireRate;
    [SerializeField, Tooltip("攻撃範囲を設定")] float _attackRange;
    [SerializeField, Tooltip("行動パターンを設定")] BehaviorType _behaviorType;
    [SerializeField, Tooltip("倒したときのスコアを設定")] int _rewardScore;

    [Header("特性設定")]
    [SerializeField, Tooltip("プレイヤーに近づいて自爆を行うかどうか")] bool _isSelfDestruct;
    [SerializeField, Tooltip("その他、特殊な設定")] SpecialFlags _specialFlags;

    #region Properties

    public int MaxHP => _maxHP;
    public float MaxMoveSpeed => _maxMoveSpeed;
    public WeaponType WeaponType => _weaponType;
    public float WeaponFireRate => _weaponFireRate;
    public float AttackRange => _attackRange;
    public BehaviorType BehaviorType => _behaviorType;
    public bool IsSelfDestruct => _isSelfDestruct;
    public int RewardScore => _rewardScore;
    public GameObject Prefab => _prefab;
    public SpecialFlags SpecialFlags => _specialFlags;

    #endregion

    public void GenerateID()
    {
        string id = _behaviorType.ToString();

        if (_isSelfDestruct)
            id += "_Crush";

        if (_specialFlags != SpecialFlags.None)
        {
            string flags = string.Join("_",
                _specialFlags.ToString().Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries)
            ).Replace(" ", "");
            id += "_" + flags;
        }

        if (_weaponType != WeaponType.None)
            id += "_" + _weaponType.ToString();

        _tankID = id;
    }
}

