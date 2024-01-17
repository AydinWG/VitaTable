using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using Org.BouncyCastle.Utilities.IO.Pem;

public class NetworkManagerPferderennen : NetworkManager
{
    private float connectionDisableTime = 10f;
    private bool disableConnections;

    // Start is called before the first frame update
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
        if (disableConnections)
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
                player = Instantiate(spawnPrefabs[numPlayers - 1]);
            }

            NetworkServer.AddPlayerForConnection(conn, player);
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        // Call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);
    }

    public void KIPferdeSpawnen()
    {
        GameObject kiHorse;

        for (int i = numPlayers; i < 4; i++)
        {
            kiHorse = Instantiate(spawnPrefabs[i - 1]);
            NetworkServer.Spawn(kiHorse);
        }
    }

    public void StartHorseMovementAfterCountdown()
    {
        // Find all Pferde_Movement scripts in the scene
        Pferde_Movement[] pferdeMovements = FindObjectsOfType<Pferde_Movement>();

        foreach (Pferde_Movement pferdeMovement in pferdeMovements)
        {
            //pferdeMovement.LosRennen(); // Start horse movement after the countdown
        }
    }
}
