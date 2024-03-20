using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TN_Splat : MonoBehaviour
{
    Renderer renderer;
    Color splatColor;
    float transparency;

    [SerializeField]
    private float timeBeforeFade = 0;
    [SerializeField]
    private float fadeTime = 1;
    [SerializeField]
    private float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        transparency = fadeTime + timeBeforeFade;
        renderer = GetComponent<Renderer>();
        splatColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (transparency <= 0)
        {
            Destroy(this.gameObject);
        }
        
        splatColor.a = math.clamp(transparency, 0.0f, 1.0f);
        renderer.material.color = splatColor;

        transparency -= Time.deltaTime * speed;
    }
}
