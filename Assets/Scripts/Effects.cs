using System.Collections;
using UnityEngine;

public class Effects : MonoBehaviour
{
    [Header("Effect Settings")]
    public float totalTimeEffect = 5f;
    public float intervalTime = 0.1f;

    private SpriteRenderer spriteRenderer;
    private float elapsedTime = 0f;
    private bool isIncreasing = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= totalTimeEffect)
        {
            SetAlphaToMax(); 
            return;
        }

        StartCoroutine(ChangeAlpha());
    }

    IEnumerator ChangeAlpha()
    {
        if (spriteRenderer == null) yield break;

        Color currentColor = spriteRenderer.color;

        if (isIncreasing)
        {
            currentColor.a += Time.deltaTime / intervalTime;
            if (currentColor.a >= 1f)
            {
                currentColor.a = 1f;
                isIncreasing = false;
            }
        }
        else
        {
            currentColor.a -= Time.deltaTime / intervalTime;
            if (currentColor.a <= 0f)
            {
                currentColor.a = 0f;
                isIncreasing = true;
            }
        }

        spriteRenderer.color = currentColor;

        yield return new WaitForSeconds(intervalTime);
    }

    void SetAlphaToMax() 
    {
        if (spriteRenderer == null) return;

        Color currentColor = spriteRenderer.color;
        currentColor.a = 1f;
        spriteRenderer.color = currentColor;
    }
}
