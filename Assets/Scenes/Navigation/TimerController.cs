using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerController : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnSliderValueChanged))] // Synchronize slider value across network and call the hook function when it changes
    private float sliderValue;

    public Slider audioTrackSlider;

    private NetworkManagerMusic networkManagerMusic;

    // Start is called before the first frame update
    void Start()
    {
        networkManagerMusic = FindObjectOfType<NetworkManagerMusic>();
    }

    private void Update()
    {
        if (networkManagerMusic.startTimer == true)
        {
            sliderValue += Time.deltaTime * (1 / networkManagerMusic.audioSources[0].clip.length);
        }

        if (sliderValue >= 1)
        {
            SceneManager.LoadScene(3);
        }
    }

    private void OnSliderValueChanged(float oldValue, float newValue)
    {
        audioTrackSlider.value = newValue;
    }
}
