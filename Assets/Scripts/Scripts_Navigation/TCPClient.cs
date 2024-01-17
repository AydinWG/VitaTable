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


public class TCPClient : MonoBehaviour
{
    public GameObject ladeAnimationVerbinde;
    public GameObject verbindungsAufbau;
    public GameObject passwordCheckIMG;
    public GameObject messageInputFieldGO;
    public GameObject verbindenButtonGO;
    public GameObject netzwerkNameTXTGO;

    public GameObject ssidButtons;

    bool isWrongPassword;

    public Text placeHolder;

    public InputField messageInputField;

    public Sprite[] visibility;

    bool pwvisible;

    public Text netzwerkNameTXT;

    //bool wlanSet;

    public Button eyeButton;

    public static string netzwerkName;

    public GameObject verfuegbareNetzwerke;
    public GameObject ladeAnimation;
    public GameObject netzwerkSuche;

    public string[] elementsToSplit;

    public Sprite[] wlanSignalStrengthsIMGArr;

    public List<string> netzwerkNamen = new List<string>();
    public List<string> staerksteEinzigartigeNetzwerkNamen = new List<string>();
    public List<string> staerksteEinzigartigesignalStaerken = new List<string>();
    public List<string> signalStaerken = new List<string>();

    public Transform scrollViewContent;
    public GameObject buttonPrefab;
    public GameObject scrollView;

    private const string ServerIp = "192.168.0.11";
    private const int ServerPort = 8383;

    public static TcpClient _client;
    public NetworkStream _stream;

    private static string _newMessage;
    private static bool _isNewMessageReceived;

    public static Thread _receiveThread;

    public static bool wlanChosen;

    string ipAddress;

    string ssidMessage;

    public System.Net.NetworkInformation.Ping ping;

    public GameObject networkFoundTXTGO;

    public GameObject netwerkSearchBTNGO;

    public GameObject networkNotFoundTXTGO;

    private void Start()
    {
        SetParameters();

        if (IsNetworkAvailable())
        {
            StartPingToGoogle();
        }
        else
        {
            networkNotFoundTXTGO.SetActive(true);
        }
    }

    private bool IsNetworkAvailable()
    {
        return NetworkInterface.GetIsNetworkAvailable();
    }

    private void SetParameters()
    {
        _client = new TcpClient();
        _client.Connect(ServerIp, ServerPort);
        _stream = _client.GetStream();

        _receiveThread = new Thread(ReceiveMessage);
        _receiveThread.Start();

        pwvisible = true;

        wlanChosen = false;

        _newMessage = "";
    }

