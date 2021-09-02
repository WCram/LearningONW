using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
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

    // Start is called before the first frame update
    void Start()
    {
        //If stage 3 is complete, set all stages to active and reset rig position else set them all to inactive
        List<GameObject> Stages = new List<GameObject>(GameObject.FindGameObjectsWithTag("Stage"));
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

    // Update is called once per frame
    void Update()
    {
        //locate the right controller
        var rightHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);

        //locate the left controller
        var leftHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);

        //make sure that there is only one left and right controller respectively
        if (rightHandDevices.Count == 1)
        {
            InputDevice device = rightHandDevices[0];
            bool primaryButtonValue;
            if (device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonValue)
                && primaryButtonValue)
            {
                Debug.Log("A is pressed, Quitting game");
                QuitGame();
            }
        }
        else if (rightHandDevices.Count > 1 || leftHandDevices.Count > 1)
        {
            Debug.Log("Found more than one left or right hand!");
        }


        if (leftHandDevices.Count == 1)
        {
            InputDevice device = leftHandDevices[0];
            bool primaryButtonValue;
            if (device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonValue)
                && primaryButtonValue)
            {
                Debug.Log("X is pressed, Restarting game");
                RestartGame();
            }
        }
        //debug
        if(Input.GetKeyDown("1")){
            SceneManager.LoadScene("HandSimonSays", LoadSceneMode.Single);
        }
    }

    /**
     * Function to restart the main game scene.
     */
    public void RestartGame()
    {
        Debug.Log("Restart Oculus Ninja Warrior");
        RestartHaptic();
        Scoreboard scoreboard = FindObjectOfType<Scoreboard>();
        Destroy(scoreboard);
        soundManager.PlayDing();
        //LoadSceneMode.Single: Closes all current loaded Scenes and loads a Scene.
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    /**
     * Function to quit the game.
     */
    public void QuitGame()
    {
        Debug.Log("Quit Oculus Ninja Warrior");
        QuitHaptic();
        Application.Quit();
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
            Stage1.SetActive(true);
            Stage2.SetActive(true);
            Stage3.SetActive(true);
            Stage4.SetActive(true);
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

    public void EnterTargetHaptic()
    {
        LeftHand.SendHapticImpulse(0.7f, 0.5f);
        RightHand.SendHapticImpulse(0.7f, 0.5f);
    }

    public void GrabObjectHaptic()
    {
        LeftHand.SendHapticImpulse(0.2f, 0.1f);
        RightHand.SendHapticImpulse(0.2f, 0.1f);
    }

    public void CorrectActionHaptic()
    {
        LeftHand.SendHapticImpulse(0.1f, 0.1f);
        LeftHand.SendHapticImpulse(0.1f, 0.1f);

        RightHand.SendHapticImpulse(0.1f, 0.1f);
        RightHand.SendHapticImpulse(0.1f, 0.1f);
    }

    public void IncorrectActionHaptic()
    {
        LeftHand.SendHapticImpulse(0.1f, 0.1f);
        RightHand.SendHapticImpulse(0.1f, 0.1f);
    }

    public void QuitHaptic()
    {
        RightHand.SendHapticImpulse(0.25f, 0.25f);
    }

    public void RestartHaptic()
    {
        LeftHand.SendHapticImpulse(0.5f, 0.5f);
        LeftHand.SendHapticImpulse(0.5f, 0.5f);
    }

    public void PlayHaptic()
    {
        LeftHand.SendHapticImpulse(0.5f, 1f);
        RightHand.SendHapticImpulse(0.5f, 1f);
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
