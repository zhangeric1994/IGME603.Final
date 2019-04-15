using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum StatsType : int
{
    // may move this part to stats manager or game manager
    Power,
    Dexterity,
    Wisdom
}

public enum statsType : int
{
    WalkSpeed,
    JumpPower,
    MaxHp,
    CriticalChance,
    CriticalDamage,
    BaseDamge,
    attackSpeed,
    
}


public enum PlayerCombatState
{
    Default = 0,
    OnGround,
    InAir,
}

public class PlayerCombatController : MonoBehaviour
{
    //[Header("Stats")]
    //[SerializeField] public float walkSpeed;
    //[SerializeField] public int jumpPower;
    //[SerializeField] public int maxHp;

    //[SerializeField] public float criticalChance;
    //[SerializeField] public float criticalDamageFactor;

    //[SerializeField] public float damageFactor;
    //[SerializeField] public float attackSpeedFactor;
    

    public Transform weaponHolder;

    public int PlayerID { get; private set; }

    private PlayerCombatState currentState;

    private Vector2 aimmingDirection;

    private SpriteRenderer renderer;
    private Rigidbody2D rb2d;
    private Animator anim;

    private bool isInAir;

    private float hp;
    private int magazine;

    private float invulnerableInterval = 0.3f;
    private float lastHit;
    
    

    [SerializeField] private GameObject shield;

    public CombatManager Combat;
    
    private Vector3 defaultScale;
    
    public bool okToAttack;

    public EventOnDataChange2<float> OnHpChange { get; private set; }

    public GameObject cam;
    
    private float lastInput;

    private HashSet<Loot> loots = new HashSet<Loot>();


