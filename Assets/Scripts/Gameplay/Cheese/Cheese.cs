using UnityEngine;

public class Cheese : MonoBehaviour
{
    [SerializeField] int cheeseId = -1;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!CheeseGameSystem.Instance.IsActive)
        {
            return;
        }

        if (col.name.Contains("Player"))
        {
            CheeseGameSystem.Instance.ShowCheeseFound(cheeseId);
            AudioManager.Instance.PlayPopSfx();
            gameObject.SetActive(false);

        }
    }
}
