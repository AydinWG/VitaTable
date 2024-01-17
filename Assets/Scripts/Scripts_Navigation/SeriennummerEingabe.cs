using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeriennummerEingabe : MonoBehaviour
{
    public InputField seriennummerTXT;
    public GameObject seriennummerTXTGO;

    public GameObject seriennummerDisplayTXTGO;
    public GameObject versionsnummerHWDisplayTXTGO;
    public GameObject versionsnummerSWDisplayTXTGO;

    public Text placeHolderTXT;

    public GameObject weiterBTNGO;

    public static string seriennummer;

    // Start is called before the first frame update
    void Start()
    {
        if (!MySQLConnector.IsSysteminformationenTableEmpty())
        {
            seriennummerTXTGO.SetActive(false);
            weiterBTNGO.SetActive(false);

            seriennummerDisplayTXTGO.SetActive(true);
            versionsnummerHWDisplayTXTGO.SetActive(true);
            versionsnummerSWDisplayTXTGO.SetActive(true);

            seriennummerDisplayTXTGO.GetComponent<Text>().text = "Seriennummer: " + MySQLConnector.GetFirstSeriennummer();
            versionsnummerHWDisplayTXTGO.GetComponent<Text>().text = "Hardwarenummer: " + MySQLConnector.GetFirstVersionsnummerHW();
            versionsnummerSWDisplayTXTGO.GetComponent<Text>().text = "Softwarenummer: " + MySQLConnector.GetFirstVersionsnummerSW();
        }
        else
        {
            seriennummerTXTGO.SetActive(true);
            weiterBTNGO.SetActive(true);

            seriennummerDisplayTXTGO.SetActive(false);
            versionsnummerHWDisplayTXTGO.SetActive(false);
            versionsnummerSWDisplayTXTGO.SetActive(false);
        }
    }

    public void SeriennummerAnlegen()
    {
        if (!string.IsNullOrEmpty(seriennummerTXT.text))
        {
            seriennummer = seriennummerTXT.text.ToString();

            MySQLConnector.SysInfEingabe();

            SceneManager.LoadScene(16);
        }
        else
        {
            placeHolderTXT.color = Color.red;
        }
    }
}
