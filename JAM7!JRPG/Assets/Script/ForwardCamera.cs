using System.Collections;
using UnityEngine;

public class ForwardCamera : MonoBehaviour
{
    public int index;
    [SerializeField] private float smoothTimeY; // the smooth time for camera change the position on Y - axis, the larger number will slow the camera moving speed. 0 will be response instantly 
    [SerializeField] private float smoothTimeX; // the smooth time for camera change the position on X - axis, the larger number will slow the camera moving speed. 0 will be response instantly 
    private Vector2 velocity; // the speed reference for camera

    //Shaking related
    private bool shaking;
    private float shakeMagnitude; // currently the smallest magnitude it allowed is 0.01f;
    private bool refeshing;

    [SerializeField] private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.GetComponent<PlayerCombatController>() != null)
                if (player.GetComponent<PlayerCombatController>().PlayerID == index)
                {
                    target = player.transform;
                }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Initialize();
            return;
        }
        float x = 0;
        float y = 0;
        if (shaking)
        {
            x = Random.Range(-1f, 1f) * shakeMagnitude;
            y = Random.Range(-1f, 1f) * shakeMagnitude;
        }

        float posx = Mathf.SmoothDamp(transform.position.x, target.position.x, ref velocity.x, smoothTimeX);
        float posy = Mathf.SmoothDamp(transform.position.y, target.position.y + 0.7f, ref velocity.y, smoothTimeY);
        transform.position = new Vector3(posx + x, posy + y, transform.position.z);
    }

    public void Shaking(float strength, float duration)
    {
        if (!shaking)
        {
            refeshing = false;
            shaking = true;
            shakeMagnitude = strength;
            StartCoroutine(shakeRelease(duration));
        }
        else
        {
            shakeMagnitude = strength;
            refeshing = true; // refresh current Coroutine
        }
    }

    IEnumerator shakeRelease(float duration)
    {
        int counter = 0;
        while (counter * 0.01f < duration)
        {
            if (refeshing)
            {
                //get refreshed
                counter = 0;
                refeshing = false;
            }
            yield return new WaitForSeconds(0.01f);
            counter++;
        }
        shaking = false;
        yield return null;
    }
}
