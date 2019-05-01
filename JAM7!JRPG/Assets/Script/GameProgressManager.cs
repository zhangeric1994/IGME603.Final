using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField]
    private GameObject Logos;
    [SerializeField]
    private GameObject ToBeContinued;
    [SerializeField]
    private GameObject Canvas;
    private Dialogue dialogue;
    private GameObject player;

    public bool TownDestroyed;
    private RectTransform creditsRectTransform;
    private RectTransform canvasRectTransform;
    private Image[] images;
    private Text toBeContinuedText;

    FMODUnity.StudioEventEmitter emitter;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        toBeContinuedText = ToBeContinued.GetComponent<Text>();
        images = Logos.transform.GetComponentsInChildren<Image>();
        canvasRectTransform = Canvas.GetComponent<RectTransform>();
        creditsRectTransform = Credits.GetComponent<RectTransform>();
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
        //AudioManager.Instance.PlaySoundEffect("Punch");
        FMOD.Studio.EventInstance punchSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Punch");
        punchSound.start();
        yield return new WaitForSeconds(0.2f);
        FlashImage.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(0.5f);



        if (dialogue.StartDialog(player.GetComponent<PlayerExplorationController>()))
        {
            player.GetComponent<PlayerExplorationController>().CurrentState = PlayerExplorationState.InTalking;
            FMOD.Studio.EventInstance jumpSound = FMODUnity.RuntimeManager.CreateInstance("event:/Dialogues/VO/You");
            jumpSound.start();
        }
        
    }

    public void RemoveMiko()
    {
        EvilMiko.SetActive(false);
        emitter.Stop();
        FMOD.Studio.EventInstance music = FMODUnity.RuntimeManager.CreateInstance("event:/BackgroundMusic");
        music.setParameterValue("NormalTown", 1f);
        music.start();
        StartCoroutine(MoveCredits());
    }

    private IEnumerator MoveCredits()
    {
        Credits.SetActive(true); 

        yield return new WaitForSeconds(0.5f);
        //MusicManager.Instance.PlayMusic("town");
        //MusicManager.Instance.Close();
        while (creditsRectTransform.anchoredPosition.y < 
               creditsRectTransform.sizeDelta.y * 2 + canvasRectTransform.sizeDelta.y)
        {
            Credits.transform.position += Time.deltaTime * Vector3.up * 100;
            yield return null;
        }
        
        Credits.SetActive(false);

        Logos.SetActive(true);
        float alpha = 0;

        while (alpha < 1.0f)
        {
            foreach (var image in images)
            {
                var color = image.color;
                color = new Color(color.r, color.g, color.b, alpha);
                image.color = color;
            }

            alpha += Time.deltaTime;
            yield return null;
        }

        alpha = 1.0f;
        
        yield return new WaitForSeconds(1.5f);
        
        while (alpha > 0.0f)
        {
            foreach (var image in images)
            {
                var color = image.color;
                color = new Color(color.r, color.g, color.b, alpha);
                image.color = color;
            }

            alpha -= Time.deltaTime;
            yield return null;
        }
        
        Logos.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        ToBeContinued.SetActive(true);
        alpha = 0;

        while (alpha < 1.0f)
        {
            var color = toBeContinuedText.color;
            color = new Color(color.r, color.g, color.b, alpha);
            toBeContinuedText.color = color;
                
            alpha += Time.deltaTime;
            yield return null;
        }

        alpha = 1.0f;
        
        yield return new WaitForSeconds(1.5f);
        
        while (alpha > 0.0f)
        {
            var color = toBeContinuedText.color;
            color = new Color(color.r, color.g, color.b, alpha);
            toBeContinuedText.color = color;

            alpha -= Time.deltaTime;
            yield return null;
        }
        ToBeContinued.SetActive(false);
        
        yield return null;
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

    void OnEnable()
    {
        var target = GameObject.Find("BackgroundMusic");
        emitter = target.GetComponent<FMODUnity.StudioEventEmitter>();
    }
}
