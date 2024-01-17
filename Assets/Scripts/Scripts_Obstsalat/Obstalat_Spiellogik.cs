using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PTClient;
using System;
using UnityEngine.Events;
using MySql.Data.MySqlClient;

public class Obstalat_Spiellogik : MonoBehaviour
{
    public int bildRandomizer;

    public int bildRandomizerUntenObst;

    public int bildRandomizerUntenGemuese;
    public bool istObst;
    public int kurbelWert;

    public Text überschrift;

    public Text obenTxt;

    public Text untenTxt;

    public Slider timerSlider;

    public Image obstGemueseIMG;

    public Image obstGemueseIMGOben;

    public Image obstGemueseIMGUnten;

    public Sprite[] obstGemueseSpriteArray;

    public GameObject pfeilOben;

    public GameObject pfeilUnten;

    public GameObject obenAuswahl;

    public GameObject untenAuswahl;

    public bool richtig;

    public bool obenObst;
    public bool untenObst;

    public bool isRunning;

    public GameObject timerButtonGO;

    public GameObject timerSliderGO;

    public Image timerButtonIMG;
    public Text timerButtonTXT;
    public Sprite timerPlaySprite;
    public Sprite timerPauseSprite;

    public Text rundenCounter;

    public int aktuelleRunde;

    public int richtigGemacht;

    public int falschGemacht;

    public Image auswertungRichtigBild;

    public Image auswertungFalschBild;

    private bool kannDrehen;

    private float timerMultiplikator;

    public Slider jaAuswahlSlider;

    public Slider neinAuswahlSlider;

    public GameObject jaAuswahlSliderGO;

    public GameObject neinAuswahlSliderGO;

    public bool jaSliderBoolisRunning;

    public bool neinSliderBoolisRunning;

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

            kurbelWert = 0;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        ElementeNachLevelLaden();

        richtigGemacht = 0;

        falschGemacht = 0;

        rundenCounterAktualisieren();

        isRunning = false;

        jaSliderBoolisRunning = false;

