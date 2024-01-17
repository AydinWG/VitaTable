using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FishSpawn_Script : MonoBehaviour
{
    public GameObject fishPrefab;

    static int numFish;
    public static GameObject[] allFish;

    public static Vector3 goalsPos = Vector3.zero;

    void Awake()
    {
        numFish = Random.Range(0, 15);

        allFish = new GameObject[numFish];
    }

    void Start()
    {
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-3, 3),
                                      Random.Range(-0.68f, 0.5f),
                                      Random.Range(-3, 3));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
        }
    }

    void Update()
    {
        if (Random.Range(0, 10000) < 50)
        {
            goalsPos = new Vector3(Random.Range(-3, 3),
                                   Random.Range(-0.68f, 0.5f),
                                   Random.Range(-3, 3));
        }
    }
}
