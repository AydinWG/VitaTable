using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using PTClient;
using System.Text;
using System;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;

public class CakeModelSwitch : MonoBehaviour
{
    public GameObject[] schokoKuchen1;
    public GameObject[] schokoKuchen2;

    public GameObject erdbeerKuchen;
    public GameObject zitronenKuchen;

    public GameObject[] regelArray;

    public int kuchenArt;

    public GameObject falscheRichtung;

    public TextMeshProUGUI kuchenCounter;

    public bool nachvorneBool;

    private int modelNumber;

    public int random;

    public int kuchenGegessen;

    public Animator falscheRichtungAnimator;

    public int drehCounter;

    public bool isRunning;

    public Image timerButtonIMG;
    public Text timerButtonTXT;
    public Sprite timerPlaySprite;
    public Sprite timerPauseSprite;

    public Slider timerSlider;

    private int kuchenGegessenvorher;

    private int kuchenGegessenDifferenz;

    public void timerButtonPressed()
    {
        if (timerButtonIMG.sprite == timerPlaySprite)
        {
            isRunning = true;

            timerButtonIMG.sprite = timerPauseSprite;

            timerButtonTXT.text = "Pause";
        }
        else
        {
            isRunning = false;

            timerButtonIMG.sprite = timerPlaySprite;

            timerButtonTXT.text = "Start";
        }
    }

    void Start()
    {
        kuchenCounter.text = kuchenGegessen.ToString();

        ElementeNachLevelLaden();

        modelNumber = 0;
    }

    private void ElementeNachLevelLaden()
    {
        Regelnanzeigen();

        switch (SceneSwitcherSpielauswahl.spielLevel)
        {
            case 1:
                KuchenGeradeUngerade();
                break;
            case 2:
                ErdbeerOderSchokokuchen();
                break;
            case 3:
                ErdbeerOderZitronenOderSchokokuchen();
                break;
        }

        MySQLConnector.InsertSpieleData(0, kuchenGegessen, 0);

        //KuchenGeradeUngerade();
    }

    private void Regelnanzeigen()
    {
        if (kuchenGegessen <= 2)
        {
            regelArray[SceneSwitcherSpielauswahl.spielLevel - 1].active = true;
        }
        else
        {
            regelArray[SceneSwitcherSpielauswahl.spielLevel - 1].active = false;
        }
    }

    private void RegelnanzeigenFuerDaus()
    {
        kuchenGegessenDifferenz = kuchenGegessen - kuchenGegessenvorher;

        if (kuchenGegessenDifferenz <= 2)
        {
            regelArray[SceneSwitcherSpielauswahl.spielLevel - 1].active = true;
        }
        else
        {
            regelArray[SceneSwitcherSpielauswahl.spielLevel - 1].active = false;
        }
    }

    private void KuchenGeradeUngerade()
    {
        kuchenArt = 1;

        modelNumber = 0;

        int random = UnityEngine.Random.Range(0, 2);

        if (random == 0)
        {
            nachvorneBool = false;

            schokoKuchen1[modelNumber].SetActive(true);

            schokoKuchen2[modelNumber].SetActive(false);
        }
        else
        {
            nachvorneBool = true;

            schokoKuchen1[modelNumber].SetActive(true);

            schokoKuchen2[modelNumber].SetActive(true);
        }
    }

    private void ErdbeerOderZitronenOderSchokokuchen()
    {
        modelNumber = 0;

        int random = UnityEngine.Random.Range(0, 3);

        switch (random)
        {
            case 0:
                kuchenArt = 1;

                schokoKuchen1[0].SetActive(true);
                zitronenKuchen.SetActive(false);
                erdbeerKuchen.SetActive(false);

                nachvorneBool = false;
                break;
            case 1:
                kuchenArt = 3;

                schokoKuchen1[0].SetActive(false);
                zitronenKuchen.SetActive(true);
                erdbeerKuchen.SetActive(false);

                nachvorneBool = true;

                break;
            case 2:
                kuchenArt = 2;

                schokoKuchen1[0].SetActive(false);
                zitronenKuchen.SetActive(false);
                erdbeerKuchen.SetActive(true);

                nachvorneBool = true;

                break;
            default:
                break;
        }
    }

    private void ErdbeerOderSchokokuchen()
    {
        modelNumber = 0;

        int random = UnityEngine.Random.Range(0, 2);

        if (random == 0)
        {
            kuchenArt = 1;

            nachvorneBool = false;

            schokoKuchen1[0].SetActive(true);

            erdbeerKuchen.SetActive(false);
        }
        else if (random == 1)
        {
            kuchenArt = 2;

            nachvorneBool = true;

            erdbeerKuchen.SetActive(true);

            schokoKuchen1[0].SetActive(false);
        }
    }

