using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using GameSparks.Api.Messages;
using UnityEngine.Events;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private float _score;
    [SerializeField] private FloatEvent OnNewHighScore;
    [SerializeField] private string _highScorePlayerPref = "HighScore";
    [SerializeField] private string _eventKey = "UPDATE_SCORE";
    [SerializeField] private string _eventAttribute = "SCORE";
    public float Score { get { return _score; } set { _score = value; } }
    public MultiplayerGameManager MultiplayerManager;

    public float HighScore
    {
        get
        {
            float score = PlayerPrefs.GetFloat(_highScorePlayerPref, 50);
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
        if (!MultiplayerManager || MultiplayerManager.IsHost)
        {
            new LogEventRequest()
                .SetEventKey(_eventKey)
                .SetEventAttribute(_eventAttribute, (int)_score)
                .Send((response) =>
                {
                    if (!response.HasErrors)
                    {
                        Debug.Log("Successfully submitted score");

                    }
                    else
                    {
                        Debug.LogWarning("Could not submit score");
                        Debug.LogWarning(response.Errors.JSON);
                    }
                });
        }

        float score = PlayerPrefs.GetFloat(_highScorePlayerPref, 50);
        if (_score > score)
        {
            PlayerPrefs.SetFloat(_highScorePlayerPref, _score);
            OnNewHighScore.Invoke(_score);
        }
    }
}
