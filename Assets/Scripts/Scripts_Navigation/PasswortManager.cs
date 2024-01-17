using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net;
using System.Linq;
using System.Collections;
using System.Net.NetworkInformation;

public class PasswortManager : MonoBehaviour
{
    public GameObject ladeAnimation;
    public GameObject verbindungsAufbau;
    public GameObject passwordCheckIMG;
    public GameObject messageInputFieldGO;
    public GameObject verbindenButtonGO;
    public GameObject netzwerkNameTXTGO;

    bool isWrongPassword;

    public Text placeHolder;

    public InputField messageInputField;

    public Sprite[] visibility;

    bool pwvisible;

    private TcpClient _client;
    private NetworkStream _stream;
    private Thread _receiveThread;
    private bool _isReceiving;

    private const string ServerIp = "192.168.0.11";
    private const int ServerPort = 8383;

    public Text netzwerkNameTXT;

    bool wlanSet;

    public Button eyeButton;

    private void Start()
    {
        _client = new TcpClient();
        _client.Connect(ServerIp, ServerPort);
        _stream = _client.GetStream();

        _isReceiving = false;

        wlanSet = false;

        pwvisible = true;

        netzwerkNameTXT.text = WlanAuswahl.netzwerkName;

        PasswordVisibility();
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
            messageInputField.ForceLabelUpdate(); // Update the input field's label to show the text
        }
    }

    private void ReceiveMessage()
    {
        byte[] buffer = new byte[1024];
        int bytesRead = _stream.Read(buffer, 0, buffer.Length);
        string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        Debug.Log(receivedMessage);

        SetButtonInteractability(true);

        if (receivedMessage.Contains("Done"))
        {
            wlanSet = true;
            isWrongPassword = false; // The password is correct
        }
        else
        {
            isWrongPassword = true;  // The password is wrong
        }
    }

    private IEnumerator SendPingCoroutine()
    {
        float timeout = 5; // Timeout in seconds
        float timer = 0;

        System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
        System.Net.NetworkInformation.PingReply reply = null;

        try
        {
            reply = ping.Send("google.com");
        }
        catch (Exception ex)
        {
            isWrongPassword = true;
        }

        // Changed the loop condition to check for timeout
        while (timer < timeout && reply == null)
        {
            timer += Time.deltaTime; // Increment timer
            yield return null; // Continue execution in the next frame
        }

        if (reply != null && reply.Status == System.Net.NetworkInformation.IPStatus.Success)
        {
            // Ping was successful
            ladeAnimation.SetActive(false);
            verbindungsAufbau.SetActive(false);
            messageInputFieldGO.SetActive(true);
            passwordCheckIMG.SetActive(true);
            verbindenButtonGO.SetActive(true);
            netzwerkNameTXTGO.SetActive(true);
        }
        else
        {
            // Ping failed or wrong password
            ladeAnimation.SetActive(false);
            verbindungsAufbau.SetActive(false);
            messageInputFieldGO.SetActive(true);
            verbindenButtonGO.SetActive(true);
            netzwerkNameTXTGO.SetActive(true);

            passwordCheckIMG.SetActive(false);
            messageInputField.text = "";

            placeHolder.text = "Falsches Passwort";
            placeHolder.color = Color.red;

            _client = new TcpClient();
            _client.Connect(ServerIp, ServerPort);
            _stream = _client.GetStream();
        }
    }

    private void Update()
    {
        if (wlanSet)
        {
            StartCoroutine(SendPingCoroutine());

            wlanSet = false;
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

    public void SendPasswort()
    {
        string message = messageInputField.text;

        _isReceiving = false;

        wlanSet = false;

        SetButtonInteractability(false);

        passwordCheckIMG.SetActive(false);

        if (!string.IsNullOrEmpty(message))
        {
            string ipAddress = GetLocalIPv4();
            string combinedMessage = ipAddress + ";setWLAN;" + WlanAuswahl.netzwerkName + ";" + messageInputField.text + ";WPA2-PSK;" + "aes";
            byte[] data = Encoding.UTF8.GetBytes(combinedMessage);

            ThreadPool.QueueUserWorkItem(_ =>
            {
                _stream?.Write(data, 0, data.Length);
            });

            ladeAnimation.SetActive(true);
            verbindungsAufbau.SetActive(true);
            messageInputFieldGO.SetActive(false);
            verbindenButtonGO.SetActive(false);
            netzwerkNameTXTGO.SetActive(false);
        }
        else
        {
            placeHolder.text = "Passwort eingeben";
            placeHolder.color = Color.red;
        }
    }

    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName())
            .AddressList.FirstOrDefault(
                f => f.AddressFamily == AddressFamily.InterNetwork)
            ?.ToString();
    }
}