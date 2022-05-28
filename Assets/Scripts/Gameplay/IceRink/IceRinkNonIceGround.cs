using UnityEngine;

public class IceRinkNonIceGround : MonoBehaviour
{
    private float playerSpeed;

    private void Start()
    {
        playerSpeed = GameController.Instance.playerController.Character.moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // reset slide direction
        IceRinkGameHelper.Instance.SlideDirection = Vector2.zero;
        IceRinkGameHelper.Instance.OnIce = false;
        GameController.Instance.playerController.Character.moveSpeed = playerSpeed;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IceRinkGameHelper.Instance.OnIce = true;
        GameController.Instance.playerController.Character.moveSpeed = playerSpeed * 1.5f;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
    }
}
