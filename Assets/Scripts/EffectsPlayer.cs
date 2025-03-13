using System.Collections;
using UnityEngine;

public class EffectsPlayer : MonoBehaviour
{
    [Header("Player Effect Settings")]
    public float totalTimePlayerEffect = 5f;
    public float intervalTimePlayer = 0.1f;

    private SpriteRenderer spriteRendererPlayer;
    private float elapsedTimePlayer = 0f;

    void Start()
    {
        spriteRendererPlayer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        elapsedTimePlayer += Time.deltaTime;

        if (elapsedTimePlayer >= totalTimePlayerEffect)
        {
            // Asegurar que el SpriteRenderer esté activado al finalizar
            spriteRendererPlayer.enabled = true;
            return;
        }

        StartCoroutine(ToggleVisibility());
    }

    IEnumerator ToggleVisibility()
    {
        if (spriteRendererPlayer == null) yield break;

        // Alternar el estado de visibilidad del SpriteRenderer
        spriteRendererPlayer.enabled = !spriteRendererPlayer.enabled;

        yield return new WaitForSeconds(intervalTimePlayer);
    }
}
