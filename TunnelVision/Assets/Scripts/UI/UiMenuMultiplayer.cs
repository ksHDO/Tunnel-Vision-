using Assets.Scripts.Constants;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.RT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMenuMultiplayer : MonoBehaviour {

    [SerializeField] private Text m_txtSearch;
    [SerializeField] private Button m_startButton;
    [SerializeField] private Text[] m_playerTexts;

    [SerializeField] private string multiplayerScene;

    [Header("Texts")]
    [SerializeField] private string m_searchText = "Waiting...";
    [SerializeField] private string m_notFoundText = "Match was not found...";

    private GameSparksManager m_gameSparksManager;
    private SwitchScene m_switchScene;
    private void Awake()
    {
        m_switchScene = GetComponent<SwitchScene>();
        m_gameSparksManager = GameObject.Find("GameSparks").GetComponent<GameSparksManager>();
        MatchFoundMessage.Listener += OnMatchFound;
        MatchNotFoundMessage.Listener += OnMatchNotFound;
        m_gameSparksManager.OnPlayerReady += OnMatchReady;
        m_gameSparksManager.OnPacketReceived += PacketReceived;
        m_gameSparksManager.OnPlayerConnect += PlayerConnected;
        m_gameSparksManager.OnPlayerDisconnect += PlayerDisconnect;
        FindMatch();

    }

    void FindMatch()
    {
        new MatchmakingRequest()
            .SetMatchShortCode("COOP_MATCH")
            .SetSkill(0)
            .Send((response) =>
            {
                if (response.HasErrors)
                {
                    Debug.LogError("Matchmaking error!\n" + response.Errors.JSON);
                } else
                {
                    Debug.Log("Request successful");
                }
            });
        m_txtSearch.text = m_searchText;
        m_startButton.gameObject.SetActive(false);
        foreach (var texts in m_playerTexts)
        {
            texts.gameObject.SetActive(false);
        }
    }

    void OnMatchFound(MatchFoundMessage message)
    {

        m_txtSearch.gameObject.SetActive(false);

        m_gameSparksManager
            .CurrentMultiplayerSession = new RTSessionInfo(message);
    }


    void OnMatchNotFound(MatchNotFoundMessage message)
    {
        m_txtSearch.text = m_notFoundText;
        Debug.Log("Match not found");
    }

    void OnMatchReady(bool ready)
    {
        if (m_gameSparksManager.RTSession.PeerId.Value == 1)
        {
            if (m_startButton)
                m_startButton.gameObject.SetActive(true);
        }
    }

    public void SendStartGame()
    {
        using(RTData data = RTData.Get())
        {
            data.SetInt(1, 1);
            Debug.Log("Start Game");
            m_gameSparksManager.RTSession.SendData(
                MultiplayerCodes.START_GAME.Int(),
                GameSparksRT.DeliveryIntent.UNRELIABLE,
                data
                );
        }
        StartGame();
    }

    void ShowAllConnected()
    {
        List<int> peers = m_gameSparksManager.RTSession.ActivePeers;
        for (int i = 0; i < m_playerTexts.Length; i++)
        {
            if (peers.Contains(i + 1))
            {
                if (m_playerTexts[i])
                    m_playerTexts[i].gameObject.SetActive(true);
            }
        }
    }

    void PlayerConnected(int peer)
    {
        ShowAllConnected();
        //m_playerTexts[peer + 1].gameObject.SetActive(true);
    }

    void PlayerDisconnect(int peer)
    {
        ShowAllConnected();
        //if (m_playerTexts[peer + 1])
        //    m_playerTexts[peer + 1].gameObject.SetActive(false);
    }

    void PacketReceived(RTPacket packet)
    {
        if (packet.OpCode == MultiplayerCodes.START_GAME.Int())
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        m_switchScene.Switch(multiplayerScene);
    }

    public void Disconnect()
    {
        GameSparksRTUnity session = m_gameSparksManager.RTSession;
        if (session != null)
            session.Disconnect();
    }

}
