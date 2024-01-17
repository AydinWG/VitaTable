using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using System;
using System.Linq;

public class Autsospiel_Spiellogik : MonoBehaviour
{
    public RectTransform zeiger;

    public float minSpeedAngle;
    public float maxSpeedAngle;

    public float speed = 0.0f;

    private float letzteSpeed;

    public bool isRunning;

    public Slider timerSlider;

    public Image timerButtonIMG;
    public Text timerButtonTXT;
    public Sprite timerPlaySprite;
    public Sprite timerPauseSprite;

    public Text rundenCounter;

    public int aktuelleRunde;

    public float drehung;

    public int[] geschwindigkeitenArray;

    public Text geschwindigkeitsText;

    public float aktuelleGeschwindigkeit;

    private float zeigerGeschwindigkeit;

    public float vorherigeGeschwindigkeit;

    public float geschwindigkeitsAnzeige;

    public static int richtigGemacht;

    public static int falschGemacht;

    public Image auswertungRichtigBild;

    public Image auswertungFalschBild;

    public static bool faehrtRichtig;

    public int randomElementAusArray;

    public SpriteRenderer[] gruenerBereichArray;

    public bool kannKurbeln;

    public bool darfAbsinken;

    public bool istImZielBereich;

    public int letzteGeschwindigkeit;

    public float startWert;

    public float umdrehung;

    public Text geschwindigkeitsAnzeigeTXT;

    public List<float> geschwindigkeitenListe;

    public float durchschnittsGeschwindigkeit;

    public float timerMultiplikator;

    public int geschwindigkeitAufOberflaeche;

    public int anzahlWertefuerDurchschnitt;

    public Image stopschild;

    public float alpha = 0.1f; // filter parameter

    private float filteredSpeed = 0.0f; // filtered speed value

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

    // Start is called before the first frame update
    void Start()
    {
        anzahlWertefuerDurchschnitt = 4;

        richtigGemacht = 0;

        falschGemacht = 0;

        RundenCounterAktualisieren();

        auswertungRichtigBild.enabled = false;

        auswertungFalschBild.enabled = false;

        isRunning = false;

        zeigerGeschwindigkeit = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (kannKurbeln == true)
        {
            GeschwindigkeitBerechnen();

            //if (isRunning == true)
            //{
            if (zeigerGeschwindigkeit >= -255 && zeigerGeschwindigkeit <= 0)
            {
                zeiger.transform.eulerAngles = new Vector3(0, 0, zeigerGeschwindigkeit);
            }
            //}
        }
        //InvokeRepeating("GeschwindigkeitBerechnen", 0, 0.5f);

        if (isRunning == true && timerSlider.value <= 1)
        {
            //10 Sekunden bei 0.1f
            timerSlider.value += Time.deltaTime * timerMultiplikator;
        }

        if (timerSlider.value >= 0.994f)
        {
            isRunning = false;

            AuswertungAnzeigen();
        }
    }

    public void GeschwindigkeitsDurchschnittErmitteln()
    {
        if (isRunning == true)
        {
            umdrehung = (Kurbeln_Skript.empfangeneDatenKurbelWerteFloat - startWert) / 20;

            speed = ((umdrehung / 0.5f)) * -255;

            // apply low-pass filter
            filteredSpeed = alpha * speed + (1 - alpha) * filteredSpeed;

            // update speed display
            if (filteredSpeed >= 0)
            {
                geschwindigkeitsAnzeigeTXT.text = Math.Round(filteredSpeed).ToString();
            }

            if (geschwindigkeitenListe.Count <= anzahlWertefuerDurchschnitt)
            {
                geschwindigkeitenListe.Add(filteredSpeed);

            }
            else
            {
                geschwindigkeitenListe.Insert(0, filteredSpeed);
                geschwindigkeitenListe.RemoveAt(anzahlWertefuerDurchschnitt + 1);
            }

            var summe = geschwindigkeitenListe.Sum();

            durchschnittsGeschwindigkeit = summe / geschwindigkeitenListe.Count();
        }
    }

