using UnityEngine;
[System.Serializable]

public class Prisoner
{
    public string name;
    public int hp;
    public int proficiency;
    public int strength;
    public string crime;
    public int erosion;
    public Sprite head;
    public Sprite body;

    public Prisoner(string name, int hp, int proficiency, int strength, string crime, int erosion, Sprite head, Sprite body)
    {
        this.name = name;
        this.hp = hp;
        this.proficiency = proficiency;
        this.strength = strength;
        this.crime = crime;
        this.erosion = erosion;
        this.head = head;
        this.body = body;
    }
}
