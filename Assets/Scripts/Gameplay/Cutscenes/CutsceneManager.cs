using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class CutsceneManager : MonoBehaviour
{
    public Scene currentScene;

    PlayerController player;

    [SerializeField] GameObject luluPrefab;
    [SerializeField] GameObject naomiPrefab;

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
        //if (currentScene.name == "Intro" && !IsRunning)
        //{
        //    RunCutscene(IntroCutscene);
        //}
        //else if (currentScene.name == "3_Island_n" && !IsRunning && !naomiCutsceneComplete)
        //{
        //    Debug.Log("Naomi Cutscene");
        //    RunCutscene(NaomiCutscene);
        //}
    }

    // change game mode
    public void StartCutscene()
    {
        OnStartCutscene?.Invoke();
    }

    public void EndCutscene()
    {
        OnEndCutscene?.Invoke();
    }

    public void RunIntroCutscene()
    {
        StartCutscene();
        IntroCutscene();
    }

    public void RunNaomiCutscene()
    {
        StartCutscene();
        NaomiFirstCutscene();

        GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.NaomiCutscene, CheckpointState.Complete);
    }

    private void IntroCutscene()
    {
        AudioManager.Instance.PlayMainMusic();

        GameObject npc = Instantiate(luluPrefab, new Vector3(2, 0, 0), Quaternion.identity);
        npc.SetActive(false);
        Character npcChar = npc.GetComponent<Character>();

        RunMultipleActions(new ICutsceneAction[] {
            new FadeOutAction(fader, 0.5f),
            new DialogueAction(new SingleDialogue[]
            {
                new SingleDialogue("Joce", "..."),
                new SingleDialogue("Joce", "..."),
                new SingleDialogue("???", "You're awake!"),
            }),
            new FaceDirectionAction(player.Character, Vector2.left),
            new WaitAction(1),
            new FaceDirectionAction(player.Character, Vector2.right),
            new WaitAction(1),
            new FaceDirectionAction(player.Character, Vector2.down),
            new DialogueAction(new SingleDialogue[]
            {
                new SingleDialogue("Joce", "... Where am I? Who are you?"),
                new SingleDialogue("???", "Oh! How silly, I should introduce myself!"),
            }),         
            new SetActiveAction(npc, true),
            new FaceDirectionAction(npcChar, Vector2.left),
            new FaceDirectionAction(player.Character, Vector2.right),
            new DialogueAction(new SingleDialogue[]
            {
                new SingleDialogue("Lulu", "I'm Lulu, one of your childhood friends! And I'm here to guide you on your journey."),
                new SingleDialogue("Joce", "Wait… If you're my childhood friend, how come I don't remember you?"),
                new SingleDialogue("Lulu", "A magic spell stole your memories, and now you have to go on a journey to retrieve them."),
                new SingleDialogue("Lulu", "At the end, you will receive your heart's desire. But many trials will stand in your way, including familiar faces."),
                new SingleDialogue("Lulu", "Defeat them, and you will receive your memories again!"),
            }),
            new SetActiveAction(npc, false),
            new WaitAction(1),
            new FaceDirectionAction(player.Character, Vector2.down),
            new FadeInAction(fader, 0.5f),
            new ChangeSceneAction(SceneMapper.Instance.GetBuildIndexBySceneName("House")),
            new FadeOutAction(fader, 0.5f)
        });
    }

    private void NaomiFirstCutscene()
    {
        GameObject npc = Instantiate(naomiPrefab, new Vector3(9.5f, 12.5f, 0f), Quaternion.identity);
        npc.GetComponent<NPCController>().npcType = NPCType.Naomi;
        Character npcChar = npc.GetComponent<Character>();

        RunMultipleActions(new ICutsceneAction[]
        {
            new DialogueAction(new SingleDialogue[]
            {
                new SingleDialogue("???", "!! Wait!!!"),
                new SingleDialogue("Joce", "!!")
            }),
            new FaceDirectionAction(player.Character, Vector2.right),
            new MoveAction(npcChar, new Vector2(-8.5f, 0f)),
            new DialogueAction(new SingleDialogue[]
            {
                new SingleDialogue("???", "You're finally here! Now I can cuddle you FOREVER!!"),
                new SingleDialogue("Joce", "W - what? I don't know who you are, I don't want to cuddle you!"),
                new SingleDialogue("???", "What! You always wanted to cuddle me before!"),
                new SingleDialogue("Joce", "Somehow, I don't think that's true..."),
                new SingleDialogue("???", "Okay fine, I might be exaggerating."),
                new SingleDialogue("???", "But if you want to remember me, you have to escape me first!"),
            }),
            new FaceDirectionAction(npcChar, Vector2.down)
        }, () => ChasingGameSystem.Instance.StartGame());
    }

    public void SpawnNaomi()
    {
        GameObject npc = Instantiate(naomiPrefab, new Vector3(10f, 12.5f, 0f), Quaternion.identity);
        npc.GetComponent<NPCController>().npcType = NPCType.Naomi;
    }

    public void NaomiTryAgainDialogue(NPCController naomi, Vector2 facingDirection)
    {
        naomi.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", $"I caught you in only {ChasingGameSystem.Instance.FinishTime} seconds!"),
                new SingleDialogue("???", "You can do better than that!")
            }),
            facingDirection,
            () => ChasingGameSystem.Instance.StartGame());
    }

    public void NaomiCompletedDialogue(NPCController naomi, Vector2 facingDirection)
    {
        naomi.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", $"Wow! You evaded me for {ChasingGameSystem.Instance.FinishTime} seconds. Do you remember my name now?"),
                new SingleDialogue("Joce", "Naomi! I can't believe I forgot about you!"),
                new SingleDialogue("Naomi", "That’s okay, you remember me now!"),
                new SingleDialogue("Naomi", "*HUGS*")
            }),
            facingDirection);
    }

    public void JaneFirstDialogue(NPCController jane, Vector2 facingDirection)
    {
        jane.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", "Halt! You don't remember me, do you?"),
                new SingleDialogue("Joce", "No...but your cat is cute!"),
                new SingleDialogue("???", "Thank you! Her name is Dumpling, and I'm OBSESSED."),
                new SingleDialogue("???", "But WAIT! Don't let me get off topic. You must battle me to regain our memories together!"),
                new SingleDialogue("Joce", "Wait, I don't even know how to fight!"),
                new SingleDialogue("???", "Well, first you need a companion who is willing to fight for you!"),
                new SingleDialogue("Joce", "Oh yeah, I have Ollie!"),
                new SingleDialogue("???", "Yeah! And Ollie will know a certain set of attacks that he can use during battle."),
                new SingleDialogue("???", "Some may do damage, and others may not. Attacks can only be used a certain number of times, or Power Points (PP)!"),
                new SingleDialogue("???", "Each companion has a health bar (HP). When the health of your opponent's companion is down to 0, you win!"),
                new SingleDialogue("Joce", "I think I get it..."),
                new SingleDialogue("Joce", "(...the way she explains things like this feels familiar to me for some reason...)"),
                new SingleDialogue("Joce", "Ok, I'm ready to battle!")
            }
            ), facingDirection,
            () => BattleSystem.Instance.StartBattle());
        GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.PokemonBattle, CheckpointState.StartedButNotComplete);
    }

    public void JaneBattleAgainDialogue(NPCController jane, Vector2 facingDirection)
    {
        jane.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", "Here to battle me again? Let's do it!")
               
            }
            ), facingDirection,
            () => BattleSystem.Instance.StartBattle()
            );
    }

    public void JaneAfterBattleDialogue(NPCController jane, Vector2 facingDirection)
    {
        jane.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", "Nooo, Dumpling!! Joce, you've beaten me!"),
                new SingleDialogue("Joce", "I remember you now! I can't believe I forgot about you!"),
                new SingleDialogue("Jane", "It's okay! Good luck on the rest of your journey!"),
                new SingleDialogue("Jane", "Meanwhile, I'll have to go find where Dumpling ran off to."),
                new SingleDialogue("Joce", "Goodbye!!"),

            }
            ), facingDirection,
            () => BattleSystem.Instance.StartBattle()
            );
    }


    private void RunMultipleActions(IEnumerable<ICutsceneAction> cutsceneActions, Action onFinished = null)
    {
        if (cutsceneActions.Count() == 0)
        {
            EndCutscene();
            onFinished?.Invoke();
            return;
        }

        StartCoroutine(cutsceneActions.First().PerformAction(() => {
            RunMultipleActions(cutsceneActions.Skip(1), onFinished);
        }));
    }
}
