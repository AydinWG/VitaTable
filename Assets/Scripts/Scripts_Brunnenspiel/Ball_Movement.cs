using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Ball_Movement : MonoBehaviour
{
    public float maxBallHeight = 15.5f;
    public float minBallHeight = 6;

    public ParticleSystem waterFountain;

    ParticleSystem.MainModule waterFountaintMainModule;

    public GameObject ball;
    public GameObject fountain;

    public Vector3 zielposition;
    public Vector3 startposition;

    public int baelleGeschafft;

    public GameObject zahlenSprite;

    private SpriteRenderer spriteRenderer;

    public Sprite[] spriteArray;

    public GameObject pivotObject;

    public GameObject ducklings;

    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        ball.transform.position = new Vector3(0, minBallHeight, 0);

        baelleGeschafft = 0;

        spriteRenderer = zahlenSprite.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = spriteArray[baelleGeschafft];

        waterFountain = fountain.GetComponent<ParticleSystem>();

        waterFountaintMainModule = waterFountain.main;

        MySQLConnector.InsertSpieleData(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (baelleGeschafft < 6)
        {
            if (ball.transform.position.y <= 15)
            {
                switch (Kurbeln_Skript.empfangeneDatenKurbelRichtungStr)
                {
                    case "forward":
                        waterFountain.Play();

                        ball.transform.position = Vector3.MoveTowards(transform.position, zielposition, 0.1f);

                        waterFountaintMainModule.gravityModifierMultiplier = 0.1f;

                        ducklings.transform.RotateAround(pivotObject.transform.position, new Vector3(0, 1, 0), rotationSpeed * Time.deltaTime);

                        CancelInvoke();
                        break;
                    case "backward":
                    case "standing":
                    default:
                        InvokeRepeating("BallFall", 2.0f, 0.5f);
                        break;
                }


                //if (Input.GetKeyDown(KeyCode.UpArrow))
                //{
                //    waterFountain.Play();

                //    ball.transform.position = Vector3.MoveTowards(transform.position, zielposition, 0.1f);

                //    waterFountaintMainModule.gravityModifierMultiplier = 0.1f;

                //    ducklings.transform.RotateAround(pivotObject.transform.position, new Vector3(0, 1, 0), rotationSpeed * Time.deltaTime);

                //    CancelInvoke();
                //}
                //else
                //{
                //    InvokeRepeating("BallFall", 2.0f, 0.5f);

                //}
            }
            else
            {
                waterFountain.Clear();

                waterFountain.Stop();

                ball.transform.position = new Vector3(0, minBallHeight, 0);

                baelleGeschafft++;

                spriteRenderer.sprite = spriteArray[baelleGeschafft];
            }

        }
        else
        {
            SceneManager.LoadScene(3);
        }
    }

    void RundeGeschafftCheck()
    {
        if (ball.transform.position.y >= maxBallHeight)
        {
            ball.transform.position = new Vector3(0, minBallHeight, 0);
        }
    }

    void BallFall()
    {
        ball.transform.position = Vector3.MoveTowards(transform.position, startposition, 0.025f);

        waterFountaintMainModule.gravityModifierMultiplier = 3.5f;
    }
}
