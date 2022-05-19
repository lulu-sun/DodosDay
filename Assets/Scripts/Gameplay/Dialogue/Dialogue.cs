using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField] List<string> lines;
    [SerializeField] List<string> names;

    public List<string> Lines { get => lines; }
    public List<string> Names { get => names; }

    public Dialogue(List<string> names, List<string> lines)
    {

        this.lines = lines;
        this.names = names;
    }
}
