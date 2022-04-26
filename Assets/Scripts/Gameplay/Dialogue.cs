using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField] List<string> lines;

    public List<string> Lines { get => lines; }
    public string Name { get; set; }

    public Dialogue(string name, List<string> lines)
    {
        this.lines = lines;
        this.Name = name;
    }
}
