using UnityEngine;

namespace Mirror.Examples.Pong
{
    public class Player1 : NetworkBehaviour
    {
        public float speed = 1500;
        public Rigidbody2D rigidbody2d;


        float horizontalInput = 0f;
        float verticalInput = 0f;

        // need to use FixedUpdate for rigidbody
        void FixedUpdate()
        {
            // only let the local player control the racket.
            // don't control other player's rackets
            if (isLocalPlayer)
            {
                switch (Kurbeln_Skript.empfangeneDatenKurbelRichtungStr)
                {
                    case "forward":
                        horizontalInput = 1f;
                        break;
                    case "backward":
                        horizontalInput = -1f;
                        break;
                    case "standing":
                    default:
                        horizontalInput = 0;
                        break;
                }

                Vector2 movement = new Vector2(horizontalInput, verticalInput);
                movement.Normalize(); // normalize vector to prevent faster diagonal movement

                rigidbody2d.velocity = movement * speed * Time.fixedDeltaTime;
            }

            //if (isLocalPlayer)
            //{
            //    rigidbody2d.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), 0) * speed * Time.fixedDeltaTime;
            //}
        }
    }
}