    private void Update()
    {
        if (_isNewMessageReceived && !_newMessage.Contains("Done"))
        {
            StartCoroutine(WaitForOneSecond());

            AddButton(_newMessage);
            _isNewMessageReceived = false;
            SetButtonInteractability(true);
        }

        if (wlanChosen)
        {
            ssidButtons.SetActive(false);
            verfuegbareNetzwerke.SetActive(false);

            netzwerkNameTXT.text = WlanAuswahl.netzwerkName;

            messageInputFieldGO.SetActive(true);
            verbindenButtonGO.SetActive(true);
            netzwerkNameTXTGO.SetActive(true);

            PasswordVisibility();

            wlanChosen = false;
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

    private void StartPingToGoogle()
    {
        ping = new System.Net.NetworkInformation.Ping();
        ping.PingCompleted += new PingCompletedEventHandler(StartPingCompletedCallback);

        try
        {
            ping.SendAsync("www.google.com", 500, new object());

            networkFoundTXTGO.SetActive(true);
            netwerkSearchBTNGO.SetActive(true);
        }
        catch
        {
            ScanSSIDs();
        }
    }

    private void StartPingCompletedCallback(object sender, PingCompletedEventArgs e)
    {
        if (e.Reply.Status == IPStatus.Success)
        {
            networkFoundTXTGO.SetActive(true);
            netwerkSearchBTNGO.SetActive(true);
        }
        else
        {
            ping.Dispose();
        }
    }

    private void SendPingToGoogle()
    {
        ping = new System.Net.NetworkInformation.Ping();
        ping.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

        SetButtonInteractability(true);

        try
        {
            ping.SendAsync("www.google.com", new object());

        }
        catch
        {
            CancelInvoke();

            ladeAnimationVerbinde.SetActive(false);
            verbindungsAufbau.SetActive(false);
            messageInputFieldGO.SetActive(true);
            verbindenButtonGO.SetActive(true);
            netzwerkNameTXTGO.SetActive(true);

            passwordCheckIMG.SetActive(false);
            messageInputField.text = "";

            placeHolder.text = "Falsches Passwort";
            placeHolder.color = Color.red;

            SetParameters();
        }
    }

    private void PingCompletedCallback(object sender, PingCompletedEventArgs e)
    {
        if (e.Reply.Status == IPStatus.Success)
        {
            // Ping was successful
            ladeAnimationVerbinde.SetActive(false);
            verbindungsAufbau.SetActive(false);
            messageInputFieldGO.SetActive(true);
            passwordCheckIMG.SetActive(true);
            verbindenButtonGO.SetActive(true);
            netzwerkNameTXTGO.SetActive(true);

            SetParameters();

            CancelInvoke();
        }
        else
        {
            ping.Dispose();
        }
    }

    public void PasswordVisibility()
    {
        if (pwvisible)
        {
            pwvisible = false;

            eyeButton.GetComponent<Image>().sprite = visibility[1];
            messageInputField.contentType = InputField.ContentType.Password; // Set the input field's content type to password
            messageInputField.ForceLabelUpdate(); // Update the input field's label to hide the text
        }
        else
        {
            pwvisible = true;

            eyeButton.GetComponent<Image>().sprite = visibility[0];
            messageInputField.contentType = InputField.ContentType.Standard; // Set the input field's content type to standard (visible)
            messageInputField.ForceLabelUpdate(); // Update the input field's label to show the text
        }
    }

    private IEnumerator WaitForOneSecond()
    {
        yield return new WaitForSeconds(1f);
    }

    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName())
            .AddressList.First(
                f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            .ToString();
    }

    public void ScanSSIDs()
    {
        networkFoundTXTGO.SetActive(false);
        netwerkSearchBTNGO.SetActive(false);

        netzwerkSuche.SetActive(true);
        ladeAnimation.SetActive(true);

        ipAddress = GetLocalIPv4();

        // Combine the selected option with the message
        ssidMessage = ipAddress + ";scan;;;;";

        SetButtonInteractability(false);

        SendToServer();
    }

    public void SetWLAN()
    {
        _newMessage = "";

        //Testfeld.text = _newMessage;

        ipAddress = GetLocalIPv4();

        if (!string.IsNullOrEmpty(messageInputField.text))
        {
            ladeAnimationVerbinde.SetActive(true);
            verbindungsAufbau.SetActive(true);
            messageInputFieldGO.SetActive(false);
            verbindenButtonGO.SetActive(false);
            netzwerkNameTXTGO.SetActive(false);
            passwordCheckIMG.SetActive(false);

            SetButtonInteractability(false);

            //ssidMessage = "192.168.0.102;setWLAN;Vodafone-8E7C;HdTzgY7tLbNExrH7;WPA2-PSK;aes";

            ssidMessage = ipAddress.ToString() + ";setWLAN;" + WlanAuswahl.netzwerkName.ToString() + ";" + messageInputField.text.ToString() + ";WPA2-PSK;" + "aes";

            SendToServer();

            if (_receiveThread != null && _receiveThread.IsAlive)
            {
                _receiveThread.Abort();
            }
        }
        else
        {
            placeHolder.text = "Passwort eingeben";
            placeHolder.color = Color.red;
        }

        InvokeRepeating("SendPingToGoogle", 120.0f, 1.0f);
    }

    public void SendToServer()
    {
        byte[] data = new byte[1024];

        data = Encoding.UTF8.GetBytes(ssidMessage);
        _stream.Write(data, 0, data.Length);
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

            _newMessage = builder.ToString();

            builder.Clear();

            _isNewMessageReceived = true;
        }
    }

