using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class AutoConnectPferdeRennspiel : MonoBehaviour
{
    public NetworkManager networkManager;

    private string ipRange = "192.168.0.";
    private int portNumber = 7779;

    private int closedports = 0;

    private bool suchtHost = true;

    private bool hatHostGefunden = false;

    private bool einClientWurdeGestartet;

    private string gefundeneClientIP = "";

    private void Start()
    {
        einClientWurdeGestartet = false;
    }

    void Update()
    {
        if (einClientWurdeGestartet == false)
        {
            AutoConnectToServer();
        }
    }

    public void AutoConnectToServer()
    {
        Parallel.For(101, 105, async i =>
        {
            string ipAddress = ipRange + i.ToString();
            //string ipAddress = "localhost";

            using (TcpClient tcpClient = new TcpClient())
            {

                // Connect to the remote endpoint asynchronously
                Task connectTask = tcpClient.ConnectAsync(ipAddress, portNumber);

                // Wait for the connection to complete or timeout after 500 milliseconds
                if (connectTask.Wait(750) && suchtHost == true)
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
        }
        );

        if (hatHostGefunden == true)
        {
            networkManager.networkAddress = gefundeneClientIP;
            networkManager.StartClient();

            einClientWurdeGestartet = true;
        }
        else if (hatHostGefunden == false)
        {
            networkManager.StartHost();
            networkManager.StartClient();

            einClientWurdeGestartet = true;
        }

    }
}

