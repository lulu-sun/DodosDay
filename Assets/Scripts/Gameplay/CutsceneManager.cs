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

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        player = GameController.Instance.playerController;
        IntroCutscene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void IntroCutscene()
    {
        IsRunning = true;

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
            // lulu appears
            new DialogueAction("Lulu", new string[]
            {
                "I'm Lulu, one of your childhood friends! And I'm here to guide you on your journey."
            }),
            new DialogueAction("Joce", new string[]
            {
                "Wait… If you're my childhood friend, how come I don't remember you?"
            }),
            new MoveAction(player.Character, new Vector2(5, 0)) // movement animation currently not working
        });
    }

    private void RunMultipleActions(IEnumerable<ICutsceneAction> cutsceneActions)
    {
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