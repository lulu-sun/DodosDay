using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceRinkNonIceGround : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        // reset slide direction
        IceRinkGameHelper.Instance.SlideDirection = Vector2.zero;
        IceRinkGameHelper.Instance.OnIce = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IceRinkGameHelper.Instance.OnIce = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
    }
}
