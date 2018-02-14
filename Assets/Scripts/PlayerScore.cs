using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerScore : MonoBehaviour
{

    [SerializeField] private float _score;
    [SerializeField] private FloatEvent OnNewHighScore;
    public float Score { get { return _score; } private set { _score = value; } }

    public float HighScore
    {
        get
        {
            float score = PlayerPrefs.GetFloat("HighScore", 50);
            return score;
        }
    }

    [SerializeField] private FloatEvent _onAddScore;

    public void AddScore(float value)
    {
        Score += value;
        _onAddScore.Invoke(value);
    }

    public void UpdateHighScore()
    {
        float score = PlayerPrefs.GetFloat("HighScore", 50);
        if (_score > score)
        {
            PlayerPrefs.SetFloat("HighScore", _score);
            OnNewHighScore.Invoke(_score);
        }
    }
}
