using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using UnityEngine;
using UnityEngine.UI;

public class UiHighScores : MonoBehaviour
{
    [SerializeField] private string SoloText = "Solo";
    [SerializeField] private string CoopText = "Coop";

    [SerializeField] private string soloShortCode = "HIGH_SCORES_GLOBAL";
    [SerializeField] private string coopShortCode = "MULTIPLAYER_HIGHSCORES_COOP_GLOBAL";

    [SerializeField] private GameObject m_scorePrefab;
    [SerializeField] private Vector2 m_scoreListingOffsets;

    [SerializeField] private Transform m_scoreParent;

    [SerializeField] private Text m_modeSwitchText;
    [SerializeField] private Text m_modeTitle;

    private bool showSolo = true;
	// Use this for initialization
	void Start ()
    {
        showSolo = true;
        m_modeSwitchText.text = CoopText;
        m_modeTitle.text = SoloText;
        SetLeaderboard(soloShortCode);
    }

    private void SetLeaderboard(string shortCode)
    {
        foreach (Transform t in m_scoreParent)
        {
            Destroy(t.gameObject);
        }
        new LeaderboardDataRequest()
            .SetLeaderboardShortCode(shortCode)
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
                            rank = (int)entry.Rank;
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

    public void ToggleDisplay()
    {
        showSolo = !showSolo;
        if (showSolo)
        {
            m_modeSwitchText.text = CoopText;
            m_modeTitle.text = SoloText;
            SetLeaderboard(soloShortCode);
        }
        else
        {
            m_modeSwitchText.text = SoloText;
            m_modeTitle.text = CoopText;
            SetLeaderboard(coopShortCode);
        }

    }

	// Update is called once per frame
	void Update () {
		
	}
}
