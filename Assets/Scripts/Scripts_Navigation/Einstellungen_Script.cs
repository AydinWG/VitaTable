using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Einstellungen_Script : MonoBehaviour
{
    public Text macAdresse;
    public Text ipAdresse;
    public Text sensorOben;
    public Text sensorUnten;

    public static string[,] platzDatenArr;

    public static int kurbelOben;

    public static int kurbelUnten;

    public static int platz;

    public static string kurbelObenStr;

    public static string kurbelUntenStr;

    public static string letzteDreiZahlenDerIPAdresse;
    public static void PlatzDatenArrayBefuellen()
    {
        platzDatenArr = new string[,] {
                                    { "101", "0", "4" },
                                    { "102", "1", "null" },
                                    { "103", "2", "5" },
                                    { "104", "3", "null" },
                                    //TODO: nach Test rausmachen
                                    //{ "105", "0", "4" },
                                    //{ "1", "2", "5" },
                                    //{ "106", "KO6", "KU6" },
                                    //{ "107", "KO7", "KU7" },
                                    //{ "108", "KO8", "KU8" },
                                    //{ "109", "KO9", "KU9" },
                                    //{ "110", "KO10", "KU10" },
    };
    }

    public static string GetLocalMacAdress()
    {
        string macAdresse = "";

        IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

        if (nics == null || nics.Length < 1)
        {
            return macAdresse;
        }

        foreach (NetworkInterface adapter in nics)
        {
            IPInterfaceProperties properties = adapter.GetIPProperties(); //  .GetIPInterfaceProperties();

            PhysicalAddress address = adapter.GetPhysicalAddress();
            byte[] bytes = address.GetAddressBytes();

            if (adapter.Name == "wlan0")
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    // Display the physical address in hexadecimal.
                    macAdresse += bytes[i].ToString("X2");
                    // Insert a hyphen after each byte, unless we are at the end of the
                    // address.
                }
            }
        }

        return macAdresse;
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    //void Start()
    //{
    //    macAdresse.text = GetLocalMacAdress();
    //    ipAdresse.text = GetLocalIPAddress();

    //    letzteDreiZahlenDerIPAdresse = GetLocalIPAddress().Substring(GetLocalIPAddress().Length - 3);

    //    for (int zeile = 0; zeile < platzDatenArr.GetLength(0); zeile++)
    //    {
    //        for (int spalte = 0; spalte < platzDatenArr.GetLength(1); spalte++)
    //        {
    //            if (platzDatenArr[zeile, 0] == letzteDreiZahlenDerIPAdresse)
    //            {
    //                sensorOben.text = platzDatenArr[zeile, 1];
    //                sensorUnten.text = platzDatenArr[zeile, 2];
    //            }
    //        }
    //    }
    //}

    public static void PlatzIdentifizierung()
    {
        letzteDreiZahlenDerIPAdresse = GetLocalIPAddress().Substring(GetLocalIPAddress().Length - 3);

        for (int zeile = 0; zeile < platzDatenArr.GetLength(0); zeile++)
        {
            for (int spalte = 0; spalte < platzDatenArr.GetLength(1); spalte++)
            {
                if (platzDatenArr[zeile, 0] == letzteDreiZahlenDerIPAdresse)
                {
                    platz = Int32.Parse(platzDatenArr[zeile, 0]);
                    kurbelOben = Int32.Parse(platzDatenArr[zeile, 1]);
                    kurbelUnten = Int32.Parse(platzDatenArr[zeile, 2]);
                }
            }
        }
    }
}
