using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public interface ICutsceneAction
{
    public IEnumerator PerformAction(Action onFinished = null);
}

public class DialogueAction : ICutsceneAction
{
    private Dialogue dialogue;

    public DialogueAction(SingleDialogue[] dialogues)
    {
        dialogue = new Dialogue(dialogues);
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

public class ChangeSceneAction : ICutsceneAction
{
    private int sceneToLoad;

    public ChangeSceneAction(int sceneToLoad)
    {
        this.sceneToLoad = sceneToLoad;
    }

    public IEnumerator PerformAction(Action onFinished = null)
    {
        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        onFinished?.Invoke();
    }
}

public class FadeInAction : ICutsceneAction
{
    private Fader fader;

    private float fadeTimeInSeconds;

    public FadeInAction(Fader fader, float fadeTimeInSeconds)
    {
        this.fader = fader;
        this.fadeTimeInSeconds = fadeTimeInSeconds;
    }

    public IEnumerator PerformAction(Action onFinished = null)
    {
        yield return fader.FadeIn(fadeTimeInSeconds);

        onFinished?.Invoke();
    }
}

public class FadeOutAction : ICutsceneAction
{
    private Fader fader;

    private float fadeTimeInSeconds;

    public FadeOutAction(Fader fader, float fadeTimeInSeconds)
    {
        this.fader = fader;
        this.fadeTimeInSeconds = fadeTimeInSeconds;
    }

    public IEnumerator PerformAction(Action onFinished = null)
    {
        yield return fader.FadeOut(fadeTimeInSeconds);

        onFinished?.Invoke();
    }
}