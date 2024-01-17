using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using PTClient;
using System.Linq.Expressions;
using Mirror;

public class SceneSwitcherSpielauswahl : MonoBehaviour
{
    public static int spielLevel;

    public static string spielName;

    public static bool isVideoWorkout = false;

    public static bool isPreWorkout = true;

    public NetworkManager networkManager;

    private void Start()
    {
        GameObject networkManagerObject = GameObject.Find("NetworkManager");

        if (networkManagerObject != null && networkManager == null)
        {
            networkManager = networkManagerObject.GetComponent<NetworkManager>();
        }
    }

    public void VideoTutorialPreWorkoutZuEnde()
    {
        //spielLevel = 1;

        isVideoWorkout = true;

        SceneManager.LoadScene(4);
    }

    public void CloseApp()
    {
        Application.Quit();
    }
    public void BackToHome()
    {
        if (networkManager != null)
        {
            KillNetworkManager();
        }

        MySQLConnector.spieleID = null;

        spielName = null;

        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        Spielladen();
    }

    public void BackToLevel()
    {
        switch (spielName)
        {
            case "Obstsalat":
                ObstsalatAuswahl();
                break;
            case "Autofahren":
                AutofahrenAuswahl();
                break;
            case "Kaffeerunde":
                KaffeerundeAuswahl();
                break;
            case "Pferderennen":
                PferderennenAuswahl();
                break;
            case "Brunnen":
                BrunnenAuswahl();
                break;
            case "Wettrechnen":
                WettrechnenAuswahl();
                break;
            case "Videotraining":
                VideotrainingAuswahl();
                break;
        }
    }

    public static void RandomSpielLaden()
    {
        string[] spieleArr = { "Obstsalat", "Autofahren", "Kaffeerunde", "Pferderennen", "Brunnen", "Wettrechnen" };

        int rnd = UnityEngine.Random.Range(0, 6);

        spielName = spieleArr[rnd];

        spielLevel = 1;

        Spielladen();
    }

    public void ObstsalatAuswahl()
    {
        spielName = "Obstsalat";

        MySQLConnector.spieleID = null;

        SceneManager.LoadScene(1);
    }

    public void PuzzleAuswahl()
    {
        spielName = "Puzzle";

        SceneManager.LoadScene(19);
    }

    public void VideotrainingAuswahl()
    {
        spielName = "Videotraining";

        MySQLConnector.spieleID = null;

        SceneManager.LoadScene(1);
    }

    public void AutofahrenAuswahl()
    {
        spielName = "Autofahren";

        MySQLConnector.spieleID = null;

        Spielladen();
    }

    public void KaffeerundeAuswahl()
    {
        spielName = "Kaffeerunde";

        MySQLConnector.spieleID = null;

        SceneManager.LoadScene(1);
    }

    public void PferderennenAuswahl()
    {
        spielName = "Pferderennen";

        SceneManager.LoadScene(1);
    }

    public void BrunnenAuswahl()
    {
        spielName = "Brunnen";

        SceneManager.LoadScene(8);
    }

    public void WettrechnenAuswahl()
    {
        spielName = "Wettrechnen";

        SceneManager.LoadScene(1);
    }

    public void MusikAuswahl()
    {
        spielName = "Musik";

        SceneManager.LoadScene(14);
    }

    public void TennisAuswahl()
    {
        spielName = "Tennis";

        SceneManager.LoadScene(11);
    }

    public void EinfachAuswahl()
    {
        spielLevel = 1;

        Spielladen();
    }

    public void MittelAuswahl()
    {
        spielLevel = 2;

        Spielladen();
    }

    public void SchwerAuswahl()
    {
        spielLevel = 3;

        Spielladen();
    }

    public void PongNeustart()
    {
        spielName = "Tennis";

        SceneManager.LoadScene(11);
    }

    public static void Spielladen()
    {
        switch (spielName)
        {
            case "Obstsalat":
                SceneManager.LoadScene(4);
                break;
            case "Autofahren":
                SceneManager.LoadScene(5);
                break;
            case "Kaffeerunde":
                SceneManager.LoadScene(6);
                break;
            case "Pferderennen":
                SceneManager.LoadScene(7);
                break;
            case "Brunnen":
                SceneManager.LoadScene(8);
                break;
            case "Wettrechnen":
                SceneManager.LoadScene(9);
                break;
            case "Tennis":
                SceneManager.LoadScene(11);
                break;
            case "Videotraining":
                SceneManager.LoadScene(12);
                break;
            case "Musik":
                SceneManager.LoadScene(14);
                break;
            case "Puzzle":
                SceneManager.LoadScene(19);
                break;
        }
    }

    public void SystemAuswahl()
    {
        SceneManager.LoadScene(16);
    }

    public void WlanAuswahl()
    {
        SceneManager.LoadScene(17);
    }

    public void UpdateAuswahl()
    {
        SceneManager.LoadScene(20);
    }

    public void Einstellungen()
    {
        SceneManager.LoadScene(10);
    }

    public void InfoAuswahl()
    {
        SceneManager.LoadScene(21);
    }

    public void KillNetworkManager()
    {
        networkManager.StopClient();
        networkManager.StopHost();


        Destroy(networkManager);
    }
}
