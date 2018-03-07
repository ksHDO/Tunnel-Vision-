using GameSparks.Api.Messages;
using GameSparks.Api.Responses;
using GameSparks.Core;
using GameSparks.RT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSparksManager : MonoBehaviour {

    private GameSparksRTUnity m_gameSparksRTUnity;

    private RTSessionInfo m_sessionInfo;

    public System.Action<int> OnPlayerConnect;
    public System.Action<int> OnPlayerDisconnect;
    public System.Action<bool> OnPlayerReady;
    public System.Action<RTPacket> OnPacketReceived;

    public GameSparksRTUnity RTSession { get { return m_gameSparksRTUnity; } }
    public RTSessionInfo CurrentMultiplayerSession
    {
        get
        {
            return m_sessionInfo;
        }
        set
        {
            m_sessionInfo = value;
            m_gameSparksRTUnity = gameObject.AddComponent<GameSparksRTUnity>();
            GSRequestData response = new GSRequestData()
                .AddNumber("port", (double)m_sessionInfo.Port)
                .AddString("host", m_sessionInfo.HostUrl)
                .AddString("accessToken", m_sessionInfo.AccessToken);
            FindMatchResponse matchResponse = new FindMatchResponse(response);
            m_gameSparksRTUnity.Configure(matchResponse,
                OnPlayerConnected,
                OnPlayerDisconnected,
                OnRTReady,
                OnPacketReceive
                );
            m_gameSparksRTUnity.Connect();
        }
    }

    private void OnPlayerConnected(int peer)
    {
        if (OnPlayerConnect != null)
            OnPlayerConnect(peer);
    }

    private void OnPacketReceive(RTPacket packet)
    {
        if (OnPacketReceived != null)
            OnPacketReceived(packet);
    }

    private void OnRTReady(bool ready)
    {
        if (OnPlayerReady != null)
            OnPlayerReady(ready);
    }

    private void OnPlayerDisconnected(int peer)
    {
        if (OnPlayerDisconnect != null)
            OnPlayerDisconnect(peer);
    }

    

}

public class RTSessionInfo
{
    private string m_matchID;
    public string MatchID { get { return m_matchID; } }
    private int m_port;
    public int Port { get { return m_port; }}
    private string m_hostUrl;
    public string HostUrl { get { return m_hostUrl; } }
    private string m_accessToken;
    public string AccessToken { get { return m_accessToken; } }

    private List<RTPlayer> m_playerList = new List<RTPlayer>();
    public List<RTPlayer> PlayerList
    {
        get
        {
            return m_playerList;
        }
    }

    public RTSessionInfo(MatchFoundMessage message)
    {
        m_port = (int) message.Port;
        m_hostUrl = message.Host;
        m_accessToken = message.AccessToken;
        m_matchID = message.MatchId;
        foreach (var p in message.Participants)
        {
            m_playerList.Add(new RTPlayer(p.Id, (int)p.PeerId));
        }
    }
}

public struct RTPlayer
{
    public string Id;
    public int PeerId;

    public RTPlayer(string id, int peerId)
    {
        Id = id;
        PeerId = peerId;
    }
}
