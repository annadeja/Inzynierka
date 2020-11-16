using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Character Data", menuName = "CharacterData")]
[Serializable]
public class CharacterData : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public Color TextColor;
}
