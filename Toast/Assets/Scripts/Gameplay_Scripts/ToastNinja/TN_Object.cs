using TMPro;
using UnityEngine;

public class TN_Object : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("Variables")]
    [SerializeField]
    float points;
    [SerializeField]
    GameObject splatter;
    [SerializeField]
    GameObject pointObject;

    [SerializeField]
    GameObject onDestroyParticles;

    [Header("Event References")]
    [SerializeField]
    private PropIntGameEvent toastNinjaScoreEvent;

    private Vector3 startPosition;



    [SerializeField]
    private float hitsToDestroy = 1;


    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void OnEnable()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Use for TNObject destroys the object and grants points
    public void Use()
    {
        hitsToDestroy--;
        if (hitsToDestroy != 0)
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

        

        if (onDestroyParticles != null)
        {
            GameObject particleObj = Instantiate(onDestroyParticles);
            particleObj.transform.position = this.transform.position;
        }

        Destroy(this.gameObject);
    }

    public void Slice(Vector3 hitDirection, float speed = 1)
    {
        speed = speed / 2f;
        speed = Mathf.Clamp(speed, 2f, 6f);
        Debug.Log(speed);

        hitDirection.z = 0;
        hitDirection.y += .2f * hitDirection.y;
        //hitDirection = Vector3.Normalize(hitDirection);

        GetComponent<Rigidbody>().AddForce(Vector3.Normalize(hitDirection) * speed * 100);
        GetComponent<Rigidbody>().AddTorque(new Vector3(0 , 0, speed) * 200 * -hitDirection.x/Mathf.Abs(hitDirection.x));

        SpawnPoints();

        points++;
    }

    public void SpawnPoints()
    {
        GameObject pointsObj = Instantiate(pointObject, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), Quaternion.identity);
        pointsObj.GetComponent<TextMeshPro>().color = this.GetComponent<Renderer>().material.color;
        pointsObj.GetComponent<TextMeshPro>().text = "";
        if (points >= 0)
        {
            pointsObj.GetComponent<TextMeshPro>().text += "+";
        }
        pointsObj.GetComponent<TextMeshPro>().text += points;
        pointsObj.transform.localScale = Vector3.one * .4f * Mathf.Abs(points);

        toastNinjaScoreEvent.RaiseEvent(gameObject.GetComponent<NewProp>(), (int)points);
    }

    public void Goodbye()
    {
        Destroy(this.gameObject);
    }
}
