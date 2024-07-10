using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPipeEntrance : MonoBehaviour
{
    [SerializeField]
    private Animator warpAnimation;

    [SerializeField]
    private WarpPipeAnimationHelper helper;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        NewProp prop = other.GetComponent<NewProp>();
        if(helper.ObjectAnimating == null)
        {
            if (prop != null)
            {
                if(Raycast.Instance.Dragging && Raycast.Instance.selectGO.Equals(other.gameObject))
                {
                    Raycast.Instance.StopDragging();
                }
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if(rb != null)
                {
                    rb.velocity = Vector3.zero;
                }
                helper.ObjectAnimating = other.gameObject;
                warpAnimation.SetTrigger("StartAnimationIn");
            }
        }
    }
}
