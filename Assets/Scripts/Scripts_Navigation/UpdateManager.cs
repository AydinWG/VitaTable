using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using UnityEngine.SceneManagement;

public class UpdateManager : MonoBehaviour
{
    public UnityEngine.Ping ping;

    public GameObject tischnotconnectedTXTGO;

    public GameObject updatesucheTXTGO;

    public GameObject netzwerkSucheBTNGO;

    public GameObject updateVerfuegbarTXTGO;

    public GameObject jaBTNGO;

    public GameObject neinBTNGO;

    public GameObject updateSucheLadeanzeigeGO;

    private const string ServerIp = "192.168.0.11";
    private const int ServerPort = 8383;

    private TcpClient _client;
    private NetworkStream _stream;

    private string newMessage;
    private bool isNewMessageReceived;

    public static Thread _receiveThread;

    string ipAddress;

    string ssidMessage;

    public GameObject networkNotFoundTXTGO;

    bool onePingFailed;

    int pingsSuccessfull;

    private bool IsNetworkAvailable()
    {
        return NetworkInterface.GetIsNetworkAvailable();
    }

    void Start()
    {
        SetParameters();

        if (IsNetworkAvailable())
        {
            StartPingToGoogle("www.google.com");
            //StartPingToGoogle("192.168.0.103");
            //StartPingToGoogle("192.168.0.104");

            CheckUpdate();
        }
        else
        {
            updateVerfuegbarTXTGO.SetActive(true);
            updateVerfuegbarTXTGO.GetComponent<Text>().text = "Verbinden Sie bitte das Gerät mit einem Netzwerk.";
        }
    }

    private void SetParameters()
    {
        //mySQLConnector = new MySQLConnector();

        onePingFailed = false;

        pingsSuccessfull = 0;

        _client = new TcpClient();
        _client.Connect(ServerIp, ServerPort);
        _stream = _client.GetStream();

        _receiveThread = new Thread(ReceiveMessage);
        _receiveThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //tcpTextAusgabe.text = newMessage;

        if (isNewMessageReceived)
        {
            SetButtonInteractability(true);
        }

        if (isNewMessageReceived && newMessage.Contains("Odroid") && pingsSuccessfull == 3)
        {
            jaBTNGO.SetActive(true);

            neinBTNGO.SetActive(true);

            updateVerfuegbarTXTGO.SetActive(true);

            updatesucheTXTGO.SetActive(false);
            updateSucheLadeanzeigeGO.SetActive(false);

            isNewMessageReceived = false;
        }
        else if (isNewMessageReceived && newMessage.Contains("Keine neue Version verfuegbar"))
        {
            updatesucheTXTGO.SetActive(false);

            updateSucheLadeanzeigeGO.SetActive(false);

            updateVerfuegbarTXTGO.SetActive(true);
            updateVerfuegbarTXTGO.GetComponent<Text>().text = "Sie sind auf dem neuesten Stand.";

            isNewMessageReceived = false;
        }

        if (isNewMessageReceived && newMessage.Contains("Download erfolgreich"))
        {
            InstallUpdate();
            isNewMessageReceived = false;
        }
        else if (isNewMessageReceived && newMessage.Contains("Download fehlerhaft"))
        {
            //TODO: Kunden anzeigen nochmal den Download zu starten

            updateVerfuegbarTXTGO.SetActive(true);

            updateVerfuegbarTXTGO.GetComponent<Text>().text = "Download war fehlerhaft. Download neu starten?";

            jaBTNGO.SetActive(true);

            neinBTNGO.SetActive(true);

            isNewMessageReceived = false;
        }

        if (isNewMessageReceived && newMessage.Contains("Updates erfolgreich installiert"))
        {
            updatesucheTXTGO.SetActive(false);

            updateSucheLadeanzeigeGO.SetActive(false);

            updateVerfuegbarTXTGO.SetActive(true);
            updateVerfuegbarTXTGO.GetComponent<Text>().text = "Updates erfolgreich installiert.";
        }
    }

