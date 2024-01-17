using Mirror;
using Mirror.Examples.Pong;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Ball : NetworkBehaviour
{
    public string lastRacketTouched;

    private string ownGoalStr;

    public float speed = 30;
    public Rigidbody2D rigidbody2d;

    public SpriteRenderer ballSpriteRenderer;

    private string ballCollisionStr;

    private PlayerStatManager playerStatManager;

    private int richtungInt = 0;

    public void Start()
    {
        playerStatManager = FindObjectOfType<PlayerStatManager>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        // only simulate ball physics on server
        rigidbody2d.simulated = true;


        // Serve the ball from left player
        rigidbody2d.velocity = Vector2.left * speed;
    }

    float HitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
    {
        return (ballPos.y - racketPos.y) / racketHeight;
    }

    // only call this on server
    [ServerCallback]
    void OnCollisionEnter2D(Collision2D col)
    {
        // Note: 'col' holds the collision information. If the
        // Ball collided with a racket, then:
        //   col.gameObject is the racket
        //   col.transform.position is the racket's position
        //   col.collider is the racket's collider

        // did we hit a racket? then we need to calculate the hit factor
        if (col.transform.GetComponent<Player>())
        {
            // Calculate y direction via hit Factor
            float y = HitFactor(transform.position,
                                col.transform.position,
                                col.collider.bounds.size.y);

            // Calculate x direction via opposite collision
            float x;
            if (col.relativeVelocity.x > 0)
            {
                x = 1f;
            }
            else
            {
                x = -1f;
            }

            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(x, y).normalized;

            // Set Velocity with dir * speed
            speed += 2.5f;
            rigidbody2d.velocity = dir * speed;
        }
        else if (col.transform.GetComponent<Player1>())
        {
            // Calculate y direction via hit Factor
            float x = HitFactor(transform.position,
                                col.transform.position,
                                col.collider.bounds.size.x);

            // Calculate y direction via opposite collision
            float y;
            if (col.relativeVelocity.y > 0)
            {
                y = 1f;
            }
            else
            {
                y = -1f;
            }

            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(y, x).normalized;

            // Set Velocity with dir * speed
            speed += 1;
            rigidbody2d.velocity = dir * speed;
        }

        switch (col.gameObject.name)
        {
            default:
            case "":
                RpcSendBallCollisionColorToClients(Color.white);
                ownGoalStr = "";
                break;
            case "Racket0(Clone)":
                RpcSendBallCollisionColorToClients(new Color(0, 255, 230, 255));
                break;
            case "Racket1(Clone)":
                RpcSendBallCollisionColorToClients(new Color32(0, 166, 255, 255));
                break;
            case "Racket2(Clone)":
                RpcSendBallCollisionColorToClients(new Color32(255, 158, 0, 255));
                break;
            case "Racket3(Clone)":
                RpcSendBallCollisionColorToClients(new Color32(255, 0, 11, 255));
                break;
            case "WallRacket0":
                ballCollisionStr = col.gameObject.name;
                RpcSendBallCollisionColorToClients(Color.white);

                if (playerStatManager.scoreList.Count() > 0)
                {
                    playerStatManager.LifeLoss(0);
                }
                speed = 30;
                break;
            case "WallRacket1":
                ballCollisionStr = col.gameObject.name;
                RpcSendBallCollisionColorToClients(Color.white);

                if (playerStatManager.scoreList.Count() > 1)
                {
                    playerStatManager.LifeLoss(1);
                }
                speed = 30;
                break;
            case "WallRacket2":
                ballCollisionStr = col.gameObject.name;
                RpcSendBallCollisionColorToClients(Color.white);

                if (playerStatManager.scoreList.Count() > 2)
                {
                    playerStatManager.LifeLoss(2);
                }
                speed = 30;
                break;
            case "WallRacket3":
                ballCollisionStr = col.gameObject.name;
                RpcSendBallCollisionColorToClients(Color.white);

                if (playerStatManager.scoreList.Count() > 3)
                {
                    playerStatManager.LifeLoss(3);
                }

                speed = 30;
                break;
        }
    }

    [ClientRpc]
    void RpcSendBallCollisionColorToClients(Color color)
    {
        ballSpriteRenderer.color = color;
    }
}