using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using UnityEngine;
using UnityEngine.UI;

public class UiHighScores : MonoBehaviour
{

    [SerializeField] private GameObject m_scorePrefab;
    [SerializeField] private Vector2 m_scoreListingOffsets;

    [SerializeField] private Transform m_scoreParent;

	// Use this for initialization
	void Start ()
	{
	    new LeaderboardDataRequest()
	        .SetLeaderboardShortCode("HIGH_SCORES_GLOBAL")
	        .SetEntryCount(100)
            .Send((response) =>
	        {
	            if (!response.HasErrors)
	            {
	                int i = 0;
	                foreach (LeaderboardDataResponse._LeaderboardData entry in response.Data)
	                {
	                    int rank = -1;
	                    if (entry.Rank != null)
	                    {
	                        rank = (int) entry.Rank;
	                    }

	                    string score = entry.JSONData["SCORE"].ToString();
	                    GameObject o = Instantiate(m_scorePrefab, m_scoreParent);
	                    o.transform.localPosition = m_scoreListingOffsets * i;
	                    foreach (Transform t in o.transform)
	                    {
	                        if (t.name == "Rank")
	                        {
	                            t.GetComponent<Text>().text = rank + ".";
	                        }
                            else if (t.name == "Score")
	                        {
	                            t.GetComponent<Text>().text = score;
	                        }
	                    }
	                    ++i;
	                }
	            }
	            else
	            {
	                Debug.LogWarning("Could not retrieve high scores");
                    Debug.LogWarning(response.Errors.JSON);
                    
	            }
	        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
