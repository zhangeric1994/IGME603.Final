using System.Collections;
using System.Collections.Generic;
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
public enum playerState {
    Moveable = 1,
    Default,
}
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Initilize Setting *")]
    [SerializeField]private int PlayerIndex; // the index for input
    
    public int HorizontalSpeed;
    public int VerticalSpeed;
    
    private Rigidbody2D rb2d;
    private Animator anim;
    
    [Header("Stats")]
    [SerializeField]private int maxHealth;
    [SerializeField]private int power;
    [SerializeField]private int accuracy;
    [SerializeField]private int defense;
    [SerializeField]private int damage;
    
    [Header("State")]
    [SerializeField] private playerState currentState;
    private playerState previousState;
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
                previousState = currentState;
                currentState = value;
                switch (currentState)
                {
                    case playerState.Moveable:
                        //switch to moveable
                        break;
                }
            }
        }
    }

    
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currentState)
        {
                case playerState.Moveable:
                    float h = Input.GetAxis("Horizontal" + PlayerIndex);
                    float v = Input.GetAxis("Vertical" + PlayerIndex);
                    anim.SetFloat("Speed",Mathf.Abs(h)+Mathf.Abs(v));	
                    rb2d.velocity = new Vector2( h * HorizontalSpeed,v *VerticalSpeed);
                    break;
                case playerState.Default:
                    break;
        }
    }
    
    void setIndex(int index)
    {
        PlayerIndex = index;
    }
    
    public void moveTo(Vector3 pos)
    {
        transform.position = pos;
    }
    
    public void setStats(int value, StatsType type, bool overwrite = false)
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
}
