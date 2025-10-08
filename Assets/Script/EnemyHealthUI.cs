using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private HealthManager _healthManager;
    [SerializeField] private GameObject _hpBarRoot; // Canvas付きのオブジェクト
    [SerializeField] private Slider _hpSlider;

    private void Start()
    {
        _hpBarRoot.SetActive(false); // 最初は非表示

        _healthManager.OnHealthChanged.AddListener(UpdateUI);
        _healthManager.OnDeath.AddListener(HandleDeath);
    }

    private void UpdateUI(int current, int max)
    {
        _hpSlider.maxValue = max;
        _hpBarRoot.SetActive(true); // 攻撃を受けたら表示
        _hpSlider.value = (float)current / max;
    }

    private void HandleDeath()
    {
        Destroy(gameObject); // 敵を消す
    }
}