    public void Update()
    {
        if (isRunning == true && timerSlider.value <= 1)
        {
            //10 Sekunden bei 0.1f
            timerSlider.value += Time.deltaTime * 0.025f;
        }

        if (timerSlider.value >= 0.994f)
        {
            isRunning = false;

            Kurbeln_Skript.richtigGemacht = kuchenGegessen;

            SceneManager.LoadScene(3);
        }


        if (Kurbeln_Skript.empfangeneDatenKurbelRichtungStr == "forward")
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isRunning = true;

            timerButtonIMG.sprite = timerPauseSprite;

            timerButtonTXT.text = "Pause";

            if (nachvorneBool == true)
            {
                drehCounter++;
                CancelInvoke();

                if (drehCounter == 20)
                //if (drehCounter == 3)
                {
                    KuchenEssen();

                    drehCounter = 0;
                }
            }
            else
            {
                kuchenGegessenvorher = kuchenGegessen;

                InvokeRepeating("RegelnanzeigenFuerDaus", 2, 1);
            }
        }

        if (Kurbeln_Skript.empfangeneDatenKurbelRichtungStr == "backward")
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isRunning = true;

            timerButtonIMG.sprite = timerPauseSprite;

            timerButtonTXT.text = "Pause";

            if (nachvorneBool == false)
            {
                drehCounter++;

                CancelInvoke();

                if (drehCounter == 20)
                //if (drehCounter == 3)
                {
                    KuchenEssen();

                    drehCounter = 0;
                }
            }
            else
            {
                kuchenGegessenvorher = kuchenGegessen;

                InvokeRepeating("RegelnanzeigenFuerDaus", 2, 1);
            }
        }
    }

    public void KuchenEssen()
    {
        switch (kuchenArt)
        {
            case 1:
                SchokoKuchenModelSwitch();
                break;
            case 2:
                ErdbeerKuchenEssen();
                break;
            case 3:
                ZitronenKuchenEssen();
                break;
        }
    }

    public void ZitronenKuchenEssen()
    {
        if (modelNumber <= 2)
        {
            modelNumber++;
            zitronenKuchen.gameObject.transform.localScale -= new Vector3(0.175f, 0, 0.175f);
        }
        else
        {
            drehCounter = 0;

            modelNumber = 0;

            zitronenKuchen.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

            ElementeNachLevelLaden();

            kuchenGegessen++;

            kuchenCounter.text = kuchenGegessen.ToString();
        }
    }

    public void ErdbeerKuchenEssen()
    {
        if (modelNumber <= 2)
        {
            modelNumber++;
            erdbeerKuchen.gameObject.transform.localScale -= new Vector3(0.0375f, 0, 0.0375f);
        }
        else
        {
            drehCounter = 0;

            modelNumber = 0;

            erdbeerKuchen.gameObject.transform.localScale = new Vector3(0.15f, 0.1f, 0.15f);

            ElementeNachLevelLaden();

            kuchenGegessen++;

            kuchenCounter.text = kuchenGegessen.ToString();
        }
    }

    public void SchokoKuchenModelSwitch()
    {
        schokoKuchen1[modelNumber].SetActive(false);

        if (nachvorneBool == true)
        {
            schokoKuchen2[modelNumber].SetActive(false);
        }

        if (modelNumber <= 2)
        {
            modelNumber++;

            schokoKuchen1[modelNumber].SetActive(true);

            if (nachvorneBool == true && SceneSwitcherSpielauswahl.spielLevel == 1)
            {
                schokoKuchen2[modelNumber].SetActive(true);
            }
        }
        else
        {
            drehCounter = 0;

            modelNumber = 0;

            schokoKuchen1[modelNumber].SetActive(true);

            if (nachvorneBool == false)
            {
                schokoKuchen2[modelNumber].SetActive(false);
            }

            ElementeNachLevelLaden();

            kuchenGegessen++;

            kuchenCounter.text = kuchenGegessen.ToString();
        }
    }

    public void FalscheRichtungAnzeigen()
    {
        drehCounter = 0;

        falscheRichtung.GetComponent<Renderer>().enabled = true;

        falscheRichtungAnimator.enabled = true;

        falscheRichtungAnimator.SetBool("falschGedreht", true);
    }
}