using UnityEngine;

public class MidBossOnDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        GameProgressManager.instance.MidBossKilled();
    }
}
