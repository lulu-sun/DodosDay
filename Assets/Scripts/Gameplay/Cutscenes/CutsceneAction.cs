using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public interface ICutsceneAction { }

public interface ISingleCutsceneAction : ICutsceneAction
{
    public IEnumerator PerformAction(Action onFinished = null);
}

public class MultipleSimultaneousCutsceneAction : ICutsceneAction
{
    private List<ISingleCutsceneAction> cutsceneActions;

    public MultipleSimultaneousCutsceneAction(IEnumerable<ISingleCutsceneAction> cutsceneActions)
    {
        this.cutsceneActions = new List<ISingleCutsceneAction>(cutsceneActions);
    }

    public IEnumerable<ISingleCutsceneAction> CutsceneActions { get => cutsceneActions; }
}

public class DialogueAction : ISingleCutsceneAction
{
    private Dialogue dialogue;

    public DialogueAction(IEnumerable<SingleDialogue> dialogues)
    {
        dialogue = new Dialogue(dialogues);
    }

    public IEnumerator PerformAction(Action onFinished = null)
    {
        return DialogueManager.Instance.ShowDialogue(dialogue, onFinished);
    }
}

public class FaceDirectionAction : ISingleCutsceneAction
{
    private Func<Character> getCharacter;

    private Vector2 direction;

    public FaceDirectionAction(Func<Character> getCharacter, Vector2 direction)
    {
        this.getCharacter = getCharacter;
        this.direction = direction;
    }

    public FaceDirectionAction(Character character, Vector2 direction) : this(() => character, direction)
    {
    }

    public IEnumerator PerformAction(Action onFinished = null)
    {
        yield return new WaitForEndOfFrame();

        getCharacter().FaceDirection(direction);

        onFinished?.Invoke();
    }
}

public class WaitAction : ISingleCutsceneAction
{
    private float seconds;

    public WaitAction(float seconds)
    {
        this.seconds = seconds;
    }

    public IEnumerator PerformAction(Action onFinished = null)
    {
        yield return new WaitForSeconds(seconds);

        onFinished?.Invoke();
    }
}

public class MoveAction : ISingleCutsceneAction
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

public class InstantiateAction : ISingleCutsceneAction
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

public class SetActiveAction : ISingleCutsceneAction
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

public class ChangeSceneAction : ISingleCutsceneAction
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

public class FadeOutAction : ISingleCutsceneAction
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

public class FadeInAction : ISingleCutsceneAction
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

public class MusicFadeOutAction : ISingleCutsceneAction
{
    private float fadeTimeInSeconds;

    public MusicFadeOutAction(float fadeTimeInSeconds)
    {
        this.fadeTimeInSeconds = fadeTimeInSeconds;
    }

    public IEnumerator PerformAction(Action onFinished = null)
    {
        yield return AudioManager.Instance.FadeMusicEnumerator(fadeTimeInSeconds, 0);

        onFinished?.Invoke();
    }
}

public class CustomAction : ISingleCutsceneAction
{
    private Action action;

    public CustomAction(Action action)
    {
        this.action = action;
    }

    public IEnumerator PerformAction(Action onFinished = null)
    {
        this.action?.Invoke();
        yield return null;

        onFinished?.Invoke();
    }
}