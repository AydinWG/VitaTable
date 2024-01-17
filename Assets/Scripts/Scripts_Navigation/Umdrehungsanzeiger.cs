using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Umdrehungsanzeiger : MonoBehaviour
{
    Transform zielPanel;

    private float previousRotationValue = 0;

    private float rotations = 0f;

    private float timer = 0f;

    float deltaRotation = 0f;

    public Text umdrehungProSekundeTXT;

    public Text zielUmdrehungTXT;

    private int zielBereich;

    private void Start()
    {
        if (zielPanel == null)
        {
            zielPanel = FindZielPanel();
        }

        if (SceneSwitcherSpielauswahl.spielLevel == 1)
        {
            zielUmdrehungTXT.text = "Ziel: 30";
            zielBereich = 30;
        }
        else if (SceneSwitcherSpielauswahl.spielLevel == 2)
        {
            zielUmdrehungTXT.text = "Ziel: 50";
            zielBereich = 50;
        }
    }

    private void Update()
    {
        if (timer >= 1)
        {
            deltaRotation = 0f;

            SensorRotationsPerSecond(Kurbeln_Skript.empfangeneDatenKurbelWerteFloat);

            //Debug.Log(Kurbeln_Skript.empfangeneDatenKurbelWerteFloat);

            timer = 0f;

            //Text umdrehungProSekunde = zielPanel.Find("UmdrehungsanzeigeTXT").GetComponent<Text>();

            //if (umdrehungProSekunde != null)
            //{
                umdrehungProSekundeTXT.text = rotations.ToString();
            //}

            if (rotations >= (zielBereich - 5) && rotations <= (zielBereich + 5))
            {
                umdrehungProSekundeTXT.color = Color.green;
            }
            else
            {
                umdrehungProSekundeTXT.color = Color.black;
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    // Define a method to find the home_button in the scene.
    private Transform FindZielPanel()
    {
        GameObject homeButton = GameObject.Find("Panel");
        if (homeButton != null)
        {
            return homeButton.transform;
        }
        else
        {
            return null;
        }
    }

    //public void SensorRotationsPerSecond(float sensorRohdaten)
    //{
    //    // Calculate the change in rotationValue since the last update
    //    if (sensorRohdaten > previousRotationValue)
    //    {
    //        deltaRotation = sensorRohdaten - previousRotationValue;
    //    }
    //    else if (sensorRohdaten < previousRotationValue)
    //    {
    //        deltaRotation = previousRotationValue - sensorRohdaten;
    //    }

    //    rotations = (deltaRotation / 20f) * 60f;

    //    previousRotationValue = sensorRohdaten;

    //    //Debug.Log("RPS: " + rotations);
    //}

    public void SensorRotationsPerSecond(float sensorRohdaten)
    {
        // Calculate the change in rotationValue since the last update
        if (sensorRohdaten > previousRotationValue)
        {
            deltaRotation = sensorRohdaten - previousRotationValue;
        }
        else if (sensorRohdaten < previousRotationValue)
        {
            deltaRotation = previousRotationValue - sensorRohdaten;
        }

        // Hier erfolgt die Berechnung der Umdrehungen
        float[] ringPuffer = new float[5]; // Ein Ringpuffer für die letzten 5 Werte
        ringPuffer[0] = deltaRotation;

        for (int i = 1; i < 5; i++)
        {
            // Fülle den Ringpuffer mit den letzten 5 Werten
            ringPuffer[i] = ringPuffer[i - 1];
        }

        // Berechne den gewichteten Mittelwert
        float sum = (ringPuffer[4] * 6) + ringPuffer[3] + ringPuffer[2] + ringPuffer[1] + ringPuffer[0];
        rotations = sum / 10;

        previousRotationValue = sensorRohdaten;

        //Debug.Log("RPS: " + rotations);
    }

}
