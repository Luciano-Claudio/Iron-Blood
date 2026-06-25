using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using EasyTransition;

// Gerencia carregamento e descarregamento de cenas additivamente.
// GameScene (Core) nunca é descarregada — contém GameManager, Player, HUD, etc.
// Apenas uma cena de área (Exterior ou Interior) fica carregada por vez.
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("Transição")]
    [SerializeField] private TransitionSettings transitionSettings;
    [SerializeField] private float transitionDelay = 0f;

    private string currentAreaScene;
    private bool isTransitioning;

    private void Awake() => Instance = this;

    // Chamado pela porta ao pressionar E
    public void LoadArea(string sceneName, Vector2 fallbackSpawnPosition = default)
    {
        if (isTransitioning) return;
        StartCoroutine(TransitionToArea(sceneName, fallbackSpawnPosition));
    }

    private IEnumerator TransitionToArea(string sceneName, Vector2 fallbackSpawn)
    {
        isTransitioning = true;
        GameEvents.RaiseSceneTransitionStarted();

        var tm = TransitionManager.Instance();
        if (tm == null)
        {
            Debug.LogError("[SceneLoader] TransitionManager não encontrado na cena.");
            isTransitioning = false;
            GameEvents.RaiseSceneTransitionEnded();
            yield break;
        }

        tm.onTransitionCutPointReached = () =>
        {
            StartCoroutine(SwapScene(sceneName, fallbackSpawn));
        };

        tm.onTransitionEnd = () =>
        {
            tm.onTransitionCutPointReached = null;
            tm.onTransitionEnd             = null;
            isTransitioning = false;
            GameEvents.RaiseSceneTransitionEnded();
        };

        tm.Transition(transitionSettings, transitionDelay);
        yield return null;
    }

    private IEnumerator SwapScene(string sceneName, Vector2 fallbackSpawn)
    {
        if (!string.IsNullOrEmpty(currentAreaScene))
            yield return SceneManager.UnloadSceneAsync(currentAreaScene);

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        currentAreaScene = sceneName;

        PlayerSpawnPoint spawn = FindFirstObjectByType<PlayerSpawnPoint>();
        if (spawn != null)
            PlayerMovement.Instance.Teleport(spawn.transform.position);
        else if (fallbackSpawn != Vector2.zero)
            PlayerMovement.Instance.Teleport(fallbackSpawn);
        else
            Debug.LogWarning($"[SceneLoader] PlayerSpawnPoint não encontrado em {sceneName}.");
    }
}
