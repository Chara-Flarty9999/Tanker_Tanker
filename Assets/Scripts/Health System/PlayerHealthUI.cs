using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private HealthManager health;
    [SerializeField] private Slider hpSlider;

    private void Start()
    {
        health.OnHealthChanged.AddListener(UpdateUI);
        health.OnDeath.AddListener(GameOver);
    }

    private void UpdateUI(int current, int max)
    {
        hpSlider.maxValue = max;
        hpSlider.value = current;
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        // ‚±‚±‚ÅƒŠƒgƒ‰ƒC‰æ–Ê‚ğo‚·‚Æ‚©
    }
}
