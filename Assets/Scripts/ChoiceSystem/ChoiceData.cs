using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//!Klasa przechowująca dane dotyczące danego wyboru.
[Serializable]
public class ChoiceData 
{
    public string PortName; //!<Nazwa portu/odpowiedzi.
    public string ChoiceTitle; //!<Tytuł wyboru.
    public bool WasMade; //!<Czy wyboru dokonano?
    public bool WasFailed; //!<Czy wybór był nieudany?
    public NarrativePath Path; //!<Ścieżka fabularna do jakiej przynależy wybór.
    public int RequiredCharisma; //!<Wymagana charyzma.
    public int RequiredDeception; //!<Wymagane oszustwo.
    public int RequiredThoughtfulness; //!<Wymagany pomyślunek.

    public ChoiceData(string portName, string choiceTitle = "Sample choice", bool wasMade = true)
    {
        this.PortName = portName;
        this.ChoiceTitle = choiceTitle;
        this.WasMade = wasMade;
        this.Path = NarrativePath.None;
        WasFailed = false;
        RequiredCharisma = 0;
        RequiredDeception = 0;
        RequiredThoughtfulness = 0;
    }
    //!Sprawdza czy gracz posiada wystarczająco wysoki poziom umiejętności by dokonać danego wyboru.
    public void skillCheck(CharacterStats playerStats) 
    {
        if (!(playerStats.Charisma >= RequiredCharisma && playerStats.Deception >= RequiredDeception && playerStats.Thoughtfulness >= RequiredThoughtfulness))
            WasFailed = true;
    }
}
