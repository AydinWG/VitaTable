using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public SceneSwitcherSpielauswahl sceneSwitcherSpielauswahl;

    public VideoPlayer video;

    public VideoClip[] trainingsVideos;

    void Awake()
    {
        video = GetComponent<VideoPlayer>();

        if (SceneSwitcherSpielauswahl.isPreWorkout == true)
        {
            video.clip = trainingsVideos[0];
        }
        else
        {
            video.clip = trainingsVideos[1];
        }

        video.Play();
        video.loopPointReached += CheckOver;
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        //SceneSwitcherSpielauswahl.isPreWorkout = false;

        sceneSwitcherSpielauswahl.VideoTutorialPreWorkoutZuEnde();

        if(SceneSwitcherSpielauswahl.isPreWorkout == false)
        {
            SceneSwitcherSpielauswahl.isPreWorkout = true;
            sceneSwitcherSpielauswahl.BackToHome();
        }
        else
        {
            SceneSwitcherSpielauswahl.isPreWorkout = false;
            sceneSwitcherSpielauswahl.VideoTutorialPreWorkoutZuEnde();
        }
    }
}
