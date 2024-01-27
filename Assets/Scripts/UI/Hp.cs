using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    public Sprite hpFull;
    public Sprite hpMedio;
    public Sprite hpLow;
    public Image hpImage;

    public void SetImage()
    {
        int x = GameManager.Instance.player.currentHP;

        if (x == 3)
            hpImage.sprite = hpFull;
        if (x == 2)
            hpImage.sprite = hpMedio;
        if (x == 1)
            hpImage.sprite = hpLow;
    }

}
