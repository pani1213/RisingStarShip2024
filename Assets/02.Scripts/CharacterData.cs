using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "character", menuName = "scriptable Obj/Caracter",order = int.MaxValue)]
public class CharacterData : ScriptableObject
{
    [SerializeField]
    private string caracter_Name;
    public string Character_name { get { return caracter_Name; } }
    [SerializeField]
    private int max_HP;
    public int Max_HP { get { return max_HP; } }


}
