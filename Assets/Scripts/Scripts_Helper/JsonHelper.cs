using UnityEngine;

[System.Serializable]
public class JsonHelper
{
    //public enumMessageType MessageTyp;
    public string Message;
    public USBDatenklasse USBDatenklasse;

    public static JsonHelper CreateFromJSON(string jsonString)
    {
        USBDatenklasse.CreateFromJSON(jsonString);
        return JsonUtility.FromJson<JsonHelper>(jsonString);
    }
}

[System.Serializable]
public class USBDatenklasse
{
    public int TS;
    public int SZ1L;
    public int SZ1R;
    public int K1;
    public int K2;
    public int K3;
    public int K4;
    public int K5;
    public int K6;
    public string Richtung;

    public static USBDatenklasse CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<USBDatenklasse>(jsonString);
    }
}
