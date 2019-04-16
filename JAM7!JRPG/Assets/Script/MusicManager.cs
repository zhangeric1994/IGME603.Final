using UnityEngine;

public class MusicManager : MonoBehaviour
{
    static public MusicManager Instance;
    private string lastMusic;
    private string currentMusic;
    private AudioSource source;
    private bool stopped;
    public bool finalBoss;
    public bool inBattle
    {
        set
        {
            if (value)
            {
                lastMusic = currentMusic;
                PlayMusic(finalBoss ? "FinalBossBattle" : "battle");
            }
            else
            {
                PlayMusic(lastMusic);
            }
        }
    }
    private void Awake()
    {
        Instance = this;
        stopped = false;
        finalBoss = false;
    }
    public void Close()
    {
        stopped = true;
    }
    public void PlayMusic(string music)
    {
        if (music == currentMusic) return;
        if (stopped) return;
        Debug.Log("Play " + music);
        source.Stop();
        source.clip = Resources.Load<AudioClip>("Audio/" + music);
        source.Play();
        currentMusic = music;
    }
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        Resources.Load<AudioClip>("Audio/town");
        Resources.Load<AudioClip>("Audio/field");
        Resources.Load<AudioClip>("Audio/battle");
        Resources.Load<AudioClip>("Audio/AnotherWorldP");
        Resources.Load<AudioClip>("Audio/RuinTown");
        Resources.Load<AudioClip>("Audio/FinalBossBattle");
        PlayMusic("town");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
