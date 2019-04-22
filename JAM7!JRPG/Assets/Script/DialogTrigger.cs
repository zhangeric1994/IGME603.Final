using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    private Dialogue dialog;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        dialog = GetComponent<Dialogue>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            player = GameObject.Find("PlayerE(Clone)");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dialog.StartDialog(player.GetComponent<PlayerExplorationController>()))
            player.GetComponent<PlayerExplorationController>().CurrentState = PlayerExplorationState.InTalking;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
