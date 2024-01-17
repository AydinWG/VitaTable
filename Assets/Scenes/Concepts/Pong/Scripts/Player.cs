using UnityEngine;

namespace Mirror.Examples.Pong
{
    public class Player : NetworkBehaviour
    {
        public float speed = 30;
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
                        verticalInput = 1f;
                        break;
                    case "backward":
                        verticalInput = -1f;
                        break;
                    case "standing":
                    default:
                        verticalInput = 0;
                        break;
                }

                Vector2 movement = new Vector2(horizontalInput, verticalInput);
                movement.Normalize(); // normalize vector to prevent faster diagonal movement

                rigidbody2d.velocity = movement * speed * Time.fixedDeltaTime;
            }

            //if (isLocalPlayer)
            //{
            //    rigidbody2d.velocity = new Vector2(0, Input.GetAxisRaw("Vertical")) * speed * Time.fixedDeltaTime;
            //}
        }
    }
}
