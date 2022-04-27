using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class CutsceneManager : MonoBehaviour
{
    Scene currentScene;

    public bool IsRunning { get; private set; }

    PlayerController player;

    [SerializeField] GameObject npcPrefab;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        player = GameController.Instance.playerController;
        
        if (currentScene.name == "Intro")
        {
            IntroCutscene();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void IntroCutscene()
    {
        GameObject npc = (GameObject)Instantiate(npcPrefab, new Vector3(2, 0, 0), Quaternion.identity);
        npc.SetActive(false);
        Character npcChar = npc.GetComponent<Character>();

        RunMultipleActions(new ICutsceneAction[] {
            new DialogueAction("Joce", new string[]
            {
                "...",
                "..."
            }),
            new DialogueAction("???", new string[]
            {
                "You're awake!"
            }),
            new FaceDirectionAction(player.Character, Vector2.left),
            new WaitAction(1),
            new FaceDirectionAction(player.Character, Vector2.right),
            new WaitAction(1),
            new FaceDirectionAction(player.Character, Vector2.down),
            new DialogueAction("Joce", new string[]
            {
                "... Where am I? Who are you?"
            }),
            new DialogueAction("???", new string[]
            {
                "Oh! How silly, I should introduce myself!"
            }),
            new SetActiveAction(npc, true),
            new FaceDirectionAction(npcChar, Vector2.left),
            new FaceDirectionAction(player.Character, Vector2.right),
            new DialogueAction("Lulu", new string[]
            {
                "I'm Lulu, one of your childhood friends! And I'm here to guide you on your journey."
            }),
            new DialogueAction("Joce", new string[]
            {
                "Wait… If you're my childhood friend, how come I don't remember you?"
            }),
            new DialogueAction("Lulu", new string[]
            {
                "A magic spell stole your memories, and now you have to go on a journey to retrieve them.",
                "At the end, you will receive your heart's desire. But many trials will stand in your way, including familiar faces.",
                "Defeat them, and you will receive your memories again!"
            }),
            new SetActiveAction(npc, false),
            new WaitAction(1),
            new FaceDirectionAction(player.Character, Vector2.down)
        });
    }

    private void RunMultipleActions(IEnumerable<ICutsceneAction> cutsceneActions)
    {
        IsRunning = true;

        if (cutsceneActions.Count() == 0)
        {
            IsRunning = false;
            return;
        }

        StartCoroutine(cutsceneActions.First().PerformAction(() => {
            RunMultipleActions(cutsceneActions.Skip(1));
        }));
    }
}

public interface ICutsceneAction
{
    public IEnumerator PerformAction(Action onFinished = null);
}

public class DialogueAction : ICutsceneAction
{
    private Dialogue dialogue;

    public DialogueAction(string name, string[] lines)
    {
        dialogue = new Dialogue(name, new List<string>(lines));
    }

    public IEnumerator PerformAction(Action onFinished = null)
    {
        return DialogueManager.Instance.ShowDialogue(dialogue, onFinished);
    }
}

public class FaceDirectionAction : ICutsceneAction
{
    private Character character;

    private Vector2 direction;

    public FaceDirectionAction(Character character, Vector2 direction)
    {
        this.character = character;
        this.direction = direction;
    }

    public IEnumerator PerformAction(Action onFinished = null)
    {
        yield return new WaitForEndOfFrame();

        character.FaceDirection(direction);

        onFinished?.Invoke();
    }
}

public class WaitAction : ICutsceneAction
{
    private float seconds;

    public WaitAction(float seconds)
    {
        this.seconds = seconds;
    }

    public IEnumerator PerformAction(Action onFinished = null)
    {
        yield return new WaitForSeconds(1);

        onFinished?.Invoke();
    }
}

public class MoveAction : ICutsceneAction
{
    private Character character;

    private Vector2 movement;

    public MoveAction(Character character, Vector2 movement)
    {
        this.character = character;
        this.movement = movement;
    }

    public IEnumerator PerformAction(Action onFinished = null)
    {
        yield return character.Move(movement, onFinished);
    }
}

public class InstantiateAction : ICutsceneAction
{
    private GameObject prefab;

    private Vector2 location;

    public InstantiateAction(GameObject prefab, Vector2 location)
    {
        this.prefab = prefab;
        this.location = location;
    }

    public IEnumerator PerformAction(Action onFinished = null)
    {
        yield return CutsceneManager.Instantiate(prefab, new Vector3(location.x, location.y, 0), Quaternion.identity);

        onFinished?.Invoke();
    }
}

public class SetActiveAction : ICutsceneAction
{
    private GameObject gameObject;

    private bool active;

    public SetActiveAction(GameObject gameObject, bool active)
    {
        this.gameObject = gameObject;
        this.active = active;
    }

    public IEnumerator PerformAction(Action onFinished = null)
    {
        gameObject.SetActive(active);
        yield return null;
        onFinished?.Invoke();
    }
}