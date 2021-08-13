using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingSequence : MonoBehaviour
{
    [Header("Manager References")]
    public GameManager gameManager;
    public SoundManager soundManager;

    public List<GameObject> balls;
    public List<GameObject> particles;
    private List<bool> boatsHidden;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.hideObjects(particles);
    }

    public void setHiddenBoat(int index)
    {
        boatsHidden[index] = true;

        if (boatsHidden[0] && boatsHidden[1] && boatsHidden[2])
        {
            //stage completed
            soundManager.PlayDing();
            gameManager.showObjects(particles);
            gameManager.GetComponent<GameManager>().ActivateStage(2);
            StartCoroutine(delayHide());
        }
    }

    public void resetBallPositions()
    {
        balls[0].transform.position = new Vector3(-7.41f, 1.16f, 2.473f);
        balls[1].transform.position = new Vector3(-7.46f, 1.16f, 2.681f);
        balls[2].transform.position = new Vector3(-7.1f, 1.16f, 2.644f);
    }

    private IEnumerator delayHide()
    {
        yield return new WaitForSeconds(3);
        gameManager.hideObjects(particles);
    }

    public void ShowBalls()
    {
        gameManager.showObjects(balls);
        resetBallPositions();
    }
}