    void SetButtonInteractability(bool interactable)
    {
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = interactable;
        }
    }

    private void StartPingToGoogle(string pingAddress)
    {
        ping = new UnityEngine.Ping(pingAddress); // Use UnityEngine.Ping instead of System.Net.NetworkInformation.Ping

        StartCoroutine(StartPingCoroutine(pingAddress));
    }

    private IEnumerator StartPingCoroutine(string pingAddress)
    {
        yield return new WaitForSeconds(1f); // Wait for a short time before starting the ping

        while (!ping.isDone)
        {
            yield return null; // Wait until the ping is done
        }

        if (ping.time >= 0) // Check if the ping was successful
        {
            updatesucheTXTGO.SetActive(true);
            updateSucheLadeanzeigeGO.SetActive(true);

            StartPingToTablet("192.168.0.101");
            StartPingToTablet("192.168.0.102");

            pingsSuccessfull++;
        }
        else
        {
            onePingFailed = true;

            ping.DestroyPing();

            StopAllCoroutines();

            updatesucheTXTGO.SetActive(false);
            updateSucheLadeanzeigeGO.SetActive(false);

            tischnotconnectedTXTGO.SetActive(true);
            netzwerkSucheBTNGO.SetActive(true);
        }
    }

    private void StartPingToTablet(string pingAddress)
    {
        ping = new UnityEngine.Ping(pingAddress); // Use UnityEngine.Ping instead of System.Net.NetworkInformation.Ping

        StartCoroutine(StartPingTabletCoroutine(pingAddress));
    }

    private IEnumerator StartPingTabletCoroutine(string pingAddress)
    {
        yield return new WaitForSeconds(1f); // Wait for a short time before starting the ping

        while (!ping.isDone)
        {
            yield return null; // Wait until the ping is done
        }

        if (ping.time >= 0) // Check if the ping was successful
        {
            updatesucheTXTGO.SetActive(true);
            updateSucheLadeanzeigeGO.SetActive(true);

            StartPingToTablet("192.168.0.101");
            StartPingToTablet("192.168.0.102");

            pingsSuccessfull++;
        }
        else
        {
            onePingFailed = true;

            ping.DestroyPing();

            StopAllCoroutines();

            tischnotconnectedTXTGO.SetActive(false);
            netzwerkSucheBTNGO.SetActive(false);

            updateSucheLadeanzeigeGO.SetActive(false);

            updatesucheTXTGO.SetActive(false);

            jaBTNGO.SetActive(false);

            neinBTNGO.SetActive(false);

            updateVerfuegbarTXTGO.SetActive(true);
            updateVerfuegbarTXTGO.GetComponent<Text>().text = "Stellen Sie sicher, dass alle Tablets an und verbunden mit dem Netzwerk sind.";
        }
    }

    public void CheckUpdate()
    {
        ssidMessage = ipAddress + ";checkupdate;;;;";

        SendToServer();
    }

    public void DownloadUpdate()
    {
        ssidMessage = ipAddress + ";downloadupdate;;;;";

        SendToServer();
    }

    public void InstallUpdate()
    {
        ssidMessage = ipAddress + ";installupdate;;;;";

        SendToServer();
    }

    private void ReceiveMessage()
    {
        byte[] data = new byte[1024];
        int bytes = 0;
        StringBuilder builder = new StringBuilder();

        while (true)
        {
            do
            {
                bytes = _stream.Read(data, 0, data.Length);
                builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
            } while (_stream.DataAvailable);

            newMessage = builder.ToString();

            builder.Clear();

            isNewMessageReceived = true;
        }
    }

    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName())
            .AddressList.First(
                f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            .ToString();
    }

    public void SendToServer()
    {
        byte[] data = new byte[1024];

        data = Encoding.UTF8.GetBytes(ssidMessage);
        _stream.Write(data, 0, data.Length);
    }

    public void DoUpdate()
    {

        DownloadUpdate();

        jaBTNGO.SetActive(false);

        neinBTNGO.SetActive(false);

        updatesucheTXTGO.SetActive(true);

        updatesucheTXTGO.GetComponent<Text>().text = "Update wird installiert";

        updateSucheLadeanzeigeGO.SetActive(true);

        updateVerfuegbarTXTGO.SetActive(false);

        SetButtonInteractability(false);
    }

    public void DontUpdate()
    {
        SceneManager.LoadScene(16);
    }
}