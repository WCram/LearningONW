using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
 * Class that handles the logic in Stage 1 when a
 * character teleports onto a stump.
 */
public class StumpInteractable : MonoBehaviour
{
    [Header("Manager References")]
    public GameManager gameManager;

    [Header("Stage 1 Game Objects")]
    public GameObject Stage1;
    public GameObject SignText;

    private int[] memorySequence;
    private int globalIndex;
    private bool isAnimating;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        memorySequence = Stage1.GetComponent<MemoryPlatforms>().getMemorySequence();
        isAnimating = Stage1.GetComponent<MemoryPlatforms>().getIsAnimating();
        globalIndex = Stage1.GetComponent<MemoryPlatforms>().getGlobalIndex();

        if (globalIndex == -1)
        {
            SignText.GetComponent<TextMeshProUGUI>().text = "_ _ _ _";
        }
    }

    /**
 * Function called when the user teleports to a stump.
 */
    public void Teleported()
    {
        if (!isAnimating)
        {
            globalIndex++;
            // Get the memory sequence
            Stage1.GetComponent<MemoryPlatforms>().setGlobalIndex(globalIndex);

            switch (memorySequence[globalIndex])
            {
                case 0:
                    if (gameObject.name == "PurpleTreeStump")
                    {
                        correctTeleport();
                    }
                    else
                    {
                        incorrectTeleport();

                    }
                    break;
                case 1:
                    if (gameObject.name == "TanTreeStump")
                    {
                        correctTeleport();
                    }
                    else
                    {
                        incorrectTeleport();
                    }
                    break;
                case 2:
                    if (gameObject.name == "OrangeTreeStump")
                    {
                        correctTeleport();
                    }
                    else
                    {
                        incorrectTeleport();
                    }
                    break;
                case 3:
                    if (gameObject.name == "PinkTreeStump")
                    {
                        correctTeleport();
                    }
                    else
                    {
                        incorrectTeleport();
                    }
                    break;
                case 4:
                    if (gameObject.name == "RedTreeStump")
                    {
                        correctTeleport();
                    }
                    else
                    {
                        incorrectTeleport();
                    }
                    break;
                default:
                    Debug.Log("Default whaaaat");
                    break;
            }
        }
    }

    /**
     * Function called if the user teleports to the correct
     * stump in the sequence.
     */
    private void correctTeleport()
    {
        gameManager.GetComponent<GameManager>().CorrectActionHaptic();

        // Update sign
        string stringBuilder = "";
        int index = 0;

        while (index < memorySequence.Length)
        {
            if (index > globalIndex)
            {
                stringBuilder += "_ ";
                if (index == 3)
                {
                    stringBuilder += " ";
                }
            }
            else
            {
                stringBuilder += "0 ";
            }
            index++;
        }

        SignText.GetComponent<TextMeshProUGUI>().text = stringBuilder;

        // If at the end of the memory sequence
        if (globalIndex == memorySequence.Length - 1)
        {
            // Completed round 1
            Stage1.GetComponent<MemoryPlatforms>().StageComplete();
        }
    }

    /**
     * Function called if the user teleports to the incorrect
     * stump in the sequence.
     */
    private void incorrectTeleport()
    {
        gameManager.GetComponent<GameManager>().IncorrectActionHaptic();

        SignText.GetComponent<TextMeshProUGUI>().text = ":(";
        globalIndex = -1;
        Stage1.GetComponent<MemoryPlatforms>().setGlobalIndex(globalIndex);
    }
}
