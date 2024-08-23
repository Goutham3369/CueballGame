using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelHandler : MonoBehaviour
{
    public static LevelHandler instance;
    [HideInInspector]public SlotHandler[] slotHandlers;
    public LevelData[] levelList;
    LevelData levelData;
    public Text levelText;
    public BoardRotator _boardRotator;
    public bool showGrayBalls = true;
    public bool shouldBallFollow = true;
    public int levelWinReward=100;

    public int currentLevel
    {
        get { return PlayerPrefs.GetInt(nameof(currentLevel), 0); }
        set { PlayerPrefs.SetInt(nameof(currentLevel), value); }
    }

    public GameObject VictoryPanel, WinPanel1st, WinPanel2nd, WinPanel3rd, lostPanel, confetti;
    public Text moveAllowTxt;
    [HideInInspector] public bool isGamePaused;

    public bool isLevelComplete;


    public int allowedMoves;
    public int curentMoves;  
    private void Awake()
    {
        instance = this;
        slotHandlers = GetComponentsInChildren<SlotHandler>();
        levelData = levelList[currentLevel];
    }
    public void UpdateLevelList()
    {
        levelList = Resources.LoadAll<LevelData>("Levels");
    }
    private void OnEnable()
    {
        moveAllowTxt = GameObject.FindWithTag("MoveAllow").GetComponent<Text>();   
        moveAllowTxt.text = "Allowed Moves " + (allowedMoves- curentMoves);
        InstantiateSlots();
        SlotHandler.onBallAdded += CheckIfLevelComplete;
    }
    private void Start()
    {
        levelText.text = "LEVEL " + (currentLevel+1);  
    }
    private void OnDisable()
    {
        SlotHandler.onBallAdded -= CheckIfLevelComplete;
    }
    void InstantiateSlots()
    {
        for (int i = 0; i < slotHandlers.Length; i++)
        {
            var ballSlot = levelData.ballSlots[i];
            if(ballSlot.isLocked)
                slotHandlers[i].LockSlot();
            else
            {
                var colorIndices = levelData.ballSlots[i].ballHolders.Select(ballHolder => (int)ballHolder.selectedColorEnum).ToArray();
                slotHandlers[i].InstantiateBalls(colorIndices, showGrayBalls);
            }
        }
    }


    public void CheckIfLevelComplete()
    {

        foreach (var bottle in slotHandlers)
        {
            if (!bottle.isSlotClosed)
            {
                if (!bottle.IsEmpty() && !bottle.isLocked){

                    if (++curentMoves>=allowedMoves) {
                        lostPanel.SetActive(true);
                        isLevelComplete = isGamePaused = true;
                    }
                    moveAllowTxt.text = "Allowed Moves " + (allowedMoves - curentMoves);
                    return;
                }
            }
        }
        isLevelComplete = isGamePaused = true;
        //if(PrefHandlre.RateUsPanelpref==1)
        ShowVictoryPanel();
        //else
        //{
        //   if ((PlayerPrefs.GetInt("LevelModel", 0) % 5) == 4 && !(PlayerPrefs.GetInt("LevelModel", 0) == 0))
        //    {
        //        UiObjectsContainer.Instance.RateUsPanel.SetActive(true);
        //    }
        //   else
        //    {
        //        ShowVictoryPanel();
        //    }
        //}
    }



    public void ShowVictoryPanel()
    {
        confetti.SetActive(true);
        PrefHandlre.Coins += levelWinReward;
        UiObjectsContainer.Instance.UpdateCoinsUI(PrefHandlre.Coins);
        _boardRotator.StartRotatioOnLevelEnd();
        AudioManager.Instance.PlayLevelWinSound();
        Invoke(nameof(OpenVictoryPanel), 2);
        
    }

    private void OpenVictoryPanel()
    {
        VictoryPanel.SetActive(true);
        Invoke(nameof(ChangePanel), 2);
    }
    
    void ChangePanel()
    {
        WinPanel1st.SetActive(false);
        confetti.SetActive(false);
        //if (PlayerPrefs.GetInt("LevelModel",0) %5 == 0 && !(PlayerPrefs.GetInt("LevelModel", 0) == 0))
        //{
        //    WinPanel2nd.SetActive(true);
        //}
        //else
        {
            WinPanel3rd.SetActive(true);
        }
    }
    public void PauseGame()
    {
        isGamePaused = true;
    }
    public void UnpauseGame()
    {
        isGamePaused = false;
    }

    public void NextLevel()
    {
        if (++currentLevel>= levelList.Length)  
            currentLevel = 0;
        PlayerPrefs.SetInt("LevelModel",PlayerPrefs.GetInt("LevelModel",0) + 1);
        SceneManager.LoadScene(0);
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
    public bool CheckIfAnySlotLocked()
    {
        for (int i = 0; i < levelData.ballSlots.Length; i++)
            if (levelData.ballSlots[i].isLocked == true)
                return true;

        return false;
    }
    
}
