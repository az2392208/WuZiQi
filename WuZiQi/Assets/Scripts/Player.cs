using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ChestType chestCloor = ChestType.black;

    private void FixedUpdate()
    {

        if (chestCloor == ChestBoard.Instacne.turn && ChestBoard.Instacne.timer > 0.5f)
        {
            PlayChest();
        }

    }

    public virtual void PlayChest()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ChestBoard.Instacne.PlayChest(new int[2] { (int)(pos.x + 7.5f), (int)(pos.y + 7.45f) });
            ChestBoard.Instacne.timer = 0;
        }
    }
}
