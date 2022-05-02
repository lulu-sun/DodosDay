using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class CutsceneManager : MonoBehaviour
{
    public Scene currentScene;

    public bool IsRunning;

    PlayerController player;

    [SerializeField] GameObject npcPrefab;

    public event Action OnStartCutscene;
    public event Action OnEndCutscene;

    Fader fader;

    public static CutsceneManager Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        currentScene = SceneManager.GetActiveScene();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameController.Instance.playerController;
        fader = FindObjectOfType<Fader>();
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        if (currentScene.name == "Intro" && !IsRunning)
        {
            RunCutscene(IntroCutscene);
        }
    }

    public void StartCutscene()
    {
        IsRunning = true;
        OnStartCutscene?.Invoke();
    }

    public void EndCutscene()
    {
        IsRunning = false;
        OnEndCutscene?.Invoke();
    }

    public void RunCutscene(Action cutscene)
    {
        StartCutscene();

        cutscene.Invoke();
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
            new FaceDirectionAction(player.Character, Vector2.down),
            new FadeInAction(fader, 0.5f),
            new ChangeSceneAction(1),
            new FadeOutAction(fader, 0.5f)
        });
    }

    private void RunMultipleActions(IEnumerable<ICutsceneAction> cutsceneActions, Action onFinished = null)
    {
        if (cutsceneActions.Count() == 0)
        {
            onFinished?.Invoke();
            EndCutscene();
            return;
        }

        StartCoroutine(cutsceneActions.First().PerformAction(() => {
            RunMultipleActions(cutsceneActions.Skip(1));
        }));
    }
}