using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WlanAuswahl : MonoBehaviour
{
    public static string netzwerkName;

    private Button clickedButton;

    public void WLANAuswahl()
    {

        clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        if (clickedButton != null)
        {
            Text[] textComponents = clickedButton.GetComponentsInChildren<Text>();
            if (textComponents.Length > 0)
            {
                netzwerkName = textComponents[0].text;
            }
        }

        TCPClient.wlanChosen = true;
    }
}

