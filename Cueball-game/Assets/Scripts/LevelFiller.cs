using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelFiller : MonoBehaviour
{
    public Image FillerImage;
    public Text progressText;
    // Start is called before the first frame update
    void OnEnable()
    {
        float FilledProgress = PlayerPrefs.GetFloat("Filler", 0);
        if (FilledProgress >= 1)
        {
            FilledProgress = 0;
        }
        StartCoroutine(FillProgress(FilledProgress));
        //print("Progress " + PlayerPrefs.GetFloat("Filler", 0));

        //if (PlayerPrefs.GetFloat("Filler", 0) == 0|| PlayerPrefs.GetFloat("Filler", 0) == 1)
        //    progressText.text = "1/5";
        //else if (PlayerPrefs.GetFloat("Filler", 0) == 0.2)
        //    progressText.text = "2/5";
        //else if (PlayerPrefs.GetFloat("Filler", 0) == 0.4)
        //    progressText.text = "3/5";
        //else if (PlayerPrefs.GetFloat("Filler", 0) == 0.6)
        //    progressText.text = "4/5";
        //else if (PlayerPrefs.GetFloat("Filler", 0) == 0.8)
        //{
        //    Debug.Log("hell g");
        //    progressText.text = "5/5";
        //}

        progressText.text = (((FilledProgress * 10)/2)+1).ToString() +"/5";

    }

    IEnumerator FillProgress(float FilledProgress)
    {
       

        float final = FilledProgress + 0.2f;
        while (FilledProgress < final)
        {
            FilledProgress += 0.01f;
            FillerImage.fillAmount = FilledProgress;
            yield return new WaitForSeconds(0.05f);
        }
        
        PlayerPrefs.SetFloat("Filler", final);
        if (final == 1)
        {
            UiObjectsContainer.Instance.chestImage.sprite = UiObjectsContainer.Instance.chestOpenSprit;
            yield return new WaitForSeconds(0.05f);
            UiObjectsContainer.Instance.OpenCongratsPanel();
        }
        yield return null;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
