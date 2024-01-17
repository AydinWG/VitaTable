using System.Collections;
using UnityEngine;
using System.Net;
using UnityEngine.Networking;
using System.Collections.Generic;
using Mirror.Discovery;
using Mirror;
using System.Net.NetworkInformation;
using Telepathy;
using System.Linq;
using System.Net.Sockets;
using System;
using PTClient;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManagerMusic : NetworkManager
{
    public AudioSource[] audioSources;

    public bool startTimer = false;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Create a new player object with a network identity
        GameObject playerObject;

        switch (numPlayers)
        {
            case 0:
            default:
                playerObject = Instantiate(playerPrefab); 
                NetworkServer.AddPlayerForConnection(conn, playerObject);
                audioSources[numPlayers - 1] = playerObject.GetComponent<AudioSource>();
                break;
            case 1:
                playerObject = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "DrumClientVolumeControl"));
                NetworkServer.AddPlayerForConnection(conn, playerObject);
                audioSources[numPlayers - 1] = playerObject.GetComponent<AudioSource>();
                break;
            case 2:
                playerObject = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "BassClientVolumeControl"));
                NetworkServer.AddPlayerForConnection(conn, playerObject);
                audioSources[numPlayers - 1] = playerObject.GetComponent<AudioSource>();
                break;
            case 3:
                playerObject = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "VocalClientVolumeControl"));
                NetworkServer.AddPlayerForConnection(conn, playerObject);
                audioSources[numPlayers - 1] = playerObject.GetComponent<AudioSource>();
                break;
        }
    }

    public void StartAudioTracks()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i] != null)
            {
                audioSources[i].GetComponent<AudioSource>().Play();
            }
        }

        startTimer = true;
    }
}
