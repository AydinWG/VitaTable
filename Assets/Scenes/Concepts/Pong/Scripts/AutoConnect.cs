using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Net.Sockets;

public class AutoConnect : MonoBehaviour
{
    public NetworkManager networkManager;
    public SceneSwitcherSpielauswahl sceneSwitcherSpielauswahl;

    private string ipRange;
    private int portNumber;
    private int closedports;
    private bool suchtHost;
    private bool hatHostGefunden;
    private bool einClientWurdeGestartet = false;
    private string gefundeneClientIP;

    private void Start()
    {
        suchtHost = true;
        closedports = 0;
        hatHostGefunden = false;
        ipRange = "192.168.0.";
        portNumber = 7777;
        gefundeneClientIP = "";

        // Subscribe to the disconnection event only once in the Start() method
        NetworkClient.OnDisconnectedEvent += OnClientDisconnected;
    }

    void Update()
    {
        if (!einClientWurdeGestartet)
        {
            AutoConnectToServer();
        }
    }

    public void AutoConnectToServer()
    {
        Parallel.For(101, 105, async i =>
        {
            string ipAddress = ipRange + i.ToString();

            using (TcpClient tcpClient = new TcpClient())
            {
                // Connect to the remote endpoint asynchronously
                Task connectTask = tcpClient.ConnectAsync(ipAddress, portNumber);

                // Wait for the connection to complete or timeout after 500 milliseconds
                if (connectTask.Wait(500) && suchtHost)
                {
                    gefundeneClientIP = ipAddress;
                    suchtHost = false;
                    hatHostGefunden = true;
                }
                else
                {
                    closedports += 1;
                    if (closedports == 4)
                    {
                        suchtHost = false;
                        hatHostGefunden = false;
                    }
                }
            }
        });

        if (hatHostGefunden)
        {
            // Connect to the host
            networkManager.networkAddress = gefundeneClientIP;
            networkManager.StartClient();

            einClientWurdeGestartet = true;
        }
        else
        {
            // Start a new host if no suitable host is found
            networkManager.StartHost();
            einClientWurdeGestartet = true;
        }
    }

    // Callback when the client disconnects
    private void OnClientDisconnected()
    {
        sceneSwitcherSpielauswahl.KillNetworkManager();
    }
}