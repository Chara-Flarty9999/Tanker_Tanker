using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    int _world = 1;
    int _stage = 1;
    int _wave = 1;

    [SerializeField] EnemyStatusSettings _enemyData;
    List<SpawnParam> _spawnList;
    EnemyParam _enemyParams;

    public int World { get; set; }
    public int Stage { get; set; }
    public int Wave
    {
        get { return _wave; }
        set
        {
            _wave = value;
            WaveUpdate();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void WaveUpdate()
    {
        var path = $"SpawnData/World{_world}/{_world}-{_stage}-{_wave}";
        var stageData = Resources.Load<SpawnData>(path);
        Debug.Log(stageData);
        _spawnList = stageData.SpawnParamList;
        for (int enemyCount = 0; enemyCount < _spawnList.Count; enemyCount++)
        {
            _enemyParams = _enemyData.GetParamByID(_spawnList[enemyCount].spawnTankID);
            EnemySpawn(enemyCount);
            Debug.Log(_enemyParams);
        }
    }

    void EnemySpawn(int enemyIndex)
    {
        var enemyObj = Instantiate(_enemyParams.Prefab, _spawnList[enemyIndex].SpawnArea, Quaternion.identity);
        var enemyMovement = enemyObj.GetComponent<EnemyMovement>();
        var enemyBullet = enemyObj.GetComponentInChildren<EnemyBullet>();
        var enemyHealth = enemyObj.GetComponent<HealthManager>();
        enemyHealth.Initialize(_enemyParams);
        enemyMovement.Initialize(_enemyParams);
        enemyBullet.Initialize(_enemyParams);
    }
    // Update is called once per frame
    void Update()
    {


    }
}
