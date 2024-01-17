using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;

public class Mathe_Kurbeln : MonoBehaviour
{
    public Text matheaufgabe;

    public double zahl1;
    public double zahl2;
    public int mathematischerOperatorWert;

    private bool IstGerade;

    public int richtigGemacht;

    public int falschGemacht;

    public int aktuelleRunde;

    public bool isRunning;

    public Image timerButtonIMG;
    public Text timerButtonTXT;
    public Sprite timerPlaySprite;
    public Sprite timerPauseSprite;

    public Text rundenCounter;

    public GameObject geradeAuswahl;

    public GameObject ungeradeAuswahl;

    private bool kannDrehen;

    public Slider timerSlider;

    public int kurbelWert;

    public bool richtig;

    public Image auswertungRichtigBildOben;

    public Image auswertungFalschBildOben;

    public Image auswertungRichtigBildUnten;

    public Image auswertungFalschBildUnten;

    public Image auswertungFalschBild;

    public Text obenTxt;

    public Text untenTxt;

    public double ergebnis;

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
        GeneriereRechnung();

        richtigGemacht = 0;

        falschGemacht = 0;

        rundenCounterAktualisieren();

        isRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning == true && timerSlider.value <= 1)
        {
            //10 Sekunden bei 0.1f
            timerSlider.value += Time.deltaTime * 0.1f;
        }

        if (timerSlider.value >= 0.994f)
        {
            isRunning = false;

            AuswertungAnzeigen();
        }

        if (kannDrehen == true)
        {
            //if (Input.GetKeyDown(KeyCode.UpArrow))
            //{
            //    geradeAuswahl.active = true;
            //    ungeradeAuswahl.active = false;

            //    kurbelWert = 1;

            //    isRunning = true;

            //    timerButtonIMG.sprite = timerPauseSprite;
            //}
            //else if (Input.GetKeyDown(KeyCode.DownArrow))
            //{
            //    geradeAuswahl.active = false;
            //    ungeradeAuswahl.active = true;

            //    kurbelWert = -1;

            //    isRunning = true;

            //    timerButtonIMG.sprite = timerPauseSprite;
            //}

            try
            {
                switch (Kurbeln_Skript.empfangeneDatenKurbelRichtungStr)
                {

                    case "forward":
                        geradeAuswahl.active = true;
                        ungeradeAuswahl.active = false;

                        kurbelWert = 1;

                        isRunning = true;

                        timerButtonIMG.sprite = timerPauseSprite;

                        timerButtonTXT.text = "Pause";
                        break;
                    case "backward":
                        geradeAuswahl.active = false;
                        ungeradeAuswahl.active = true;

                        kurbelWert = -1;

                        isRunning = true;

                        timerButtonIMG.sprite = timerPauseSprite;

                        timerButtonTXT.text = "Pause";
                        break;
                    case "standing":
                    default:
                        geradeAuswahl.active = false;
                        ungeradeAuswahl.active = false;

                        kurbelWert = 0;
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    public int ZahlenRandomizer()
    {
        int random = UnityEngine.Random.Range(1, 11);

        return random;
    }

    public int OperatorenRandomizer()
    {
        int random = 0;

        switch (SceneSwitcherSpielauswahl.spielLevel)
        {
            case 1:
                if (zahl1 > zahl2)
                {
                    random = UnityEngine.Random.Range(0, 2);
                }
                else
                {
                    random = UnityEngine.Random.Range(1, 2);
                }
                break;
            case 2:
                if (zahl1 > zahl2)
                {
                    random = UnityEngine.Random.Range(0, 3);
                }
                else
                {
                    random = UnityEngine.Random.Range(1, 2);
                }
                break;
            case 3:
                if (zahl1 % zahl2 == 0)
                {
                    //if (zahl1 > zahl2)
                    //{
                    random = 3;
                    //}
                    //else
                    //{
                    //    random = UnityEngine.Random.Range(1, 3);
                    //}
                }
                else
                {
                    if (zahl1 > zahl2)
                    {
                        random = UnityEngine.Random.Range(0, 3);
                    }
                    else
                    {
                        random = UnityEngine.Random.Range(1, 2);
                    }
                }
                break;
        }


        return random;
    }

    public void GeneriereRechnung()
    {
        geradeAuswahl.active = false;
        ungeradeAuswahl.active = false;

        zahl1 = ZahlenRandomizer();

        zahl2 = ZahlenRandomizer();

        if (zahl1 + zahl2 == 0)
        {
            zahl2++;
        }

        mathematischerOperatorWert = OperatorenRandomizer();

        kannDrehen = true;

        Berechnung();
    }

    public void Berechnung()
    {
        ergebnis = 0;

        switch (mathematischerOperatorWert)
        {
            case 0:
                matheaufgabe.text = zahl1.ToString() + "-" + zahl2.ToString() + " = ?";
                ergebnis = zahl1 - zahl2;
                break;
            case 1:
                matheaufgabe.text = zahl1.ToString() + "+" + zahl2.ToString() + " = ?";
                ergebnis = zahl1 + zahl2;
                break;
            case 2:
                matheaufgabe.text = zahl1.ToString() + "x" + zahl2.ToString() + " = ?";
                ergebnis = zahl1 * zahl2;
                break;
            case 3:
                matheaufgabe.text = zahl1.ToString() + "÷" + zahl2.ToString() + " = ?";
                ergebnis = zahl1 / zahl2;
                break;
        }

        if (ergebnis % 2 == 0)
        {
            IstGerade = true;
        }
        else
        {
            IstGerade = false;
        }
    }

    public void KurbelAuswertung()
    {
        kannDrehen = false;

        if (IstGerade == true && kurbelWert == 1)
        {
            richtig = true;
            auswertungRichtigBildOben.enabled = true;
        }
        else if (IstGerade == false && kurbelWert == 1)
        {
            richtig = false;
            auswertungFalschBildOben.enabled = true;
        }
        else if (IstGerade == false && kurbelWert == -1)
        {
            richtig = true;
            auswertungRichtigBildUnten.enabled = true;
        }
        else if (IstGerade == true && kurbelWert == -1)
        {
            richtig = false;
            auswertungFalschBildUnten.enabled = true;
        }
        else if (kurbelWert == 0)
        {
            richtig = false;
            auswertungFalschBild.enabled = true;

            obenTxt.enabled = false;
            untenTxt.enabled = false;
        }

        kurbelWert = 0;
    }

    public void AuswertungAnzeigen()
    {
        timerSlider.value = 0;

        KurbelAuswertung();

        if (richtig == true)
        {
            richtigGemacht++;
        }
        else
        {
            falschGemacht++;
        }

        switch (mathematischerOperatorWert)
        {
            case 0:
                matheaufgabe.text = zahl1.ToString() + "-" + zahl2.ToString() + " = " + ergebnis.ToString();
                break;
            case 1:
                matheaufgabe.text = zahl1.ToString() + "+" + zahl2.ToString() + " = " + ergebnis.ToString();
                break;
            case 2:
                matheaufgabe.text = zahl1.ToString() + "x" + zahl2.ToString() + " = " + ergebnis.ToString();
                break;
            case 3:
                matheaufgabe.text = zahl1.ToString() + "/" + zahl2.ToString() + " = " + ergebnis.ToString();
                break;
        }

        Invoke("rundenCounterAktualisieren", 3);
    }

    public void rundenCounterAktualisieren()
    {
        if (aktuelleRunde == 10)
        {
            Kurbeln_Skript.richtigGemacht = richtigGemacht;
            Kurbeln_Skript.falschGemacht = falschGemacht;

            SceneManager.LoadScene(3);
        }
        else
        {
            aktuelleRunde++;

            auswertungRichtigBildOben.enabled = false;

            auswertungFalschBildOben.enabled = false;

            auswertungRichtigBildUnten.enabled = false;

            auswertungFalschBildUnten.enabled = false;

            auswertungFalschBild.enabled = false;

            obenTxt.enabled = true;
            untenTxt.enabled = true;

            GeneriereRechnung();

            isRunning = true;

            rundenCounter.text = aktuelleRunde.ToString() + " / 10";

            MySQLConnector.InsertSpieleData(aktuelleRunde, richtigGemacht, falschGemacht);
        }
    }
}
