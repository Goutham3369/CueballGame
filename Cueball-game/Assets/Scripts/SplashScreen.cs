using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public Slider slider;
    public float duration = 2f;

    void Start()
    {
        StartCoroutine(MoveSlider());
    }

    IEnumerator MoveSlider()
    {
        float initialPosition = slider.value;
        float elapsed = 0;

        while (elapsed < 1)
        {
            elapsed += Time.deltaTime / duration;
            float newPosition = Mathf.Lerp(initialPosition, 0.9f, elapsed);

            slider.value = newPosition;

            yield return null;
        }

        gameObject.SetActive(false);
    }


}
