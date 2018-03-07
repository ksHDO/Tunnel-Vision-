using GameSparks.RT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Constants;

public class MultiplayerGameManager : MonoBehaviour {

    [SerializeField] private string GameSparksManagerObjectName;
    [SerializeField] private Transform SpawnPointParent;

    [SerializeField] private GameObject m_bulletContainer;
    [SerializeField] private GameObject m_particleContainer;
    
    [SerializeField] private GameObject[] PlayerPrefabs;

    [Header("Update Rate")]
    [SerializeField]
    private float _playerUpdateRate = 0.01f;

    private GameSparksManager m_gameSparksManager;
    private Transform[] m_spawnPoints;

    private PlayerController[] m_players;

	// Use this for initialization
	void Start () {
        GameObject gameSparksObject = GameObject.Find(GameSparksManagerObjectName);
        if (!gameSparksObject)
        {
            Debug.LogError("GameSparks not in scene!");
            return;
        }

        m_gameSparksManager = gameSparksObject.GetComponent<GameSparksManager>();
        m_gameSparksManager.OnPacketReceived += PacketReceived;
        m_gameSparksManager.OnPlayerDisconnect += OnPlayerDisconnected;

        m_spawnPoints = SpawnPointParent.GetComponentsInChildren<Transform>();

        InstantiatePlayers(m_gameSparksManager.CurrentMultiplayerSession);
    }

    void InstantiatePlayers(RTSessionInfo session)
    {
        Transform thisTransform = transform;
        List<RTPlayer> players = session.PlayerList;
        GameSparksRTUnity gameSparksSession = m_gameSparksManager.RTSession;
        EnemyGenerator enemyGenerator = GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();
        int count = players.Count;
        enemyGenerator.players = new Rigidbody2D[count];

        m_players = new PlayerController[count];

        if (PlayerPrefabs.Length < count)
        {
            Debug.LogError("Player prefabs less than connected players!");
            return;
        }
        if (m_spawnPoints.Length < count)
        {
            Debug.LogError("Spawn points less than connected players!");
            return;
        }

        for (int i = 0; i < count; ++i)
        {
            GameObject player = Instantiate(
                PlayerPrefabs[i],
                m_spawnPoints[i].position,
                m_spawnPoints[i].rotation
            );
            Transform playerTransfrom = player.transform;
            PlayerController playerController = player.GetComponent<PlayerController>();
            Turret playerTurret = player.GetComponent<Turret>();
            enemyGenerator.players[i] = player.GetComponent<Rigidbody2D>();
            player.name = "Player " + players[i].PeerId.ToString();
            playerController.PeerID = players[i].PeerId;
            playerController.UpdateRate = _playerUpdateRate;
            playerTransfrom.SetParent(thisTransform);
            playerTurret.BulletContainer = m_bulletContainer;
            playerTurret.ParticleContainer = m_particleContainer;

            bool isPlayer = players[i].PeerId == gameSparksSession.PeerId;
            playerController.GameSparks = m_gameSparksManager;
            playerController.SetupMultiplayer(m_spawnPoints[i], isPlayer);

            // Set player multiplayer component instead...
            m_players[i] = playerController;
        }
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void PacketReceived(RTPacket packet)
    {
        switch (packet.OpCode)
        {
            case (int) MultiplayerCodes.PLAYER_POSITION:
                UpdatePlayers(packet);
                break;
            case (int) MultiplayerCodes.PLAYER_BULLETS:
                UpdatePlayerBullets(packet);
                break;
        }
    }

    private void UpdatePlayers(RTPacket packet)
    {
        for (int i = 0; i < m_players.Length; ++i)
        {
            if (m_players[i].PeerID == packet.Sender)
            {
                Transform playerTransform = m_players[i].transform;
                Rigidbody2D playerRigidBody = m_players[i].GetComponent<Rigidbody2D>();
                float playerPosZ = playerTransform.position.z;
                Quaternion playerRot = playerTransform.rotation;
                Vector3 playerAngles = playerRot.eulerAngles;

                // Referenced from PlayerController.SendTransformUpdates()
                Vector2 newPos = (Vector2)packet.Data.GetVector2(1);
                float newRot = (float)packet.Data.GetFloat(2);
                Vector2 newVel = (Vector2)packet.Data.GetVector2(3);

                playerTransform.position = new Vector3(newPos.x, newPos.y, playerPosZ);
                playerRigidBody.velocity = newVel;
                playerAngles.z = newRot;
                playerRot.eulerAngles = playerAngles;

                playerTransform.rotation = playerRot;
                break;
            }
        }
    }

    private void UpdatePlayerBullets(RTPacket packet)
    {

    }

    private void OnPlayerDisconnected(int peer)
    {
    }
}
