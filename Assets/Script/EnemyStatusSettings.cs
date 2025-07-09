using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatusSettings", menuName = "Scriptable Objects/EnemyStatusSettings")]
public class EnemyStatusSettings : ScriptableObject
{
    [Tooltip("�Q�[���ɓo�ꂷ��G�����ׂČʂō쐬����B�����������Ⴄ���͈̂Ⴄ�̂Ɣ��f����")]public List<EnemyParam> EnemyParamList = new List<EnemyParam>();

    public EnemyParam GetParamByID(string id)
    {
        return EnemyParamList.FirstOrDefault(p => p.tankID == id);
    }
}

[System.Serializable]
public class EnemyParam
{
    [Header("��{���")]

    [Tooltip("���̓G�̌ŗLID��ݒ�")] public string tankID;
    [Tooltip("���̓G�̖��O��ݒ�B��ʂɕ\������")] public string tankName;
    [SerializeField, Tooltip("���̃^���N�̃v���n�u��ݒ肷��")] GameObject prefab;
    [SerializeField, Tooltip("�ő�̗͂�ݒ�")] int maxHP;
    [SerializeField, Tooltip("��Ԃ̈ړ����x��ݒ�")] float maxMoveSpeed;
    [SerializeField, Tooltip("�������Ă��镐����ݒ�")] WeaponType weaponType;
    [SerializeField, Tooltip("�U���p�x��ݒ�")] float weaponFireRate;
    [SerializeField, Tooltip("�U���͈͂�ݒ�")] float attackRange;
    [SerializeField, Tooltip("�s���p�^�[����ݒ�")] BehaviorType behaviorType;
    [SerializeField, Tooltip("�|�����Ƃ��̃X�R�A��ݒ�")] int rewardScore;

    [Header("�����ݒ�")]
    [SerializeField, Tooltip("�v���C���[�ɋ߂Â��Ď������s�����ǂ���")] bool isSelfDestruct;
    [SerializeField, Tooltip("true�̏ꍇ�ړ����Ȃ��Ȃ�")] bool isStationary;
    [SerializeField, Tooltip("���̑��A����Ȑݒ�")] SpecialFlags specialFlags;

    

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
    Assault,      // �ˌ�
    Turret,   // �Œ�C��
    Tactical,     // ������
    Mouse,        // �`�����`����(���@��)
    Dreadnought   // ����{�X
}

[System.Flags]
public enum SpecialFlags
{
    None = 0,
    Stealth = 1 << 0,
    WallWalker = 1 << 1,
    ExplodesOnDeath = 1 << 2,
    Shiny = 1 << 3 // GoldTank����
}