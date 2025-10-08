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
        hpSlider.value = (float)current / max;
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        // �����Ń��g���C��ʂ��o���Ƃ�
    }
}
