using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Class to handle the logic for the ending finale sequence.
 */
public class FinaleSequence : MonoBehaviour
{
    [Header("Finale Game Objects")]
    public GameObject FinaleParticle;
    private GameObject WinnerParticle1;
    private GameObject WinnerParticle2;
    private GameObject WinnerMenu;
    private GameObject TimerInfo;
    public GameObject NextLevelObjects;
    private Scoreboard scoreboard;

    void Start()
    {
        scoreboard = FindObjectOfType<Scoreboard>();
        WinnerParticle1 = GameObject.FindWithTag("particleLeft");
        WinnerParticle2 = GameObject.FindWithTag("particleRight");
        TimerInfo = GameObject.FindWithTag("Timer");
        WinnerMenu = GameObject.FindWithTag("WinnerMenu");

        WinnerParticle1.GetComponent<Renderer>().enabled = false;
        WinnerParticle2.GetComponent<Renderer>().enabled = false;
        FinaleParticle.GetComponent<Renderer>().enabled = false;
        NextLevelObjects.SetActive(false);
    }

    /**
     * Function to deactivate the timer game objects
     * and activate the "winner" insignia and particles
     */
    public void PlayFinale()
    {
        TimerInfo.SetActive(false);
        NextLevelObjects.SetActive(true); 
        scoreboard.ToggleWinnerMenu(true);
        WinnerParticle1.GetComponent<Renderer>().enabled = true;
        WinnerParticle2.GetComponent<Renderer>().enabled = true;
        FinaleParticle.GetComponent<Renderer>().enabled = true;

        StartCoroutine(HideParticles());
        GameObject[] targets =  GameObject.FindGameObjectsWithTag("TargetArea");
        //remove the targets that trigger stages
        foreach( GameObject target in targets){
            target.SetActive(false);
        }
    }

    /**
     * Function to stop the finale particles.
     */
    private IEnumerator HideParticles()
    {
        float sec = 3f;
        yield return new WaitForSeconds(sec);
        FinaleParticle.SetActive(false);
    }
}
