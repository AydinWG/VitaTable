using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System;

public class Pferde_Movement : MonoBehaviour
{
    public static List<string> pferdeImZiel;

    public GameObject horse;

    public GameObject hindernis;

    //public GameObject hindernis2;

    public bool passedFinishLine;

    public bool canRun;

    public bool stehtVorHindernis1;

    //public bool stehtVorHindernis2;

    public float startWert;

    public float differenz;

    public float positionVorHindernis;

    //public float positionVorHindernis2;

    public Text countdown;

    public float timeRemaining = 3;

    public int countdownToDisplay;

    public GameObject richtungsAnweisung;

    // Start is called before the first frame update
    void Start()
    {
        pferdeImZiel = new List<string>();

        horse.SetActive(true);

        if (SceneSwitcherSpielauswahl.spielLevel == 2)
        {
            hindernis.active = true;
        }

        canRun = false;

        Invoke("LosRennen", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining >= 0)
        {
            timeRemaining -= Time.deltaTime;

            countdownToDisplay = Convert.ToInt32(timeRemaining);

            countdown.text = timeRemaining.ToString("0");
        }

        if (timeRemaining < 0.5f)
        {
            countdown.text = "LOS!";
        }

        if (pferdeImZiel.Count == 4)
        {
            SceneManager.LoadScene(3);
        }

        if (horse.transform.position.x <= -4.25 && SceneSwitcherSpielauswahl.spielLevel == 2 && horse.transform.position.x >= -4.3f)
        //if (horse.transform.position.x <= positionVorHindernis && SceneSwitcherSpielauswahl.spielLevel == 2 && horse.transform.position.x >= positionVorHindernis - 0.5f)
        {
            richtungsAnweisung.active = true;

            canRun = false;
            stehtVorHindernis1 = true;

            if (Kurbeln_Skript.empfangeneDatenKurbelRichtungStr == "backward")
            {
                HindernisRunterKurbeln();
            }
        }
        //else if (horse.transform.position.x <= positionVorHindernis2 && SceneSwitcherSpielauswahl.spielLevel == 2 && horse.transform.position.x >= positionVorHindernis2 - 0.5f)
        //{
        //    canRun = false;
        //    stehtVorHindernis2 = true;

        //    if (Kurbeln_Skript.empfangeneDatenKurbelRichtungStr == "backward")
        //    {
        //        HindernisRunterKurbeln();
        //    }
        //}
        else if (canRun == true)
        {
            PferdBewegen();
        }
    }

    private void HindernisRunterKurbeln()
    {
        if (hindernis.transform.position.y >= 2.98 && stehtVorHindernis1 == true)
        {
            hindernis.transform.position += new Vector3(0, -0.01f, 0);
        }
        else
        {
            canRun = true;
            stehtVorHindernis1 = false;

            horse.transform.position -= new Vector3(0.06f, 0, 0);

            startWert = Kurbeln_Skript.empfangeneDatenKurbelWerteFloat;
        }
    }

    void LosRennen()
    {
        countdown.enabled = false;

        startWert = Kurbeln_Skript.empfangeneDatenKurbelWerteFloat;

        canRun = true;
    }

    void PferdBewegen()
    {
        switch (Kurbeln_Skript.empfangeneDatenKurbelRichtungStr)
        {
            case "forward":
                if (transform.position.x < -14.2)
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

                    canRun = true;

                    differenz = Kurbeln_Skript.empfangeneDatenKurbelWerteFloat - startWert;

                    horse.transform.position -= new Vector3(differenz * 0.000175f, 0, 0);

                    richtungsAnweisung.active = false;
                }
                break;
            case "backward":
                if (stehtVorHindernis1 == false)
                {
                    richtungsAnweisung.active = true;
                }
                break;
            case "standing":
            default:
                //InvokeRepeating("PferdVerlangsamen", 2f, 1);
                PferdVerlangsamen();
                break;
        }
        //if (Kurbeln_Skript.empfangeneDatenKurbelRichtungStr == "forward" && canRun == true)
        ////if (Input.GetKeyDown(KeyCode.UpArrow) && canRun == true)
        //{
        //}
    }

    void PferdVerlangsamen()
    {
        differenz /= 10;
        transform.position -= new Vector3(differenz * 0.0001f, 0, 0);
        startWert = Kurbeln_Skript.empfangeneDatenKurbelWerteFloat;
    }
}