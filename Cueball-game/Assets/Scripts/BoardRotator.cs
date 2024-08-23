using System.Collections;
using UnityEngine;

public class BoardRotator : MonoBehaviour
{
    public float RotateSpeed;
    public float RotateSpeed2;
    public bool StartRotation;
    public bool RotateWithFinger;

    // Total duration of the shake effect
    public float shakeDuration = 2f;

    // Initial intensity of the shake
    public float shakeIntensity = 0.7f;

    // Rate at which the shake intensity decreases
    public float decreaseFactor = 1.0f;

    private Vector3 originalPosition;
    private float currentShakeDuration;
    float initShakeIntensity;

    void Start()
    {
        initShakeIntensity = shakeIntensity; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RotateWithFinger = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            RotateWithFinger = false;
        }
        if (StartRotation)
        {
            transform.RotateAround(transform.position, Vector3.up, RotateSpeed * Time.deltaTime);
        }
        else
        {
            //if (RotateWithFinger)
            //{
            //    // Rotate the character around the Y-axis based on the input axis (change as needed)
            //    float horizontalInput = Input.GetAxis("Mouse X");
            //    transform.RotateAround(transform.position,Vector3.up, RotateSpeed2 * -horizontalInput * Time.deltaTime);
            //}
        }
    }

    public void StartRotatioOnLevelEnd()
    {
        StartRotation = true;
    }
    // Call this method to start the shaking effect
    public void StartShaking()
    {
        shakeIntensity = initShakeIntensity;
        originalPosition = transform.position;
        currentShakeDuration = shakeDuration;
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        while (currentShakeDuration > 0)
        {
            transform.position = originalPosition + Random.insideUnitSphere * shakeIntensity;
            currentShakeDuration -= Time.deltaTime * decreaseFactor;
            shakeIntensity = Mathf.Lerp(0, shakeIntensity, currentShakeDuration / shakeDuration);
            yield return null;
        }

        // When shaking is finished, reset position
        transform.position = originalPosition;
    }

}
