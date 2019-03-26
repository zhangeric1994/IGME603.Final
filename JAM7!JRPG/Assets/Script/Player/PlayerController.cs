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
    [SerializeField] private int walkSpeed;
    [SerializeField] private int jumpPower;
    [SerializeField] private int maxMagazine;
    [SerializeField] private int maxHp;

    [SerializeField] private int power;
    [SerializeField] private int accuracy;
    [SerializeField] private int defense;
    [SerializeField] private int damage;
    
    public Transform gunHolder;

    private int id;
    private PlayerState currentState;

    private Vector2 aimmingDirection;

    private SpriteRenderer renderer;
    private Rigidbody2D rb2d;
    private Animator anim;

    private bool isInAir;

    private int hp;
    private int magazine;

    public EventOnDataChange<int> OnHpChange { get; private set; }
    public EventOnDataUpdate<int> OnMagazineUpdate { get; private set; }

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

                Debug.Log(LogUtility.MakeLogStringFormat("PlayerController", "{0} --> {1}", previousState, currentState));

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

    public int Hp
    {
        get
        {
            return hp;
        }

        private set
        {
            if (value != hp)
            {
                hp = value;

                OnHpChange.Invoke(hp, maxHp);
            }
        }
    }

    public int MaxHp
    {
        get
        {
            return maxHp;
        }
    }

    public int Magazine
    {
        get
        {
            return magazine;
        }

        private set
        {
            if (value != magazine)
            {
                magazine = value;

                OnMagazineUpdate.Invoke(magazine);
            }
        }
    }

    public void TeleportTo(Vector3 pos)
    {
        transform.position = pos;
    }
    
    public Vector2 GetAllignment()
    {
        return aimmingDirection;
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
                maxHp = overwrite?maxHp+value:value;
                break;

            case StatsType.Defense:
                defense = overwrite?defense+value:value;
                break;

            case StatsType.Power:
                power = overwrite?power+value:value;
                break;
        }
    }

    private void Awake()
    {
        OnHpChange = new EventOnDataChange<int>();
        OnMagazineUpdate = new EventOnDataUpdate<int>();
    }

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        hp = maxHp;

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

                    if (x != 0 || y != 0)
                    {
                        aimmingDirection = new Vector2(x, Mathf.Clamp01(y)).normalized;
                        gunHolder.right = aimmingDirection;
                    }
                    //anim.SetFloat("Speed",Mathf.Abs(h)+Mathf.Abs(v));

                    if (x > 0)
                    {
                        rb2d.velocity = new Vector2(walkSpeed, rb2d.velocity.y);
                        renderer.flipX = false;
                        gunHolder.GetComponentInChildren<SpriteRenderer>().flipY = false;
                    }
                    else if (x < 0)
                    {
                        rb2d.velocity = new Vector2(-walkSpeed, rb2d.velocity.y);
                        renderer.flipX = true;
                        gunHolder.GetComponentInChildren<SpriteRenderer>().flipY = y != 0;
                    }
                    else
                        rb2d.velocity = new Vector2(0, rb2d.velocity.y);

                    if (Input.GetButtonDown("Jump" + Id))
                        CurrentState = PlayerState.InAir;

                    if (Input.GetButtonDown("Pick" + Id))
                        getItem();
                }
                break;

            case PlayerState.InAir:
                {
                    float x = Input.GetAxis("Horizontal" + Id);
                    float y = Input.GetAxis("Vertical" + Id);

                    if (x != 0 || y != 0)
                    {
                        aimmingDirection = new Vector2(x, Mathf.Clamp01(y)).normalized;
                        gunHolder.right = aimmingDirection;
                    }
                    //anim.SetFloat("Speed",Mathf.Abs(h)+Mathf.Abs(v));

                    if (x > 0)
                    {
                        rb2d.velocity = new Vector2(walkSpeed, rb2d.velocity.y);
                        renderer.flipX = false;
                        gunHolder.GetComponentInChildren<SpriteRenderer>().flipY = false;
                        //gunHolder.localScale = new Vector3(1.0f, 1.0f, 0.0f);
                    }
                    else if (x < 0)
                    {
                        rb2d.velocity = new Vector2(-walkSpeed, rb2d.velocity.y);
                        renderer.flipX = true;
                        gunHolder.GetComponentInChildren<SpriteRenderer>().flipY = y != 0;
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

    private void getItem()
    {
        var items = FindObjectsOfType<Item>();
        foreach (var item in items)
        {
                float distanceToItem = (item.transform.position - transform.position).sqrMagnitude;
                if (distanceToItem <= 0.1f && item.gameObject.activeInHierarchy)
                {
                    if (item.getType() == ItemTag.Weapon)
                    {
                        gunHolder.GetComponentInChildren<Gun>().Destroy();
                        item.GetComponent<Item>().Trigger(this);
                    }else if (item.getType() == ItemTag.Heal && Hp < maxHp)
                    {
                        Hp++;
                        //TODO do UI update 
                    }
                }
        }
    }
}
