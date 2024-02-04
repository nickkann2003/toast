using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditorInternal;
using UnityEngine;

public class LittleFella : MonoBehaviour
{

    private enum GrabStatus
    {
        Rest,
        Reaching,
        Taking,
        Returning,
        Withdrawing,
        Gifting
    }

    [SerializeField]
    Vector3 dragGrabPos; // The position of the object he takes
    [SerializeField]
    Vector3 dragHomePos; // The position he takes the object to
    [SerializeField]
    Vector3 dragGiftPos; // The position of the gift he gives
    [SerializeField]
    GameObject edibleObject;
    [SerializeField]
    GameObject grabHand;

    [SerializeField]
    GrabStatus status = GrabStatus.Rest;

    [SerializeField]
    float grabSpeed;

    [SerializeField]
    int giftTarget = 5;

    int currentGifts = 0;

    [SerializeField]
    float moveProgress = 0.0f;

    [SerializeField]
    float handLevel = -2.0f; // Used to balance where the hand is grabbing, rather than the initial height;

    [SerializeField]
    GameObject giftPrefab;

    GameObject giftObject;

    // Update is called once per frame
    void Update()
    {
        switch(status)
        {
            // Reaching for the player's object
            case GrabStatus.Reaching:
                grabHand.transform.position = Vector3.Lerp(dragHomePos, dragGrabPos, moveProgress);
                moveProgress += Time.deltaTime * grabSpeed;
                if (moveProgress >= 1.0f)
                {
                    status = GrabStatus.Taking;
                    moveProgress = 0.0f;
                }

                break;

            // Dragging the player's object back
            case GrabStatus.Taking:
                grabHand.transform.position = Vector3.Lerp(dragGrabPos, dragHomePos, moveProgress);
                edibleObject.transform.position = grabHand.transform.position;
                moveProgress += Time.deltaTime * grabSpeed;
                if (moveProgress >= 1.0f)
                {
                    if (edibleObject.GetComponent<IEatable>() == null)
                    {
                        status = GrabStatus.Returning;
                        moveProgress = 0.0f;
                    }
                    else
                    {
                        AudioManager.instance.PlayOneShotSound(AudioManager.instance.eatingBread);
                        Destroy(edibleObject);

                        RequirementEvent rEvent;
                        if (edibleObject.GetComponent<ObjectVariables>() != null)
                        {
                            rEvent = new RequirementEvent(RequirementType.EatObject, edibleObject.GetComponent<ObjectVariables>(), true);
                        }
                        else
                        {
                            rEvent = new RequirementEvent(RequirementType.EatObject, new ObjectVariables(), true);
                        }

                        ObjectiveManager.instance.UpdateObjectives(rEvent);
                        
                        currentGifts++;

                        if(currentGifts == giftTarget)
                        {
                            GiveGift();
                        }
                        else
                        {
                            status = GrabStatus.Withdrawing;
                        }
                    }
                }
                break;

            // Can't eat the player's object, give it back
            case GrabStatus.Returning:
                grabHand.transform.position = Vector3.Lerp(dragHomePos, dragGiftPos, moveProgress);
                edibleObject.transform.position = grabHand.transform.position;
                moveProgress += Time.deltaTime * grabSpeed;
                if (moveProgress >= 1.0f)
                {
                    status = GrabStatus.Withdrawing;
                    edibleObject = null;
                    moveProgress = 0.0f;
                }
                break;

            // Withdraw hand to default position
            case GrabStatus.Withdrawing:
                grabHand.transform.position = Vector3.Lerp(grabHand.transform.position, dragHomePos, moveProgress);
                moveProgress += Time.deltaTime * grabSpeed;
                if (moveProgress >= 1.0f)
                {
                    status = GrabStatus.Rest;
                    moveProgress = 0.0f;
                }
                break;

            // Player has fed enough, give a gift
            case GrabStatus.Gifting:
                grabHand.transform.position = Vector3.Lerp(dragHomePos, dragGiftPos, moveProgress);
                giftObject.transform.position = grabHand.transform.position;
                moveProgress += Time.deltaTime * grabSpeed;
                if(moveProgress >= 1.0f)
                {
                    status = GrabStatus.Withdrawing;
                }
                break;

            // Nothing to do, stay in place
            case GrabStatus.Rest:
                if(edibleObject != null)
                {
                    if(Math.Abs(edibleObject.transform.position.y - handLevel) < 0.3)
                    {
                        status = GrabStatus.Reaching;
                    }
                }
                break;

        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != grabHand && other.gameObject != giftObject)
        {
            if (edibleObject == null)
            {
                edibleObject = other.gameObject;
                dragGrabPos = other.transform.position;
                dragGrabPos.y = handLevel;
                //status = GrabStatus.Reaching;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == edibleObject)
        {
            edibleObject = null;
            moveProgress = 0.0f;
            status = GrabStatus.Withdrawing;
        }
    }

    private void GiveGift()
    {
        // Create the gift
        giftObject = Instantiate(giftPrefab, dragHomePos, Quaternion.identity);
        moveProgress = 0.0f;
        status = GrabStatus.Gifting;
    }
}
