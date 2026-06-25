using UnityEngine;
using System.Collections;

// Tela de carregamento exibida no boot do jogo (GameScene → primeira área).
// Cobre a tela desde o frame 1 — o jogador nunca vê a GameScene vazia.
// Transições de portas usam EasyTransitions, não este componente.
public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.6f;

    private void Awake()
    {
        Instance = this;
        canvasGroup.alpha          = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable   = false;
    }

    public void Hide() => StartCoroutine(FadeOut());

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha          = 0f;
        canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
    }
}
