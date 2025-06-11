using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastRunner : MonoBehaviour
{
    /// <summary>
    /// Is this minigame active?
    /// </summary>
    [SerializeField] private bool active = true;

    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private ObstacleManager obstacleManager;

    // Start is called before the first frame update
    void Start()
    {
        obstacleManager.active = active;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            player.Jump();
        }
    }
}
