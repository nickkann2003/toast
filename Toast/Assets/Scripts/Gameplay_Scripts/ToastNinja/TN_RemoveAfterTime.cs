using UnityEngine;

public class TN_RemoveAfterTime : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [SerializeField]
    private float timer = .5f;

    // ------------------------------- Functions -------------------------------
    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }

        timer -= Time.deltaTime;
    }
}
