using GameSparks.RT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Constants;
using System;
using System.Linq;
using UnityStandardAssets.Utility;
using UnityEngine.Events;

public class MultiplayerGameManager : MonoBehaviour {

    [SerializeField] private string GameSparksManagerObjectName;
    [SerializeField] private Transform SpawnPointParent;
    [SerializeField] private Transform UiCanvas;

    [SerializeField] private GameObject m_bulletContainer;
    [SerializeField] private GameObject m_particleContainer;

    
    [SerializeField] private GameObject[] PlayerPrefabs;
    [SerializeField] private GameObject m_hpBarPrefab;
    [SerializeField] private CameraFollowObject m_camera;
    [SerializeField] private EnemyGenerator m_enemyGenerator;
    [SerializeField] private PlayerScore m_playerScore;

    [SerializeField] private UnityEvent m_onAllPlayersDead;

    [Header("Update Rate")]
    [SerializeField]
    private float _playerUpdateRate = 0.01f;
    [SerializeField] private float _enemiesPosUpdateRate = 0.1f;

    private GameSparksManager m_gameSparksManager;
    private Transform[] m_spawnPoints;

    private PlayerController[] m_players;
    private Turret[] m_turrets;
    private PlayerHealth[] m_playerHealth;

    public bool IsHost { get; private set; }

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
        m_turrets = new Turret[count];
        m_playerHealth = new PlayerHealth[count];

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
        players.Sort((p, p2) => (p.PeerId.CompareTo(p2.PeerId)));

        for (int i = 0; i < count; ++i)
        {
            // Spawn Player
            GameObject player = Instantiate(
                PlayerPrefabs[i],
                m_spawnPoints[i].position,
                m_spawnPoints[i].rotation
            );
            Transform playerTransform = player.transform;
            PlayerController playerController = player.GetComponent<PlayerController>();
            m_playerHealth[i] = player.GetComponent<PlayerHealth>();
            m_turrets[i] = player.GetComponent<Turret>();
            enemyGenerator.players[i] = player.GetComponent<Rigidbody2D>();
            player.name = "Player " + players[i].PeerId.ToString();
            playerController.PeerID = players[i].PeerId;
            playerController.UpdateRate = _playerUpdateRate;
            playerTransform.SetParent(thisTransform);
            m_turrets[i].BulletContainer = m_bulletContainer;
            m_turrets[i].ParticleContainer = m_particleContainer;
            

            bool isPlayer = players[i].PeerId == gameSparksSession.PeerId;
            if (isPlayer)
            {
                m_camera.ObjectToFollow = playerTransform;
            }

            playerController.GameSparks = m_gameSparksManager;
            playerController.SetupMultiplayer(m_spawnPoints[i], isPlayer);

            // Set player multiplayer component instead...
            m_players[i] = playerController;

            // Spawn HpBar
            GameObject hpBar = Instantiate(m_hpBarPrefab);
            hpBar.transform.SetParent(UiCanvas);
            hpBar.GetComponent<FollowTarget>().target = playerTransform;
            HpBarUi hpBarUi = hpBar.GetComponent<HpBarUi>();
            hpBarUi.Health = m_playerHealth[i];
            m_playerHealth[i]._onPlayerHpModified.AddListener(hpBarUi.UpdateHealth);
            m_playerHealth[i]._onPlayerDeath.AddListener(PlayerDead);
        }

