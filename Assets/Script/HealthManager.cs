using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �G�������ʂ�HP�X�N���v�g�B
/// �ʂňႤ���\���K�v�ȏꍇ����ɉ����č�邱�ƁB
/// </summary>
public class HealthManager : MonoBehaviour
{
    EnemyParam _param;
    [SerializeField] int _maxHP = 100;
    int _currentHP;

    public UnityEvent<int, int> OnHealthChanged; // (current, max)
    public UnityEvent OnDeath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _currentHP = _maxHP;
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Damaged");
        _currentHP -= amount;
        if (_currentHP < 0) _currentHP = 0;

        OnHealthChanged.Invoke(_currentHP, _maxHP);

        if (_currentHP == 0)
        {
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
    }
    // Update is called once per frame
    void Update()
    {

    }
}
