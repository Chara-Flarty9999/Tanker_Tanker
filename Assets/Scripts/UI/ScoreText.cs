using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreText : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI _scoreText;
    int _score;
    public int AddScore
    {
        set
        {
            _score += value;
            ScoreUpdate(_score);
        }
    }
    public int Score => _score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _scoreText.text = $"Score:{_score.ToString("D6")}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ScoreUpdate(int score)
    {
        if (_scoreText != null)
        {
            Debug.Log($"Score Updated: {score}");
            _scoreText.text = $"Score:{score.ToString("D6")}";
        }
    }
}
