using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]

public class PokemonBase : ScriptableObject
{

    [SerializeField] new string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;

    public List<Sprite> attackAnim;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    // Base Stats
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;
    [SerializeField] List<LearnableMove> learnableMoves;


    //public IEnumerator PlayAttackAnimation(float delay)
    //{
    //    Debug.Log("Animation playing");
    //    Sprite frontSpriteSaved = frontSprite;

    //    foreach(Sprite sprite in attackAnim)
    //    {
    //        frontSprite = sprite;
    //        yield return new WaitForSeconds(delay);
    //    }

    //    frontSprite = frontSpriteSaved;
    //}

    public string Name {
        // this is a property
        get { return name; }
    }

    public string Description {
        get { return description; }
    }

    public Sprite FrontSprite {
        get {return frontSprite; }
    }
    //public Sprite BackSprite {
    //    get {return backSprite; }
    //}

    public PokemonType Type1 {
        get {return type1; }
    }
    public PokemonType Type2 {
        get {return type2; }
    }

    // Base Stats
    public int MaxHp {
        get {return maxHp; }
    }
    public int Attack {
        get {return attack; }
    }
    public int Defense {
        get {return defense; }
    }
    public int SpAttack {
        get {return spAttack; }
    }
    public int SpDefense {
        get {return spDefense; }
    }
    public int Speed {
        get {return speed; }
    }

    public List<LearnableMove> LearnableMoves {
        get {
            return learnableMoves;
        }
    }


}

[System.Serializable]

public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base {
        get {
            return moveBase;
        }
    }

    public int Level {
        get {
            return level;
        }
    }

}

public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon
}

