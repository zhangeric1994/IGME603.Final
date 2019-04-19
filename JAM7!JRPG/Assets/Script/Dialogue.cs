using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    // Start is called before the first frame update
    public List<string> dialogues;
    public List<string> alternativeDialogues;
    private int currentDialogIndex;
    private bool active;
    private bool alternative;
    private PlayerExplorationController playerInTalking;
    public UnityEvent eventAfterDialog;
    void Start()
    {
        currentDialogIndex = 0;
        alternative = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;
        if (Input.GetButtonDown("Submit"))
        {
            ++currentDialogIndex;
            int count;
            if (alternative) count = alternativeDialogues.Count;
            else count = dialogues.Count;
            if (currentDialogIndex == count)
            {
                DialogueBox.instance.Hide();
                playerInTalking.CurrentState = PlayerExplorationState.Exploring;
                active = false;
                eventAfterDialog?.Invoke();
                return;
            }
            DialogueBox.instance.Text = alternative ? alternativeDialogues[currentDialogIndex] : dialogues[currentDialogIndex];
            //AudioManager.Instance.PlaySoundEffect("Text", false, false);
        }
    }

    public bool StartDialog(PlayerExplorationController player)
    {
        if (currentDialogIndex > 0)
        {
            currentDialogIndex = 0;
            alternative = true;
            return false;
        }
        Debug.Log(player.name + " started dialog.");
        currentDialogIndex = 0;
        playerInTalking = player;
        active = true;
        DialogueBox.instance.Show();
        DialogueBox.instance.Text = alternative ? alternativeDialogues[0] : dialogues[0];
        //AudioManager.Instance.PlaySoundEffect("Text", false, false);
        return true;
    }

}
