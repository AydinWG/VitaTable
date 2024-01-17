using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class CountdownControllerPferderennspiel : NetworkBehaviour
{
    [SyncVar]
    private bool countdownStarted = false;

    private int countdownSeconds = 10;
    private float timer;

    public Text countDownText;

    public static bool countdownFinished = false;

    public delegate void UpdateCountdownDelegate(int seconds);
    public static event UpdateCountdownDelegate OnUpdateCountdown;
    public static event UpdateCountdownDelegate OnUpdateSecondCountdown;

    // Second countdown variables
    private bool secondCountdownStarted = false;
    private int secondCountdownSeconds = 4; // Adjust as needed
    private float secondTimer;

    public Text secondCountDownText;

    private void Update()
    {
        if (isServer)
        {
            if (NetworkServer.connections.Count > 0 && !countdownStarted)
            {
                countdownStarted = true;
            }

            if (countdownStarted)
            {
                timer += Time.deltaTime;

                if (timer >= 1f)
                {
                    if (countdownSeconds <= 0)
                    {
                        countdownStarted = false;

                        // Start the second countdown
                        secondCountdownStarted = true;

                        RpcUpdateCountdown(0);
                    }
                    else
                    {
                        timer = 0f;
                        countdownSeconds--;
                    }

                    RpcUpdateCountdown(countdownSeconds);
                }
            }

            // Second Countdown Logic
            if (secondCountdownStarted)
            {
                secondTimer += Time.deltaTime;

                if (secondTimer >= 1f)
                {
                    if (secondCountdownSeconds == -1)
                    {
                        secondCountdownStarted = false;
                    }
                    else
                    {
                        secondTimer = 0f;
                        secondCountdownSeconds--;
                    }

                    RpcUpdateSecondCountdown(secondCountdownSeconds);
                }
            }
        }
    }

    [ClientRpc]
    private void RpcUpdateCountdown(int seconds)
    {
        if (seconds > 0)
        {
            countDownText.text = "Warten auf mögliche Mitspieler. \r\n\r\nRunde startet in " + seconds + " Sekunden.";
        }
        else
        {
            countDownText.text = "";

            if (isServer && !countdownFinished)
            {
                FindObjectOfType<NetworkManagerPferderennen>().KIPferdeSpawnen();
                countdownFinished = true;
            }
        }

        OnUpdateCountdown?.Invoke(seconds);
    }

    [ClientRpc]
    private void RpcUpdateSecondCountdown(int seconds)
    {
        // Update the second countdown UI only on clients
        if (isClient)
        {
            if (seconds > 0)
            {
                secondCountDownText.text = seconds.ToString();
            }
            else if (seconds == 0)
            {
                secondCountDownText.text = "LOS!";
            }
            else
            {
                secondCountDownText.text = " ";
            }
        }

        // Invoke the event for second countdown update
        OnUpdateSecondCountdown?.Invoke(seconds);
    }
}
