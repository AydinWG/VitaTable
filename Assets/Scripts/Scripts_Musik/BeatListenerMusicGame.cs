using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatListenerMusicGame : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnRectTransformChanged))]
    public Vector2 sizeDelta = Vector2.zero; 

    public Image image; // Reference to the Image component on the UI element
    public AudioSource audioSource; // Reference to the AudioSource component

    private float minHeight = 500f;
    private float maxHeight = 1000f;
    private float minWidth = 500f;
    private float maxWidth = 1000f;
    public string childObjectName; // The name of the child object whose scale should not be changed

    private float outputVolume = 0; // The output volume of the audio source

    private NetworkManagerMusic networkManagerMusic;

    private VolumeControl volumeControl;

    public void Start()
    {
        networkManagerMusic = FindObjectOfType<NetworkManagerMusic>();

        volumeControl = FindObjectOfType<VolumeControl>();
    }

    public void Update()
    {
        if (networkManagerMusic.startTimer == true)
        {
            if (audioSource != null)
            {
                // Get the output volume of the audio source
                outputVolume = GetOutputVolume();

                // Scale the image based on the output volume
                float newHeight = Mathf.Lerp(minHeight, maxHeight, outputVolume);
                float newWidth = Mathf.Lerp(minWidth, maxWidth, outputVolume);

                // Update the image's rect transform
                RectTransform rectTransform = image.rectTransform;
                rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
                sizeDelta = rectTransform.sizeDelta;
            }
            else
            {
                switch (image.name)
                {
                    case "VocalCircle":
                        audioSource = networkManagerMusic.audioSources[0];
                        break;
                    case "GuitarCircle":
                        audioSource = networkManagerMusic.audioSources[1];
                        break;
                    case "BassCircle":
                        audioSource = networkManagerMusic.audioSources[2];
                        break;
                    case "DrumCircle":
                        audioSource = networkManagerMusic.audioSources[3];
                        break;
                    default:
                        Debug.LogWarning("BeatListenerMusicGame: No AudioSource found for " + image.name);
                        break;
                }
            }
        }
    }

    void OnRectTransformChanged(Vector2 oldSize, Vector2 newSize)
    {
        // Update the sizeDelta of the parent object
        image.rectTransform.sizeDelta = newSize;
    }

    float GetOutputVolume()
    {
        // Get the output volume of the audio source by sampling the audio data
        float[] samples = new float[1024];
        audioSource.GetOutputData(samples, 0);
        float sum = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += Mathf.Abs(samples[i]);
        }
        return sum / samples.Length;
    }
}
