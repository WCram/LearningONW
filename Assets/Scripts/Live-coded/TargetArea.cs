using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetArea : MonoBehaviour
{
    [Header("Manager References")]
    public SoundManager soundManager;

    [Header("Player")]
    public XRRig rig;

    [Header("Current Stage")]
    public int StageNum;

 /**
 * Function that gets called when a player enters
 * the target area.
 * @param other The object that enters the collider.
 */
    private void OnTriggerEnter(Collider other)
    {
        switch (StageNum)
        {
            case 0:
                soundManager.PlayDialogue(2);
                break;
            case 1:
                soundManager.PlayDialogue(3);
                break;
            case 2:
                soundManager.PlayDialogue(4);
                break;
            case 3:
                soundManager.PlayDialogue(5);
                break;
            case 4:
                soundManager.PlayDialogue(6);
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("ON target exit");
    }
}
