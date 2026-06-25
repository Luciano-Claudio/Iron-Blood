using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using EasyTransition;

// Gerencia carregamento e descarregamento de cenas additivamente.
// GameScene (Core) nunca é descarregada — contém GameManager, Player, HUD, etc.
// Apenas uma cena de área fica carregada por vez.
//
// FLUXO INICIAL (boot):
//   GameScene → LoadingScreen cobre tela → área inicial carrega → LoadingScreen some
//   Sem EasyTransitions no boot — a LoadingScreen já cobre tudo desde o frame 1.
//
// FLUXO DE PORTAS:
//   DoorInteractable.Interact() → LoadArea(cena, spawnId) → EasyTransitions →
//   no cut point: descarrega cena atual + carrega nova → teleporta ao spawnId correto
//
// SPAWN ID:
//   Cada porta tem um targetSpawnId. Cada PlayerSpawnPoint tem um spawnId correspondente.
//   Ex: porta "ferraria_entrada" → PlayerSpawnPoint.spawnId = "ferraria_entrada"
//   Funciona para qualquer transição: Exterior↔Ferraria, Ferraria↔Porão, etc.
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("Cena Inicial")]
    [SerializeField] private string startingScene   = "Exterior";
    [SerializeField] private string startingSpawnId = "default";

    [Header("Transição de Portas")]
    [SerializeField] private TransitionSettings transitionSettings;
    [SerializeField] private float transitionDelay = 0f;

    private string currentAreaScene;
    private bool isTransitioning;

    private void Awake() => Instance = this;

    private void Start()
    {
        if (!string.IsNullOrEmpty(startingScene))
            StartCoroutine(InitialLoad());
    }

    // ─────────────────────────────────────────────────────────────────
    // CARGA INICIAL — sem EasyTransitions, LoadingScreen já cobre a tela
    // ─────────────────────────────────────────────────────────────────

    private IEnumerator InitialLoad()
    {
        isTransitioning = true;
        GameStateManager.Instance.LockPlayer();
        GameEvents.RaiseSceneTransitionStarted();

        yield return StartCoroutine(SwapScene(startingScene, startingSpawnId));

        isTransitioning = false;
        GameStateManager.Instance.UnlockPlayer();
        GameEvents.RaiseSceneTransitionEnded();
        LoadingScreen.Instance?.Hide();
    }

    // ─────────────────────────────────────────────────────────────────
    // TRANSIÇÃO DE PORTAS — usa EasyTransitions
    // ─────────────────────────────────────────────────────────────────

    public void LoadArea(string sceneName, string spawnId = "default")
    {
        if (isTransitioning) return;
        StartCoroutine(TransitionToArea(sceneName, spawnId));
    }

    private IEnumerator TransitionToArea(string sceneName, string spawnId)
    {
        isTransitioning = true;
        GameStateManager.Instance.LockPlayer();
        GameEvents.RaiseSceneTransitionStarted();

        var tm = TransitionManager.Instance();
        if (tm == null)
        {
            Debug.LogError("[SceneLoader] TransitionManager não encontrado na cena.");
            isTransitioning = false;
            GameStateManager.Instance.UnlockPlayer();
            GameEvents.RaiseSceneTransitionEnded();
            yield break;
        }

        tm.onTransitionCutPointReached = () =>
            StartCoroutine(SwapScene(sceneName, spawnId));

        tm.onTransitionEnd = () =>
        {
            tm.onTransitionCutPointReached = null;
            tm.onTransitionEnd             = null;
            isTransitioning = false;
            GameStateManager.Instance.UnlockPlayer();
            GameEvents.RaiseSceneTransitionEnded();
        };

        tm.Transition(transitionSettings, transitionDelay);
        yield return null;
    }

    // ─────────────────────────────────────────────────────────────────
    // SWAP DE CENA — compartilhado pelo boot e pelas portas
    // ─────────────────────────────────────────────────────────────────

    private IEnumerator SwapScene(string sceneName, string spawnId)
    {
        if (!string.IsNullOrEmpty(currentAreaScene))
        {
            var previous = SceneManager.GetSceneByName(currentAreaScene);
            if (previous.isLoaded)
                yield return SceneManager.UnloadSceneAsync(currentAreaScene);
        }

        var loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (loadOp == null)
        {
            Debug.LogError($"[SceneLoader] Cena '{sceneName}' não encontrada. " +
                           "Adicione-a em File → Build Profiles.");
            yield break;
        }

        yield return loadOp;
        currentAreaScene = sceneName;

        TeleportToSpawn(spawnId, sceneName);
    }

    private void TeleportToSpawn(string spawnId, string sceneName)
    {
        var allSpawns = FindObjectsByType<PlayerSpawnPoint>(FindObjectsSortMode.None);

        PlayerSpawnPoint target = null;
        foreach (var sp in allSpawns)
        {
            if (sp.spawnId == spawnId) { target = sp; break; }
        }

        if (target == null && allSpawns.Length > 0)
        {
            target = allSpawns[0];
            Debug.LogWarning($"[SceneLoader] SpawnPoint '{spawnId}' não encontrado em " +
                             $"{sceneName} — usando '{target.spawnId}' como fallback.");
        }

        if (target != null)
            PlayerMovement.Instance.Teleport(target.transform.position);
        else
            Debug.LogWarning($"[SceneLoader] Nenhum PlayerSpawnPoint encontrado em {sceneName}.");
    }
}
