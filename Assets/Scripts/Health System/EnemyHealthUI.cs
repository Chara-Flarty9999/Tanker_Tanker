using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    EnemyParam _param;
    [SerializeField] private HealthManager _healthManager;
    [SerializeField] private GameObject _hpBarRoot; // Canvas付きのオブジェクト
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private ScoreText _scoreText;
    int _addScore = 0;
    private Transform _target = default;

    private void Start()
    {
        _hpBarRoot.SetActive(false); // 最初は非表示
        _target = GameObject.FindWithTag("Player").transform;
        _scoreText = GameObject.FindFirstObjectByType<ScoreText>().GetComponent<ScoreText>();
        _healthManager.OnHealthChanged.AddListener(UpdateUI);
    }

    private void Update()
    {
        Vector3 p = Camera.main.transform.position;
        p.y = _hpBarRoot.transform.position.y;
        _hpBarRoot.transform.LookAt(p);
        
    }

    private void UpdateUI(int current, int max)
    {
        _hpSlider.maxValue = max;
        _hpBarRoot.SetActive(true); // 攻撃を受けたら表示
        _hpSlider.value = (float)current;
    }

    private void HandleDeath()
    {
        _scoreText.AddScore = _addScore;
        Debug.Log($"Score Added: {_addScore}");
        Destroy(gameObject); // 敵を消す
    }
    public void Initialize(EnemyParam enemyParam)
    {
        _param = enemyParam;
        _addScore = _param.RewardScore;
        _hpSlider.maxValue = _param.MaxHP;
        _healthManager.OnDeath.AddListener(HandleDeath);
    }
}
