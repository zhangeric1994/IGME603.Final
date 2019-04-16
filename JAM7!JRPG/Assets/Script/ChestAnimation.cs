using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnimation : MonoBehaviour
{

    public void SetChestStat(bool flag)
    {
        GetComponent<Animator>().SetBool("isOpened", flag);
    }

}
