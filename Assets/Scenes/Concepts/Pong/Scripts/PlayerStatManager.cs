using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerStatManager : NetworkBehaviour
{
    public readonly SyncList<int> scoreList = new SyncList<int>();

    public readonly SyncList<int> platzierungList = new SyncList<int>();

    private NetworkManagerPong networkManagerPong;

    [SerializeField] private GameObject ausgeschiedenTXT;

    private void Start()
    {
        networkManagerPong = FindObjectOfType<NetworkManagerPong>();
    }

    public void AddScorePickup()
    {
        scoreList.Add(3);

        if (isServer)
        {
            ColorWalls();
        }
    }

    [ClientRpc]
    void ColorWalls()
    {
        for (int i = 0; i < scoreList.Count(); i++)
        {
            GameObject wallRacketObject = GameObject.Find("WallRacket" + i);

            SpriteRenderer wallRacketRenderer = wallRacketObject.GetComponent<SpriteRenderer>();

            switch (i)
            {
                case 0:
                    wallRacketRenderer.color = new Color32(0, 255, 230, 255);
                    break;
                case 1:
                    wallRacketRenderer.color = new Color32(0, 166, 255, 255);
                    break;
                case 2:
                    wallRacketRenderer.color = new Color32(255, 158, 0, 255);
                    break;
                case 3:
                    wallRacketRenderer.color = new Color32(255, 0, 11, 255);
                    break;
                default:
                    wallRacketRenderer.color = Color.gray;
                    break;
            }
        }
    }

    public void LifeLoss(int playerIndex)
    {
        if (scoreList[playerIndex] > 1)
        {
            UpdateUI(playerIndex);
            scoreList[playerIndex]--;
        }
        else if (networkManagerPong.numPlayers > 2)
        {
            int platzierung = networkManagerPong.numPlayers - platzierungList.IndexOf(playerIndex);

            SpielerAusgeschieden(playerIndex);
        }
        else if (scoreList[playerIndex] == 1)
        {
            platzierungList.Add(playerIndex);

            int platzierung = networkManagerPong.numPlayers - platzierungList.IndexOf(playerIndex);

            networkManagerPong.StopClient();
            networkManagerPong.StopHost();


            Destroy(networkManagerPong);
        }
    }

    [ClientRpc]
    void UpdateUI(int playerIndex)
    {
        GameObject scoreToChange = GameObject.Find("Racket" + playerIndex + "(Clone)");
        Canvas canvasToChange = scoreToChange.GetComponentInChildren<Canvas>();
        Transform lebenTransform = canvasToChange.transform.Find("Leben" + scoreList[playerIndex]);

        if (lebenTransform != null)
        {
            lebenTransform.gameObject.SetActive(false);
        }
    }

    void SpielerAusgeschieden(int playerIndex)
    {
        if (networkManagerPong.activeGoalsStr[playerIndex] != "")
        {
            platzierungList.Add(playerIndex);

            if (isServer)
            {
                MakeWallGray(playerIndex);
            }

            RpcRacketVerschwindenLassen(playerIndex);
        }

        string strToReplace = "WallRacket" + playerIndex;

        int index = Array.IndexOf(networkManagerPong.activeGoalsStr, strToReplace);

        networkManagerPong.activeGoalsStr[index] = "";

        // Disconnect the client when the player is out of the game
        if (isLocalPlayer)
        {
            NetworkManager.singleton.StopClient();
        }
    }

    [ClientRpc]
    void MakeWallGray(int playerIndex)
    {
        GameObject wallRacketObject = GameObject.Find("WallRacket" + playerIndex);

        SpriteRenderer wallRacketRenderer = wallRacketObject.GetComponent<SpriteRenderer>();

        wallRacketRenderer.color = Color.gray;
    }

    [ClientRpc]
    void RpcRacketVerschwindenLassen(int playerIndex)
    {
        GameObject scoreToChange = GameObject.Find("Racket" + playerIndex + "(Clone)");

        scoreToChange.SetActive(false);
    }

    public void OnClientDisconnected()
    {
        // Call the method to make the own wall racket grey when the client disconnects
        if (isLocalPlayer)
        {
            // Get the player index associated with the client's connection
            int playerIndex = NetworkConnection.LocalConnectionId;
            SpielerAusgeschieden(playerIndex);
        }
    }


    void Update()
    {
        NetworkClient.OnDisconnectedEvent += OnClientDisconnected;
    }
}
