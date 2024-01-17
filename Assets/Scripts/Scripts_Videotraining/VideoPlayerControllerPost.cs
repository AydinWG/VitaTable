using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerControllerPost : MonoBehaviour
{
    public SceneSwitcherSpielauswahl sceneSwitcherSpielauswahl;

    public VideoPlayer video;

    void Awake()
    {
        video = GetComponent<VideoPlayer>();
        video.Play();
        video.loopPointReached += CheckOver;
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        sceneSwitcherSpielauswahl.BackToHome();
    }
}
