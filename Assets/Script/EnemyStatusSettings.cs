using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Enums;

[CreateAssetMenu(fileName = "EnemyStatusSettings", menuName = "Scriptable Objects/EnemyStatusSettings")]
public class EnemyStatusSettings : ScriptableObject
{
    [Tooltip("�Q�[���ɓo�ꂷ��G�����ׂČʂō쐬����B�����������Ⴄ���͈̂Ⴄ�̂Ɣ��f����")]public List<EnemyParam> enemyParamList = new List<EnemyParam>();

    public EnemyParam GetParamByID(string id)
    {
        return enemyParamList.FirstOrDefault(p => p._tankID == id);//�v�f���ŏ��Ɉ�v����������Ԃ��B
    }
}

[System.Serializable]
public class EnemyParam : Enums
{
    [Header("��{���")]

    [Tooltip("���̓G�̌ŗLID��ݒ�")] public string _tankID;
    [Tooltip("���̓G�̖��O��ݒ�B��ʂɕ\������")] public string _tankName;
    [SerializeField, Tooltip("���̃^���N�̃v���n�u��ݒ肷��")] GameObject _prefab;
    [SerializeField, Tooltip("�ő�̗͂�ݒ�")] int _maxHP;
    [SerializeField, Tooltip("��Ԃ̈ړ����x��ݒ�")] float _maxMoveSpeed;
    [SerializeField, Tooltip("�������Ă��镐����ݒ�")] WeaponType _weaponType;
    [SerializeField, Tooltip("�U���p�x��ݒ�")] float _weaponFireRate;
    [SerializeField, Tooltip("�U���͈͂�ݒ�")] float _attackRange;
    [SerializeField, Tooltip("�s���p�^�[����ݒ�")] BehaviorType _behaviorType;
    [SerializeField, Tooltip("�|�����Ƃ��̃X�R�A��ݒ�")] int _rewardScore;

    [Header("�����ݒ�")]
    [SerializeField, Tooltip("�v���C���[�ɋ߂Â��Ď������s�����ǂ���")] bool _isSelfDestruct;
    [SerializeField, Tooltip("���̑��A����Ȑݒ�")] SpecialFlags _specialFlags;

    #region Properties

    public int MaxHP => _maxHP;
    public float MaxMoveSpeed => _maxMoveSpeed;
    public new WeaponType WeaponType => _weaponType;
    public float WeaponFireRate => _weaponFireRate;
    public float AttackRange => _attackRange;
    public new BehaviorType BehaviorType => _behaviorType;
    public bool IsSelfDestruct => _isSelfDestruct;
    public int RewardScore => _rewardScore;
    public GameObject Prefab => _prefab;
    public new SpecialFlags SpecialFlags => _specialFlags;

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

