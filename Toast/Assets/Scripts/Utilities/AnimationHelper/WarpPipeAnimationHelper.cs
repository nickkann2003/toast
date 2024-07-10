using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarpPipeAnimationHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject animationEncaps;

    [SerializeField]
    private Collider entranceCollider;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject objectAnimating;
    private GameObject copyObject;
    
    [SerializeField]
    private float lerpTime;
    private Vector3 lerpStart;
    private float lerpProgress;

    public GameObject ObjectAnimating { get => objectAnimating; set => objectAnimating = value; }


    // Start is called before the first frame update
    void Start()
    {
        animator.speed = 1.0f / lerpTime;
    }

    // Update is called once per frame
    void Update()
    {
        lerpProgress += Time.deltaTime;
        if(copyObject != null)
        {
            copyObject.transform.localPosition = Vector3.Lerp(lerpStart, Vector3.zero, lerpProgress / lerpTime);
        }
    }

    public void AnimateIn()
    {
        copyObject = Instantiate(objectAnimating);
        copyObject.transform.position = objectAnimating.transform.position;
        lerpStart = animationEncaps.transform.InverseTransformPoint(objectAnimating.transform.position);    
        CleanCopyObject();
        objectAnimating.SetActive(false);
        copyObject.transform.SetParent(animationEncaps.transform, true);
        lerpProgress = 0;
        entranceCollider.enabled = false;
    }

    public void AnimateOut()
    {
        objectAnimating.transform.position = copyObject.transform.position;
        objectAnimating.SetActive(true);
        foreach (Transform child in animationEncaps.transform)
        {
            Destroy(child.gameObject);
        }
        animator.ResetTrigger("StartAnimationIn");
        objectAnimating = null;
        entranceCollider.enabled = true;
    }

    private void CleanCopyObject()
    {
        Transform objTransform = copyObject.transform;
        MeshFilter meshFilter = copyObject.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = copyObject.GetComponent<MeshRenderer>();
        foreach(Component c in copyObject.GetComponents<Component>())
        {
            if(c != objTransform && c != meshFilter && c != meshRenderer)
            {
                Destroy(c);
            }
        }
    }
}
