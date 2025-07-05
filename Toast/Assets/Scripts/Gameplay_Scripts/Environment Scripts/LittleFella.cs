using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditorInternal;
using UnityEngine;

public class LittleFella : MonoBehaviour
{
    // ------------------------------- Enums -------------------------------
    private enum GrabStatus
    {
        Rest,
        Reaching,
        Taking,
        Returning,
        Withdrawing,
        Gifting
    }

    // ------------------------------- Variables -------------------------------
    [Header("Grab Item Variables")]
    [SerializeField]
    private Vector3 dragGrabPos; // The position of the object he takes
    [SerializeField]
    private Vector3 dragHomePos; // The position he takes the object to
    [SerializeField]
    private Vector3 dragGiftPos; // The position of the gift he gives
    [SerializeField]
    float grabSpeed;
    [SerializeField]
    private int giftTarget = 5;

    [Header("Other Variables")]
    [SerializeField]
    private GameObject edibleObject;
    private List<GameObject> nearbyObjects = new List<GameObject>();
    [SerializeField]
    private GameObject grabHand;
    [SerializeField]
    private GrabStatus status = GrabStatus.Rest;
    [SerializeField]
    private Animator littleFellaAnimator;

    int currentGifts = 0;

    [SerializeField]
    float moveProgress = 0.0f;

    //[SerializeField]
    //float handLevel = 0.2f; // Used to balance where the hand is grabbing, rather than the initial height;

    [Header("Gift References")]
    [SerializeField]
    private GameObject giftPrefab;

    GameObject giftObject;

    [Header("Event References")]
    [SerializeField]
    private PropIntGameEvent littleFellaEvent;

    [Header("Audio")]
    [SerializeField]
    private AudioEvent alert;
    [SerializeField]
    private AudioEvent taking;
    [SerializeField]
    private AudioEvent pondering;
    [SerializeField]
    private AudioEvent accept;
    [SerializeField]
    private AudioEvent reject;
    [SerializeField]
    private AudioEvent interrupt;

    private AudioSource source1;
    private AudioSource source2;


    // ------------------------------- Functions -------------------------------
    private void Start()
    {
        source1 = gameObject.AddComponent<AudioSource>();
        source2 = gameObject.AddComponent<AudioSource>();

        source1.spatialBlend = 1.0f;
        source2.spatialBlend = 1.0f;
        source1.dopplerLevel = 0.0f;
        source2.dopplerLevel = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Switch based on current GrabStatus
        switch (status)
        {
            // Reaching for the player's object
            case GrabStatus.Reaching:

                // PLayer moved object, withdraw
                if (edibleObject.GetComponent<Rigidbody>().velocity != Vector3.zero)
                {
                    status = GrabStatus.Withdrawing;
                    interrupt.Play(source2);
                    break;
                }

                grabHand.transform.position = Vector3.Lerp(dragHomePos, dragGrabPos, moveProgress);
                moveProgress += Time.deltaTime * grabSpeed;
                if (moveProgress >= 1.0f)
                {
                    status = GrabStatus.Taking;
                    taking.Play(source2);
                    pondering.Play(source1);
                    moveProgress = 0.0f;
                }

                break;

            // Dragging the player's object back
            case GrabStatus.Taking:
                grabHand.transform.position = Vector3.Lerp(dragGrabPos, dragHomePos, moveProgress);
                edibleObject.transform.position = grabHand.transform.position;
                moveProgress += Time.deltaTime * grabSpeed;
                if(moveProgress > 0.4f && moveProgress < 0.42f)
                {
                    littleFellaAnimator.SetTrigger("Happy");
                }

                if (moveProgress >= 1.0f)
                {
                    littleFellaEvent.RaiseEvent(edibleObject.GetComponent<NewProp>(), 1);

                    // Local stats integration
                    SaveHandler.instance.StatsHandler.LittleFellaGiven += 1;

                    // Steam stats integration
                    int itemsGiven = 0;

                    try
                    {
                        if (SteamManager.Initialized)
                        {
                            SteamUserStats.GetStat("LITTLE_FELLA_GIVEN", out itemsGiven);
                            itemsGiven++;
                            SteamUserStats.SetStat("LITTLE_FELLA_GIVEN", itemsGiven);
                            SteamUserStats.StoreStats();
                        }
                    }
                    catch
                    {
                        Debug.Log("Steamworks not properly intialized");
                    }


                    Debug.Log(edibleObject.GetComponent<NewProp>().propFlags);
                    if (edibleObject.GetComponent<NewProp>().HasFlag(PropFlags.Bread) || edibleObject.GetComponent<NewProp>().HasFlag(PropFlags.Jam))
                    {
                        edibleObject.transform.position = dragGiftPos;
                        edibleObject.GetComponent<NewProp>().AddAttribute(StatAttManager.instance.eatenAtt);
                        //Destroy(edibleObject);
                        AudioManager.instance.PlayOneShotSound(AudioManager.instance.eatingBread);
                        accept.Play(source1);
                        nearbyObjects.Remove(edibleObject);

                        currentGifts++;

                        if (currentGifts == giftTarget)
                        {
                            GiveGift();
                        }
                        else
                        {
                            status = GrabStatus.Withdrawing;
                        }
                    }
                    else
                    {
                        status = GrabStatus.Returning;
                        reject.Play(source1);
                        littleFellaAnimator.SetTrigger("Angry");
                        moveProgress = 0.0f;
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
                    littleFellaAnimator.SetTrigger("Go To Sleep");
                    moveProgress = 0.0f;
                    for(int i = nearbyObjects.Count-1; i >= 0; i--)
                    {
                        if (nearbyObjects[i] == null)
                        {
                            nearbyObjects.RemoveAt(i);
                        }
                    }
                    if(nearbyObjects.Count > 0)
                    {
                        edibleObject = nearbyObjects[0];
                        dragGrabPos = edibleObject.transform.position;
                    }
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
                if(edibleObject != null && edibleObject.GetComponent<Rigidbody>().velocity == Vector3.zero)
                {
                    dragGrabPos.y = edibleObject.transform.position.y;
                    status = GrabStatus.Reaching;
                    alert.Play(source1);
                    littleFellaAnimator.SetTrigger("Wake Up");
                }
                break;

        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != grabHand && other.gameObject != giftObject && other.gameObject.GetComponent<NewProp>())
        {
            if (edibleObject == null)
            {
                edibleObject = other.gameObject;
                dragGrabPos = other.transform.position;                
            }
            nearbyObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == edibleObject && status != GrabStatus.Taking)
        {
            edibleObject = null;
            moveProgress = 0.0f;
            status = GrabStatus.Withdrawing;
        }

        if (nearbyObjects.Contains(other.gameObject))
        {
            nearbyObjects.Remove(other.gameObject);
        }
    }

    private void GiveGift()
    {
        // Create the gift
        giftObject = Instantiate(giftPrefab, dragHomePos, Quaternion.Euler(0,90,0));
        moveProgress = 0.0f;
        status = GrabStatus.Gifting;
    }
}
