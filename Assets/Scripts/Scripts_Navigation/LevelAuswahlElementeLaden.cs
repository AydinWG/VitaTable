using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelAuswahlElementeLaden : MonoBehaviour
{
    public Text überschrift;

    public Text zweitüberschrift;

    public Text btn1überschrift;
    public Text btn2überschrift;
    public Text btn3überschrift;

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
                überschrift.text = "Obstsalat";
                titelBild.sprite = titelBildSpriteArray[0];
                break;
            case "Autofahren":
                überschrift.text = "Autofahren";
                titelBild.sprite = titelBildSpriteArray[1];
                break;
            case "Kaffeerunde":
                überschrift.text = "Kaffeerunde";
                titelBild.sprite = titelBildSpriteArray[2];
                break;
            case "Pferderennen":
                überschrift.text = "Pferderennen";
                titelBild.sprite = titelBildSpriteArray[3];

                schwer.active = false;
                break;
            case "Brunnen":
                überschrift.text = "Brunnen";
                titelBild.sprite = titelBildSpriteArray[4];
                break;
            case "Wettrechnen":
                überschrift.text = "Wettrechnen";
                titelBild.sprite = titelBildSpriteArray[5];
                break;
            case "Tennis":
                überschrift.text = "Tennis";
                zweitüberschrift.text = "Spielmodus wählen";

                btn1überschrift.text = "Freies Spiel";
                btn2überschrift.text = "Durchhalten";
                btn3überschrift.text = "Bis 10";

                titelBild.sprite = titelBildSpriteArray[6];
                break;
            case "Musik":
                überschrift.text = "Musik";
                zweitüberschrift.text = "Spielmodus wählen";

                btn1überschrift.text = "Lied 1";
                btn2überschrift.text = "Lied 2";
                btn3überschrift.text = "Lied 3";

                titelBild.sprite = titelBildSpriteArray[7];
                break;
            case "Videotraining":
                überschrift.text = "Videotraining";
                btn1überschrift.text = "30 Umdrehungen";
                btn2überschrift.text = "50 Umdrehungen";

                titelBild.sprite = titelBildSpriteArray[8];

                schwer.SetActive(false);
                break;
        }
    }
}
