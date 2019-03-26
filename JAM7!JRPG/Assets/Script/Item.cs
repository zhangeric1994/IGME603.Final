using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;
public enum ItemTag : int
{
    Weapon,
    Heal
}

public class Item : MonoBehaviour
{
    private bool chase;
    public bool triggered;
		
    protected GameObject[] players;
	
    private Vector3 direction;
    [SerializeField] private ItemTag type;
    [SerializeField] private SpriteRenderer levelText;
    
    [SerializeField] private GunType gunType;
    [SerializeField] private int level;
    private float speed = 2.5f;
    // Use this for initialization
    void Start ()
    {
        chase = false;
        triggered = false;
        players = GameObject.FindGameObjectsWithTag("Player");
    }
	
    // Update is called once per frame

    public void Trigger(PlayerCombatController target)
    {
        if (!triggered && gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            triggered = true;
            if (type == ItemTag.Weapon)
            {
                GunManager._instance.equipWeapon(target, gunType, level);
            }
        }
    }


    public void setType(ItemTag tag)
    {
        type = tag;
    }

    public ItemTag getType()
    {
        return type;
    }

    public SpriteRenderer getLevelSprite()
    {
        return levelText;
    }
}
