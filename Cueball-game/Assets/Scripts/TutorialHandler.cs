using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    public List<Collider> AllSlots = new List<Collider>();
    public List<int> Indexes = new List<int>();
    public List<int> Indexes2 = new List<int>();
    public List<GameObject> Hands;
    public int index;
    public int index2;
    // Start is called before the first frame update
    private void Awake()
    {
        return;
        SlotHandler.onBallAdded += PlayTutorial;
    }
    void Start()
    {
        return;
        if (PlayerPrefs.GetInt("LevelModel",0) == 0)
        {
            AllSlots[Indexes[index]].enabled = true;
            Hands[Indexes[index2]].SetActive(true);
        }
        if (PlayerPrefs.GetInt("LevelModel", 0) == 2)
        {
            UiObjectsContainer.Instance.SetUiForRotator();
        }
        if (PlayerPrefs.GetInt("LevelModel", 0) == 3 && PrefHandlre.HammarTutorial == 0)
        {
            UiObjectsContainer.Instance.SetUIForHammer();
        }
    }

    public void CloseSlot()
    {
        return;
        if (PlayerPrefs.GetInt("LevelModel", 0) == 0)
        {
            Debug.Log("Tutorial");
            for (int i = 0; i < Hands.Count; i++)
            {
                Hands[i].SetActive(false);
            }
            AllSlots[Indexes[index]].enabled = false;
            index++;
            index2++;
            if (index < Indexes.Count)
            {
                Hands[Indexes2[index2]].SetActive(true);
            }

            if (index < Indexes.Count)
            {
                AllSlots[Indexes[index]].enabled = true;
            }
        }
    }
    
    public void EnableSlot()
    {
        // if (index < Indexes.Count)
        // {
        //     AllSlots[Indexes[index]].enabled = false;
        //     index++;
        //     if (PlayerPrefs.GetInt("LevelModel",0) == 0)
        //     {
        //         AllSlots[Indexes[index]].enabled = true;
        //     }
        // }
    }
    
    // Update is called once per frame

    private void OnDestroy()
    {
        return;
        SlotHandler.onBallAdded += PlayTutorial;
    }
    public int move = 0;
    void PlayTutorial()
    {
        return;
        if (PlayerPrefs.GetInt("LevelModel", 0) == 1)
        {
            move++;
            //print("move " + move);
            //print("hint tutorial " + PrefHandlre.HintTutorial);
            if (PrefHandlre.UndoTutorial == 0)
            {
                UiObjectsContainer.Instance.SetUIForUndo();
            }
            else if (PrefHandlre.HintTutorial == 0 && move>4)
            {
                UiObjectsContainer.Instance.SetUIForHint();
            }

        }
        
    }
}