    private void AddButton(string message)
    {
        elementsToSplit = message.Split(',');

        int maxIndex = elementsToSplit.Length - 1; // Get the index of the last element

        bool networkNameWasEmpty = false;

        for (int i = elementsToSplit.Length - 1; i >= 0; i--)
        {
            if (i != maxIndex &&
                elementsToSplit[i] != "AES" &&
                elementsToSplit[i] != "WPA2-PSK" &&
                elementsToSplit[i] != "WPA-PSK" &&
                elementsToSplit[i] != "None" &&
                !string.IsNullOrEmpty(elementsToSplit[i]) &&
                !Regex.IsMatch(elementsToSplit[i], @"^\d$") &&
                !Regex.IsMatch(elementsToSplit[i], @"^\d\d$") &&
                !Regex.IsMatch(elementsToSplit[i], @"^\d\d\d$"))
            {
                netzwerkNamen.Add(elementsToSplit[i]);

                networkNameWasEmpty = false;
            }
            else if (string.IsNullOrEmpty(elementsToSplit[i]))
            {
                networkNameWasEmpty = true;
            }
            else if (Regex.IsMatch(elementsToSplit[i], @"^\d$") && !networkNameWasEmpty || Regex.IsMatch(elementsToSplit[i], @"^\d\d$") && !networkNameWasEmpty || Regex.IsMatch(elementsToSplit[i], @"^\d\d\d$") && !networkNameWasEmpty)
            {
                signalStaerken.Add(elementsToSplit[i]);
            }
        }

        for (int i = 0; i < netzwerkNamen.Count; i++)
        {
            // Check if the network name is already in the unique set
            if (!staerksteEinzigartigeNetzwerkNamen.Contains(netzwerkNamen[i]))
            {
                staerksteEinzigartigeNetzwerkNamen.Add((netzwerkNamen[i]));
                staerksteEinzigartigesignalStaerken.Add((signalStaerken[i]));
            }
        }

        float buttonHeight = buttonPrefab.GetComponent<RectTransform>().rect.height;
        float spacing = 10f; // Vertical spacing between buttons
        float totalHeight = staerksteEinzigartigeNetzwerkNamen.Count * (buttonHeight + spacing) - spacing;

        // Set the height of the content based on the total height
        RectTransform contentTransform = scrollViewContent.GetComponent<RectTransform>();
        contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, totalHeight);

        // Calculate the initial y position of the top button
        float initialY = totalHeight / 2f;

        int numElements = Mathf.Min(staerksteEinzigartigeNetzwerkNamen.Count, staerksteEinzigartigesignalStaerken.Count);

        for (int i = numElements - 1; i >= 0; i--)
        {
            GameObject buttonObject = Instantiate(buttonPrefab, scrollViewContent);

            RectTransform buttonTransform = buttonObject.GetComponent<RectTransform>();
            buttonTransform.anchoredPosition = new Vector2(0, initialY - (staerksteEinzigartigeNetzwerkNamen.Count - 1 - i) * (buttonHeight + spacing));
            buttonTransform.pivot = new Vector2(0.5f, 1f);

            string buttonText = staerksteEinzigartigeNetzwerkNamen[i].Replace("?", " ");
            buttonObject.GetComponentInChildren<Text>().text = buttonText;

            int signalStrength = int.Parse(staerksteEinzigartigesignalStaerken[i]);

            Image imageElement = buttonObject.transform.Find("WLAN_Strength_IMG").GetComponent<Image>();

            if (signalStrength > 70)
            {
                imageElement.sprite = wlanSignalStrengthsIMGArr[0];
            }
            else if (signalStrength >= 50 && signalStrength < 70)
            {
                imageElement.sprite = wlanSignalStrengthsIMGArr[1];
            }
            else if (signalStrength >= 30 && signalStrength < 50)
            {
                imageElement.sprite = wlanSignalStrengthsIMGArr[2];
            }
            else if (signalStrength <= 30)
            {
                imageElement.sprite = wlanSignalStrengthsIMGArr[3];
            }

            Button buttonComponent = buttonObject.GetComponent<Button>();
            string selectedElement = staerksteEinzigartigeNetzwerkNamen[i];
        }

        netzwerkSuche.SetActive(false);
        ladeAnimation.SetActive(false);
        verfuegbareNetzwerke.SetActive(true);
        scrollView.SetActive(true);
    }
}