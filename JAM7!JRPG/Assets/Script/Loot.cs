using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;


public enum LootType : int
{
    Weapon,
    Potion,
    Item,
}


public class Loot : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private LootType type;
    [SerializeField] private int id;
    public int level;

    [Header("References")]
    [SerializeField] private SpriteRenderer levelText;

    private bool chase;
    public bool triggered;
	
    private Vector3 direction;
    private float speed = 2.5f;


    public int Id
    {
        get
        {
            return id;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
    }

    public LootType Type
    {
        get
        {
            return type;
        }
    }


    void Awake()
    {
        chase = false;

        if (type == LootType.Item)
        {
            triggered = true;

            StartCoroutine(Activate(2));
        }
        else
        {
            triggered = false;
        }
    }

    private IEnumerator Activate(float t)
    {
        yield return new WaitForSeconds(t);
        triggered = false;
    }
	
    public void Trigger(PlayerCombatController playerController)
    {
        if (!triggered && gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            triggered = true;

            switch (type)
            {
                case LootType.Weapon:
                    WeaponManager._instance.equipWeapon(playerController, (WeaponType)id, level);
                    break;


                case LootType.Potion:
                    playerController.Avatar.ApplyHealing(DataTableManager.singleton.GetItemData(id).Attributes);
                    break;
            }

            Destroy(gameObject);
        }
    }

    public SpriteRenderer getLevelSprite()
    {
        return levelText;
    }
}
