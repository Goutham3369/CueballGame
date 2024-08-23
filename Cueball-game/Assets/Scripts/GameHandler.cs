using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;
    [Header("-1 if you are not testing else enter the level num")]
    public int  LoadCustomLevel=-1;
    public int Level;
    public List<GameObject> AllLevels = new List<GameObject>();
    public Camera _camera;
    public Hammer _hammer;
    [HideInInspector]
    public static int rewardNum;

    private void Awake()
    {
        instance = this;
        if (LoadCustomLevel!=-1)
        {
            PlayerPrefs.SetInt("LevelModel", LoadCustomLevel);
            PlayerPrefs.SetInt("currentLevel", LoadCustomLevel);
        }
        LoadLevel(PlayerPrefs.GetInt("LevelModel",0));
    }
    public Transform centerPoint;
    public SlotHandler firstSlot, secondSlot;
    public bool isUsingHammer;
    Coroutine bounceCoroutine;

    private void LoadLevel(int Index)
    {
        if(Index>4)
        UiObjectsContainer.Instance.ToughPanel.SetActive( (Index - 5) % 5 == 0 );
        if (Index <= AllLevels.Count - 2)
        {
            AllLevels[Index].SetActive(true);
        }
        else
        {
            _camera.orthographicSize = 7f;
            AllLevels[^1].SetActive(true);
        }
    }
    
    public void SelectSlot(SlotHandler slot)
    {
        if (slot.isSlotBusy)
        {
            //print("busy " + slot.name);
            return;
        }
       
        //print("selected " + slot.name);
        if (firstSlot == null)
        {
            if (slot.IsEmpty()) return;
            SetFirstSlot(slot);
        }
        else if (firstSlot == slot)
        {
            DeselectSlot(slot);
        }
        else if (secondSlot == null)
        {
            SetSecondSlot(slot);
        }
    }

    void SetFirstSlot(SlotHandler slot)
    {
        
        AudioManager.Instance.PlaySelectSound();
        firstSlot = slot;
        var ball = slot.GetTopBall().transform;
        if (slot.movingCoroutine != null)
            StopCoroutine(slot.movingCoroutine);
        slot.movingCoroutine =
            StartCoroutine(Helper.MoveToPosition(ball, centerPoint, 10f, () => { bounceCoroutine = StartCoroutine(Helper.BounceInfinite(ball, .8f, 2)); }));
    }

    void SetSecondSlot(SlotHandler slot)
    {
        secondSlot = slot;
        if (CanAddBall())
        {
            // FindObjectOfType<TutorialHandler>().CloseSlot();
            StartCoroutine(ShiftBall());
        }
        else
        {
            SwapFirstSlot();
        }
    }
    bool shiftedTwice;
    IEnumerator ShiftBall()
    {   
        StopCoroutine(bounceCoroutine);
        Ball movingBall = firstSlot.GetTopBall();
        firstSlot.isSlotBusy = true;
        secondSlot.isSlotBusy = true;
        if (!shiftedTwice)
        {
            if(firstSlot.movingCoroutine != null)
            StopCoroutine(firstSlot.movingCoroutine);
            yield return firstSlot.movingCoroutine = StartCoroutine(
                Helper.MoveToPosition(movingBall.transform, secondSlot.ballHolder[secondSlot.topIndex], 10f,
                () => { firstSlot.isSlotBusy = false; secondSlot.isSlotBusy = false; AudioManager.Instance.PlayCollisionSound(); }));
        }
        else
        {
            yield return firstSlot.movingCoroutine = StartCoroutine(
                Helper.MoveToPosition(movingBall.transform, centerPoint, 10f,
                () => {  AudioManager.Instance.PlayCollisionSound(); }));
            yield return firstSlot.movingCoroutine = StartCoroutine(
                Helper.MoveToPosition(movingBall.transform, secondSlot.ballHolder[secondSlot.topIndex], 10f,
                () => { firstSlot.isSlotBusy = false; secondSlot.isSlotBusy = false; AudioManager.Instance.PlayCollisionSound(); }));
        }

        firstSlot.RemoveTopBall();
        secondSlot.AddBallToTop(movingBall);
        if (LevelHandler.instance.shouldBallFollow && !firstSlot.IsEmpty() && CanAddBall())
        {
            shiftedTwice = true;
            SetSecondSlot(secondSlot);
        }
        else
        {
            lastFirstSlot = firstSlot;
            lastSecondSlot = secondSlot;
            firstSlot = secondSlot = null;
            shiftedTwice = false;
        }
       
        
    }   
    void SwapFirstSlot()
    {
        DeselectSlot(firstSlot);
        SetFirstSlot(secondSlot);
        secondSlot = null;
    }
    void DeselectSlot(SlotHandler slot)
    {
        slot.isSlotBusy = true;
        if (bounceCoroutine!=null)
            StopCoroutine(bounceCoroutine);
        if (slot.movingCoroutine != null)
            StopCoroutine(slot.movingCoroutine);
        
        Ball movingBall = slot.GetTopBall();

        slot.movingCoroutine = StartCoroutine(Helper.MoveToPosition(movingBall.transform, slot.ballHolder[slot.topIndex-1], 12f,()=> {
            
            if (slot == firstSlot) 
                firstSlot = null; 
            else if (slot == secondSlot) 
                secondSlot = null;
            slot.isSlotBusy = false;

        }));
    }
    bool CanAddBall()
    {
        return secondSlot.IsEmpty() || (firstSlot.GetTopBallIndex() == secondSlot.GetTopBallIndex() && !secondSlot.IsFull());
    }
    #region POWERUPS
    SlotHandler lastFirstSlot, lastSecondSlot;
    public void UndoMove()
    {
        if (lastSecondSlot == null)
        {
            UiObjectsContainer.Instance.ShowWarning("No move to Undo");
            return;

        }
        if (PrefHandlre.UndosAvailable <= 0)
        {
            UiObjectsContainer.Instance.OpenMoreItemsPanel(0);
            return;
        }
          
        PrefHandlre.UndosAvailable--;
        UiObjectsContainer.Instance.UpdatePowerUpsUi();
        if (PrefHandlre.UndoTutorial == 0)
        {
            UiObjectsContainer.Instance.SetUndoUiAfterClicked();

        }
        if (firstSlot != null) DeselectSlot(firstSlot);
        secondSlot = lastFirstSlot;
        StartCoroutine(StartUndo());
    }
    IEnumerator StartUndo()
    {
        SetFirstSlot(lastSecondSlot);
        yield return new WaitForSeconds(.15f);
        StartCoroutine(ShiftBall());
    }
    public void UseHammer()
    {
        if (!LevelHandler.instance.CheckIfAnySlotLocked())
        {
            UiObjectsContainer.Instance.ShowWarning("Can't Use this Power Up Right Now");
            return;

        }
        if (isUsingHammer)
        {
            UiObjectsContainer.Instance.ShowWarning("Already Activated");
            return;

        }
        if (PrefHandlre.HammarAvailable <= 0)
        {
            UiObjectsContainer.Instance.OpenMoreItemsPanel(1);
            return;
        }
        PrefHandlre.HammarAvailable--;
        UiObjectsContainer.Instance.UpdatePowerUpsUi();
        if (PrefHandlre.HammarTutorial == 0)
        {
           UiObjectsContainer.Instance.SetHammerUiAfterClicked();
           
        }
        isUsingHammer = true;
    }
    public void GetHint()
    {
        bool matchFound = false;
        var slotHandlers = LevelHandler.instance.slotHandlers;
        for (int i = 0; i < slotHandlers.Length - 1; i++)
        {
            if (slotHandlers[i].isLocked || slotHandlers[i].IsEmpty())
                continue;

            for (int j = i + 1; j < slotHandlers.Length; j++)
            {
                if (slotHandlers[j].isLocked || slotHandlers[j].IsEmpty() || slotHandlers[j].IsFull()) 
                    continue;
                int f = slotHandlers[i].GetTopBallIndex();
                int e = slotHandlers[j].GetTopBallIndex();

                if (slotHandlers[i].GetTopBallIndex() == slotHandlers[j].GetTopBallIndex())
                {
                    if (PrefHandlre.HintAvailable <= 0)
                    {
                        UiObjectsContainer.Instance.OpenMoreItemsPanel(2);
                        return;
                    }
                    PrefHandlre.HintAvailable--;
                    if (PrefHandlre.HintTutorial == 0)
                    {
                        UiObjectsContainer.Instance.SetHintUiAfterClicked();

                    }
                    UiObjectsContainer.Instance.UpdatePowerUpsUi();
                    StartCoroutine( Helper.Shake(slotHandlers[i].GetTopBall().transform,.6f,.05f));
                    StartCoroutine( Helper.Shake(slotHandlers[j].GetTopBall().transform,.6f,.05f));
                    matchFound = true;
                    return;  
                }
            }
        }
        if (!matchFound)
        {
            UiObjectsContainer.Instance.ShowWarning("No Match Found");
            UiObjectsContainer.Instance.SetHintUiAfterClicked();
        }
    }
    

    #endregion

    public void WatchAdsToSkipLevel()
    {

    }
    
    

}
