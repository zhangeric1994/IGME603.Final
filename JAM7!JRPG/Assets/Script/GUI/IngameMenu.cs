using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IngameMenu : GUIWindow
{
    [Header("References")]
    [SerializeField] private Transform list;

    private Action closeCallback;

    private int maxIndex;
    private int choosenIndex;


    private int ChoosenIndex
    {
        set
        {
            if (value != choosenIndex)
            {
                list.GetChild(choosenIndex).GetComponent<Text>().color = Color.white;

                choosenIndex = value;

                list.GetChild(choosenIndex).GetComponent<Text>().color = Color.green;
            }
        }
    }


    public override void OnOpen(params object[] args)
    {
        closeCallback = (Action)args[0];

        maxIndex = list.childCount;
        ChoosenIndex = 0;

        StartToCheckForPlayerInput();
    }

    public override void OnClose()
    {
        closeCallback?.Invoke();
    }

    private void StartToCheckForPlayerInput()
    {
        StartCoroutine(CheckForPlayerInput());
    }

    private IEnumerator CheckForPlayerInput()
    {
        yield return null;

        for (;;)
        {
            if (Input.GetButtonDown("Submit"))
            {
                GUIManager.Singleton.Open(list.GetChild(choosenIndex).name, (Action)StartToCheckForPlayerInput);
                FMOD.Studio.EventInstance confirmSound = FMODUnity.RuntimeManager.CreateInstance("event:/Interface/Confirm");
                confirmSound.start();
                yield break;
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                //AudioManager.Instance.PlaySoundEffect("ClickDown");
                Close();
                yield break;
            }
            else if (Input.GetButtonDown("Up"))
            {
                ChoosenIndex = (maxIndex + choosenIndex - 1) % maxIndex;
                //AudioManager.Instance.PlaySoundEffect("Click");
                FMOD.Studio.EventInstance selectSound = FMODUnity.RuntimeManager.CreateInstance("event:/Interface/Selected");
                selectSound.start();
            }
            else if (Input.GetButtonDown("Down"))
            {
                ChoosenIndex = (maxIndex + choosenIndex + 1) % maxIndex;
                //AudioManager.Instance.PlaySoundEffect("Click");
                FMOD.Studio.EventInstance selectSound = FMODUnity.RuntimeManager.CreateInstance("event:/Interface/Selected");
                selectSound.start();
            }
            yield return null;
        }
    }
}
