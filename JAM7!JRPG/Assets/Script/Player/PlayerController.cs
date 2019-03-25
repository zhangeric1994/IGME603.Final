using UnityEngine;

public enum StatsType : int
{
    // may move this part to stats manager or game manager
    Health,
    Power,
    Accuracy,
    Defense,
    Damage
}

public enum playerState
{
    Default = 0,
    OnGround,
}

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int walkSpeed;
    [SerializeField] private int jumpPower;
    [SerializeField] private int maxHealth;
    [SerializeField] private int power;
    [SerializeField] private int accuracy;
    [SerializeField] private int defense;
    [SerializeField] private int damage;

    private int id;
    private playerState currentState;

    private Vector2 aimmingDirection;

    private Rigidbody2D rb2d;
    private Animator anim;

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

    public playerState CurrentState
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
                playerState previousState = currentState;
                currentState = value;
                switch (currentState)
                {
                    case playerState.OnGround:
                        //switch to moveable
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
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case playerState.OnGround:
                {
                    float x = Input.GetAxis("Horizontal" + Id);
                    float y = Input.GetAxis("Vertical" + Id);

                    if (x != 0 && y != 0)
                        aimmingDirection = new Vector2(Mathf.Clamp01(x), y).normalized;

                    //anim.SetFloat("Speed",Mathf.Abs(h)+Mathf.Abs(v));

                    if (y > 0)
                        rb2d.velocity = new Vector2(0, walkSpeed);
                    else if (y < 0)
                        rb2d.velocity = new Vector2(0, -walkSpeed);
                    else
                        rb2d.velocity = Vector2.zero;
                }
                break;
        }
    }
}
