using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private HealthManager _healthManager;
    [SerializeField] private GameObject _hpBarRoot; // Canvas�t���̃I�u�W�F�N�g
    [SerializeField] private Slider _hpSlider;

    private void Start()
    {
        _hpBarRoot.SetActive(false); // �ŏ��͔�\��

        _healthManager.OnHealthChanged.AddListener(UpdateUI);
        _healthManager.OnDeath.AddListener(HandleDeath);
    }

    private void UpdateUI(int current, int max)
    {
        _hpSlider.maxValue = max;
        _hpBarRoot.SetActive(true); // �U�����󂯂���\��
        _hpSlider.value = (float)current / max;
    }

    private void HandleDeath()
    {
        Destroy(gameObject); // �G������
    }
}