    public void GeschwindigkeitBerechnen()
    {
        if (Kurbeln_Skript.empfangeneDatenKurbelWerteFloat != startWert)
        {
            isRunning = true;

            timerButtonIMG.sprite = timerPauseSprite;

            timerButtonTXT.text = "Pause";

            GeschwindigkeitsDurchschnittErmitteln();

            CancelInvoke();
        }
        else
        {
            InvokeRepeating("GeschwindigkeitsDurchschnittErmitteln", 2f, 1f);
        }

        if (istImZielBereich == true && randomElementAusArray != 0)
        {
            zeigerGeschwindigkeit = Mathf.Lerp(zeigerGeschwindigkeit, durchschnittsGeschwindigkeit, 0.0025f);
        }
        else
        {
            //if (zeigerGeschwindigkeit > -180)
            //{
            zeigerGeschwindigkeit = Mathf.Lerp(zeigerGeschwindigkeit, durchschnittsGeschwindigkeit, 0.01f);
            //}
            //else
            //{
            //zeigerGeschwindigkeit = Mathf.Lerp(zeigerGeschwindigkeit, durchschnittsGeschwindigkeit, 0.025f);
            //}
        }

        geschwindigkeitAufOberflaeche = Convert.ToInt32(zeigerGeschwindigkeit / -1.8214f);

        if (geschwindigkeitAufOberflaeche >= 0 && geschwindigkeitAufOberflaeche <= 140)
        {
            geschwindigkeitsAnzeigeTXT.text = geschwindigkeitAufOberflaeche.ToString();
        }

        if (geschwindigkeitAufOberflaeche <= geschwindigkeitenArray[randomElementAusArray] + 5 && geschwindigkeitAufOberflaeche >= geschwindigkeitenArray[randomElementAusArray] - 5)
        {
            istImZielBereich = true;
        }
        else
        {
            istImZielBereich = false;
        }

        startWert = Kurbeln_Skript.empfangeneDatenKurbelWerteFloat;
    }

    public void RundenCounterAktualisieren()
    {
        startWert = Kurbeln_Skript.empfangeneDatenKurbelWerteFloat;

        kannKurbeln = true;

        darfAbsinken = true;

        if (aktuelleRunde == 10)
        {
            Kurbeln_Skript.richtigGemacht = richtigGemacht;
            Kurbeln_Skript.falschGemacht = falschGemacht;

            SceneManager.LoadScene(3);
        }
        else
        {
            GeschwindigkeitAktualisieren();

            aktuelleRunde++;

            auswertungRichtigBild.enabled = false;

            auswertungFalschBild.enabled = false;
            isRunning = true;

            rundenCounter.text = aktuelleRunde.ToString() + " / 10";

            MySQLConnector.InsertSpieleData(aktuelleRunde, richtigGemacht, falschGemacht);
        }
    }

    public void GeschwindigkeitGenrieren()
    {
        //geschwindigkeitenArray = new int[] { 20, 40, 60, 80, 100, 120, 140, 0 };

        geschwindigkeitenArray = new int[] { 0, 20, 40, 60, 80 };

        randomElementAusArray = UnityEngine.Random.Range(0, 5);
    }

    public void GeschwindigkeitAktualisieren()
    {
        geschwindigkeitenListe = new List<float>();

        GeschwindigkeitGenrieren();

        if (aktuelleRunde != 1)
        {
            gruenenBereichAnzeigen(randomElementAusArray);

            geschwindigkeitsText.text = geschwindigkeitenArray[randomElementAusArray].ToString();

            timerMultiplikator = 0.1f;

            if (randomElementAusArray < 1)
            {
                stopschild.enabled = true;
            }
            else if (randomElementAusArray >= 1)
            {
                stopschild.enabled = false;
            }

            letzteGeschwindigkeit = randomElementAusArray;
        }

        else
        {
            if (letzteGeschwindigkeit != randomElementAusArray)
            {
                gruenenBereichAnzeigen(randomElementAusArray);

                geschwindigkeitsText.text = geschwindigkeitenArray[randomElementAusArray].ToString();

                timerMultiplikator = 0.1f;

                if (randomElementAusArray < 1)
                {
                    stopschild.enabled = true;
                }
                else if (randomElementAusArray >= 1)
                {
                    stopschild.enabled = false;
                }

                letzteGeschwindigkeit = randomElementAusArray;
            }
            else
            {
                GeschwindigkeitGenrieren();
            }
        }
    }

    public void gruenenBereichAnzeigen(int arrayElement)
    {
        foreach (var item in gruenerBereichArray)
        {
            item.enabled = false;
        }

        gruenerBereichArray[arrayElement].enabled = true;
    }

    public void KurbelAuswertung()
    {
        int rotation = Convert.ToInt32(geschwindigkeitsAnzeigeTXT.text);

        if (rotation <= geschwindigkeitenArray[randomElementAusArray] + 5 && rotation >= geschwindigkeitenArray[randomElementAusArray] - 5)
        {
            faehrtRichtig = true;
        }
        else
        {
            faehrtRichtig = false;
        }
    }

    public void AuswertungAnzeigen()
    {
        darfAbsinken = false;

        timerSlider.value = 0;

        KurbelAuswertung();

        kannKurbeln = false;

        stopschild.enabled = false;

        if (faehrtRichtig == true)
        {
            richtigGemacht++;

            auswertungRichtigBild.enabled = true;
        }
        else
        {
            falschGemacht++;

            auswertungFalschBild.enabled = true;
        }

        Invoke("RundenCounterAktualisieren", 3);
    }
}
