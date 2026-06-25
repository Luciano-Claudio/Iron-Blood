using UnityEngine;

// Controla o estado de pausa/lock do jogo.
//
// PLAYER LOCK — bloqueia input do jogador, o mundo continua rodando
//   LockPlayer() / UnlockPlayer()
//   Usado por: transições de cena, sono, inventário, forja, loading screen
//   Counter-based: múltiplos sistemas bloqueiam simultaneamente sem conflito.
//   O unlock só dispara quando TODOS os sistemas liberarem.
//
// FULL PAUSE — Time.timeScale = 0, mundo para completamente
//   PauseGame() / ResumeGame()
//   Usado por: menu de configurações, cutscenes futuras
//   Automaticamente bloqueia o player também.
//
// WORLD LOCK — NPCs e inimigos param de agir (sem pausa de tempo)
//   LockWorld() / UnlockWorld() — TODO: implementar quando NPCs e inimigos existirem
//   GameEvents.OnWorldLocked / OnWorldUnlocked — cada NPC/Enemy subscreve e para
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    private int playerLockCount = 0;

    public bool IsPlayerLocked => playerLockCount > 0;
    public bool IsGamePaused   => Time.timeScale == 0f;

    private void Awake() => Instance = this;

    // ─────────────────────────────────────────────────────────────────
    // PLAYER LOCK
    // ─────────────────────────────────────────────────────────────────

    public void LockPlayer()
    {
        playerLockCount++;
        if (playerLockCount == 1)
            GameEvents.RaisePlayerLocked();
    }

    public void UnlockPlayer()
    {
        playerLockCount = Mathf.Max(0, playerLockCount - 1);
        if (playerLockCount == 0)
            GameEvents.RaisePlayerUnlocked();
    }

    // ─────────────────────────────────────────────────────────────────
    // FULL PAUSE
    // ─────────────────────────────────────────────────────────────────

    public void PauseGame()
    {
        Time.timeScale = 0f;
        LockPlayer();
        // TODO: LockWorld() — NPCs e inimigos param (Semana 6+)
        GameEvents.RaiseGamePaused();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        UnlockPlayer();
        // TODO: UnlockWorld()
        GameEvents.RaiseGameResumed();
    }

    // ─────────────────────────────────────────────────────────────────
    // WORLD LOCK (futuro)
    // ─────────────────────────────────────────────────────────────────

    // public void LockWorld()   => GameEvents.RaiseWorldLocked();
    // public void UnlockWorld() => GameEvents.RaiseWorldUnlocked();
}
