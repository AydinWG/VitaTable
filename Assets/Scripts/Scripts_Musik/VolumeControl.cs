using UnityEngine;
using Mirror;

public class VolumeControl : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnVolumeChange))]
    private float volume;

    public AudioSource audioSource;

    private void Start()
    {
        // Set the initial volume to 0
        audioSource.volume = 0f;
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        switch (Kurbeln_Skript.empfangeneDatenKurbelRichtungStr)
        {
            case "forward":
                volume += 0.01f;
                CmdUpdateVolume(volume);
                break;
            case "backward":
                volume -= 0.01f;
                CmdUpdateVolume(volume);
                break;
            case "standing":
                volume -= 0.0025f;
                CmdUpdateVolume(volume);
                break;
        }
    }

    [Command]
    void CmdUpdateVolume(float newVolume)
    {
        volume = newVolume;
    }

    void OnVolumeChange(float oldValue, float newValue)
    {
        audioSource.volume = newValue;
    }
}
