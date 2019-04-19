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
    [SerializeField]
    private GameObject EvilMiko;
    [SerializeField]
    private GameObject Credits;
    private Dialogue dialogue;
    private GameObject player;

    public bool TownDestroyed;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        TownDestroyed = false;
        dialogue = gameObject.GetComponent<Dialogue>();

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            player = GameObject.Find("PlayerE(Clone)");

    }

    public void MidBossKilled()
    {
        City.SetActive(false);
        RuinCity.SetActive(true);
        if (DuskOverlay != null) DuskOverlay.SetActive(true);
        TownDestroyed = true;
    }

    public void TeleportToMaouCity()
    {
        if (player == null)
        {
            Debug.LogError("Error when getting player object");
            return;
        }
        StartCoroutine(TeleportEffects());
    }

    public void BeatDevil()
    {
        EvilMiko.SetActive(true);
    }

    public void HitByEvilMiko()
    {
        StartCoroutine(BeingHit());
    }

    private IEnumerator BeingHit()
    {
        TeleportOverlay.SetActive(true);
        Image FlashImage = TeleportOverlay.GetComponent<Image>();
        float a = 0;
        while (a < 1)
        {
            a += (Time.deltaTime*2);
            FlashImage.color = new Color(1, 1, 1, a);
            yield return null;
        }

        while (a > 0)
        {
            a -= (Time.deltaTime);
            FlashImage.color = new Color(a, a, a, 1);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        FlashImage.color = new Color(1, 1, 1, 1);
        AudioManager.Instance.PlaySoundEffect("Punch");
        yield return new WaitForSeconds(0.2f);
        FlashImage.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(0.5f);



        if (dialogue.StartDialog(player.GetComponent<PlayerExplorationController>()))
            player.GetComponent<PlayerExplorationController>().CurrentState = PlayerExplorationState.InTalking;
    }

    public void RemoveMiko()
    {
        EvilMiko.SetActive(false);

        StartCoroutine(MoveCredits());
    }

    private IEnumerator MoveCredits()
    {
        Credits.SetActive(true);
        MusicManager.Instance.PlayMusic("town");
        MusicManager.Instance.Close();
        while (true)
        {
            Credits.transform.position += Time.deltaTime * Vector3.up * 100;
            yield return null;
        }
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
