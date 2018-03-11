using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupApp : MonoBehaviour
{

    [SerializeField] private string startScene;

    // Use this for initialization
    void Start()
    {
        GameSparks.Core.GS.GameSparksAvailable += Setup;
        

    }

    void Setup(bool available)
    {
        if (available)
        {
            DeviceAuthenticationRequest request = new DeviceAuthenticationRequest();
            request.Send(OnLoginSuccess);
        }
    }

    void OnLoginSuccess(AuthenticationResponse response)
    {
        SceneManager.LoadScene(startScene);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
