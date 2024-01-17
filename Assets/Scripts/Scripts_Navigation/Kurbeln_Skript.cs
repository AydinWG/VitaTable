using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using UnityEngine.Events;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class Kurbeln_Skript : MonoBehaviour
{
    public Slider kurbelSlider;

    public static int richtigGemacht;

    public static int falschGemacht;

    public static Mutex mutexCIN = new Mutex();
    public static Mutex mutexCOUT = new Mutex();
    public static string globalMessageIN = "";
    public static string globalMessageOUT = "Hallo vom *** CLIENT ***";

    public static bool udpGestartet = false;

    public static string empfangeneDatenKurbelRichtungStr;

    public static float empfangeneDatenKurbelWerteFloat;

    public static string empfangeneDatenStr;

    public UnityAction kurbelListener;

    public static string personID;


    private void Awake()
    {
        Einstellungen_Script.PlatzDatenArrayBefuellen();

        personID = Guid.NewGuid().ToString();
    }

    // Start is called before the first frame update
    // UDP Starten
    private void Start()
    {
        if (!udpGestartet)
        {
            Thread udpCThread = new Thread(UDPThread);
            udpCThread.Priority = System.Threading.ThreadPriority.Highest;
            udpCThread.Start();

            udpGestartet = true;
        }

        Einstellungen_Script.PlatzIdentifizierung();
    }

    private void UDPThread()
    {
        //UDP
        int listenPort = 11000;
        UdpClient listener = new UdpClient(listenPort);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

        string[] nachPipe;
        string[] empfangeneStrArr;
        string[] empfangeWerte;

        try
        {
            while (true)
            {
                //TODO in Spiele einbinden
                byte[] bytes = listener.Receive(ref groupEP);

                empfangeneDatenStr = Encoding.ASCII.GetString(bytes, 0, bytes.Length);

                nachPipe = empfangeneDatenStr.Split('|');

                empfangeneStrArr = nachPipe[1].Split(':');

                empfangeWerte = empfangeneStrArr[Einstellungen_Script.kurbelOben].Split(' ');

                string newKurbelRichtungStr;
                float newKurbelWertFloat;

                switch (empfangeWerte[0])
                {
                    case "f":
                        newKurbelRichtungStr = "forward";
                        break;
                    case "b":
                        newKurbelRichtungStr = "backward";
                        break;
                    case "s":
                    default:
                        newKurbelRichtungStr = "standing";
                        break;
                }

                if (float.TryParse(empfangeWerte[1], out newKurbelWertFloat))
                {
                    mutexCIN.WaitOne();
                    empfangeneDatenKurbelRichtungStr = newKurbelRichtungStr;
                    empfangeneDatenKurbelWerteFloat = newKurbelWertFloat;
                    mutexCIN.ReleaseMutex();

                    MySQLConnector.InsertSensorData(empfangeneDatenKurbelWerteFloat);
                }
                else
                {
                    // Handle the failed float conversion
                    Debug.LogError("Failed to parse float value: " + empfangeWerte[1]);
                }
            }
        }
        catch (SocketException e)
        {
            Debug.LogError(e);
        }
    }

    private void Update()
    {
        kurbelWirdBetaetigt();
    }

    public void kurbelWirdBetaetigt()
    {
        if (kurbelSlider.value != 1)
        {
            if (empfangeneDatenKurbelRichtungStr == "forward" || empfangeneDatenKurbelRichtungStr == "backward")
            {
                kurbelSlider.value += 0.01f;

                if (kurbelSlider.value >= 0.1f)
                {
                    kurbelSlider.gameObject.SetActive(true);
                }

                CancelInvoke();
            }
            else
            {
                InvokeRepeating("VersteckeSlider", 3, 1);
            }
        }
        else
        {
            //entsprechende Szene laden lassen
            SceneSwitcherSpielauswahl.RandomSpielLaden();
        }
    }

    public void VersteckeSlider()
    {
        kurbelSlider.value = 0;

        kurbelSlider.gameObject.SetActive(false);
    }

}
