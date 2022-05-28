using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum BattleState {
    Start, PlayerAction, PlayerMove, EnemyMove, Busy
}


public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] Camera worldCamera;
    [SerializeField] Camera battleSystemCamera;


    public static BattleSystem Instance { get; private set; }

    BattleState state;
    int currentAction;
    int currentMove;

    public event Action OnStartBattle;
    public event Action OnEndBattle;

    private void Awake()
    {
        Instance = this;
    }
    public void StartBattle()
    { 
        OnStartBattle?.Invoke();
        StartCoroutine(SetupBattle());
        AudioManager.Instance.PlayBattleMusic();
        battleSystemCamera.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false); 
    }

    public void EndBattle()
    { 
        OnEndBattle?.Invoke();
        AudioManager.Instance.FadeMusic(0.5f, 0f);
        battleSystemCamera.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true); 
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup();
        enemyUnit.Setup();


        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared.");
        // yield return new WaitForSeconds(1f);

        PlayerAction();

    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);

    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        var move = playerUnit.Pokemon.Moves[currentMove];


        yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} used {move.Base.Name}");
        move.PP--;

        yield return playerUnit.PlayAttackAnimation(0.3f);
        AudioManager.Instance.PlayOllieAttack();
        yield return new WaitForSeconds(1f);


        if (move.Base.Name == "Whine")
        {
            yield return dialogBox.TypeDialog("But nothing happened!");
        }
        

        if (move.Base.Power != 0)
        {
            yield return AudioManager.Instance.PlayTakeDamage();
            enemyUnit.PlayHitAnimation();
        }

        // yield return new WaitForSeconds(1f);


        var damageDetails = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);

        yield return enemyHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            //yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} Fainted");
            yield return dialogBox.TypeDialog("Dumpling lost interest!");
            yield return dialogBox.TypeDialog("Dumpling ran away!");
            enemyUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(1f);

            GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.PokemonBattle, CheckpointState.Complete);
            //Destroy(FindObjectsOfType<NPCController>().Where(npc => npc.gameObject.name == "Dumpling").Single().gameObject);

            EndBattle();
        }

        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        var move = enemyUnit.Pokemon.GetRandomMove();

        yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} used {move.Base.Name}");
        move.PP--;
        yield return enemyUnit.PlayAttackAnimation(0.3f);
        //enemyUnit.PlayAttackAnimation();
        AudioManager.Instance.PlayDumplingAttack();
        yield return new WaitForSeconds(1f);

        if (move.Base.Name == "Meow")
        {
            yield return dialogBox.TypeDialog("Ollie wants to be friends\nwith Dumpling!");
            yield return dialogBox.TypeDialog("Ollie's defense falls!");
            playerUnit.PlayHitAnimation();
        }

        if (move.Base.Name == "Hide")
        {
            yield return dialogBox.TypeDialog("Dumpling is scared of everything!");
            yield return dialogBox.TypeDialog("Dumpling's defense goes up!");
        }


        if (move.Base.Power != 0)
        {
            yield return AudioManager.Instance.PlayTakeDamage();
            playerUnit.PlayHitAnimation();
       
        }

        // yield return new WaitForSeconds(1f);

        var damageDetails = playerUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"Ollie is too afraid of Dumpling!");
            yield return dialogBox.TypeDialog($"Ollie ran away!");

            playerUnit.PlayFaintAnimation();
            yield return new WaitForSeconds(1f);
            EndBattle();
        }

        else
        {
            PlayerAction();
        }

    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        yield return null;
    }

    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }

        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }

    void HandleActionSelection()
    {
        if (Controls.GetDownKeyDown())
        {
            if (currentAction < 1)
            {
                ++ currentAction;
            }
        }

        else if (Controls.GetUpKeyDown())
        {
            if (currentAction > 0)
            {
                -- currentAction;
            }
        }

        dialogBox.UpdateActionSelection(currentAction);

        if (Controls.GetSelectKeyDown())
        {
            if (currentAction == 0)
            {
                //Fight
                PlayerMove();
            }
            else if (currentAction == 1)
            {
                //Run
                StartCoroutine(TypeRunAway());
                
            }
        }
    }

    IEnumerator TypeRunAway()
    {
        state = BattleState.Busy;

        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(true);
        dialogBox.EnableMoveSelector(false);

        
        yield return dialogBox.TypeDialog("You ran away ...");
        yield return new WaitForSeconds(0.5f);
        EndBattle();
    }

    void HandleMoveSelection()
    {
        if (Controls.GetRightKeyDown())
        {
            if (currentMove < playerUnit.Pokemon.Moves.Count - 1)
            {
                ++ currentMove;
            }
        }

        else if (Controls.GetLeftKeyDown())
        {
            if (currentMove > 0)
            {
                -- currentMove;
            }
        }

        else if (Controls.GetDownKeyDown())
        {
            if (currentMove < playerUnit.Pokemon.Moves.Count - 2)
            {
                currentMove += 2;
            }
        }

        else if (Controls.GetUpKeyDown())
        {
            if (currentMove > 1)
            {
                currentMove -= 2;
            }
        }

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Controls.GetSelectKeyDown())
        {
            

            var move = playerUnit.Pokemon.Moves[currentMove];

            if (move.PP > 0)
            {
                dialogBox.EnableMoveSelector(false);
                dialogBox.EnableDialogText(true);
                StartCoroutine(PerformPlayerMove());

            }
        }

    }

}
