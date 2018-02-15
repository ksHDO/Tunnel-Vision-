using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGameOver : MonoBehaviour
{

    [SerializeField] private GameObject _gameOver;

	// Use this for initialization
	void Start () {
		_gameOver.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
