using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField] List<SingleDialogue> lines;

    public List<SingleDialogue> Dialogues { get => lines; }

    public Dialogue(IEnumerable<SingleDialogue> lines)
    {
        this.lines = new List<SingleDialogue>(lines);
    }

    public Dialogue()
    {
        this.lines = new List<SingleDialogue>();
    }

    public void AddDialogue(string name, string line)
    {
        this.lines.Add(new SingleDialogue(name, line));
    }
}

[System.Serializable]
public class SingleDialogue
{
    [SerializeField] string line;
    [SerializeField] string name;

    public string Line { get => line; }
    public string Name { get => name; }

    public SingleDialogue(string name, string line)
    {
        this.name = name;
        this.line = line;
    }
}