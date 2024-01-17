using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saegenscript : MonoBehaviour
{
    public float maxHin;
    public float maxHer;
    public float maxRunter;
    public float startHoehe;

    public GameObject saege;

    // Start is called before the first frame update
    void Start()
    {
        maxHin = 3.2f;
        maxHer = 0.6f;
        startHoehe = 1.7f;
        maxRunter = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && saege.transform.position.z <= maxHin)
        {
            saege.transform.position +=  new Vector3(0, -0.01f, 0.25f);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && saege.transform.position.z >= maxHer)
        {
            saege.transform.position -= new Vector3(0, 0.01f, 0.25f);
        }

    }
}
