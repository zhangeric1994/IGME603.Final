using UnityEngine;

public class MusicManager : MonoBehaviour
{
    static public MusicManager Instance;
    private string lastMusic;
    private string currentMusic;
    private AudioSource source;
    public bool inBattle
    {
        set
        {
            if (value)
            {
                lastMusic = currentMusic;
                PlayMusic("battle");
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
    }
    public void PlayMusic(string music)
    {
        if (music == currentMusic) return;
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
