using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ClientSpriteChanger : NetworkBehaviour
{
    public Sprite localClientSprite;
    public Sprite remoteClientSprite;

    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (isLocalPlayer)
        {
            spriteRenderer.sprite = localClientSprite;
        }
        else
        {
            spriteRenderer.sprite = remoteClientSprite;
        }
    }
}
