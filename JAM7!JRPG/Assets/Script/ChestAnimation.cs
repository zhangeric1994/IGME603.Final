using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnimation : MonoBehaviour
{
    private bool isOpen = false;
    public void SetChestStat(bool flag)
    {
        GetComponent<Animator>().SetBool("isOpened", flag);
        if (!isOpen)
        {
            isOpen = true;
            FMOD.Studio.EventInstance lootSound = FMODUnity.RuntimeManager.CreateInstance("event:/AutoLoot");
            lootSound.start();
        }
    }

}
