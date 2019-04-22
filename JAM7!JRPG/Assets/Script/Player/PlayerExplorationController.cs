using System;
using UnityEngine;

public enum PlayerExplorationState
{
    Default = 0,
    Exploring = 1,
    InMenu = 2,
    InCombat = 3,
    InTalking = 4,
}

public enum Heading
{
    Up,
    Down,
    Left,
    Right,
}


public class PlayerExplorationController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] float walkSpeed = 1;

    public int PlayerID { get; private set; }

    private PlayerExplorationState currentState;
    private Animator animator;

    public GameObject cam;

    private Heading heading;
    FMODUnity.StudioEventEmitter emitter;
    FMODUnity.StudioEventEmitter PlayerFootstepEmitter;
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int Right = Animator.StringToHash("Right");
    private static readonly int Left = Animator.StringToHash("Left");
    private static readonly int Up = Animator.StringToHash("Up");
    private static readonly int Down = Animator.StringToHash("Down");

    public PlayerExplorationState CurrentState
    {
        // this allowed to triggger codes when the state switched
        get
        {
            return currentState;
        }

        set
        {
            if (value == currentState)
            {
                // nothing
            }
            else
            {
                switch (currentState)
                {
                    //    case PlayerExplorationState.InMenu:
                    //        GUIManager.Singleton.Close("IngameMenu");
                    //        break;


                    case PlayerExplorationState.InCombat:
                        //HUD.Singleton.ShowExplorationUI(PlayerID);
                        //HUD.Singleton.HideCombatUI(PlayerID);
                        gameObject.SetActive(true);
                        cam.GetComponent<ForwardCamera>().enabled = false;
                        cam.GetComponent<OverworldCamera>().enabled = true;
                        GUIManager.Singleton.Close("CombatHUD");
                        break;
                }

                PlayerExplorationState previousState = currentState;
                currentState = value;

                Debug.Log(LogUtility.MakeLogStringFormat("PlayerExplorationController", "{0} --> {1}", previousState, currentState));

                switch (currentState)
                {
                    case PlayerExplorationState.InMenu:
                        //HUD.Singleton.ShowMenu(PlayerID);
                        animator.SetBool(Moving, false);
                        GUIManager.Singleton.Open("IngameMenu", (Action)ReturnToExploration);
                        break;


                    case PlayerExplorationState.InCombat:
                        //HUD.Singleton.HideExplorationUI(PlayerID);
                        //HUD.Singleton.ShowCombatUI(PlayerID);
                        gameObject.SetActive(false);
                        cam.GetComponent<ForwardCamera>().enabled = true;
                        cam.GetComponent<OverworldCamera>().enabled = false;
                        GUIManager.Singleton.Open("CombatHUD");
                        break;
                    
                    case PlayerExplorationState.InTalking:
                        animator.SetBool(Moving, false);
                        break;
                }
            }
        }
    }

    private PlayerExplorationController() { }

    public void Initialize(int id)
    {
        PlayerID = id;
    }

    public void StartCombat(EnemyProxy enemy, string desiredLevel)
    {
        CurrentState = PlayerExplorationState.InCombat;

        enemy.StartCombat(this, desiredLevel);
    }

    public void ReturnToExploration()
    {
        CurrentState = PlayerExplorationState.Exploring;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        Debug.Log("OnTriggerEnter2D, other: " + go.name);
        switch (go.tag)
        {
            case "Enemy":
                StartCombat(go.GetComponent<EnemyProxy>(), "NormalLevel");
                break;
            case "Boss":
                StartCombat(go.GetComponent<EnemyProxy>(), "BossLevel");
                break;
            case "FinalBoss":
                StartCombat(go.GetComponent<EnemyProxy>(), "FinalLevel");
                break;
            case "MusicCollider":
                if (PlayerID == 1) break;
                string[] splitName = go.name.Split('_');
                //MusicManager.Instance.PlayMusic(splitName[1]);
                GameObject another = go.GetComponent<mutual>().another;
                go.SetActive(false);
                another.SetActive(true);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.collider.gameObject;
        Debug.Log("OnCollisionEnter2D, other: " + go.name);
        switch (go.tag)
        {
            case "Enemy":
                StartCombat(go.GetComponent<EnemyProxy>(), "NormalLevel");
                break;
            case "Boss":
                StartCombat(go.GetComponent<EnemyProxy>(), "BossLevel");
                break;
            case "FinalBoss":
                StartCombat(go.GetComponent<EnemyProxy>(), "FinalLevel");
                break;
            case "MusicCollider":
                if (PlayerID == 1) break;
                string[] splitName = go.name.Split('_');
                //MusicManager.Instance.PlayMusic(splitName[1]);
                GameObject another = go.GetComponent<mutual>().another;
                go.SetActive(false);
                another.SetActive(true);
                break;
        }
    }

    private void Start()
    {
        CurrentState = PlayerExplorationState.Exploring;
        PlayerFootstepEmitter = null;
        animator = GetComponent<Animator>();
    }

    public void GetCamera()
    {
        var cameras = GameObject.FindGameObjectsWithTag("MainCamera");

        if (cameras.Length != 0)
            cam = cameras[0];

    }

    private void Update()
    {
        if (cam == null)
        {
            GetCamera();
            return;
        }
        if(PlayerFootstepEmitter == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            PlayerFootstepEmitter = player.GetComponent<FMODUnity.StudioEventEmitter>();
        }
        switch (currentState)
        {
            case PlayerExplorationState.Exploring:
                if (Input.GetButtonDown("Start"))
                    CurrentState = PlayerExplorationState.InMenu;
                else
                {
                    float horizontal = Input.GetAxisRaw("Horizontal");
                    float vertical = Input.GetAxisRaw("Vertical");
                    animator.ResetTrigger("Right");
                    animator.ResetTrigger("Left");
                    animator.ResetTrigger("Up");
                    animator.ResetTrigger("Down");
                    if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
                    {
                        vertical = 0;
                        if (horizontal > 0)
                        {
                            animator.SetTrigger(Right);
                            PlayerFootstepEmitter.SetParameter("Speed", 1);
                            heading = Heading.Right;
                        }
                        else if (horizontal < 0)
                        {
                            animator.SetTrigger(Left);
                            PlayerFootstepEmitter.SetParameter("Speed", 1);
                            heading = Heading.Left;
                        }
                        else
                        {
                            PlayerFootstepEmitter.SetParameter("Speed", 0);
                            PlayerFootstepEmitter.SetParameter("Grass", 0);
                        }
                    }
                    else
                    {
                        horizontal = 0;
                        if (vertical > 0)
                        {
                            animator.SetTrigger(Up);
                            PlayerFootstepEmitter.SetParameter("Speed", 1);
                            heading = Heading.Up;
                        }
                        else if (vertical < 0)
                        {
                            animator.SetTrigger(Down);
                            PlayerFootstepEmitter.SetParameter("Speed", 1);
                            heading = Heading.Down;
                        }
                        else
                        {
                            PlayerFootstepEmitter.SetParameter("Speed", 0);
                            PlayerFootstepEmitter.SetParameter("Grass", 0);
                        }

                    }
                    Vector3 move = Time.deltaTime * walkSpeed * new Vector3(horizontal, vertical, 0);
                    if (Input.GetKey(KeyCode.LeftShift)) move *= 2;
                    transform.Translate(move);
                    animator.SetBool(Moving, move.magnitude > 0.01f);
                }

                break;
        }



        if (transform.position.x > 50)
        {
            //MusicManager.Instance.PlayMusic("field");
            emitter.SetParameter("Forest", 1);
            emitter.SetParameter("NormalTown", 0);
            emitter.SetParameter("RuinedTown", 0);
        }
        else if (transform.position.x < -50)
        {
            emitter.SetParameter("DarkDimension", 1);
            emitter.SetParameter("NormalTown", 0);
            emitter.SetParameter("RuinedTown", 0);
            //MusicManager.Instance.PlayMusic("AnotherWorldP");
        }
        else
        {
            if (GameProgressManager.instance.TownDestroyed)
            {
                //MusicManager.Instance.PlayMusic("RuinTown");
                var target = GameObject.Find("Fire");
                FMODUnity.StudioEventEmitter fire = target.GetComponent<FMODUnity.StudioEventEmitter>();
                fire.SetParameter("MixFire", 1);
                emitter.SetParameter("RuinedTown", 1);
                emitter.SetParameter("Forest", 0);
                
            }
            else
            {
                emitter.SetParameter("NormalTown", 1);
                emitter.SetParameter("Forest", 0);
            }
        }
    }

    private void LateUpdate()
    {
        switch (currentState)
        {
            case PlayerExplorationState.Exploring:
                if (Input.GetButtonDown("Submit"))
                {
                    Vector2 direction = Vector2.zero;
                    switch (heading)
                    {
                        case Heading.Down:
                            direction = Vector2.down;
                            break;
                        case Heading.Left:
                            direction = Vector2.left;
                            break;
                        case Heading.Right:
                            direction = Vector2.right;
                            break;
                        case Heading.Up:
                            direction = Vector2.up;
                            break;
                    }
                    RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, direction, 2f);
                    Dialogue dialogue = null;
                    if (hit)
                    {
                        dialogue = hit.collider.gameObject.GetComponent<Dialogue>();
                    }

                    if (dialogue != null)
                    {
                        if (dialogue.StartDialog(this))
                        {
                            animator.SetBool(Moving, false);
                            currentState = PlayerExplorationState.InTalking;
                        }

                        Loot loot = dialogue.GetComponent<Loot>();
                        if (loot && !loot.triggered)
                        {
                            Player.GetPlayer(0).Loot(loot);
                            loot.triggered = true;

                            dialogue.GetComponent<ChestAnimation>().SetChestStat(true);

                        }
                    }
                }
                break;
        }
    }
    void OnEnable()
    {
        var target = GameObject.Find("BackgroundMusic");
        emitter = target.GetComponent<FMODUnity.StudioEventEmitter>();
    }
}
