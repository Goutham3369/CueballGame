using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PrefHandlre : MonoBehaviour
{
    public static int RateUsPanelpref
    {
        get { return PlayerPrefs.GetInt(nameof(RateUsPanelpref), 0); }
        set { PlayerPrefs.SetInt(nameof(RateUsPanelpref), value); }
    }
    public static int Coins
    {
        get { return PlayerPrefs.GetInt(nameof(Coins), 0); }
        set { PlayerPrefs.SetInt(nameof(Coins), value); }
    }
    public static int UndosAvailable
    {
        get { return PlayerPrefs.GetInt(nameof(UndosAvailable), 0); }
        set { PlayerPrefs.SetInt(nameof(UndosAvailable), value); }
    }
    public static int HammarAvailable
    {
        get { return PlayerPrefs.GetInt(nameof(HammarAvailable), 0); }
        set { PlayerPrefs.SetInt(nameof(HammarAvailable), value); }
    }
    public static int HintAvailable
    {
        get { return PlayerPrefs.GetInt(nameof(HintAvailable), 0); }
        set { PlayerPrefs.SetInt(nameof(HintAvailable), value); }
    }
    public static int HammarTutorial
    {
        get { return PlayerPrefs.GetInt(nameof(HammarTutorial), 0); }
        set { PlayerPrefs.SetInt(nameof(HammarTutorial), value); }
    }
    public static int UndoTutorial
    {
        get { return PlayerPrefs.GetInt(nameof(UndoTutorial), 0); }
        set { PlayerPrefs.SetInt(nameof(UndoTutorial), value); }
    }
    public static int HintTutorial
    {
        get { return PlayerPrefs.GetInt(nameof(HintTutorial), 0); }
        set { PlayerPrefs.SetInt(nameof(HintTutorial), value); }
    }
}
