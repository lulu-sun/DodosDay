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
        MemoriesSystem.Instance.SetActive(false);

        GameObject npc = Instantiate(luluPrefab, new Vector3(player.transform.position.x + 2, player.transform.position.y, 0), Quaternion.identity);
        npc.SetActive(false);
        Character npcChar = npc.GetComponent<Character>();

        RunMultipleActions(new ISingleCutsceneAction[] {
            new FaceDirectionAction(player.Character, Vector2.down),
            new FadeInAction(fader, 0.5f),
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
            new FadeOutAction(fader, 0.5f),
            new ChangeSceneAction(SceneMapper.Instance.GetBuildIndexBySceneName("House")),
            new CustomAction(() =>
            {
                player.transform.position = FindObjectsOfType<SpawnPoint>().Single(sp => sp.portalId == PortalId.E).gameObject.transform.position;
            }),
            new FadeInAction(fader, 0.5f)
        }, () => MemoriesSystem.Instance.SetActive(true));
    }

    public void OllieFirstDialogue(NPCController ollie, Vector2 facingDirection)
    {
        ollie.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", "Bork!"),
                new SingleDialogue("Joce", "What a cute dog!"),
                new SingleDialogue("???", "Bork!"),
                new SingleDialogue("Joce", "Your name tag says...Ollie? Ollie!"),
                new SingleDialogue("Ollie", "!!!!"),
            }),
            facingDirection);
        GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.MeetingOllie, CheckpointState.StartedButNotComplete);
    }

    public void OllieBorkDialogue(NPCController ollie, Vector2 facingDirection)
    {
        ollie.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("Ollie", "Bork!"),
                new SingleDialogue("Joce", "Good boy!!!!"),
            }),
            facingDirection);
    }

    public void DumplingDialogue(NPCController dumpling, Vector2 facingDirection)
    {
        dumpling.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("Dumpling", "Meow"),
            }),
            facingDirection);
    }

    public void OllieJaneIslandDialogue(NPCController ollie, Vector2 facingDirection)
    {
        ollie.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("Ollie", "Bork!"),
                new SingleDialogue("Joce", "What are you doing here?"),
            }),
            facingDirection);
    }

    private void NaomiFirstCutscene()
    {
        GameObject npc = Instantiate(naomiPrefab, new Vector3(9.5f, 12.35f, 0f), Quaternion.identity);
        npc.GetComponent<NPCController>().npcType = NPCType.Naomi;
        Character npcChar = npc.GetComponent<Character>();
        npcChar.moveSpeed = player.Character.moveSpeed * 1.05f;

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
                new SingleDialogue("???", "But you don't have a choice, because I'm going to hug you anyway!"),
                new SingleDialogue("Joce", "What?? No!!"),
            }),
            new FaceDirectionAction(player.Character, Vector2.left),
            new MoveAction(player.Character, new Vector2(-0.7f, 0f)),
            new FaceDirectionAction(player.Character, Vector2.down),
            new MultipleSimultaneousCutsceneAction(new ISingleCutsceneAction[]
            {
                new MoveAction(npcChar, new Vector2(-1.8f, 0f)),
                new MoveAction(player.Character, new Vector2(0f, -1.8f)),
            }),
            new FaceDirectionAction(npcChar, Vector2.down),
            new MultipleSimultaneousCutsceneAction(new ISingleCutsceneAction[]
            {
                new MoveAction(npcChar, new Vector2(0f, -8f)),
                new MoveAction(player.Character, new Vector2(0f, -8f))
            }),
        },
        () => ChasingGameSystem.Instance.StartGame());
    }

    public void SpawnNaomi()
    {
        GameObject npc = Instantiate(naomiPrefab, new Vector3(-1.1f, 3.9f, 0f), Quaternion.identity);
        npc.GetComponent<NPCController>().npcType = NPCType.Naomi;
    }

    public void NaomiTryAgainDialogue(NPCController naomi, Vector2 facingDirection)
    {
        naomi.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", "It was so easy to catch you!"),
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
                new SingleDialogue("???", "Wow! You evaded me for FOREVER!!! Do you remember my name now?"),
                new SingleDialogue("Joce", "Naomi! I can't believe I forgot about you!"),
                new SingleDialogue("Naomi", "That’s okay, you remember me now!"),
                new SingleDialogue("Naomi", "*HUGS*")
            }),
            facingDirection,
            () =>
            {
                if (GameCheckpoints.Instance.NotComplete(Checkpoint.NaomiChasingMemoryRecorded))
                {
                    MemoriesSystem.Instance.MarkMemoryFound();
                    GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.NaomiChasingMemoryRecorded, CheckpointState.Complete);
                }
            });
    }

    public void NaomiChaseAgainDialogue(NPCController naomi, Vector2 facingDirection)
    {
        naomi.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("Naomi", "I LOVE cuddles!!")

            }),
            facingDirection,
            () => ChasingGameSystem.Instance.StartGame());
    }

    public void JaneFirstDialogue(NPCController jane, Vector2 facingDirection)
    {
        jane.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", "Joce! You don't remember me, do you?"),
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
            }),
            facingDirection,
            () => BattleSystem.Instance.StartBattle());

        GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.PokemonBattle, CheckpointState.StartedButNotComplete);
    }

    public void JaneBattleAgainDialogue(NPCController jane, Vector2 facingDirection)
    {
        jane.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("Jane", "Here to battle me again? Let's do it!")
               
            }), facingDirection,
            () => BattleSystem.Instance.StartBattle());
    }

    public void JaneAfterBattleDialogue(NPCController jane, Vector2 facingDirection)
    {
        jane.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", "Nooo, Dumpling!! Joce, you've beaten me!"),
                new SingleDialogue("Joce", "I remember you now! I can't believe I forgot about you!"),
                new SingleDialogue("Jane", "It's okay, I still love you! Good luck on the rest of your journey!"),
                new SingleDialogue("Joce", "Goodbye!!"),

            }),
            facingDirection,
            () =>
            {   
                if (GameCheckpoints.Instance.NotComplete(Checkpoint.PokemonBattleMemoryRecorded))
                {
                    GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.PokemonBattleMemoryRecorded, CheckpointState.Complete);
                    MemoriesSystem.Instance.MarkMemoryFound();
                }
            });
    }

    public void JaneGameEndDialogue(NPCController jane, Vector2 facingDirection)
    {
        jane.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("Jane", "Ready for a rematch?"),
            }), facingDirection,
            () => BattleSystem.Instance.StartBattle());
    }

    public void RadioStartMusic(NPCController radio, Vector2 facingDirection)
    {
        radio.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("Radio", "Playing: Love Me Or Leave Me by Day6"),
            }), facingDirection,
            () => AudioManager.Instance.PlayDay6Music());

        GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.RadioPlayingMusic, CheckpointState.Complete);
    }


    public void RadioAlreadyPlayingMusic(NPCController radio, Vector2 facingDirection)
    {
        radio.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("Joce", "..."),
                new SingleDialogue("Joce", "there's no way to turn it off"),
            }), facingDirection);
    }

    public void JuanJuanFirstDialogue(NPCController juanjuan, Vector2 facingDirection)
    {
        Vector2 originalDirection = juanjuan.character.Direction;

        RunMultipleActions(new ICutsceneAction[]
        {
            new FaceDirectionAction(juanjuan.character, -facingDirection),
            new DialogueAction(new SingleDialogue[]
            {
                new SingleDialogue("???", " AAaaaAAahh!!! "),
                new SingleDialogue("Joce", "Are you ok?? Do you need help?"),
                new SingleDialogue("???", "JOCELYN! Yes! You came at just the right time!"),
                new SingleDialogue("Joce", "How can I help?"),
                new SingleDialogue("???", "I heard this arcade vending machine has the best ice cream on this island!"),
                new SingleDialogue("???", $"But it looks like you have to get at least {CatchingGameSystem.Instance.winningScore} points to win an ice cream cone, and I've been trying for so long..."),
                new SingleDialogue("Joce", "Oh? Let me see if I can help!"),
                new SingleDialogue("???", "Thanks so much! You have to catch the yellow ducklings, and avoid catching the rotten ones - you'll have 3 lives!"),
            }),
            new MoveAction(juanjuan.character, new Vector2(-1, 0)),
            new FaceDirectionAction(juanjuan.character, Vector2.right),
            new DialogueAction(new SingleDialogue[]
            {
                new SingleDialogue("???", "Give it a try!")
            })
        },
        () => GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.CatchingGame, CheckpointState.StartedButNotComplete));
    }

    public void JuanJuanTryAgainDialogue(NPCController juanjuan, Vector2 facingDirection)
    {
        juanjuan.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", $"You have to get at least {CatchingGameSystem.Instance.winningScore} points!"),
                new SingleDialogue("???", "Did you get the ice cream yet?"),                
            }), facingDirection);
    }

    public void JuanJuanCompletedDialogue(NPCController juanjuan, Vector2 facingDirection)
    {
        juanjuan.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", "Thank you so much!!"),
                new SingleDialogue("Joce", "JuanJuan! I remember you now!!"),
                new SingleDialogue("JuanJuan", "Wait, you forgot about me?"),
                new SingleDialogue("Joce", "Nevermind, I hope you enjoy your ice cream!"),
                new SingleDialogue("JuanJuan", "I will, thanks to you! I'll see you later!"),
                new SingleDialogue("Joce", "(Wait I will? Are you my heart's desire...?)"),
            }),
            facingDirection,
            () =>
            {
                if (GameCheckpoints.Instance.NotComplete(Checkpoint.CatchingGameMemoryRecorded))
                {
                    GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.CatchingGameMemoryRecorded, CheckpointState.Complete);
                    MemoriesSystem.Instance.MarkMemoryFound();
                }
            });
    }

    public void JuanJuanGameEndDialogue(NPCController juanjuan, Vector2 facingDirection)
    {
        juanjuan.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("JuanJuan", "If you want more ice cream, you can get it from the arcade machine!"),
                
            }), facingDirection);

    }

    public void StartArcadeGame(NPCController arcade, Vector2 facingDirection)
    {
        arcade.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("Arcade", "Starting game . . ."),

            }),
            facingDirection,
            () =>
            {
                CatchingGameSystem.Instance.StartGame();
                if (GameCheckpoints.Instance.NeverStarted(Checkpoint.CatchingGame))
                {
                    GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.CatchingGame, CheckpointState.StartedButNotComplete);
                }
            });
    }

    public void BlockArcadeGame()
    {
        RunMultipleActions(new ICutsceneAction[]
        {
            new DialogueAction(new SingleDialogue[]
            {
                new SingleDialogue("Arcade Machine", "Someone is already playing at the moment..."),
            })
        });
    }

    public void RachelFirstDialogue(NPCController rachel, Vector2 facingDirection)
    {
        rachel.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", "Jocelyn! You're here!"),
                new SingleDialogue("Joce", "Who are you again?"),
                new SingleDialogue("???", "You don't remember?"),
                new SingleDialogue("???", "I'll tell you my name if you help me with something!"),
                new SingleDialogue("Joce", "Okay, what do you need?"),
                new SingleDialogue("???", "I need some cheese to complete my cheese collection!"),
                new SingleDialogue("???", "There's some cheese lying around this island. Could you get 5 pieces for me?"),
                new SingleDialogue("Joce", "That's quite a lot of cheese. You really need that much?"),
                new SingleDialogue("???", "I LOVE CHEESE!"),
                new SingleDialogue("Joce", "I'll be right back with your cheese..."),

            }), facingDirection,
            () => CheeseGameSystem.Instance.StartGame());

        GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.CheeseGame, CheckpointState.StartedButNotComplete);
    }

    public void RachelWaitingDialogue(NPCController rachel, Vector2 facingDirection)
    {
        rachel.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", "Do you have all the cheese yet? Mmm, you kind of smell like cheese..."),
                
            }), facingDirection);
    }

    public void RachelHasCheeseDialogue(NPCController rachel, Vector2 facingDirection)
    {
        rachel.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", "YES! The cheese! Thank you so much!"),
                new SingleDialogue("???", "To fulfill my promise, my name is -"),
                new SingleDialogue("Joce", "RACHEL! I remember you now!"),
                new SingleDialogue("Rachel", "Finally!! Would you like to eat some of this cheese with me?"),
                new SingleDialogue("Joce", "I would love to, but I think I have to go. I'm looking for my heart's desire."),
                new SingleDialogue("Rachel", "Oh, it's not cheese? Well, I hope you find it, whatever it is!"),
                new SingleDialogue("Joce", "I hope so too!"),

            }),
            facingDirection,
            () => 
            {
                CheeseGameSystem.Instance.EndGame();
                if (GameCheckpoints.Instance.NotComplete(Checkpoint.CheeseGameMemoryRecorded))
                {
                    MemoriesSystem.Instance.MarkMemoryFound();
                    GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.CheeseGameMemoryRecorded, CheckpointState.Complete);
                }                
            });
    }

    public void RachelEndGameDialogue(NPCController rachel, Vector2 facingDirection)
    {
        rachel.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("Rachel", "Can you help me get some more cheese?"),

            }), facingDirection,
            () => CheeseGameSystem.Instance.StartGame());
    }

    public void EnterIceRinkThought()
    {
        RunMultipleActions(new ICutsceneAction[]
        {
            new DialogueAction(new SingleDialogue[]
            {
                new SingleDialogue("Joce", "The ice looks really slippery..."),
            })
        },
        () => GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.EnteredIceRink, CheckpointState.Complete));
    }

    public void NoelleFirstDialogue(NPCController noelle, Vector2 facingDirection)
    {
        noelle.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", "Hey! Are you ready to start studying?"),
                new SingleDialogue("Joce", "Study? I don't think I'm in school anymore. And who are you?"),
                new SingleDialogue("???", "Never mind that, we have a lot of work to do!"),
                new SingleDialogue("???", "The only thing that could make all this work tolerable would be some wings..."),
                new SingleDialogue("???", "I heard there's a Wings Over stand at the Norris ice rink. Could you pick some up?"),
                new SingleDialogue("???", "And you know what goes well with wings? A scary TV show…"),
                new SingleDialogue("Joce", "Hmm, this feels somewhat familiar to me. I'll be right back with the wings!"),

            }), facingDirection);

        GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.IceRinkGameAndChickenWings, CheckpointState.StartedButNotComplete);
    }

    public void NoelleWaitingDialogue(NPCController noelle, Vector2 facingDirection)
    {
        noelle.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", "Are you back with the wings?"),
            }), facingDirection);
    }

    public void NoelleCompletedDialogue(NPCController noelle, Vector2 facingDirection)
    {
        noelle.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("???", "You're back! Let’s watch American Horror Story!"),
                new SingleDialogue("Joce", " I thought we were supposed to be studying!"),
                new SingleDialogue("???", "Oh, right..."),
                new SingleDialogue("Joce", "Noelle! How could I have forgotten! We used to do this all the time together!"),
                new SingleDialogue("Noelle", "Now you're one step closer to remembering everything...and a special someone..."),
                new SingleDialogue("Joce", "Who?"),
                new SingleDialogue("Noelle", "Oops! Nothing! I think you'd better get going now!"),
                new SingleDialogue("Joce", "(Is this something related to my quest?)"),
            }),
            facingDirection,
            () =>
            {
                ChickenWingsSystem.Instance.SetActive(false);
                if (!GameCheckpoints.Instance.Complete(Checkpoint.ChickenWingsMemoryRecorded))
                {
                    MemoriesSystem.Instance.MarkMemoryFound();
                    GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.ChickenWingsMemoryRecorded, CheckpointState.Complete);
                }
            });
    }

    public void NoelleEndGameDialogue(NPCController noelle, Vector2 facingDirection)
    {
        noelle.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("Noelle", "I think I've had enough wings. But I think the Wings Over stand is still at the ice rink!"),
            }), facingDirection);
    }


    public void SchoolSignDialogue(NPCController sign, Vector2 facingDirection)
    {
        sign.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("Joce", "Is this Northwestern??"),
                new SingleDialogue("Lulu", "GO CATS"),
                new SingleDialogue("Lulu", "Go U Northwestern, break right through that line, with your colors flying we will cheer you all the time U RAH RAH! Go U Northwestern fight for victory! Spread far the fame of our fair name,"),
                new SingleDialogue("Lulu", "Go Northwestern win that game GO CATS!!"),
                new SingleDialogue("Joce", "..."),
                new SingleDialogue("Joce", "Oh."),

            }), facingDirection);
    }

    public void WingsSignDialogue(NPCController sign, Vector2 facingDirection)
    {
        sign.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("Sign", "Wings Stop Over"),
                new SingleDialogue("Joce", "Wings Stop Over? That doesn't sound quite right."),

            }), facingDirection);
    }

    public void WingsShopFirstDialogue(NPCController shopKeeper, Vector2 facingDirection)
    {
        shopKeeper.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("Counter server", "Order up! Here are your chicken wings."),
            }),
            facingDirection,
            () =>
            {
                ChickenWingsSystem.Instance.SetActive(true);
                GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.IceRinkGameAndChickenWings, CheckpointState.Complete);
                AudioManager.Instance.PlayPopSfx();
            });
    }

    public void WingsShopWingsAcquiredDialogue(NPCController shopKeeper, Vector2 facingDirection)
    {
        shopKeeper.Talk(new Dialogue(
            new SingleDialogue[]
            {
                new SingleDialogue("Counter server", "Sorry, we're all out."),
            }),
            facingDirection);
    }

    public void EnterAllisonDialogue()
    {
        RunMultipleActions(new ISingleCutsceneAction[] {
            new DialogueAction(new SingleDialogue[]
            {
                new SingleDialogue("Joce", "It's bigger on the inside!"),
            })
        });

        GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.EnteredAllison, CheckpointState.Complete);
    }

    public void BlockFinalIsland()
    {
        RunMultipleActions(new ICutsceneAction[]
        {
            new DialogueAction(new SingleDialogue[]
            {
                new SingleDialogue("Lulu", "Hey, you haven't gotten all your memories back yet!")
            })
        });
    }

    public void BlockPortalsForCheeseGame()
    {
        RunMultipleActions(new ICutsceneAction[]
        {
            new DialogueAction(new SingleDialogue[]
            {
                new SingleDialogue("???", "Hey! What about my cheese?!")
            })
        });
    }

    public void RunFinalIslandCutscene()
    {
        StartCutscene();
        FinalIslandCutscene();
    }

    private void FinalIslandCutscene()
    {
        // Walk up and talk to phillip
        RunMultipleActions(new ICutsceneAction[]
        {
            new FaceDirectionAction(player.Character, Vector2.up),
            new DialogueAction(new SingleDialogue[]
            {
                new SingleDialogue("Joce", "!!!!")
            }),
            new MoveAction(player.Character, new Vector2(0f, 7.4f)),
            new DialogueAction(new SingleDialogue[]
            {
                new SingleDialogue("???", "You finally made it!"),
                new SingleDialogue("Joce", "I'm sorry, I don't know who you are."),
                new SingleDialogue("???", "That's okay, you will soon. Do you wish to accept your final quest?"),
                new SingleDialogue("Joce", "Yes."),
                new SingleDialogue("???", "Then take my hand."),
                new SingleDialogue("???", "Your last quest. . ."),
                new SingleDialogue("Phillip", "Our last quest is to spend the rest of our lives together, loving each other."),
                new SingleDialogue("Phillip", "Do you accept?"),
                new SingleDialogue("Joce", "Yes!"),
            }),
            new CustomAction(() =>
            {
                GameObject.Find("Phillip").GetComponent<Character>().moveSpeed = 2;
                player.Character.moveSpeed = 2;
            }),
            new MoveAction(player.Character, new Vector2(1f, 0f)),
            new FaceDirectionAction(player.Character, Vector2.down),
            new MoveAction(GameObject.Find("Phillip").GetComponent<Character>(), new Vector2(0f, -1f)),

            // fade to black
            new MultipleSimultaneousCutsceneAction(new ISingleCutsceneAction[]
            {
                new FadeOutAction(fader, 6f),
                new MusicFadeOutAction(6f),
                new MoveAction(player.Character, new Vector2(0f, -8f)),
                new MoveAction(GameObject.Find("Phillip").GetComponent<Character>(), new Vector2(0f, -8f)),
            }),
            new CustomAction(() => MemoriesSystem.Instance.SetActive(false)),
            new ChangeSceneAction(SceneMapper.Instance.GetBuildIndexBySceneName("Intro")),
            new FaceDirectionAction(player.Character, Vector2.down),
            new FadeInAction(fader, 3f),
            // fade out bg music

            // lulu npc appears
            new CustomAction(() => player.transform.position = Vector3.zero),
            new InstantiateAction(luluPrefab, new Vector2(2f, 0f)),
            new FaceDirectionAction(() => GameObject.Find("Lulu(Clone)").GetComponent<Character>(), Vector2.left),
            new FaceDirectionAction(player.Character, Vector2.right),

            // lulu npc talks
            new DialogueAction(new SingleDialogue[]
            {
                new SingleDialogue("Lulu", "You did it! You've regained all your memories and found your happily ever after."),
                new SingleDialogue("Lulu", "After restarting the game, you can check out some additional content we prepared for you!"),
                new SingleDialogue("Lulu", "On the title screen you can look out for two things!"),
                new SingleDialogue("Lulu", "The first is hidden in the front door of the house :)"),
                new SingleDialogue("Lulu", "And be sure to check the mailbox for some personal messages written to you!"),
                new SingleDialogue("Lulu", "Goodbye!"),
            }),

            new FadeOutAction(fader, 0.5f),

            // back to title screen
        }, () =>
        {
            Application.Quit();
        });
    }



    private void RunMultipleActions(IEnumerable<ICutsceneAction> cutsceneActions, Action onFinished = null)
    {
        if (cutsceneActions.Count() == 0)
        {
            EndCutscene();
            onFinished?.Invoke();
            return;
        }

        ICutsceneAction cutsceneAction = cutsceneActions.First();

        ISingleCutsceneAction singleCutsceneAction = cutsceneAction as ISingleCutsceneAction;

        if (cutsceneAction is MultipleSimultaneousCutsceneAction multipleCutsceneAction)
        {
            foreach (ISingleCutsceneAction c in multipleCutsceneAction.CutsceneActions.Skip(1))
            {
                StartCoroutine(c.PerformAction());
            }

            singleCutsceneAction = multipleCutsceneAction.CutsceneActions.First();
        }

        StartCoroutine(singleCutsceneAction.PerformAction(() => {
            RunMultipleActions(cutsceneActions.Skip(1), onFinished);
        }));

    }
}
