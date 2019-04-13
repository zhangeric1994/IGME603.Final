using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GunType
{
    Shotgun, 
    MachineGun, 
    SniperRifle,
    Pistol
}

public struct GunInfo
{
    public int[] damage;
    public int[] range;
    public float[] reloadSpeed;
    public int[] magazineSize;
    public float[] fireRate;
}
public class GunManager : MonoBehaviour
{

    [SerializeField] private GameObject[] Guns;
    
    
    [SerializeField] private GameObject[] GunsItem;
    
    [SerializeField] private GameObject Heal;
    
    [SerializeField] private Sprite[] levelTag;
    public static GunManager _instance;
    
    // gun Info

    private GunInfo MachineGun = new GunInfo();
    private GunInfo SniperRifle = new GunInfo();
    private GunInfo ShotGun = new GunInfo();
    private GunInfo Pistol = new GunInfo();
    
    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
        // MachineGun Assignment
        MachineGun.damage = new int[] {2,3,4};
        MachineGun.range = new int[] {5,5,6};
        MachineGun.reloadSpeed = new float[] {0.5f,0.4f,0.3f};
        MachineGun.magazineSize = new int[] {10,20,30};
        MachineGun.fireRate = new float[] {0.2f,0.15f,0.1f};
        
        
        //SniperRifle Assignment
        SniperRifle.damage = new int[] {9,19,30};
        SniperRifle.range = new int[] {5,6,7};
        SniperRifle.reloadSpeed = new float[] {1.2f,1.1f,1.0f};
        SniperRifle.magazineSize = new int[] {5,6,7};
        SniperRifle.fireRate = new float[] {0.65f,0.6f,0.55f};
        
        //ShotGun Assignment
        ShotGun.damage = new int[] {15,20,25};
        ShotGun.range = new int[] {1,1,2};
        ShotGun.reloadSpeed = new float[] {0.9f,0.85f,0.8f};
        ShotGun.magazineSize = new int[] {2,3,4};
        ShotGun.fireRate = new float[] {0.5f,0.4f,0.3f};
        
        //Pistol Assignment
        Pistol.damage = new int[] {4,8,9};
        Pistol.range = new int[] {3,4,5};
        Pistol.reloadSpeed = new float[] {0.3f,0.2f,0.1f};
        Pistol.magazineSize = new int[] {7,8,9};
        Pistol.fireRate = new float[] {0.3f,0.25f,0.2f};
    }


    public GunInfo getGunData(GunType type)
    {
        switch (type)
        {
                case GunType.MachineGun:
                    return MachineGun;
                case GunType.Pistol:
                    return Pistol;
                case GunType.Shotgun:
                    return ShotGun;
                case GunType.SniperRifle:
                    return SniperRifle;
        }

        return new GunInfo();
    }


    public void equipWeapon(PlayerCombatController player, GunType type, int level)
    {
        var defaultPos = Guns[(int) type].transform.localPosition;
        var temp = Instantiate(Guns[(int) type]);

        temp.transform.parent = player.weaponHolder;
        temp.transform.localPosition = defaultPos;
        temp.transform.right = player.GetComponent<SpriteRenderer>().flipX ? -Vector2.right : Vector2.right; 
        temp.GetComponent<Gun>().type = type;
        temp.GetComponent<Gun>().AcquireGun(level);
    }
    
    
    public void generateDrop(Vector3 player, GunType type, int level)
    {
        var temp = Instantiate(GunsItem[(int) type]);
        temp.GetComponent<Item>().getLevelSprite().sprite = levelTag[level - 1];
        //todo Add a parent to organize
        temp.transform.position = player;
    }
    
    public void generateHealDrop(Transform player)
    {
        var temp = Instantiate(Heal);
        //todo Add a parent to organize
        temp.transform.position = player.position;
    }
    
    
    
    public void randomDrop(Vector3 Pos)
    {
        int random = Random.Range(0, 100);
        int type = Random.Range(0, 3);
        if (random < 15)
        {
            //level three
            generateDrop(Pos, (GunType) type, 3);
        }else if (random > 15 && random < 45)
        {
            generateDrop(Pos, (GunType) type, 2);
        }
        else
        {
            generateDrop(Pos, (GunType) type, 1);
        }
    }
}