        neinSliderBoolisRunning = false;
    }

    private void ElementeNachLevelLaden()
    {
        if (SceneSwitcherSpielauswahl.spielName == "Videotraining")
        {
            überschrift.text = "Ist das Obst?";

            obenTxt.text = "Ja!";
            untenTxt.text = "Nein!";

            obstGemueseIMGOben.enabled = false;

            obstGemueseIMGUnten.enabled = false;

            BildRandomizerLeicht();

            kannDrehen = true;

            timerButtonGO.active = false;

            timerSliderGO.active = false;

            if (aktuelleRunde < 2)
            {
                rundenCounter.transform.position += new Vector3(225, 0, 0);
            }
        }
        else
        {
            switch (SceneSwitcherSpielauswahl.spielLevel)
            {
                case 1:
                    überschrift.text = "Ist das Obst?";

                    obenTxt.text = "Ja!";
                    untenTxt.text = "Nein!";

                    obstGemueseIMGOben.enabled = false;

                    obstGemueseIMGUnten.enabled = false;

                    BildRandomizerLeicht();

                    kannDrehen = true;

                    timerButtonGO.active = false;

                    timerSliderGO.active = false;

                    if (aktuelleRunde < 2)
                    {
                        rundenCounter.transform.position += new Vector3(225, 0, 0);
                    }
                    break;
                case 2:
                    überschrift.text = "Ist das Obst?";

                    obenTxt.text = "Ja!";
                    untenTxt.text = "Nein!";

                    obstGemueseIMGOben.enabled = false;

                    obstGemueseIMGUnten.enabled = false;

                    jaAuswahlSliderGO.active = false;
                    neinAuswahlSliderGO.active = false;

                    BildRandomizerLeicht();

                    kannDrehen = true;

                    timerMultiplikator = 0.1f;
                    break;
                case 3:
                    überschrift.text = "Wo ist Obst?";

                    obenTxt.text = "Oben!";
                    untenTxt.text = "Unten!";

                    obstGemueseIMG.enabled = false;

                    jaAuswahlSliderGO.active = false;
                    neinAuswahlSliderGO.active = false;

                    BildRandomizerMittelSchwer();

                    obstGemueseIMGOben.enabled = true;

                    obstGemueseIMGUnten.enabled = true;

                    kannDrehen = true;

                    timerMultiplikator = 0.1f;
                    break;
            }
        }
    }

    private void BildRandomizerMittelSchwer()
    {
        bildRandomizer = UnityEngine.Random.Range(0, 28);

        bildRandomizerUntenObst = UnityEngine.Random.Range(0, 15);
        bildRandomizerUntenGemuese = UnityEngine.Random.Range(15, 28);

        if (bildRandomizer <= 14)
        {
            obstGemueseIMGOben.sprite = obstGemueseSpriteArray[bildRandomizer];

            obenObst = true;
            untenObst = false;

            obstGemueseIMGUnten.sprite = obstGemueseSpriteArray[bildRandomizerUntenGemuese];
        }

        if (bildRandomizer > 14)
        {
            obstGemueseIMGOben.sprite = obstGemueseSpriteArray[bildRandomizer];

            obenObst = false;
            untenObst = true;

            obstGemueseIMGUnten.sprite = obstGemueseSpriteArray[bildRandomizerUntenObst];
        }

        kannDrehen = true;
    }

    public int BildRandomizerLeicht()
    {
        bildRandomizer = UnityEngine.Random.Range(0, 28);

        if (bildRandomizer <= 14)
        {
            istObst = true;
        }

        if (bildRandomizer > 14)
        {
            istObst = false;
        }

        obstGemueseIMG.sprite = obstGemueseSpriteArray[bildRandomizer];

        return bildRandomizer;
    }

    // Update is called once per frame
    void Update()
    {
        if (kannDrehen == true)
        {
            if (SceneSwitcherSpielauswahl.spielLevel != 1 && SceneSwitcherSpielauswahl.spielName != "Videotraining")
            {
                if (isRunning == true && timerSlider.value <= 1)
                {
                    //10 Sekunden bei 0.1f
                    timerSlider.value += Time.deltaTime * timerMultiplikator;

                    //if (SceneSwitcherSpielauswahl.spielLevel == 3 && timerSlider.value >= 0.5f)
                    //{
                    //    BilderVerstecken();
                    //}
                }

                if (timerSlider.value >= 0.994f)
                {
                    obenAuswahl.active = false;
                    untenAuswahl.active = false;

                    isRunning = false;

                    AuswertungAnzeigen();
                }

                switch (Kurbeln_Skript.empfangeneDatenKurbelRichtungStr)
                {
                    case "forward":
                        obenAuswahl.active = true;
                        untenAuswahl.active = false;

                        kurbelWert = 1;

                        isRunning = true;

                        timerButtonIMG.sprite = timerPauseSprite;

                        timerButtonTXT.text = "Pause";

                        break;
                    case "backward":
                        obenAuswahl.active = false;
                        untenAuswahl.active = true;

                        kurbelWert = -1;

                        isRunning = true;

                        timerButtonIMG.sprite = timerPauseSprite;

                        timerButtonTXT.text = "Pause";
                        break;
                    case "standing":
                    default:
                        obenAuswahl.active = false;
                        untenAuswahl.active = false;

                        kurbelWert = 0;
                        break;
                }
            }
            else
            {
                obenAuswahl.active = false;
                untenAuswahl.active = false;

                if (jaSliderBoolisRunning == true && jaAuswahlSlider.value <= 1)
                {
                    //10 Sekunden bei 0.1f
                    jaAuswahlSlider.value += Time.deltaTime * 0.4f;

                }

                if (neinSliderBoolisRunning == true && neinAuswahlSlider.value <= 1)
                {
                    //10 Sekunden bei 0.1f
                    neinAuswahlSlider.value += Time.deltaTime * 0.4f;

                }

                if (jaAuswahlSlider.value >= 0.994f || neinAuswahlSlider.value >= 0.994f)
                {
                    jaAuswahlSliderGO.active = false;
                    neinAuswahlSliderGO.active = false;

                    jaSliderBoolisRunning = false;
                    neinSliderBoolisRunning = false;

                    AuswertungAnzeigen();
                }

                switch (Kurbeln_Skript.empfangeneDatenKurbelRichtungStr)
                {
                    case "forward":
                        jaAuswahlSliderGO.active = true;
                        neinAuswahlSliderGO.active = false;

                        jaSliderBoolisRunning = true;
                        neinSliderBoolisRunning = false;

                        neinAuswahlSlider.value = 0;

                        kurbelWert = 1;

                        break;
                    case "backward":
                        jaAuswahlSliderGO.active = false;
                        neinAuswahlSliderGO.active = true;

                        neinSliderBoolisRunning = true;
                        jaSliderBoolisRunning = false;

                        jaAuswahlSlider.value = 0;

                        kurbelWert = -1;

                        break;
                    case "standing":
                    default:
                        jaAuswahlSliderGO.active = false;
                        neinAuswahlSliderGO.active = false;

                        neinSliderBoolisRunning = false;
                        jaSliderBoolisRunning = false;

                        jaAuswahlSlider.value = 0;
                        neinAuswahlSlider.value = 0;

                        kurbelWert = 0;
                        break;
                }
            }
        }
    }

    private void BilderVerstecken()
    {
        obstGemueseIMGOben.sprite = obstGemueseSpriteArray[30];
        obstGemueseIMGUnten.sprite = obstGemueseSpriteArray[30];
    }

    public void rundenCounterAktualisieren()
    {
        if (aktuelleRunde == 10 && SceneSwitcherSpielauswahl.isVideoWorkout == false)
        {
            Kurbeln_Skript.richtigGemacht = richtigGemacht;
            Kurbeln_Skript.falschGemacht = falschGemacht;

            SceneManager.LoadScene(3);
        }
        else if (aktuelleRunde == 10 && SceneSwitcherSpielauswahl.isVideoWorkout == true)
        {
            SceneManager.LoadScene(12);
        }
        else
        {
            aktuelleRunde++;

            auswertungRichtigBild.enabled = false;

            auswertungFalschBild.enabled = false;

            obenTxt.enabled = true;
            untenTxt.enabled = true;

            isRunning = true;

            rundenCounter.text = aktuelleRunde.ToString() + " / 10";

            ElementeNachLevelLaden();

            MySQLConnector.InsertSpieleData(aktuelleRunde, richtigGemacht, falschGemacht);
        }
    }

    public void KurbelAuswertung()
    {
        kannDrehen = false;

        switch (SceneSwitcherSpielauswahl.spielLevel)
        {
            case 1:
            case 2:
                if (istObst == true && kurbelWert == 1)
                {
                    richtig = true;
                }
                else if (istObst == false && kurbelWert == -1)
                {
                    richtig = true;
                }
                else
                {
                    richtig = false;
                }
                break;
            case 3:
                if (bildRandomizer <= 14)
                {
                    obstGemueseIMGOben.sprite = obstGemueseSpriteArray[bildRandomizer];

                    obenObst = true;
                    untenObst = false;

                    obstGemueseIMGUnten.sprite = obstGemueseSpriteArray[bildRandomizerUntenGemuese];
                }

                if (bildRandomizer > 14)
                {
                    obstGemueseIMGOben.sprite = obstGemueseSpriteArray[bildRandomizer];

                    obenObst = false;
                    untenObst = true;

                    obstGemueseIMGUnten.sprite = obstGemueseSpriteArray[bildRandomizerUntenObst];
                }

                if (obenObst == true && kurbelWert == 1)
                {
                    richtig = true;
                }
                else if (untenObst == true && kurbelWert == -1)
                {
                    richtig = true;
                }
                else
                {
                    richtig = false;
                }
                break;
        }
    }

    public void AuswertungAnzeigen()
    {
        timerSlider.value = 0;

        jaAuswahlSlider.value = 0;

        neinAuswahlSlider.value = 0;

        KurbelAuswertung();

        kurbelWert = 0;

        obenTxt.enabled = false;
        untenTxt.enabled = false;

        if (richtig == true)
        {
            richtigGemacht++;

            auswertungRichtigBild.enabled = true;

        }
        else
        {
            falschGemacht++;

            auswertungFalschBild.enabled = true;
        }

        Invoke("rundenCounterAktualisieren", 3);
    }
}
