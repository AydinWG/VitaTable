using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class OberflaechenManagement : MonoBehaviour
{
    public GameObject[] buttonSet1;
    public GameObject[] buttonSet2;

    public GameObject userPanel;
    public GameObject moodPanel;
    public GameObject serienNummerPanel;

    public static int mood;

    public static string vorname;

    public static int alter;

    public static string geschlecht;

    public Text vornameTXT;
    public GameObject vornameTXTGO;

    public Text alterTXT;

    public GameObject alterTXTGO;

    public Text seriennummerTXT;

    public GameObject seriennummerTXTGO;

    public TMPro.TMP_Dropdown geschlechtDD;
    public GameObject geschlechtDDGO;

    public GameObject saveBTN;

    public GameObject weiterBTN;

    public GameObject weiterBTN2;

    bool weiterGedrueckt;

    bool personenAuswahl;

    public GameObject userAuswahlBTN;

    bool personAusgewehlt;

    //private void Awake()
    //{
    //    MySQLConnector.SysInfEntryCheck();
    //}

    // Start is called before the first frame update
    void Start()
    {
        ScrollToTheLeft();


        if (MySQLConnector.datenbankzugriffGewaehrt && MySQLConnector.personID == null)
        {
            userPanel.SetActive(true);

            moodPanel.SetActive(false);

            userAuswahlBTN.SetActive(true);
        }
        else if (MySQLConnector.personID != null)
        {
            userPanel.SetActive(false);
            moodPanel.SetActive(false);
        }
        else
        {
            userPanel.SetActive(false);
            moodPanel.SetActive(false);
        }

        if (!MySQLConnector.datenbankzugriffGewaehrt)
        {
            userAuswahlBTN.SetActive(false);
        }
    }

    public void ZurueckZurPersonenAuswahl()
    {
        personenAuswahl = true;

        MySQLConnector.personID = null;
    }


    public void ScrollToTheRight()
    {
        foreach (GameObject button in buttonSet1)
        {
            button.SetActive(false);
        }

        foreach (GameObject button in buttonSet2)
        {
            button.SetActive(true);
        }
    }

    public void ScrollToTheLeft()
    {
        foreach (GameObject button in buttonSet2)
        {
            button.SetActive(false);
        }

        foreach (GameObject button in buttonSet1)
        {
            button.SetActive(true);
        }
    }

    public void Weiter()
    {
        if (vornameTXT != null)
        {
            vorname = vornameTXT.GetComponent<Text>().text;
            MySQLConnector.CheckIfPersonExists(vorname);

            weiterGedrueckt = true;
        }
        else
        {
            Debug.LogError("vornameTXT is null");
        }
    }


    private void Update()
    {
        if (!MySQLConnector.personExists && weiterGedrueckt)
        {
            alterTXTGO.SetActive(true);

            geschlechtDDGO.SetActive(true);

            weiterBTN.SetActive(false);

            saveBTN.SetActive(true);

            weiterGedrueckt = false;
        }
        else if (MySQLConnector.personExists && weiterGedrueckt)
        {
            userPanel.SetActive(false);
            moodPanel.SetActive(true);

            MySQLConnector.personID = vorname;

            weiterGedrueckt = false;
        }

        if (MySQLConnector.datenbankzugriffGewaehrt && MySQLConnector.personID == null && personenAuswahl)
        {
            userPanel.SetActive(true);

            moodPanel.SetActive(false);

            personenAuswahl = false;
        }

        if (personenAuswahl == true)
        {
            vornameTXT.text = "";

            geschlechtDD.value = 0;

            alterTXT.text = "";

            personenAuswahl = false;
        }
    }

    public void UserSpeichern()
    {
        // Get the values from the text elements and dropdown
        int.TryParse(alterTXT.text, out alter);
        geschlecht = geschlechtDD.options[geschlechtDD.value].text;

        MySQLConnector.PersonAnlegen();

        alterTXTGO.SetActive(false);

        geschlechtDDGO.SetActive(false);

        weiterBTN.SetActive(true);

        saveBTN.SetActive(false);

        userPanel.SetActive(false);
        moodPanel.SetActive(true);
    }



    public void Happy()
    {
        mood = 2;
        moodPanel.SetActive(false);
    }

    public void Meh()
    {
        mood = 1;
        moodPanel.SetActive(false);
    }

    public void Sad()
    {
        mood = 0;
        moodPanel.SetActive(false);
    }

    public void Seriennummerspeichern()
    {

    }
}
