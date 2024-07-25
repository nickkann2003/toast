using TMPro;
using UnityEngine;

public class TN_Object : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("Variables")]
    [SerializeField]
    private TN_ItemScriptableObject _itemScriptableObject;

    [SerializeField]
    GameObject splatter;

    private Vector3 startPosition;

    private int hitsToDestroy = 0;
    private int hitsTaken = 0;

    [SerializeField]
    private RS_ToastNinja runtimeSet;

    private ToastNinjaScore toastNinjaScore;


    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void OnEnable()
    {
        startPosition = transform.position;
        hitsToDestroy = _itemScriptableObject.HitsToDestroy;
        toastNinjaScore = _itemScriptableObject.ToastNinjaScore;

        if (runtimeSet != null)
        {
            runtimeSet.Add(this.GetComponent<TN_Object>());
        }
    }

    // Use for TNObject destroys the object and grants points
    public void Use()
    {
        if (hitsToDestroy - hitsTaken != 0)
        {
            //Vector3 hitDirection = new Vector3(Random.Range(-.5f, .5f) * 2, 1, 0);
            //this.GetComponent<Rigidbody>().AddForce(hitDirection * 250);
            //Debug.Log("Hit");
            return;
        }

        AudioManager.instance.PlayOneShotSound(AudioManager.instance.eatingBread);

        //GameObject obj = Instantiate(splatter, transform.position, transform.rotation);
        //obj.GetComponent<Renderer>().material.color = this.GetComponent<Renderer>().material.color;
        //obj.transform.Rotate(new Vector3(0, 0, Random.Range(-30, 30)*2), Space.Self);

        _itemScriptableObject.SpawnDestroyParticles(this.transform.position);

        if (_itemScriptableObject.IsBomb)
        {
            toastNinjaScore.BombHit();
            runtimeSet.DestroyAll();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Slice(Vector3 hitPosition, Vector3 hitDirection, float speed = 1)
    {
        hitsTaken++;

        speed = speed / 2f;
        speed = Mathf.Clamp(speed, 3f, 6f);

        hitDirection.z = 0;
        hitDirection.y += .5f * hitDirection.y;
        //hitDirection = Vector3.Normalize(hitDirection);

        GetComponent<Rigidbody>().AddForce(Vector3.Normalize(hitDirection) * speed * 100);
        GetComponent<Rigidbody>().AddTorque(new Vector3(0 , 0, speed) * 200 * -hitDirection.x/Mathf.Abs(hitDirection.x));

        _itemScriptableObject.SpawnPoints(hitPosition, hitsTaken);
    }

    public void Goodbye()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (runtimeSet != null)
        {
            runtimeSet.Remove(this);
        }
    }
}
