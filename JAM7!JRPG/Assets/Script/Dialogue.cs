using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    // Start is called before the first frame update
    public List<string> dialogues;
    public List<string> alternativeDialogues;
    private List<string> currentDialogues;
    private int currentDialogIndex;
    private bool active;
    private PlayerExplorationController playerInTalking;
    void Start()
    {
        currentDialogIndex = 0;
        currentDialogues = dialogues;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;
        if (Input.GetButtonDown("Submit"))
        {
            ++currentDialogIndex;
            if (currentDialogIndex == currentDialogues.Count)
            {
                DialogueBox.instance.Hide();
                playerInTalking.CurrentState = PlayerExplorationState.Exploring;
                active = false;
                currentDialogues = alternativeDialogues;
                return;
            }
            DialogueBox.instance.Text = currentDialogues[currentDialogIndex];
            AudioManager.Instance.PlaySoundEffect("Text", false, false);
        }
    }

    public bool StartDialog(PlayerExplorationController player)
    {
        if (currentDialogIndex > 0)
        {
            currentDialogIndex = 0;
            return false;
        }
        Debug.Log(player.name + " started dialog.");
        currentDialogIndex = 0;
        playerInTalking = player;
        active = true;
        DialogueBox.instance.Show();
        DialogueBox.instance.Text = currentDialogues[currentDialogIndex];
        AudioManager.Instance.PlaySoundEffect("Text", false, false);
        return true;
    }
       
}
