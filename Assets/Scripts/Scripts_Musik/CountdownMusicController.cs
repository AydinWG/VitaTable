using Mirror;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class CountdownMusicController : NetworkBehaviour
{
    [SyncVar/*(hook = nameof(OnCountdownStarted))*/]
    private bool countdownStarted = false;

    [SyncVar(hook = nameof(OnCountdownSeconds))]
    private int countdownSeconds = 10;

    private float timer;

    public Text countDownText;

    private NetworkManagerMusic networkManagerMusic;

    private void Start()
    {
        networkManagerMusic = FindObjectOfType<NetworkManagerMusic>();

        countdownStarted = true;
    }

    private void Update()
    {
        if (!isServer)
        {
            return;
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
    }

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
                networkManagerMusic.StartAudioTracks();
            }
        }
    }
}
