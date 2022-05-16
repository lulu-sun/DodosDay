using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceRinkNonIceGround : MonoBehaviour
{
    private int count;

    private void OnTriggerEnter2D(Collider2D col)
    {
        // reset slide direction
        IceRinkGameHelper.Instance.SlideDirection = Vector2.zero;
        IceRinkGameHelper.Instance.IsSlippery = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IceRinkGameHelper.Instance.IsSlippery = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
    }
}
