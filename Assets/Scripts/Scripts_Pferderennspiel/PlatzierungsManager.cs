using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class PlatzierungsManager : NetworkBehaviour
{
    public readonly SyncList<string> pferdeImZiel = new SyncList<string>();

    public static List<string> pferdeImZielList;

    private void Start()
    {
        pferdeImZielList = new List<string>();
    }

    public void UpdatePferdeImZielList()
    {
        pferdeImZielList.Clear(); // Clear the existing list before updating

        foreach (var pferd in pferdeImZiel)
        {
            pferdeImZielList.Add(pferd); // Add each element from SyncList to List
        }
    }
}
