using System.Collections.Generic;
using UnityEngine;


[System.Serializable] public class WeaponData : DataTableEntry
{
    [SerializeField] private int id;
    [SerializeField] private WeaponType type;
    [SerializeField] private int level;
    [SerializeField] private Sprite sprite;
    [SerializeField] private AnimationClip animation;
    [SerializeField] private AttributeSet attributes;


    public int Id
    {
        get
        {
            return id;
        }
    }

    public WeaponType Type
    {
        get
        {
            return type;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
    }

    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
    }

    public AnimationClip Animation
    {
        get
        {
            return animation;
        }
    }

    public AttributeSet Attributes
    {
        get
        {
            return attributes;
        }
    }

    public override int Index
    {
        get
        {
            return id;
        }
    }

    public override int GetHashCode()
    {
        return id.GetHashCode();
    }
}


public struct WeaponInfo
{
    public int[] damage;
    public int[] range;
    public float[] reloadSpeed;
    public int[] magazineSize;
    public float[] fireRate;
}


public class WeaponManager : MonoBehaviour
{

    [SerializeField] private GameObject[] Weapons;
    
    
    [SerializeField] private GameObject[] WeaponsItem;
    
    [SerializeField] private GameObject[] PowerUpItem;
    
    [SerializeField] private GameObject Heal;
    
    [SerializeField] public Sprite[] levelTag;
    public static WeaponManager _instance;
    
    // gun Info

    private WeaponInfo GiantSword = new WeaponInfo();
    private WeaponInfo LightSword = new WeaponInfo();
    private WeaponInfo Hammer = new WeaponInfo();
    //private WeaponInfo Pistol = new GunInfo();
    
    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
        // GiantSword Assignment
        GiantSword.damage = new int[] {15,20,25};
        GiantSword.range = new int[] {5,5,6};
        GiantSword.reloadSpeed = new float[] {0.5f,0.4f,0.3f};
        GiantSword.magazineSize = new int[] {10,20,30};
        GiantSword.fireRate = new float[] {0.2f,0.15f,0.1f};
        
        
        //LightSword Assignment
        LightSword.damage = new int[] {5,10,15};
        LightSword.range = new int[] {5,6,7};
        LightSword.reloadSpeed = new float[] {1.2f,1.1f,1.0f};
        LightSword.magazineSize = new int[] {5,6,7};
        LightSword.fireRate = new float[] {0.65f,0.6f,0.55f};
        
        //Hammer Assignment
        Hammer.damage = new int[] {35,40,45};
        Hammer.range = new int[] {1,1,2};
        Hammer.reloadSpeed = new float[] {0.9f,0.85f,0.8f};
        Hammer.magazineSize = new int[] {2,3,4};
        Hammer.fireRate = new float[] {0.5f,0.4f,0.3f};
    }

    public WeaponInfo getGunData(WeaponType type)
    {
        switch (type)
        {
                case WeaponType.GiantSword:
                    return GiantSword;
                case WeaponType.LightSword:
                    return LightSword;
                case WeaponType.Hammer:
                    return Hammer;
        }

        return new WeaponInfo();
    }

    public WeaponData EquipWeapon(PlayerCombatController player, WeaponData weapon)
    {
        WeaponData previousWeapon = player.Avatar.equipment.ChangeWeapon(weapon);

        equipWeapon(player, weapon.Type, weapon.Level);

        return previousWeapon;
    }

    public void equipWeapon(PlayerCombatController player, WeaponType type, int level)
    {
        var defaultPos = Weapons[(int) type].transform.localPosition;
        var defaultRot = Weapons[(int) type].transform.rotation;
        var temp = Instantiate(Weapons[(int) type]);

        temp.transform.parent = player.weaponHolder;
        temp.transform.localPosition = defaultPos;
        temp.transform.rotation = defaultRot;
        if (player.transform.localScale.x < 0)
        {
            temp.transform.eulerAngles = new Vector3(0,0,360 - temp.transform.eulerAngles.z);
        }
        
        temp.GetComponent<Weapon>().type = type;
        temp.GetComponent<Weapon>().AcquireGun(level);
    }
    
    public void generateDrop(Vector3 pos, WeaponType type, int level)
    {
        Loot loot = Instantiate(WeaponsItem[(int) type], pos, Quaternion.identity).GetComponent<Loot>();
        loot.getLevelSprite().sprite = levelTag[level - 1];
        loot.Id = ((int)type << 16) + level - 1;
    }
    
    public void generateHealDrop(Vector3 pos)
    {
        var temp = Instantiate(Heal);
        //todo Add a parent to organize
        temp.transform.position = pos;
    }
    
    public void generatePowerDrop(Vector3 pos)
    {
        var index = Random.Range(0, PowerUpItem.Length);
        //todo Add a parent to organize
        var temp = Instantiate(PowerUpItem[index], pos, Quaternion.identity);
    }
    
    public void randomDrop(Vector3 Pos)
    {
        int random = Random.Range(0, 100);
        int type = Random.Range(0, 2);

        if (random < 8)
        {
            //level three
            generateDrop(Pos, (WeaponType) type, 3);
        }else if (random >= 8 && random < 18)
        {
            generateDrop(Pos, (WeaponType) type, 2);
        }
        else if (random >= 18 && random < 38)
        {
            generateDrop(Pos, (WeaponType) type, 1);
        }else if (random >= 38 && random < 55)
        {
            generateHealDrop(Pos);
        }
        else
        {
            generatePowerDrop(Pos);
        }
    }
}
