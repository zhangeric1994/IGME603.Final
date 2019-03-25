using UnityEngine;

public class PlayerHealthWidget : GUIWidget
{
    public override void Redraw(params object[] args)
    {
        PlayerController player = (PlayerController)args[0];

        UpdateHealth(player.Hp, player.MaxHp);

        player.OnHpChange.AddListener(UpdateHealth);
    }

    public void UpdateHealth(int hp, int maxHp)
    {
        float p = (hp * 5) / (float)maxHp;

        for (int i = 0; i < 5; ++i)
        {
            float d = p - i;
            if (d > 0.5)
                transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = GUIManager.Singleton.fullHeart;
            else if (d > 0)
                transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = GUIManager.Singleton.halfHeart;
            else
                transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = GUIManager.Singleton.emptyHeart;
        }
    }
}