    public Player Avatar
    {
        get
        {
            return Player.GetPlayer(PlayerID);
        }
    }

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
                        rb2d.AddForce(Avatar.GetStatistic(StatisticType.JumpPower) * Vector2.up);
                        break;
                }
            }
        }
    }

    public float Hp
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

                OnHpChange.Invoke(hp, Avatar.GetStatistic(StatisticType.MaxHp));
            }
        }
    }


    private PlayerCombatController() { }

    public void Initialize(int id)
    {
        PlayerID = id;

        OnHpChange = new EventOnDataChange2<float>();
    }

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        defaultScale = transform.localScale;
        hp = Avatar.GetStatistic(StatisticType.MaxHp);

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
                        transform.localScale = x < 0 ? new Vector3(-defaultScale.x,defaultScale.y,defaultScale.z) 
                            : new Vector3(defaultScale.x,defaultScale.y,defaultScale.z);
                    }

                    //anim.SetFloat("Speed",Mathf.Abs(h)+Mathf.Abs(v));

                    if (okToAttack && x > 0)
                    {
                        float walkSpeed = Avatar.GetStatistic(StatisticType.WalkSpeed);
                        rb2d.velocity = new Vector2(walkSpeed, rb2d.velocity.y);
                        anim.SetFloat("Speed",walkSpeed);
                        //renderer.flipX = false;
                    }
                    else if (okToAttack && x < 0)
                    {
                        float walkSpeed = Avatar.GetStatistic(StatisticType.WalkSpeed);
                        rb2d.velocity = new Vector2(-walkSpeed, rb2d.velocity.y);
                        anim.SetFloat("Speed",walkSpeed);
                        //renderer.flipX = true;
                    }
                    else
                    {
                        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                        anim.SetFloat("Speed",0f);
                    }

                    if (Input.GetButtonDown("Jump") && lastInput != Time.time + 10f)
                    {
                        CurrentState = PlayerCombatState.InAir;
                        lastInput = Time.time;
                    }

                    if (Input.GetButtonDown("Pick") && lastInput != Time.time + 10f)
                    {
                        Loot();
                        lastInput = Time.time;
                    }
                        

                    if (Input.GetButton("Fire") && okToAttack && lastInput != Time.time + 10f)
                    {
                        okToAttack = false;
                        anim.Play(weaponHolder.GetComponentInChildren<Weapon>().getAnimationName());
                        anim.speed = Avatar.GetStatistic(StatisticType.AttackSpeed);
                        lastInput = Time.time;
                    }
                        
                }
                break;
            

            case PlayerCombatState.InAir:
                {
                    float x = Input.GetAxis("Horizontal");
                    float y = Input.GetAxis("Vertical");

                    if (okToAttack && (x != 0 || y != 0))
                    {
                        transform.localScale = x < 0 ? new Vector3(-defaultScale.x,defaultScale.y,defaultScale.z) 
                            : new Vector3(defaultScale.x,defaultScale.y,defaultScale.z);
                    }

                    if (okToAttack && x > 0)
                    {
                        rb2d.velocity = new Vector2(Avatar.GetStatistic(StatisticType.WalkSpeed), rb2d.velocity.y);
                        //renderer.flipX = false;
                        //gunHolder.localScale = new Vector3(1.0f, 1.0f, 0.0f);
                    }
                    else if (okToAttack && x < 0)
                    {
                        rb2d.velocity = new Vector2(-Avatar.GetStatistic(StatisticType.WalkSpeed), rb2d.velocity.y);
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

//    public void setStates(statsType type, float num)
//    {
//        switch (type)
//        {
//            case statsType.attackSpeed:
//                attackSpeedFactor += num;
//                break;
//            
//            case statsType.JumpPower:
//                jumpPower += (int)num;
//                break;
//            
//            case statsType.CriticalChance:
//                criticalChance += num;
//                break;
//            
//            case statsType.CriticalDamage:
//                criticalDamageFactor += num;
//                break;
//            
//            case statsType.MaxHp:
//                maxHp += (int)num;
//                hp += (int)num;
//                break;
//            
//            case statsType.BaseDamge:
//                damageFactor += num;
//                break;
//            
//            case statsType.WalkSpeed:
//                walkSpeed += num;
//                break;
//            
//        }
//        
//    }


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

        if (gameObject.GetComponentInChildren<Weapon>().type == WeaponType.Hammer)
        {
            ForwardCamera._instance.Shaking(0.05f,0.1f);
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
        anim.speed = 1;
    }

    public void pauseAtkAnim(float hitStop)
    {
        anim.speed /= 10;
        StartCoroutine(resetAtkAnim(hitStop));
    }

    IEnumerator resetAtkAnim(float hitStop)
    {
        yield return new WaitForSeconds(hitStop);
        anim.speed = Avatar.GetStatistic(StatisticType.AttackSpeed);
    }

//    private void Ability()
//    {
//        lastAbility = Time.unscaledTime;
//        inAbility = true;
//        switch (type)
//        {
//            case HeroType.Knight:
////                float temp = weaponHolder.GetComponentInChildren<Gun>().reloadSpeed;
//
//                StartCoroutine(resetReloadDelay(temp));
//                break;
//            case HeroType.Nurse:
//                GunManager._instance.generateHealDrop(transform);
//                inAbility = false;
//                break;
//            case HeroType.Fat:
//                shield.SetActive(true);
//                StartCoroutine(resetShieldDelay());
//                break;
//        }
//    }


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

    private void Loot()
    {
        if (loots.Count > 0)
        {
            float minDistance = float.MaxValue;
            Loot targetLoot = null;

            float x = gameObject.transform.position.x;
            foreach (Loot loot in loots)
            {
                float distance = Mathf.Abs(loot.gameObject.transform.position.x - x);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetLoot = loot;
                }
            }

            loots.Remove(targetLoot);

            switch (targetLoot.Type)
            {
                case LootType.Weapon:
                    weaponHolder.GetComponentInChildren<Weapon>().Destroy();
                    break;
            }

            targetLoot.Trigger(this);
        }

        //var items = FindObjectsOfType<Loot>();
        //foreach (var item in items)
        //{
        //    float distanceToItem = (item.transform.position - transform.position).sqrMagnitude;
        //    if (distanceToItem <= 0.1f && item.gameObject.activeInHierarchy)
        //    {
        //        if (item.Type() == LootType.Weapon)
        //        {
        //            weaponHolder.GetComponentInChildren<Weapon>().Destroy();
        //            item.GetComponent<Loot>().Trigger(this);
        //        }
        //        else if (item.Type() == LootType.Potion)
        //        {
        //            //Hp++;
        //            item.GetComponent<Loot>().Trigger(this);
        //            //TODO do UI update
        //        }
        //        else if (item.Type() == LootType.Item)
        //        {
        //            switch (item.getStatsType())
        //            {
        //                //TODO: Inventory


        //                //case statsType.WalkSpeed:
        //                //    walkSpeed += 0.05f;
        //                //    break;


        //                //case statsType.JumpPower:
        //                //    jumpPower += 10;
        //                //    break;


        //                //case statsType.MaxHp:
        //                //    maxHp += 1;
        //                //    hp += 1;
        //                //    break;


        //                //case statsType.CriticalChance:
        //                //    criticalChance += 5f;
        //                //    break;


        //                //case statsType.CriticalDamage:
        //                //    criticalDamageFactor += 0.1f;
        //                //    break;


        //                //case statsType.BaseDamge:
        //                //    damageFactor += 0.1f;
        //                //    break;


        //                //case statsType.attackSpeed:
        //                //    attackSpeedFactor += 0.1f;
        //                //    break;
        //            }

        //            item.GetComponent<Loot>().Trigger(this);
        //            //TODO do UI update
        //        }
        //    }
        //}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Loot loot = other.GetComponent<Loot>();

        if (loot)
        {
            switch (loot.Type)
            {
                case LootType.Item:
                    if (!loot.triggered)
                    {
                        Avatar.Loot(loot);
                        loot.triggered = true;
                        Destroy(loot.gameObject);
                    }
                    break;


                default:
                    loots.Add(loot);
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Loot loot = other.GetComponent<Loot>();

        if (loot)
        {
            switch (loot.Type)
            {
                case LootType.Item:
                    break;


                default:
                    loots.Remove(loot);
                    break;
            }
        }
    }

    //    IEnumerator resetReloadDelay(float val)
    //    {
    ////        //simple animation
    ////        weaponHolder.GetComponentInChildren<Gun>().reloadSpeed = 0;
    ////        weaponHolder.GetComponentInChildren<Gun>().fireRate *= 0.5f;
    ////        yield return new WaitForSeconds(4f);
    ////        weaponHolder.GetComponentInChildren<Gun>().reloadSpeed = val;
    ////        weaponHolder.GetComponentInChildren<Gun>().fireRate /= 0.5f;
    ////        inAbility = false;
    //    }

    IEnumerator resetShieldDelay()
    {
        //simple animation
        yield return new WaitForSeconds(6f);
        shield.SetActive(false);
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
