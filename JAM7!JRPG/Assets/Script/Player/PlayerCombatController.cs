using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum StatsType : int
{
    // may move this part to stats manager or game manager
    Power,
    Dexterity,
    Wisdom
}

public enum HeroType : int
{
    // may move this part to stats manager or game manager
    Knight,
    Nurse,
    Fat
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
    [SerializeField] private float walkSpeed;
    [SerializeField] private int jumpPower;
    [SerializeField] private int maxMagazine;
    [SerializeField] private int maxHp;

    public int Power
    {
        get
        {
            return Player.GetPlayer(PlayerID).Power;
        }
    }

    public int Dexterity
    {
        get
        {
            return Player.GetPlayer(PlayerID).Dexterity;
        }
    }

    public int Wisdom
    {
        get
        {
            return Player.GetPlayer(PlayerID).Wisdom;
        }
    }

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

    private float invulnerableInterval = 0.3f;
    private float lastHit;

    [SerializeField] private HeroType type;

    [SerializeField] private float coolDown;
    private float lastAbility;

    [SerializeField] private GameObject shield;

    public CombatManager Combat;
    private bool inAbility = false;

    public EventOnDataChange2<int> OnHpChange { get; private set; }
    public EventOnDataChange1<int> OnMagazineUpdate { get; private set; }

    public GameObject cam;

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

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        hp = maxHp;

        CurrentState = PlayerCombatState.OnGround;
    }

    public void GetCamera()
    {
        var cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (var camera in cameras)
        {
            if (camera.GetComponent<ForwardCamera>().index == PlayerID)
            {
                cam = camera;
            }
        }
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

                    if (Input.GetButtonDown("Pick" + PlayerID) && !inAbility)
                        GetItem();

                    if (Input.GetButtonDown("Ability" + PlayerID) && lastAbility + coolDown * (1 - Wisdom * 0.1f) < Time.unscaledTime) {
                        Ability();
                        var doors = GameObject.FindGameObjectsWithTag("Door");
                        foreach (var door in doors) {
                            if ((door.transform.position - transform.position).sqrMagnitude < 0.1f && door.activeInHierarchy) {
                                Combat.endCombat();
                                Combat = null;
                            }
                        }
                    }
                        
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

    public void Hurt()
    {
        if (lastHit + invulnerableInterval < Time.unscaledTime)
        {
            //Hp--;
            //todo fix hp add
            if (Hp < 0)
            {
                //dead
            }
            else
            {
                StartCoroutine(HurtDelay());
            }

            lastHit = Time.unscaledTime;
        }

    }


    private void Ability()
    {
        lastAbility = Time.unscaledTime;
        inAbility = true;
        switch (type)
        {
            case HeroType.Knight:
                float temp = gunHolder.GetComponentInChildren<Gun>().reloadSpeed;

                StartCoroutine(resetReloadDelay(temp));
                break;
            case HeroType.Nurse:
                GunManager._instance.generateHealDrop(transform);
                inAbility = false;
                break;
            case HeroType.Fat:
                shield.SetActive(true);
                StartCoroutine(resetShieldDelay());
                break;
        }
    }

    private void GetItem()
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
                }
                else if (item.getType() == ItemTag.Heal)
                {
                    //Hp++;
                    item.GetComponent<Item>().Trigger(this);
                    //TODO do UI update
                }
            }
        }
    }

    IEnumerator resetReloadDelay(float val)
    {
        //simple animation
        gunHolder.GetComponentInChildren<Gun>().reloadSpeed = 0;
        gunHolder.GetComponentInChildren<Gun>().fireRate *= 0.5f;
        yield return new WaitForSeconds(4f);
        gunHolder.GetComponentInChildren<Gun>().reloadSpeed = val;
        gunHolder.GetComponentInChildren<Gun>().fireRate /= 0.5f;
        inAbility = false;
    }
    IEnumerator resetShieldDelay()
    {
        //simple animation
        yield return new WaitForSeconds(6f);
        shield.SetActive(false);
        inAbility = false;
    }

    IEnumerator HurtDelay()
    {
        //simple animation
        anim.SetBool("Hurt", true);
        renderer.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        renderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        renderer.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        renderer.color = Color.white;
        anim.SetBool("Hurt", false);
    }
}
