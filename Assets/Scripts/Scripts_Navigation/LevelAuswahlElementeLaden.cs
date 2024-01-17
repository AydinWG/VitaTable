using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelAuswahlElementeLaden : MonoBehaviour
{
    public Text �berschrift;

    public Text zweit�berschrift;

    public Text btn1�berschrift;
    public Text btn2�berschrift;
    public Text btn3�berschrift;

    public Image titelBild;

    public Sprite[] titelBildSpriteArray;

    public GameObject mittel;

    public GameObject schwer;

    // Start is called before the first frame update
    void Start()
    {
        switch (SceneSwitcherSpielauswahl.spielName)
        {
            case "Obstsalat":
                �berschrift.text = "Obstsalat";
                titelBild.sprite = titelBildSpriteArray[0];
                break;
            case "Autofahren":
                �berschrift.text = "Autofahren";
                titelBild.sprite = titelBildSpriteArray[1];
                break;
            case "Kaffeerunde":
                �berschrift.text = "Kaffeerunde";
                titelBild.sprite = titelBildSpriteArray[2];
                break;
            case "Pferderennen":
                �berschrift.text = "Pferderennen";
                titelBild.sprite = titelBildSpriteArray[3];

                schwer.active = false;
                break;
            case "Brunnen":
                �berschrift.text = "Brunnen";
                titelBild.sprite = titelBildSpriteArray[4];
                break;
            case "Wettrechnen":
                �berschrift.text = "Wettrechnen";
                titelBild.sprite = titelBildSpriteArray[5];
                break;
            case "Tennis":
                �berschrift.text = "Tennis";
                zweit�berschrift.text = "Spielmodus w�hlen";

                btn1�berschrift.text = "Freies Spiel";
                btn2�berschrift.text = "Durchhalten";
                btn3�berschrift.text = "Bis 10";

                titelBild.sprite = titelBildSpriteArray[6];
                break;
            case "Musik":
                �berschrift.text = "Musik";
                zweit�berschrift.text = "Spielmodus w�hlen";

                btn1�berschrift.text = "Lied 1";
                btn2�berschrift.text = "Lied 2";
                btn3�berschrift.text = "Lied 3";

                titelBild.sprite = titelBildSpriteArray[7];
                break;
            case "Videotraining":
                �berschrift.text = "Videotraining";
                btn1�berschrift.text = "30 Umdrehungen";
                btn2�berschrift.text = "50 Umdrehungen";

                titelBild.sprite = titelBildSpriteArray[8];

                schwer.SetActive(false);
                break;
        }
    }
}
