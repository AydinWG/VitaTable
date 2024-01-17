using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class NetworkManagerPong : NetworkManager
{
    GameObject ball;
    public string[] activeGoalsStr;
    public PlayerStatManager playerStatManager;
    private float connectionDisableTime = 10f; // 10 seconds for closing the host
    private bool disableConnections;

    public override void Start()
    {
        // Call the Start method directly
        base.Start();
        StartCoroutine(DisableNewConnections());
    }

    private IEnumerator DisableNewConnections()
    {
        yield return new WaitForSeconds(connectionDisableTime);

        // Set maxConnections to the number of connected players after 10 seconds
        maxConnections = numPlayers;
        disableConnections = true;

    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (disableConnections == true)
        {
            conn.Disconnect();
            return;
        }
        else
        {
            GameObject player;

            if (numPlayers == 0)
            {
                player = Instantiate(playerPrefab);
            }
            else
            {
                player = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Racket" + numPlayers));
            }

            activeGoalsStr[numPlayers] = "WallRacket" + numPlayers.ToString();

            NetworkServer.AddPlayerForConnection(conn, player);

            playerStatManager.AddScorePickup();
        }
    }

    public void BallSpawn()
    {
        ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
        NetworkServer.Spawn(ball);
    }

    public void DespawnBall()
    {
        NetworkServer.Destroy(ball);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        // Call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);
    }
}