using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class plays Audio for the HandTracking scene.
 * 
 * From the scene it takes in the GameManager game object, the Ding sound,
 * audio clips to match the text in the GameManager, and the SoundManager game object.
 */
public class SoundManager : MonoBehaviour
{
    /** GameManager object */
    public GameManagerFactory manager;

    /** Audio source that plays the ding fx */
    public AudioSource dingSource;

    /** Audio source that plays the dialogue */
    public AudioSource dialogueSource;

    /** Other scripts can call functions in SoundManager */
    public static SoundManager instance = null;

    /** List of dialogue audio */
    public List<AudioClip> dialogue;

    /**  Ding fx */
    public AudioClip ding;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            SoundManager[] soundManagers = FindObjectsOfType<SoundManager>();
            foreach (SoundManager manager in soundManagers)
            {
                Destroy(manager);
            }
            instance = this;
        }

        // If instance doesnt't exist, set to this

        // Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading the scene
        //DontDestroyOnLoad(transform.gameObject);
    }

    /** 
     * Waits until audio is finished and then calls DialogueFinished in GameManager
     * @param Number of seconds
     */
    private IEnumerator WaitAudio(int i)
    {
        yield return new WaitForSeconds(dialogueSource.clip.length);
        Debug.Log("Calling manager dialogue finished");
        manager.DialogueFinished(i);
    }

    /**
     * Plays the dialogue associated with dialogueID
     * @param dialogueID Index of the dialogue being played </param>
     */
    public void PlayDialogue(int dialogueID)
    {
        Debug.Log("Playing " + dialogueSource);
        // Select the current dialogue sound file
        dialogueSource.clip = dialogue[dialogueID];

        // Play the dialogue
        dialogueSource.Play();

        // Fire an event when the dialogue is finished
        StartCoroutine(WaitAudio(dialogueID));
    }

    /**
     * Plays a ding sound effect
     */
    public void PlayDing()
    {
        dingSource.clip = ding;
        dingSource.Play();
    }
}
