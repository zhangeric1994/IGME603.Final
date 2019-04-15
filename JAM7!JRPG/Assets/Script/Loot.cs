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
    [SerializeField] private int level;

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

        set
        {
            id = value;
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

            StartCoroutine(Activate(1));
        }
        else
            triggered = false;
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
            triggered = true;

            switch (type)
            {
                case LootType.Weapon:
                    // WeaponManager._instance.equipWeapon(playerController, (WeaponType)id, level);
                    WeaponData previousWeapon = WeaponManager._instance.EquipWeapon(playerController, DataTableManager.singleton.GetWeaponData(id));
                    id = previousWeapon.Id;
                    GetComponent<SpriteRenderer>().sprite = previousWeapon.Sprite;
                    levelText.sprite = WeaponManager._instance.levelTag[previousWeapon.Level - 1];
                    StartCoroutine(Activate(0.5f));
                    break;


                case LootType.Potion:
                    playerController.Avatar.ApplyHealing(DataTableManager.singleton.GetItemData(id).Attributes);
                    Destroy(gameObject);
                    break;
            }
        }
    }

    public SpriteRenderer getLevelSprite()
    {
        return levelText;
    }
}
