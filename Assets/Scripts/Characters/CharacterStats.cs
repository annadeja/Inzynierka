using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//!Klasa przechowująca i obsługująca statystyki wszystkich postaci.
[Serializable]
public class CharacterStats 
{
    //Wszytkie pola tej klasy są przedstawione jako wartości publiczne zamiast właściwości w celu edycji ich wartość w edytorze Unity.
    [Header("Progression stats")]
    public int Level; //!<Poziom postaci.
    public int CurrentXP; //!<Punkty doświadczenia.
    public int ToNextLevel; //!<Ilość punktów doświadczenia niezbędna do przejścia na nowy poziom.
    public int RewardXP; //!<Ilość punktów doświadczenia ofiarowana graczowi po śmierci.
    public int SkillPoints; //!<Ilość punktów do ulepszenia statystyk.

    [Header("Combat stats")]
    public int MaxHP; //!<Maksymalne zdrowie.
    public int CurrentHP; //!<Aktualne zdrowie.
    public int Attack; //!<Siła ataku.
    public int Defense; //!<Siła obrony.
    public float AttackCooldown; //!<Obecny czas odświeżania ataku.
    public float MaxCooldown; //!<Minimalny czas pomiędzy atakami/maksymalny czas odświeżania.

    [Header("Dialog stats")]
    public int Charisma; //!<Poziom charyzmy postaci.
    public int Deception; //!<Poziom oszutswa postaci.
    public int Thoughtfulness; //!<Poziom pomyślunku postaci.

    public CharacterStats()
    {
        Level = 1;
        CurrentXP = 0;
        ToNextLevel = 10;
        RewardXP = 10;
        SkillPoints = 0;

        MaxHP = 20;
        CurrentHP = MaxHP;
        Attack = 10;
        Defense = 10;
        AttackCooldown = 2.0f;
        MaxCooldown = AttackCooldown;

        Charisma = 10;
        Deception = 10;
        Thoughtfulness = 10;
    }
    //!Zadaje określoną liczbę obrażeń postaci.
    public void takeDamage(int damage)
    {
        CurrentHP -= damage;
    }
    //!Obniża czas odświeżania w zalezności od czasu rzeczywistego.
    public void decreaseCooldown(float time)
    {
        AttackCooldown -= time;
    }
    //!Pozwala na zyskanie doświadczenia oraz przejscie na nowy poziom gdy warunki do tego zostaną spełnione.
    public void levelUp(int experience)
    {
        CurrentXP += experience;
        if (CurrentXP >= ToNextLevel)
        {
            Level++;
            CurrentXP = 0;
            ToNextLevel *= 2;
            MaxHP = (int)((float)MaxHP * 1.25);
            SkillPoints = 3;
            Debug.Log("Leveled up.");
        }
    }
}
