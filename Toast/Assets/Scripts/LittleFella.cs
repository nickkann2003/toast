using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
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
    float grabSpeed = 1.0f;

    [SerializeField]
    int giftTarget = 5;

    int currentGifts = 0;

    float moveProgress = 0.0f;

    float handLevel = -2.0f; // Used to balance where the hand is grabbing, rather than the initial height;

    // Update is called once per frame
    void Update()
    {
        switch(status)
        {
            case GrabStatus.Reaching:
                grabHand.transform.position = Vector3.Lerp(grabHand.transform.position, dragGrabPos, moveProgress);
                moveProgress += Time.deltaTime * grabSpeed;
                if (moveProgress >= 1.0f)
                {
                    status = GrabStatus.Taking;
                    moveProgress = 0.0f;
                }

                break;

            case GrabStatus.Taking:
                grabHand.transform.position = Vector3.Lerp(grabHand.transform.position, dragHomePos, moveProgress);
                edibleObject.transform.position = grabHand.transform.position;
                moveProgress += Time.deltaTime * grabSpeed;
                if (moveProgress >= 1.0f)
                {
                    if (edibleObject.GetComponent<IEatable>() != null)
                    {
                        status = GrabStatus.Returning;
                        moveProgress = 0.0f;
                    }
                    else
                    {
                        Destroy(edibleObject);
                        currentGifts++;

                        if(currentGifts >= giftTarget)
                        {
                            GiveGift();
                        }
                    }
                }
                break;

            case GrabStatus.Returning:
                grabHand.transform.position = Vector3.Lerp(grabHand.transform.position, dragGiftPos, moveProgress);
                edibleObject.transform.position = grabHand.transform.position;
                moveProgress += Time.deltaTime * grabSpeed;
                if (moveProgress >= 1.0f)
                {
                    status = GrabStatus.Withdrawing;
                    moveProgress = 0.0f;
                }
                break;

            case GrabStatus.Withdrawing:
                grabHand.transform.position = Vector3.Lerp(grabHand.transform.position, dragHomePos, moveProgress);
                moveProgress += Time.deltaTime * grabSpeed;
                if (moveProgress >= 1.0f)
                {
                    status = GrabStatus.Rest;
                    moveProgress = 0.0f;
                }
                break;

            case GrabStatus.Gifting:
                break;

            case GrabStatus.Rest:
                return;

        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(edibleObject == null)
        {
            edibleObject = other.gameObject;
            dragGrabPos = other.transform.position;
            dragGrabPos.y = handLevel;
            status = GrabStatus.Reaching;
        }
    }

    private void GiveGift()
    {
        // Create the gift

        //status = GrabStatus.Gifting;
    }
}
