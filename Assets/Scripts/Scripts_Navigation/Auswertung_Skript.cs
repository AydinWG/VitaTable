using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Auswertung_Skript : MonoBehaviour
{
    public Text richtigeTXT;
    public Text falscheTXT;
    public Text auswertungTXT;

    public Image falscheIMG;
    public Image richtigeIMG;

    public GameObject stern1IMG;
    public GameObject stern2IMG;
    public GameObject stern3IMG;

    public GameObject stern4IMG;
    public GameObject stern5IMG;
    public GameObject stern6IMG;

    public int platzierungsIndex;

    public GameObject pokalSprite;

    public SpriteRenderer pokalSpriteRenderer;

    public Sprite[] pokalSpriteArray;

    public Text platzierungTXT;

    public GameObject levelBtn;

    public Slider againSlider;

    private PlatzierungsManager platzierungsManager;

    // Start is called before the first frame update
    void Start()
    {
        pokalSpriteRenderer = pokalSprite.GetComponent<SpriteRenderer>();

        if (SceneSwitcherSpielauswahl.spielName == "Obstsalat" || SceneSwitcherSpielauswahl.spielName == "Wettrechnen")
        {
            platzierungTXT.enabled = false;

            levelBtn.active = true;

            richtigeTXT.text = Kurbeln_Skript.richtigGemacht.ToString() + "x";

            falscheTXT.text = Kurbeln_Skript.falschGemacht.ToString() + "x";

            SternAuswertung();
        }

        if (SceneSwitcherSpielauswahl.spielName == "Autofahren")
        {
            platzierungTXT.enabled = false;

            richtigeTXT.text = Kurbeln_Skript.richtigGemacht.ToString() + "x";

            falscheTXT.text = Kurbeln_Skript.falschGemacht.ToString() + "x";

            SternAuswertung();
        }

        if (SceneSwitcherSpielauswahl.spielName == "Kaffeerunde")
        {
            platzierungTXT.enabled = false;

            levelBtn.active = true;

            falscheIMG.enabled = false;
            falscheTXT.enabled = false;

            richtigeTXT.text = Kurbeln_Skript.richtigGemacht.ToString() + "x";

            SternAuswertungKaffeRunde();
        }

        if (SceneSwitcherSpielauswahl.spielName == "Brunnen")
        {
            platzierungTXT.enabled = false;

            falscheIMG.enabled = false;
            richtigeIMG.enabled = false;
            richtigeTXT.enabled = false;
            falscheTXT.enabled = false;

            stern4IMG.active = true;
            stern5IMG.active = true;
            stern6IMG.active = true;

            auswertungTXT.text = "Toll gemacht!";
        }

        if (SceneSwitcherSpielauswahl.spielName == "Puzzle")
        {
            platzierungTXT.enabled = false;

            falscheIMG.enabled = false;
            richtigeIMG.enabled = false;
            richtigeTXT.enabled = false;
            falscheTXT.enabled = false;

            stern4IMG.active = true;
            stern5IMG.active = true;
            stern6IMG.active = true;

            auswertungTXT.text = "Toll gemacht!";
        }

        if (SceneSwitcherSpielauswahl.spielName == "Musik")
        {
            platzierungTXT.enabled = false;

            falscheIMG.enabled = false;
            richtigeIMG.enabled = false;
            richtigeTXT.enabled = false;
            falscheTXT.enabled = false;

            stern4IMG.active = true;
            stern5IMG.active = true;
            stern6IMG.active = true;

            auswertungTXT.text = "Toll gemacht!";
        }

        platzierungsManager = FindObjectOfType<PlatzierungsManager>();

        if (SceneSwitcherSpielauswahl.spielName == "Pferderennen")
        {
            levelBtn.active = false;

            falscheIMG.enabled = false;
            richtigeIMG.enabled = false;
            richtigeTXT.enabled = false;
            falscheTXT.enabled = false;

            //platzierungsIndex = PlatzierungsManager.pferdeImZielList.FindIndex(p => p.Contains(Pferde_Movement.horseName));
            platzierungsIndex = Pferde_Movement.pferdeImZiel.FindIndex(p => p.Contains("Pferd_Rot"));

            switch (platzierungsIndex)
            {
                default:
                case 3:
                    pokalSprite.active = false;
                    auswertungTXT.text = "Einfach dranbleiben!";
                    platzierungTXT.text = "4. Platz";
                    break;
                case 2:
                    pokalSprite.active = true;
                    auswertungTXT.text = "Gut gemacht!";
                    platzierungTXT.text = "3. Platz";
                    pokalSpriteRenderer.sprite = pokalSpriteArray[2];
                    break;
                case 1:
                    pokalSprite.active = true;
                    auswertungTXT.text = "Hervorragend!";
                    platzierungTXT.text = "2. Platz";
                    pokalSpriteRenderer.sprite = pokalSpriteArray[1];
                    break;
                case 0:
                    pokalSprite.active = true;
                    auswertungTXT.text = "Perfekt!";
                    platzierungTXT.text = "1. Platz";
                    pokalSpriteRenderer.sprite = pokalSpriteArray[0];
                    break;
            }
        }

        PlatzierungsManager.pferdeImZielList = new List<string>();
    }

    public void SternAuswertung()
    {
        switch (Kurbeln_Skript.richtigGemacht)
        {
            default:
            case <= 3:
                auswertungTXT.text = "Einfach dranbleiben!";
                break;
            case <= 5:
                auswertungTXT.text = "In Ordnung!";
                break;
            case <= 7:
                auswertungTXT.text = "Gut gemacht!";
                stern2IMG.active = true;
                break;
            case <= 9:
                auswertungTXT.text = "Hervorragend!";
                stern2IMG.active = true;
                stern3IMG.active = true;
                break;
            case > 9:
                auswertungTXT.text = "Perfekt!";
                stern1IMG.active = true;
                stern2IMG.active = true;
                stern3IMG.active = true;
                break;
        }
    }

    public void SternAuswertungKaffeRunde()
    {
        switch (Kurbeln_Skript.richtigGemacht)
        {
            default:
            case <= 2:
                auswertungTXT.text = "Einfach dranbleiben!";
                break;
            case <= 4:
                auswertungTXT.text = "In Ordnung!";
                break;
            case <= 9:
                auswertungTXT.text = "Gut gemacht!";
                stern2IMG.active = true;
                break;
            case <= 12:
                auswertungTXT.text = "Hervorragend!";
                stern2IMG.active = true;
                stern3IMG.active = true;
                break;
            case > 12:
                auswertungTXT.text = "Perfekt!";
                stern1IMG.active = true;
                stern2IMG.active = true;
                stern3IMG.active = true;
                break;
        }
    }

    private void Update()
    {
        if(againSlider.value < 1)
        {
            againSlider.value += Time.deltaTime * 0.2f;
        }
        else
        {
            SceneSwitcherSpielauswahl.Spielladen();
        }
    }
}
