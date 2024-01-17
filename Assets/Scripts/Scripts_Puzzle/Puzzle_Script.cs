using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Puzzle_Script : MonoBehaviour
{
    private Transform chosenSpriteContainer;
    public GameObject[] spritesContainer;
    private SpriteRenderer[] allSpriteRenderers; // Array to hold all SpriteRenderers
    private Sprite[] puzzleSprites;

    public Canvas anweisung;

    private int visibleCount = 2;

    private float alphaValue = 0;

    private int inputCounter = 0;
    private const int inputsPerIncrease = 50;

    private static int knownIndex;

    public void Start()
    {
        int index = 0;

        while (index == knownIndex)
        {
            index = Random.Range(0, spritesContainer.Length);
        }

        knownIndex = index;

        SetFirstPuzzleElements();
    }

    private void SetFirstPuzzleElements()
    {
        chosenSpriteContainer = spritesContainer[knownIndex].GetComponent<Transform>();
        chosenSpriteContainer.gameObject.SetActive(true); // Only activate the chosen container
        allSpriteRenderers = chosenSpriteContainer.GetComponentsInChildren<SpriteRenderer>();

        puzzleSprites = new Sprite[allSpriteRenderers.Length]; // Initialize puzzleSprites array

        int childCount = chosenSpriteContainer.childCount;
        puzzleSprites = new Sprite[childCount];
        allSpriteRenderers = new SpriteRenderer[childCount]; // Initialize the allSpriteRenderers array

        for (int i = 0; i < childCount; i++)
        {
            Transform child = chosenSpriteContainer.GetChild(i);
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                puzzleSprites[i] = spriteRenderer.sprite;
                allSpriteRenderers[i] = spriteRenderer;
                spriteRenderer.enabled = false; // Deactivate the sprite renderer
            }
        }

        RandomizeActiveSprites();
    }

    private void RandomizeActiveSprites()
    {
        if (visibleCount >= puzzleSprites.Length)
            return;

        for (int i = 0; i < puzzleSprites.Length; i++)
        {
            int randomIndex = Random.Range(i, puzzleSprites.Length);

            // Make sure the random index is different from the current index i
            if (randomIndex != i)
            {
                Sprite temp = puzzleSprites[randomIndex];
                puzzleSprites[randomIndex] = puzzleSprites[i];
                puzzleSprites[i] = temp;
                SpriteRenderer temp2 = allSpriteRenderers[randomIndex];
                allSpriteRenderers[randomIndex] = allSpriteRenderers[i];
                allSpriteRenderers[i] = temp2;
            }
        }

        for (int i = 0; i < allSpriteRenderers.Length; i++)
        {
            allSpriteRenderers[i].enabled = i < 2;
        }
    }

    private void Update()
    {
        if (Kurbeln_Skript.empfangeneDatenKurbelRichtungStr == "forward" || Kurbeln_Skript.empfangeneDatenKurbelRichtungStr == "backward")
        {
            inputCounter++;

            if (inputCounter >= inputsPerIncrease)
            {
                visibleCount++;
                inputCounter = 0;

                alphaValue = 0;

                if (visibleCount > allSpriteRenderers.Length)
                {
                    SceneManager.LoadScene(3);
                }
            }
            else
            {
                SetSpritesVisibility(visibleCount);
            }


            if (visibleCount > 3)
            {
                anweisung.enabled = false;
            }
        }
    }

    private void SetSpritesVisibility(int count)
    {
        allSpriteRenderers[count].enabled = true;

        alphaValue = Mathf.Lerp(0f, 1f, (float)inputCounter / (float)inputsPerIncrease);

        Color spriteColor = allSpriteRenderers[count].color;
        spriteColor.a = alphaValue;
        allSpriteRenderers[count].color = spriteColor;
    }
}