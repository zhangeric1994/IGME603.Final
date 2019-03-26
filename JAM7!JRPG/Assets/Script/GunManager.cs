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
        MachineGun.reloadSpeed = new float[] {1.5f,1.4f,1.3f};
        MachineGun.magazineSize = new int[] {10,15,20};
        MachineGun.fireRate = new float[] {0.2f,0.15f,0.1f};
        
        
        //SniperRifle Assignment
        SniperRifle.damage = new int[] {9,11,13};
        SniperRifle.range = new int[] {9,10,11};
        SniperRifle.reloadSpeed = new float[] {2.1f,1.9f,1.8f};
        SniperRifle.magazineSize = new int[] {4,5,6};
        SniperRifle.fireRate = new float[] {0.65f,0.6f,0.55f};
        
        //ShotGun Assignment
        ShotGun.damage = new int[] {14,20,25};
        ShotGun.range = new int[] {2,3,3};
        ShotGun.reloadSpeed = new float[] {2.0f,1.95f,1.9f};
        ShotGun.magazineSize = new int[] {2,3,4};
        ShotGun.fireRate = new float[] {0.7f,0.65f,0.6f};
        
        //Pistol Assignment
        Pistol.damage = new int[] {4,6,7};
        Pistol.range = new int[] {5,6,7};
        Pistol.reloadSpeed = new float[] {1.2f,1.15f,1.1f};
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


    public void equipWeapon(PlayerController player, GunType type, int level)
    {
        var defaultPos = Guns[(int) type].transform.localPosition;
        var temp = Instantiate(Guns[(int) type]);

        temp.transform.parent = player.gunHolder;
        temp.transform.localPosition = defaultPos;
        temp.transform.right = player.GetComponent<SpriteRenderer>().flipX ? -Vector2.right : Vector2.right; 
        temp.GetComponent<Gun>().type = type;
        temp.GetComponent<Gun>().AcquireGun(level);
    }
    
    
    public void generateDrop(Transform player, GunType type, int level)
    {
        var temp = Instantiate(GunsItem[(int) type]);
        temp.GetComponent<Item>().getLevelSprite().sprite = levelTag[level - 1];
        //todo Add a parent to organize
        temp.transform.position = player.position;
    }
}
