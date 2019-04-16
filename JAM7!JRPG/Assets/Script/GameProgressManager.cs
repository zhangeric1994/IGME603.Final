using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager instance;
    [SerializeField]
    private GameObject City;
    [SerializeField]
    private GameObject RuinCity;
    [SerializeField]
    private GameObject DuskOverlay;
    [SerializeField]
    private Transform TeleportationPoint;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MidBossKilled()
    {
        City.SetActive(false);
        RuinCity.SetActive(true);
        DuskOverlay.SetActive(true);
    }

    public void TeleportToMaouCity()
    {
        GameObject player = GameObject.Find("PlayerE(Clone)");
        if (player == null)
        {
            Debug.LogError("Error when getting player object");
            return;
        }
        DuskOverlay.SetActive(false);

        player.transform.position = TeleportationPoint.position;
    }
}
