using UnityEngine;

public class TN_GoUp : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [SerializeField]
    private Vector3 vel = new Vector3(0, 1, 0);

    // ------------------------------- Functions -------------------------------
    // Update is called once per frame
    void Update()
    {
        transform.Translate(vel * Time.deltaTime);
    }
}
