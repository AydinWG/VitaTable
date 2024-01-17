using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pferde_KI : MonoBehaviour
{
    //private bool dirUp = true;

    public float horseSpeed;

    public GameObject horse;

    public bool passedFinishLine;

    public bool canRun;

    public float positionVorHindernis;

    //public float positionVorHindernis2;

    public bool stehtVorHindernis;

    public GameObject hindernis;

    //public GameObject hindernis2;

    // Start is called before the first frame update
    void Start()
    {
        canRun = false;

        if (SceneSwitcherSpielauswahl.spielLevel == 2)
        {
            hindernis.active = true;
        }

        Invoke("LosRennen", 4f);
    }

    public void LosRennen()
    {
        canRun = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Pferde_Movement.pferdeImZiel.Count == 4)
        //{
        //    SceneManager.LoadScene(3);
        //}

        if (horse.transform.position.x <= positionVorHindernis && SceneSwitcherSpielauswahl.spielLevel == 2)
        {
            canRun = false;
            stehtVorHindernis = true;

            HindernisRunterKurbeln();
        }
        else if (canRun == true)
        {
            HorseMovement();
        }
    }

    void HorseMovement()
    {
        if (horse.transform.position.x < -14.2)
        {
            passedFinishLine = true;

            canRun = false;

            if (!Pferde_Movement.pferdeImZiel.Contains(horse.name))
            {
                Pferde_Movement.pferdeImZiel.Add(horse.name);
            }
        }
        else
        {
            passedFinishLine = false;

            transform.position += new Vector3(-horseSpeed, 0, 0);
        }
    }

    private void HindernisRunterKurbeln()
    {
        if (hindernis.transform.position.y >= 2.98)
        {
            hindernis.transform.position += new Vector3(0, -0.001f, 0);
        }
        else
        {
            canRun = true;
            stehtVorHindernis = false;

            HorseMovement();
        }
    }

    //void Galopp()
    //{
    //    if (dirUp == true)
    //    {
    //        galopp = 0.001f;

    //        dirUp = false;
    //    }
    //    else
    //    {
    //        galopp = -0.001f;

    //        dirUp = true;
    //    }
    //}
}