        // Don't generate if not host
        m_enemyGenerator.GameSparksManager = m_gameSparksManager;
        IsHost = m_gameSparksManager.RTSession.PeerId == 1;
        m_enemyGenerator.SetupMultiplayer(IsHost);
        m_playerScore.MultiplayerManager = this;
    }
	
	private void PlayerDead()
    {
        bool playersStillAlive = false;
        for (int i = 0; i < m_players.Length; ++i)
        {
            if (!m_players[i]) continue;
            PlayerHealth ph = m_players[i].GetComponent<PlayerHealth>();
            if (ph.HP > 0)
            {
                playersStillAlive = true;
                break;
            }
        }
        if (!playersStillAlive)
        {
            m_onAllPlayersDead.Invoke();
        }
    }

    private void PacketReceived(RTPacket packet)
    {
        switch (packet.OpCode)
        {
            case (int)MultiplayerCodes.GAME_POINTS:
                UpdatePoints(packet);
                break;
            case (int) MultiplayerCodes.PLAYER_POSITION:
                UpdatePlayers(packet);
                break;
            case (int) MultiplayerCodes.PLAYER_BULLETS:
                UpdatePlayerBullets(packet);
                break;
            case (int)MultiplayerCodes.PLAYER_HEALTH:
                UpdatePlayerHealth(packet);
                break;
            case (int)MultiplayerCodes.ENEMY_SPAWN:
                UpdateEnemiesSpawn(packet);
                break;
        }
    }

    private void CheckPeers(RTPacket packet, Action<int> action)
    {
        for (int i = 0; i < m_players.Length; ++i)
        {
            if (m_players[i].PeerID == packet.Sender)
            {
                action(i);
                break;
            }
        }
    }

    private void UpdatePoints(RTPacket packet)
    {
        m_playerScore.Score = packet.Data.GetFloat(1).Value;
    }

    public void SendPoints(float value)
    {
        using (RTData data = RTData.Get())
        {
            float score = m_playerScore.Score;
            data.SetFloat(1, score);
            m_gameSparksManager.RTSession.SendData(
                MultiplayerCodes.GAME_POINTS.Int(),
                GameSparksRT.DeliveryIntent.RELIABLE,
                data
            );
        }
    }

    private void UpdatePlayers(RTPacket packet)
    {
        CheckPeers(packet, (index) =>
        {
            Transform playerTransform = m_players[index].transform;
            Rigidbody2D playerRigidBody = m_players[index].GetComponent<Rigidbody2D>();
            Quaternion playerRot = playerTransform.rotation;
            Vector3 playerAngles = playerRot.eulerAngles;

            // Referenced from PlayerController.SendTransformUpdates()
            Vector3 newPos = packet.Data.GetVector3(1).Value;
            Signs sign = (Signs) packet.Data.GetInt(2);
            newPos = SignsExt.Vector3Sign(newPos, sign);

            float newRot = packet.Data.GetFloat(3).Value;
            Vector2 newVel = packet.Data.GetVector2(4).Value;
            newVel = SignsExt.Vector2Sign(newVel, (Signs)packet.Data.GetInt(5));

            playerTransform.position = newPos;
            playerRigidBody.velocity = newVel;
            playerAngles.z = newRot;
            playerRot.eulerAngles = playerAngles;

            playerTransform.rotation = playerRot;

        });
    }

    private void UpdatePlayerBullets(RTPacket packet)
    {
        CheckPeers(packet, (index) =>
        {
            m_turrets[index].Fire();
        });
    }

    private void UpdatePlayerHealth(RTPacket packet)
    {
        CheckPeers(packet, (index) =>
        {
            m_playerHealth[index].HP = packet.Data.GetFloat(1).Value;
        });
    }

    private void UpdateEnemiesSpawn(RTPacket packet)
    {
        RTData data = packet.Data;

        int id = data.GetInt(1).Value;
        Vector2 position = data.GetVector2(3).Value;
        position = SignsExt.Vector2Sign(position, (Signs)data.GetInt(4).Value);
        float rotation = data.GetFloat(5).Value;
        Vector2 velocity = data.GetVector2(6).Value;
        velocity = SignsExt.Vector3Sign(velocity, (Signs)data.GetInt(7).Value);

        m_enemyGenerator.GenerateEnemy(id, position, rotation, velocity, true);
    }


    private void OnPlayerDisconnected(int peer)
    {
    }
}
