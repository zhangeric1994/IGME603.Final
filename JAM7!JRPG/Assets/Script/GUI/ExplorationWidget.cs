using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationWidget : GUIWidget
{
    [Header("References")]
    [SerializeField] private MenuWidget menu;

    public override void Initialize(params object[] args)
    {
        PlayerController player = (PlayerController)args[0];
    }
}
