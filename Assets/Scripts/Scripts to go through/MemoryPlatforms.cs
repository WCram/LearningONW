using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPlatforms : MonoBehaviour
{
    [Header("Manager References")]
    public GameManager gameManager;
    public SoundManager soundManager;

    public List<GameObject> stumps;

    public List<GameObject> highlightedStumps;

    public List<GameObject> particles;

    private int sequenceLength = 4;
    private int seqIndex = -1;
    private bool isAnimating;

    private int[] memorySequence;

    private int globalIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject stump in highlightedStumps)
        {
            stump.SetActive(false);
        }
        foreach (GameObject particle in particles)
        {
            particle.SetActive(false);
        }
        memorySequence = new int[sequenceLength];
        SetUpMemorySequence();
    }

    private void SetUpMemorySequence()
    {
        int number = 0;
        int last_number = -1;
        int index = 0;

        do
        {
            number = Random.Range(0, sequenceLength+1);
            if (number == last_number) continue;
            memorySequence[index] = number;
            index++;
            last_number = number;
        } while (index < sequenceLength);
    }

    public void ShowStage1Animation()
    {
        InvokeRepeating("AnimatePlatforms", 1.0f, 2.0f);
    }

    private void AnimatePlatforms()
    {
        //TODO use this variable
        isAnimating = true;
        seqIndex++;

        foreach (GameObject stump in stumps)
        {
            stump.SetActive(true);
        }
        foreach (GameObject stump in highlightedStumps)
        {
            stump.SetActive(false);
        }

        if (seqIndex == sequenceLength)
        {
            CancelInvoke("AnimatePlatforms");
            isAnimating = false;
            seqIndex = -1;
            return;
        }

        switch (memorySequence[seqIndex])
        {
            case 0:
                stumps[0].SetActive(false);
                highlightedStumps[0].SetActive(true);
                break;
            case 1:
                stumps[1].SetActive(false);
                highlightedStumps[1].SetActive(true);
                break;
            case 2:
                stumps[2].SetActive(false);
                highlightedStumps[2].SetActive(true);
                break;
            case 3:
                stumps[3].SetActive(false);
                highlightedStumps[3].SetActive(true);
                break;
            case 4:
                stumps[4].SetActive(false);
                highlightedStumps[4].SetActive(true);
                break;
            default:
                stumps[0].SetActive(false);
                highlightedStumps[0].SetActive(true);
                break;
        }
    }

    //return helper functions
    public int[] getMemorySequence()
    {
        return memorySequence;
    }

    public bool getIsAnimating()
    {
        return isAnimating;
    }

    public int getGlobalIndex()
    {
        return globalIndex;
    }

    public void setGlobalIndex(int num)
    {
        globalIndex = num;
    }

    public void StageComplete()
    {
        soundManager.PlayDing();
        foreach(GameObject particle in particles)
        {
            particle.SetActive(true);
        }
        StartCoroutine(HideParticles());
        gameManager.GetComponent<GameManager>().ActivateStage(1);
    }

    private IEnumerator HideParticles()
    {
        float sec = 3f;
        yield return new WaitForSeconds(sec);
        foreach (GameObject particle in particles)
        {
            particle.SetActive(false);
        }
    }
}
