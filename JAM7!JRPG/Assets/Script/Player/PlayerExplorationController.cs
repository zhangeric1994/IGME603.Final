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
                    case PlayerExplorationState.InMenu:
                        HUD.Singleton.HideMenu(PlayerID);
                        break;


                    case PlayerExplorationState.InCombat:
                        HUD.Singleton.ShowExplorationUI(PlayerID);
                        HUD.Singleton.HideCombatUI(PlayerID);
                        gameObject.SetActive(true);
                        cam.GetComponent<ForwardCamera>().enabled = false;
                        cam.GetComponent<OverworldCamera>().enabled = true;
                        break;
                }

                PlayerExplorationState previousState = currentState;
                currentState = value;

                Debug.Log(LogUtility.MakeLogStringFormat("PlayerExplorationController", "{0} --> {1}", previousState, currentState));

                switch (currentState)
                {
                    case PlayerExplorationState.InMenu:
                        HUD.Singleton.ShowMenu(PlayerID);
                        break;


                    case PlayerExplorationState.InCombat:
                        HUD.Singleton.HideExplorationUI(PlayerID);
                        HUD.Singleton.ShowCombatUI(PlayerID);
                        gameObject.SetActive(false);
                        cam.GetComponent<ForwardCamera>().enabled = true;
                        cam.GetComponent<OverworldCamera>().enabled = false;
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

    public void StartCombat(EnemyProxy enemy, bool isBoss = false)
    {
        CurrentState = PlayerExplorationState.InCombat;

        enemy.StartCombat(this, isBoss);
    }

    public void EndCombat()
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
                StartCombat(go.GetComponent<EnemyProxy>());
                break;
            case "Boss":
                StartCombat(go.GetComponent<EnemyProxy>(), true);
                break;
            case "MusicCollider":
                if (PlayerID == 1) break;
                string[] splitName = go.name.Split('_');
                MusicManager.Instance.PlayMusic(splitName[1]);
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
                StartCombat(go.GetComponent<EnemyProxy>());
                break;
            case "Boss":
                StartCombat(go.GetComponent<EnemyProxy>(), true);
                break;
            case "MusicCollider":
                if (PlayerID == 1) break;
                string[] splitName = go.name.Split('_');
                MusicManager.Instance.PlayMusic(splitName[1]);
                GameObject another = go.GetComponent<mutual>().another;
                go.SetActive(false);
                another.SetActive(true);
                break;
        }
    }

    private void Start()
    {
        CurrentState = PlayerExplorationState.Exploring;
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
        switch (currentState)
        {
            case PlayerExplorationState.Exploring:
                if (Input.GetButtonDown("Start" + PlayerID))
                    CurrentState = PlayerExplorationState.InMenu;
                else if (Input.GetButtonDown("Submit"))
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
                            currentState = PlayerExplorationState.InTalking;
                    }
                }
                else
                {
                    float horizontal = Input.GetAxisRaw("Horizontal" + PlayerID);
                    float vertical = Input.GetAxisRaw("Vertical" + PlayerID);
                    animator.ResetTrigger("Right");
                    animator.ResetTrigger("Left");
                    animator.ResetTrigger("Up");
                    animator.ResetTrigger("Down");
                    if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
                    {
                        vertical = 0;
                        if (horizontal > 0)
                        {
                            animator.SetTrigger("Right");
                            heading = Heading.Right;
                        }
                        else if (horizontal < 0)
                        {
                            animator.SetTrigger("Left");
                            heading = Heading.Left;
                        }
                    }
                    else
                    {
                        horizontal = 0;
                        if (vertical > 0)
                        {
                            animator.SetTrigger("Up");
                            heading = Heading.Up;
                        }
                        else if (vertical < 0)
                        {
                            animator.SetTrigger("Down");
                            heading = Heading.Down;
                        }
                    }
                    Vector3 move = Time.deltaTime * walkSpeed * new Vector3(horizontal, vertical, 0);
                    if (Input.GetKey(KeyCode.LeftShift)) move *= 2;
                    transform.Translate(move);
                    animator.SetBool("Moving", move.magnitude > 0.01f);
                }

                break;
        }
    }

}
