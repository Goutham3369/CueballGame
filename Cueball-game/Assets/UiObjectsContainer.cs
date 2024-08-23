using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiObjectsContainer : MonoBehaviour
{
    public static UiObjectsContainer Instance;
    #region WinningPanelUI
    [Header("Victory Panel UI")]
    public Text levelWinCoinText;
    public CoinCollectAnimation coinAnimator, coinAnimator2;
    public Button claimButton;
    public GameObject CongratsPanel, LevelCompletePanel;
    public Image chestImage,chestImageCongratsPanel;
    public Sprite chestOpenSprit;
    public static Action<int> AdWatchedForPowerUps;
    public void OnPressingClaimButton()
    {
        PrefHandlre.Coins += LevelHandler.instance.levelWinReward;
        coinAnimator.OnClaimButtonPressed(LevelHandler.instance.levelWinReward);
        claimButton.interactable = false;
    }
    public void OpenCongratsPanel()
    {
        CongratsPanel.SetActive(true);
    }
    //public void GiveClaimButtonReward()
    //{
    //    PrefHandlre.Coins += LevelHandler.instance.levelWinReward;
    //    coinAnimator.OnClaimButtonPressed(LevelHandler.instance.levelWinReward);
    //    claimButton.interactable = false;
    //}
    public void OnPressingGetAllButton()
    {
        PrefHandlre.HammarAvailable += 3;
        PrefHandlre.Coins += 50;
        UpdatePowerUpsUi();
        coinAnimator2.OnClaimButtonPressed(50);
        chestImageCongratsPanel.sprite = chestOpenSprit;
        //CongratsPanel.SetActive(false);
        //LevelCompletePanel.SetActive(true);
    }
    public void ShowRewardedAd(int num)
    {

    }

    #endregion
    [Space(30)]

    
    public GameObject RateUsPanel;
    public GameObject ToughPanel;

    

    #region GamePlayUI

    [Header("More Item UI")]
    public GameObject moreItemPanel;
    public Image[] moreItemMainImages;
    public Sprite[] selectedMorePanelSprite;
    public Button addPowerThrougMoneyButton;
    int seletedPower;
    public void OpenMoreItemsPanel(int num)
    {
        seletedPower = num;
        moreItemPanel.SetActive(true);
        moreItemMainImages[0].sprite = moreItemMainImages[1].sprite = selectedMorePanelSprite[num];

    }
    public void AddPowerUpsThrougAds()
    {
        GameHandler.rewardNum = 1;

    }

    public void AddPowerUps(int moneyNeededToPowerUp)
    {
        print("selected power " + seletedPower);

        if (moneyNeededToPowerUp != -1)  // -1 mean that use watched the ad 
        {
            if (moneyNeededToPowerUp > PrefHandlre.Coins)
            {
                addPowerThrougMoneyButton.interactable = false;
                return;
            }
            addPowerThrougMoneyButton.interactable = true;
            PrefHandlre.Coins -= moneyNeededToPowerUp;
        }

        if (seletedPower == 0)
            PrefHandlre.UndosAvailable += 3;
        else if (seletedPower == 1)
            PrefHandlre.HammarAvailable += 3;
        else if (seletedPower == 2)
            PrefHandlre.HintAvailable += 3;

        UpdatePowerUpsUi();
        UpdateCoinsUI(PrefHandlre.Coins);
        moreItemPanel.SetActive(false);
    }

    #endregion 

    [Space(30)]
    #region GamePlayUI
    [Header("GamePlay UI")]
    public Text undoText;
    public Text hammarText, HintText;
    public Image undoImage, hammarImage, hintImage;
    public Sprite plusSprite, yellowSprite;
    public Text coinText,warningText;
    

    public void UpdatePowerUpsUi()
    {
        if (PrefHandlre.UndosAvailable > 0)
        {
            undoText.text=PrefHandlre.UndosAvailable.ToString();
            undoText.gameObject.SetActive(true);
            undoImage.sprite = yellowSprite;
        }
        else
        {
            undoText.gameObject.SetActive(false);
            undoImage.sprite = plusSprite;
        }
        if (PrefHandlre.HammarAvailable > 0)
        {
            hammarText.text = PrefHandlre.HammarAvailable.ToString();
            hammarText.gameObject.SetActive(true);
            hammarImage.sprite = yellowSprite;
           
        }
        else
        {
            hammarText.gameObject.SetActive(false);
            hammarImage.sprite = plusSprite;
        }
        if (PrefHandlre.HintAvailable > 0)
        {
            HintText.text = PrefHandlre.HintAvailable.ToString();
            HintText.gameObject.SetActive(true);
            hintImage.sprite = yellowSprite;
        }
        else
        {
            HintText.gameObject.SetActive(false);
            hintImage.sprite = plusSprite;
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinsUI(PrefHandlre.Coins);
        UpdatePowerUpsUi();
        levelWinCoinText.text="+"+LevelHandler.instance.levelWinReward.ToString();
    }
    private void Awake()
    {
        Instance = this;
    }
    public void UpdateCoinsUI(int coins)
    {
        coinText.text = coins.ToString();
    }
    private void OnEnable()
    {
        AdWatchedForPowerUps += AddPowerUps;
    }
    private void OnDisable()
    {
        AdWatchedForPowerUps -= AddPowerUps;
    }
    #region shopPanel
    [Space(20)]
    [Header("shopPanel  UI")]
    public CoinCollectAnimation coinCollectShop;
    #endregion


    [Space(20)]
    #region TutorialPanel
    [Header("Tutorial UI")]
    public GameObject blackPanel;
    public GameObject undoButton, hammarButton, hintButton;
    public GameObject thumbForUndo,thumbForHmammar,thumbForHint;
    public Text tutorialText, tutorialText2;
    public GameObject canvasForLockedSlot;
    public GameObject headArrow,dragImage;


    public void  SetUIForHammer()
    {
        blackPanel.transform.SetAsLastSibling(); 
        blackPanel.SetActive(true);
        hammarButton.transform.SetAsLastSibling();
        thumbForHmammar.SetActive(true);
        tutorialText.gameObject.SetActive(true);
        tutorialText.gameObject.transform.SetAsLastSibling();
        tutorialText.text = "USE HAMMAR TO BREAK LOCKED SLOTS";
        PrefHandlre.HammarAvailable++;
        UpdatePowerUpsUi();
    }
    public void SetUIForUndo()
    {
        blackPanel.transform.SetAsLastSibling();
        blackPanel.SetActive(true);
        undoButton.transform.SetAsLastSibling();
        thumbForUndo.SetActive(true);
        tutorialText.gameObject.SetActive(true);
        tutorialText.gameObject.transform.SetAsLastSibling();
        tutorialText.text = "UNDO THE MOVE IF YOU TAP ACCIDENTALY";
        PrefHandlre.UndosAvailable+=2;
        UpdatePowerUpsUi();
    }
    public void SetUIForHint()
    {
        blackPanel.transform.SetAsLastSibling();
        blackPanel.SetActive(true);
        hintButton.transform.SetAsLastSibling();
        thumbForHint.SetActive(true);
        tutorialText.gameObject.SetActive(true);
        tutorialText.gameObject.transform.SetAsLastSibling();
        tutorialText.text = "Tap to get a hint of next move";
        PrefHandlre.HintAvailable += 2;
        UpdatePowerUpsUi();
    }
    public void SetHammerUiAfterClicked()
    {
        blackPanel.SetActive(false);
        thumbForHmammar.SetActive(false);
        tutorialText.text = "NOW TAP ON SLOT TO UNLOCK IT";
        canvasForLockedSlot.SetActive(true);
    }
    public void SetUndoUiAfterClicked()
    {
        blackPanel.SetActive(false);
        thumbForUndo.SetActive(false);
        tutorialText.text = "WellDone";
        PrefHandlre.UndoTutorial = 1;
        Invoke(nameof(DeactiveTutorialText), 1);
    }
    public void SetHintUiAfterClicked()
    {
        blackPanel.SetActive(false);
        thumbForHint.SetActive(false);
        tutorialText.gameObject.SetActive(false);
        tutorialText.text = "WellDone";
        PrefHandlre.HintTutorial = 1;
        Invoke(nameof(DeactiveTutorialText), 1);
    }
    void DeactiveTutorialText()
    {
        tutorialText.gameObject.SetActive(false);
    }
    public void FinnishHammarTutorial()
    {
        tutorialText.gameObject.SetActive(false);
        canvasForLockedSlot.SetActive(false);
        PrefHandlre.HammarTutorial = 1;
    }
    public void SetUiForRotator()
    {
        headArrow.SetActive(true);
        tutorialText2.gameObject.SetActive(true); 
        tutorialText2.text = "Swipe Left or Right to Rotate the Disk";
        dragImage.SetActive(true);
    }
    public void finishRotatorTutorialUI()
    {
        headArrow.SetActive(false);
        tutorialText2.gameObject.SetActive(true);
        tutorialText2.text = "WellDone";
        Invoke(nameof(DeactiveTutorial2Text), 1);
    }
    public void DeactiveTutorial2Text()
    {
        tutorialText2.gameObject.SetActive(false);
    }
    public void ShowWarning(string str)
    {
        warningText.gameObject.SetActive(false);
        warningText.gameObject.SetActive(true);
        warningText.text = str;
    }
    #endregion TutorialPanel
}
