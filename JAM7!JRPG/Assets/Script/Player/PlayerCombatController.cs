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
    [SerializeField] private int maxHp;

    public Transform weaponHolder;

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

    public bool okToAttack;

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
        okToAttack = true;
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
                    float x = Input.GetAxis("Horizontal");
                    float y = Input.GetAxis("Vertical");

                    if (okToAttack && x != 0 || y != 0)
                    {
                        transform.localScale = x < 0 ? new Vector3(-1,1,1) : new Vector3(1,1,1);
                    }

                    //anim.SetFloat("Speed",Mathf.Abs(h)+Mathf.Abs(v));

                    if (okToAttack && x > 0)
                    {
                        rb2d.velocity = new Vector2(walkSpeed, rb2d.velocity.y);
                        //renderer.flipX = false;
                    }
                    else if (okToAttack && x < 0)
                    {
                        rb2d.velocity = new Vector2(-walkSpeed, rb2d.velocity.y);
                        //renderer.flipX = true;
                    }
                    else
                        rb2d.velocity = new Vector2(0, rb2d.velocity.y);

                    if (Input.GetButtonDown("Jump"))
                        CurrentState = PlayerCombatState.InAir;

                    if (Input.GetButtonDown("Pick") && !inAbility)
                        GetItem();

                    if (Input.GetButton("Fire") && okToAttack)
                    {
                        okToAttack = false;
                        anim.Play(weaponHolder.GetComponentInChildren<Weapon>().getAnimationName());
                    }
                        
                }
                break;


            case PlayerCombatState.InAir:
                {
                    float x = Input.GetAxis("Horizontal");
                    float y = Input.GetAxis("Vertical");

                    if (okToAttack && (x != 0 || y != 0))
                    {
                        transform.localScale = x < 0 ? new Vector3(-1,1,1) : new Vector3(1,1,1);
                    }

                    if (okToAttack && x > 0)
                    {
                        rb2d.velocity = new Vector2(walkSpeed, rb2d.velocity.y);
                        //renderer.flipX = false;
                        //gunHolder.localScale = new Vector3(1.0f, 1.0f, 0.0f);
                    }
                    else if (okToAttack && x < 0)
                    {
                        rb2d.velocity = new Vector2(-walkSpeed, rb2d.velocity.y);
                        //renderer.flipX = true;
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


    public void activeAttackBox()
    {
        var box = gameObject.GetComponentInChildren<BoxCollider2D>();
        if (!box.isActiveAndEnabled)
        {
            box.enabled = true;
        }

        gameObject.GetComponentInChildren<Weapon>().setAttackId();
    }
    
    public void inactiveAttackBox()
    {
        var box = gameObject.GetComponentInChildren<BoxCollider2D>();
        if (box.isActiveAndEnabled)
        {
            box.enabled = false;
        }
    }
    
    
    public void resetAttack()
    {
       
        anim.Play("Knight_Idle");
        StartCoroutine(resetAtk());
    }
    
    IEnumerator resetAtk()
    {
        yield return new WaitForSeconds(0.02f);
        okToAttack = true;
    }

    public void pauseAtkAnim(float hitStop)
    {
        anim.speed = 0.1f;
        StartCoroutine(resetAtkAnim(hitStop));
    }

    IEnumerator resetAtkAnim(float hitStop)
    {
        yield return new WaitForSeconds(hitStop);
        anim.speed = 1f;
    }

    private void Ability()
    {
        lastAbility = Time.unscaledTime;
        inAbility = true;
        switch (type)
        {
            case HeroType.Knight:
                float temp = weaponHolder.GetComponentInChildren<Gun>().reloadSpeed;

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


//    public void levelUp()
//    {
//        switch (type)
//        {
//            case HeroType.Knight:
//                dexterity++;
//                break;
//            case HeroType.Nurse:
//                wisdom++;
//                break;
//            case HeroType.Fat:
//                maxHp++;
//                break;
//        }
//        Hp = maxHp;
//    }

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
                    weaponHolder.GetComponentInChildren<Gun>().Destroy();
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
        weaponHolder.GetComponentInChildren<Gun>().reloadSpeed = 0;
        weaponHolder.GetComponentInChildren<Gun>().fireRate *= 0.5f;
        yield return new WaitForSeconds(4f);
        weaponHolder.GetComponentInChildren<Gun>().reloadSpeed = val;
        weaponHolder.GetComponentInChildren<Gun>().fireRate /= 0.5f;
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
