using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnData1", menuName = "Scriptable Objects/SpawnData1")]
public class SpawnData : ScriptableObject
{
    [Tooltip("�Q�[�����ŃE�F�[�u���Ƃɓo�ꂷ��G����́B�����ʒu�ƓGID�����X�g�ɓo�^����B")]public List<Spawnparam> SpawnParamList = new List<Spawnparam>();

    public Spawnparam GetParamByID(string id)
    {
        return SpawnParamList.FirstOrDefault(p => p.spawnTankID == id);
    }
}

[System.Serializable]
public class Spawnparam
{
    [Header("��{���")]

    [Tooltip("��������G��ID��ݒ�")] public string spawnTankID;
    [SerializeField, Tooltip("���̃^���N�̃v���n�u��ݒ肷��")] Vector3 spawnArea;

    

    #region Properties
    public Vector3 SpawnArea => spawnArea;

    #endregion
}
