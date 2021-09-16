using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/**
 * Class to move a player controller in the physics direction
 * if the player is touching a climbable object.
 */
public class Climber : MonoBehaviour
{
    private CharacterController character;
    public static XRRayInteractor climbingHand;
    private ActionBasedContinuousMoveProvider continuousMovement;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        continuousMovement = GetComponent<ActionBasedContinuousMoveProvider>();
    }

    // FixedUpdate is called once per physics frame --> does not depend on game's frame rate
    void FixedUpdate()
    {
        if (climbingHand)
        {
            continuousMovement.enabled = false;
            string leftOrRight = climbingHand.name;

            if (leftOrRight == "LeftHand Controller")
            {
                Climb(XRNode.LeftHand);
            }
            else
            {
                Climb(XRNode.RightHand);
            }
        }
        else
        {
            continuousMovement.enabled = true;
        }
    }

    /** 
     * Function to detect the climbing computations according
     * to the velocity of the controller.
     */
    void Climb(XRNode hand)
    {
        InputDevices.GetDeviceAtXRNode(hand).TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);
        // time.deltaTime is the time between 2 frames, time.FixedDeltaTime is the duration between 2 fixedUpdate calls, used when Physics is required
        character.Move(transform.rotation * -velocity * Time.fixedDeltaTime);
    }
}
