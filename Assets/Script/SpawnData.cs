using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnData1", menuName = "Scriptable Objects/SpawnData1")]
public class SpawnData : ScriptableObject
{
    [Tooltip("�Q�[�����ŃE�F�[�u���Ƃɓo�ꂷ��G����́B�����ʒu�ƓGID�����X�g�ɓo�^����B")]
    public List<SpawnParam> SpawnParamList = new List<SpawnParam>();
}

[System.Serializable]
public class SpawnParam
{
    [Header("��{���")]

    [Tooltip("��������G��ID��ݒ�")] public string spawnTankID;
    [SerializeField, Tooltip("���̃^���N�̏����ꏊ�����W�Őݒ�")] Vector3 spawnArea;

    

    #region Properties
    public Vector3 SpawnArea => spawnArea;

    #endregion
}
