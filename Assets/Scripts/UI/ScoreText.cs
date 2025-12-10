using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreText : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI _scoreText;
    int _score;
    public int Score { set { _score = Score; ScoreUpdate(_score); }  get { return _score; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _score = 0;
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
            _scoreText.text = $"Score:{score.ToString("D6")}";
        }
    }
}
