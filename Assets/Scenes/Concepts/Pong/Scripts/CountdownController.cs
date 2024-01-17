using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : NetworkBehaviour
{
    [SyncVar/*(hook = nameof(OnCountdownStarted))*/]
    private bool countdownStarted = false;

    [SyncVar(hook = nameof(OnCountdownSeconds))]
    private int countdownSeconds = 10;

    private float timer;

    public Text countDownText;

    private NetworkManagerPong networkManagerPong;

    private void Start()
    {
        networkManagerPong = FindObjectOfType<NetworkManagerPong>();
    }

    private void Update()
    {
        if (!isServer)
        {
            return;
        }

        if (NetworkServer.connections.Count > 0 && !countdownStarted)
        {
            countdownStarted = true;
        }

        if (countdownStarted)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                if (countdownSeconds == 0)
                {
                    // Start the game or do something else
                    //countDownText.text = "Starting game!";
                    countdownStarted = false;
                    return;
                }
                else
                {
                    timer = 0f;
                    countdownSeconds--;
                }

                // Update the countdown on clients
                RpcUpdateCountdown(countdownSeconds);
            }
        }
    }

    [ClientRpc]
    private void RpcUpdateCountdown(int seconds)
    {
        // Check if the client has the networked object that this script is attached to
        if (!hasAuthority)
        {
            return;
        }

        // Update the countdown UI or do something else
        //countDownText.text = "Warten auf mögliche Mitspieler. \r\n\r\nRunde startet in " + seconds + " Sekunden.";
        //countDownText.text = "Countdown: " + seconds + " seconds left";
    }

    //private void OnCountdownStarted(bool oldStarted, bool newStarted)
    //{
    //    countDownText.text = "Countdown started: " + newStarted;
    //}

    private void OnCountdownSeconds(int oldSeconds, int newSeconds)
    {
        if (newSeconds > 0)
        {
            countDownText.text = "Warten auf mögliche Mitspieler. \r\n\r\nRunde startet in " + newSeconds + " Sekunden.";
        }
        else
        {
            countDownText.text = "";

            if (isServer)
            {
                networkManagerPong.BallSpawn();
            }
        }
    }
}
