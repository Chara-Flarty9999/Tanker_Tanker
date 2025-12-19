using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 敵味方共通のHPスクリプト。
/// 個別で違う性能が必要な場合それに応じて作ること。
/// </summary>
public class HealthManager : MonoBehaviour
{
    EnemyParam _param;
    [SerializeField] int _maxHP = 50;
    int _currentHP;
    public int CurrentHP => _currentHP;
    bool _isDead = false;

    public UnityEvent<int, int> OnHealthChanged; // (current, max)
    public UnityEvent OnDeath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _currentHP = _maxHP;
    }

    public void TakeDamage(int amount)
    {
        Debug.Log($"{amount}のダメージを受けた");
        _currentHP += amount;
        if (_currentHP < 0) _currentHP = 0;
        OnHealthChanged.Invoke(_currentHP, _maxHP);
        Debug.Log($"現在のHP: {_currentHP}/{_maxHP}");
        if (_currentHP == 0 && !_isDead)
        {
            _isDead = true;
            OnDeath.Invoke();
        }
    }

    public void Heal(int amount)
    {
        _currentHP = Mathf.Min(_currentHP + amount, _maxHP);
        OnHealthChanged.Invoke(_currentHP, _maxHP);
    }

    public void Initialize(EnemyParam enemyParam)
    {
        _param = enemyParam;
        _maxHP = _param.MaxHP;
        _currentHP = _maxHP;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
