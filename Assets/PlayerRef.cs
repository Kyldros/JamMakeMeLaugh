using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRef : MonoBehaviour
{
    public Player player => GameManager.Instance.player;

    public void PLaystepClip()
    {
        player.PlayStepClip();
    }
}
