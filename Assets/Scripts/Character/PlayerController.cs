using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    BoxCollider2D boxCollider;

    Vector2 movement;

    Character character; 

    [SerializeField] bool allowDiagonalMovement = false;

    [SerializeField] bool animate = true;

    public Character Character { get => character; }

    private void Awake()
    {
        character = GetComponent<Character>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        if (SceneManager.GetActiveScene().name.Equals("IceRink") &&
            IceRinkGameHelper.Instance.IsSlippery)
        {
            IceRinkHandleUpdate();
        }
        else
        {
            NormalHandleUpdate();
        }
    }

    private void NormalHandleUpdate()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (!allowDiagonalMovement)
        {
            // Prevent diagonal movement by prioritizing
            // horizontal movement.
            if (movement.x != 0)
            {
                movement.y = 0;
            }
        }

        movement.Normalize();

        if (Controls.GetSelectKeyDown())
        {
            Debug.Log("Player trying to interact");
            Interact();
        }

        character.MoveOneFrame(movement);
        character.HandleUpdate();
        if (animate)
        {
            character.Animate(movement);
        }
    }

    private void IceRinkHandleUpdate()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Prevent diagonal movement by prioritizing
        // horizontal movement.
        if (movement.x != 0)
        {
            movement.y = 0;
        }

        movement.Normalize();

        if (!IceRinkGameHelper.Instance.IsSliding(transform.position))
        {
            IceRinkGameHelper.Instance.SlideDirection = movement;
            character.Animate(movement);
        }

        character.MoveOneFrame(IceRinkGameHelper.Instance.SlideDirection);
        character.HandleUpdate();

        IceRinkGameHelper.Instance.PreviousFramePosition = transform.position;
    }

    private void Interact()
    {
        // Get the position of what I'm interacting with based on the direction I'm facing.
        // Need to scale the distance based on boxCollider size. (Longer in x direction, shorter for y)
        var interactPos = transform.position + new Vector3(
            character.Direction.x * boxCollider.size.x,
            character.Direction.y * boxCollider.size.y);

        Debug.DrawLine(transform.position, interactPos, Color.red, 0.5f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.Instance.InteractableLayer);
        if (collider != null)
        {
            Debug.Log(collider.GetComponent<Interactable>());
            collider.GetComponent<Interactable>()?.Interact(character.Direction);
        }
    }
}