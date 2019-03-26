using System.Collections.Generic;
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

public enum PlayerCombatState
{
    Default = 0,
    OnGround,
    InAir,
}

public class PlayerCombatController : MonoBehaviour
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

    public int PlayerID { get; private set; }

    private PlayerCombatState currentState;

    private Vector2 aimmingDirection;

    private SpriteRenderer renderer;
    private Rigidbody2D rb2d;
    private Animator anim;

    private bool isInAir;

    private int hp;
    private int magazine;

    public EventOnDataChange2<int> OnHpChange { get; private set; }
    public EventOnDataChange1<int> OnMagazineUpdate { get; private set; }

    public PlayerCombatState CurrentState
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
                PlayerCombatState previousState = currentState;
                currentState = value;

                Debug.Log(LogUtility.MakeLogStringFormat("PlayerController", "{0} --> {1}", previousState, currentState));

                switch (currentState)
                {
                    case PlayerCombatState.OnGround:
                        isInAir = false;
                        break;

                    case PlayerCombatState.InAir:
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

                OnHpChange.Invoke(hp, MaxHp);
            }
        }
    }

    public int MaxHp
    {
        get
        {
            return Player.GetPlayer(PlayerID).MaxHp;
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

    private PlayerCombatController() { }

    public void Initialize(int id)
    {
        PlayerID = id;

        OnHpChange = new EventOnDataChange2<int>();
        OnMagazineUpdate = new EventOnDataChange1<int>();
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

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        hp = maxHp;

        CurrentState = PlayerCombatState.OnGround;
    }

    private void Update()
    {
        switch (currentState)
        {
            case PlayerCombatState.OnGround:
                {
                    float x = Input.GetAxis("Horizontal" + PlayerID);
                    float y = Input.GetAxis("Vertical" + PlayerID);

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

                    if (Input.GetButtonDown("Jump" + PlayerID))
                        CurrentState = PlayerCombatState.InAir;
                }
                break;


            case PlayerCombatState.InAir:
                {
                    float x = Input.GetAxis("Horizontal" + PlayerID);
                    float y = Input.GetAxis("Vertical" + PlayerID);

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
                        CurrentState = PlayerCombatState.OnGround;
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
