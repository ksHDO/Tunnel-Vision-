using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiPoints : MonoBehaviour
{

    [SerializeField] private PlayerScore _score;
    [SerializeField] private Text _txtScore;
    [SerializeField] private Text _txtHighScore;
    [SerializeField] private UnityEvent _onScoreFinishUpdate;

    [SerializeField] private float _smoothTime;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _scaleDamp;

    private float _curScore;
    private float _velScore;

    private float _curHighScore;
    private float _velHighScore;

    private Transform _txtScoreTransform;
    private Transform _txtHighScoreTransform;
    private Vector3 _initialScale;
    private Vector3 _initialHighScale;
	// Use this for initialization
	void Start ()
	{
	    _velScore = 0;
	    _velHighScore = 0;
	    _txtScoreTransform = _txtScore.transform;
	    _initialScale = _txtScoreTransform.localScale;
	    _curHighScore = 0;
	    if (_txtHighScore)
	    {
	        _txtHighScoreTransform = _txtHighScore.transform;
	        _initialHighScale = _txtHighScoreTransform.localScale;
	        _txtHighScore.text = "0 points";
	    }
    }
	
	// Update is called once per frame
	void Update ()
	{
	    _curScore = Mathf.SmoothDamp(_curScore, _score.Score, ref _velScore, _smoothTime, _maxSpeed);
	    _txtScoreTransform.localScale = _initialScale * (1 + (_velScore * _scaleDamp));
	    int val = Mathf.RoundToInt(_curScore);
        _txtScore.text = val + " points";
	    if (val == (int) _score.Score)
	    {

            _onScoreFinishUpdate.Invoke();

	    }

	    if (_txtHighScore)
	    {
	        _curHighScore = Mathf.SmoothDamp(_curHighScore, _score.HighScore, ref _velHighScore, _smoothTime, _maxSpeed);
	        _txtHighScoreTransform.localScale = _initialHighScale * (1 + (_velHighScore * _scaleDamp));
	        _txtHighScore.text = Mathf.RoundToInt(_curHighScore) + " points";
	    }

    }
}
