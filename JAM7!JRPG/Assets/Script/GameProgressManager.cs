using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    private GameObject TeleportOverlay;
    [SerializeField]
    private Transform TeleportationPoint;

    private GameObject player;
    

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
        player = GameObject.Find("PlayerE(Clone)");
        if (player == null)
        {
            Debug.LogError("Error when getting player object");
            return;
        }
        StartCoroutine(TeleportEffects());
    }

    private IEnumerator TeleportEffects()
    {
        TeleportOverlay.SetActive(true);
        Image FlashImage = TeleportOverlay.GetComponent<Image>();
        FlashImage.color = new Color(1, 1, 1, 0.7f);
        yield return new WaitForSeconds(0.2f);
        FlashImage.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.2f);
        FlashImage.color = new Color(1, 1, 1, 0.7f);
        yield return new WaitForSeconds(0.2f);
        FlashImage.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.2f);

        float a = 0;

        while (a < 1)
        {
            a += Time.deltaTime;
            FlashImage.color = new Color(1, 1, 1, a);
            yield return null;
        }
        DuskOverlay.SetActive(false);
        player.transform.position = TeleportationPoint.position;
        while (a > 0)
        {
            a -= Time.deltaTime;
            FlashImage.color = new Color(1, 1, 1, a);
            yield return null;
        }
        TeleportOverlay.SetActive(false);
        yield return null;
        


    }
}
