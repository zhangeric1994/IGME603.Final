using UnityEngine;
using UnityEngine.UI;


public class CombatHUD : GUIWindow
{
    [Header("References")]
    //[SerializeField] private Text magazineText;
    [SerializeField] private Transform list;

    private Player player;

    //private void UpdateMagazine(int n)
    //{
    //    magazineText.text = n.ToString();
    //}

    private void UpdateAll()
    {
        UpdateHp(player.GetStatistic(StatisticType.Hp), player.GetStatistic(StatisticType.MaxHp));
    }

    private void UpdateHp(float hp, float maxHp)
    {
        float p = hp * 5 / maxHp;

        for (int i = 0; i < 5; ++i)
        {
            float d = p - i;

            Debug.Log(list);
            Debug.Log(list.GetChild(i));
            Debug.Log(list.GetChild(i).GetComponent<Image>());
            Debug.Log(list.GetChild(i).GetComponent<Image>().sprite);
            Debug.Log(GUIManager.Singleton);
            Debug.Log(GUIManager.Singleton.fullHeart);

            if (d > 0.5)
                list.GetChild(i).GetComponent<Image>().sprite = GUIManager.Singleton.fullHeart;
            else if (d > 0)
                list.GetChild(i).GetComponent<Image>().sprite = GUIManager.Singleton.halfHeart;
            else
                list.GetChild(i).GetComponent<Image>().sprite = GUIManager.Singleton.emptyHeart;
        }
    }

    private void Start()
    {
        player = Player.GetPlayer(0);

        UpdateAll();

        player.OnHpChange.AddListener(UpdateHp);
    }

    private void OnDestroy()
    {
        if (player != null)
            player.OnHpChange.RemoveListener(UpdateHp);
    }
}
