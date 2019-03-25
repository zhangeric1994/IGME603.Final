using UnityEngine;
using UnityEngine.Tilemaps;

public enum StatsType : int
{
    // may move this part to stats manager or game manager
    Health,
    Power,
    Accuracy,
    Defense,
    Damage
}

public enum PlayerState
{
    Default = 0,
    OnGround,
    InAir,
}

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private int maxHealth;
    [SerializeField] private int power;
    [SerializeField] private int accuracy;
    [SerializeField] private int defense;
    [SerializeField] private int damage;

    private int id;
    private PlayerState currentState;

    private Vector2 aimmingDirection;

    private SpriteRenderer renderer;
    private Rigidbody2D rb2d;
    private Animator anim;

    private bool isInAir;

    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            if (id < 0)
                id = value;
        }
    }

    public PlayerState CurrentState
    {
        // this allowed to triggger codes when the state switched
        get
        {
            return currentState;
        }

        private set
        {
            if (value == currentState)
            {
                // nothing
            }
            else
            {
                PlayerState previousState = currentState;
                currentState = value;

                Debug.LogFormat("[Player] {0} --> {1}", previousState, currentState);

                switch (currentState)
                {
                    case PlayerState.OnGround:
                        isInAir = false;
                        break;

                    case PlayerState.InAir:
                        rb2d.AddForce(jumpPower * Vector2.up);
                        break;
                }
            }
        }
    }
    
    public void TeleportTo(Vector3 pos)
    {
        transform.position = pos;
    }
    
    public void SetStats(int value, StatsType type, bool overwrite = false)
    {
        // overwrite current Stats in that type
        switch (type)
        {
            case StatsType.Accuracy:
                accuracy = overwrite?accuracy+value:value;
                break;

            case StatsType.Damage:
                damage = overwrite?damage+value:value;
                break;

            case StatsType.Health:
                maxHealth = overwrite?maxHealth+value:value;
                break;

            case StatsType.Defense:
                defense = overwrite?defense+value:value;
                break;

            case StatsType.Power:
                power = overwrite?power+value:value;
                break;
        }
    }

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        CurrentState = PlayerState.OnGround;
    }

    private void Update()
    {
        switch (currentState)
        {
            case PlayerState.OnGround:
                {
                    float x = Input.GetAxis("Horizontal" + Id);
                    float y = Input.GetAxis("Vertical" + Id);
            
                    if (x != 0 && y != 0)
                        aimmingDirection = new Vector2(x, Mathf.Clamp01(y)).normalized;

                    //anim.SetFloat("Speed",Mathf.Abs(h)+Mathf.Abs(v));

                    if (x > 0)
                    {
                        rb2d.velocity = new Vector2(walkSpeed, rb2d.velocity.y);
                        renderer.flipX = false;
                    }
                    else if (x < 0)
                    {
                        rb2d.velocity = new Vector2(-walkSpeed, rb2d.velocity.y);
                        renderer.flipX = true;
                    }
                    else
                        rb2d.velocity = new Vector2(0, rb2d.velocity.y);

                    if (Input.GetButtonDown("Jump" + Id))
                        CurrentState = PlayerState.InAir;
                }
                break;

            case PlayerState.InAir:
                {
                    float x = Input.GetAxis("Horizontal" + Id);
                    float y = Input.GetAxis("Vertical" + Id);

                    if (x != 0 && y != 0)
                        aimmingDirection = new Vector2(x, y).normalized;

                    //anim.SetFloat("Speed",Mathf.Abs(h)+Mathf.Abs(v));

                    if (x > 0)
                    {
                        rb2d.velocity = new Vector2(walkSpeed, rb2d.velocity.y);
                        renderer.flipX = false;
                    }
                    else if (x < 0)
                    {
                        rb2d.velocity = new Vector2(-walkSpeed, rb2d.velocity.y);
                        renderer.flipX = true;
                    }
                    else
                        rb2d.velocity = new Vector2(0, rb2d.velocity.y);

                    if (isInAir && rb2d.IsTouching(GameObject.FindGameObjectWithTag("Ground").GetComponent<TilemapCollider2D>()))
                        CurrentState = PlayerState.OnGround;
                    else
                        isInAir = true;
                }
                break;
        }
    }
}
