using UnityEngine;

public class OverworldCamera : MonoBehaviour
{
    public int index;
    [SerializeField] private Transform target;
    [SerializeField]
    float camMin;
    [SerializeField]
    float camMax;
    Camera cam;

    private float size;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        size = cam.orthographicSize;
        Initialize();
    }

    void Initialize()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.GetComponent<PlayerExplorationController>() != null)
            {
                target = player.transform;
            }
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
        {
            Initialize();
            return;
        }

        var center = target.position;

        center.z = -10.0f;
        cam.orthographicSize = size;
        transform.position = center;

    }
}
