using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spielfeld_drehen : MonoBehaviour
{
    public GameObject spielFeld;

    public GameObject ball;

    public GameObject zielKreis;

    public float randomX;

    public float randomZ;

    public float rotationX;

    public float rotationZ;

    void Start()
    {
        randomX = Random.Range(-4, 4.1f);

        randomZ = Random.Range(-4, 4.1f);

        rotationX = 1f;

        //rotationY = 0.05f;

        zielKreis.transform.position = new Vector3(randomX, -0.012f, randomZ);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rotationX += 1f;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            rotationX -= 1f;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rotationZ += 1f;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rotationZ -= 1f;
        }

        ball.transform.TransformDirection(Vector3.forward);

        spielFeld.transform.rotation = Quaternion.Euler(rotationX, 0, rotationZ);

        if (zielKreis.transform.position.x == ball.transform.position.x && zielKreis.transform.position.x == ball.transform.position.x)
        {
            zielKreis.transform.position = new Vector3(randomX, 0.1f, randomZ);
        }
    }
}
