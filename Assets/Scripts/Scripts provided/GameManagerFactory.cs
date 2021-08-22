using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerFactory : MonoBehaviour
{
    public virtual void DialogueFinished(int i)
    {
        Debug.Log("Calling Factory Dialogue finished");
    }
}
