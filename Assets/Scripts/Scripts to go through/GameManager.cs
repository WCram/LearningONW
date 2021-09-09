using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class GameManager : GameManagerFactory
{
    [Header("Manager References")]
    public SoundManager soundManager;

    [Header("XRRig Components")]
    public XRRig rig;
    public XRBaseController LeftHand;
    public XRBaseController RightHand;

    [Header("Stage Game Objects")]
    public GameObject Stage1;
    public GameObject Stage2;
    public GameObject Stage3;
    public GameObject Stage4;
    public GameObject Finale;

    public static bool isStage3Done;
    private bool debug = true;
    private List<GameObject> Stages;

    // Start is called before the first frame update
    void Start()
    {
        //If stage 3 is complete, set all stages to active and reset rig position else set them all to inactive
        Stages = new List<GameObject>(GameObject.FindGameObjectsWithTag("Stage"));
        if (!isStage3Done)
        {
            StartCoroutine(DelayDialogue(1, 0));
            hideObjects(Stages);
        }
        else
        {
            showObjects(Stages);
            rig.transform.position = new Vector3(9.9f, 0.87f, 0.8f);
        }
    }

    /**
     * Function to show the objects for the next stage. Called
     * when previous stage has been completed.
     * @param stageNum The number of the stage to activate.
     */
    public void ActivateStage(int stageNum)
    {
        if (debug)
        {
            showObjects(Stages);
            Finale.SetActive(true);
            return;
        }

        switch (stageNum)
        {
            case 0:
                Stage1.SetActive(true);
                break;
            case 1:
                Stage2.SetActive(true);
                break;
            case 2:
                Stage3.SetActive(true);
                break;
            case 3:
                Stage4.SetActive(true);
                break;
            default:
                Debug.Log("Default stage");
                break;
        }
    }

    /**
     * Function to control all gameplay within the main game scene.
     * Called when dialogue finishes playing in SoundManager.
     * @param i Dialogue index that JUST played
     */
    public override void DialogueFinished(int i)
    {
        switch (i)
        {
            case 0:
                StartCoroutine(DelayDialogue(5, 1));
                break;
            case 2: // Stage 1 audio complete
                Stage1.GetComponent<MemoryPlatforms>().ShowStage1Animation();
                break;
            case 3: // Stage 2 audio complete
                Stage2.GetComponent<ThrowingSequence>().ShowBalls();
                break;
            case 4: // Stage 3 audio complete
                isStage3Done = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case 5: // Stage 4 audio complete
                Stage4.GetComponent<RockWall>().ShowRocks();
                break;
            case 6: // Finale audio complete
                Finale.GetComponent<FinaleSequence>().PlayFinale();
                break;
            default:
                break;
        }
    }

    /**
    * Delays dialogue by secs seconds 
    * @param secs Number of seconds to delay dialogue 
    * @param dialogue Dialogue index to play
    */
    IEnumerator DelayDialogue(int secs, int dialogue)
    {
        yield return new WaitForSeconds(secs);
        soundManager.PlayDialogue(dialogue);
    }
    
    public void CorrectActionHaptic()
    {
        LeftHand.SendHapticImpulse(0.2f, 0.1f);
        LeftHand.SendHapticImpulse(0.2f, 0.1f);

        RightHand.SendHapticImpulse(0.2f, 0.1f);
        RightHand.SendHapticImpulse(0.2f, 0.1f);
    }

    public void IncorrectActionHaptic()
    {
        LeftHand.SendHapticImpulse(0.1f, 0.1f);
        RightHand.SendHapticImpulse(0.1f, 0.1f);
    }

    public void hideObjects(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }
    }

    public void showObjects(List<GameObject> objects)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(true);
        }
    }
}
