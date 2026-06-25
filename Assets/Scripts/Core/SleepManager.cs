using UnityEngine;
using System.Collections;
using EasyTransition;

// ═══════════════════════════════════════════════════════════════════════════════
// SLEEP MANAGER — Iron & Blood  |  Task C-2
// ═══════════════════════════════════════════════════════════════════════════════
//
// VISÃO GERAL
// O jogador precisa dormir uma vez por dia para recuperar energia e avançar o
// calendário. Existem dois casos de sono, descritos abaixo.
//
// ───────────────────────────────────────────────────────────────────────────────
// CASO 1 — SONO NORMAL (voluntário)
// ───────────────────────────────────────────────────────────────────────────────
//   • Disponível a partir das 18:00.
//   • Trigger: o jogador se aproxima de uma cama e pressiona E (Interact).
//   • A cama chama SleepManager.Instance.TrySleep().
//   • Sequência:
//       1. Clock pausa + movimento bloqueado imediatamente.
//       2. Animação de dormir do player (2s — placeholder por enquanto).
//       3. Transição Brush cobre a tela.
//       4. No cut point (tela 100% coberta): dia avança, clock pula para 06:00.
//       5. Brush descobre a tela → clock volta → player pode se mover.
//
// ───────────────────────────────────────────────────────────────────────────────
// CASO 2 — SONO FORÇADO (punição por não dormir)
// ───────────────────────────────────────────────────────────────────────────────
//   • Hora configurável via Inspector: forcedSleepHour (padrão 02:00).
//   • Avisos antes do sono forçado:
//       - 10 min antes: ícone ZZZ aparece sobre o player (TODO: HUDManager).
//       -  5 min antes: ícone ZZZ aparece novamente.
//   • Na hora forçada: player dorme onde estiver, sem escolha.
//   • Mesma sequência de transição do Caso 1, mas com penalidades ao acordar:
//       - Ouro: -10% do total, máximo de 1.000 gold de perda (TODO: EconomyManager).
//       - Saúde e energia: reduzidas a 50% ao acordar (TODO: StatsManager).
//       - Respawn: player acorda na Igreja, não onde dormiu (TODO: churchSpawnPoint).
//   • Durante o sono forçado, combate é interrompido e inimigos ignoram o player
//     (TODO: CombatManager + EnemyAIManager).
//
// ───────────────────────────────────────────────────────────────────────────────
// REGRAS DE IMPLEMENTAÇÃO
// ───────────────────────────────────────────────────────────────────────────────
//   R1. A transição usa EasyTransitions (Brush). O asset já usa WaitForSecondsRealtime
//       internamente, garantindo que o fade não seja afetado pelo Time.timeScale.
//   R2. GameClock não sabe que o fade existe. Só expõe PauseForSleep(), Sleep() e
//       WakeUp() — a orquestração completa fica aqui no SleepManager.
//   R3. Clock e movimento são pausados no instante zero da sequência, antes da
//       animação, para evitar bugs de duplo-sleep (ex: sono forçado disparando
//       enquanto o player já está na sequência de dormir voluntariamente).
//   R4. isSleeping bloqueia todas as entradas (TrySleep, ForceSleep, CheckSleepTime)
//       enquanto a sequência estiver em andamento.
//
// ═══════════════════════════════════════════════════════════════════════════════

public class SleepManager : MonoBehaviour
{
    public static SleepManager Instance;

    [Header("Sono Forçado")]
    // Hora em que o sono forçado acontece se o jogador não dormiu — padrão 02:00
    [SerializeField] private int forcedSleepHour = 2;

    [Header("Transição")]
    // Arraste o asset Assets/EasyTransitions/Transitions/Brush/Brush.asset aqui
    [SerializeField] private TransitionSettings sleepTransition;
    [SerializeField] private float transitionDelay = 0f;

    [Header("Respawn (atribuir quando a Igreja existir no mapa)")]
    // TODO: criar Transform no spawn point da Igreja e arrastar aqui — Semana 2+
    [SerializeField] private Transform churchSpawnPoint;

    private bool isSleeping;

    private void Awake() => Instance = this;

    private void OnEnable()  => GameEvents.OnTimeChanged += CheckSleepTime;
    private void OnDisable() => GameEvents.OnTimeChanged -= CheckSleepTime;

    // ─────────────────────────────────────────────────────────────────
    // VERIFICAÇÃO DE HORÁRIO
    // ─────────────────────────────────────────────────────────────────

    private void CheckSleepTime(int hour, int minute)
    {
        if (isSleeping) return;

        int current = hour * 60 + minute;
        int forced  = forcedSleepHour * 60;

        if (current == forced - 10) ShowSleepWarning(10);
        if (current == forced - 5)  ShowSleepWarning(5);
        if (current == forced)      ForceSleep();
    }

    private void ShowSleepWarning(int minutesLeft)
    {
        // TODO: HUDManager.Instance.ShowSleepIcon() — ícone ZZZ sobre o player
        Debug.Log($"[SleepManager] Aviso: {minutesLeft} minutos para o sono forçado.");
    }

    // ─────────────────────────────────────────────────────────────────
    // SONO NORMAL — chamado pela cama ao pressionar E
    // ─────────────────────────────────────────────────────────────────

    public void TrySleep()
    {
        if (isSleeping) return;

        if (GameClock.Instance.CurrentHour < 18)
        {
            // TODO: HUDManager.Instance.ShowToast("Ainda é cedo para dormir.");
            Debug.Log("[SleepManager] Ainda é cedo para dormir.");
            return;
        }

        StartCoroutine(SleepSequence(forced: false));
    }

    // ─────────────────────────────────────────────────────────────────
    // SONO FORÇADO — disparado automaticamente às forcedSleepHour:00
    // ─────────────────────────────────────────────────────────────────

    public void ForceSleep()
    {
        if (isSleeping) return;

        // TODO: pausar combate e fazer inimigos ignorarem o player
        // CombatManager.Instance.SetActive(false);
        // EnemyAIManager.Instance.SetPlayerAsThreat(false);

        StartCoroutine(SleepSequence(forced: true));
    }

    // ─────────────────────────────────────────────────────────────────
    // SEQUÊNCIA DE SONO
    // ─────────────────────────────────────────────────────────────────

    private IEnumerator SleepSequence(bool forced)
    {
        isSleeping = true;

        // Pausa o clock e bloqueia o player imediatamente — antes de qualquer animação
        GameClock.Instance.PauseForSleep();
        GameStateManager.Instance.LockPlayer();

        // 1. Animação placeholder de dormir (2 segundos)
        // TODO: PlayerAnimator.Instance.PlaySleep();
        Debug.Log("[SleepManager] [placeholder] Animação de dormir — 2s.");
        yield return new WaitForSecondsRealtime(2f);

        // 2. Conecta callbacks no TransitionManager
        var tm = TransitionManager.Instance();
        if (tm == null)
        {
            Debug.LogError("[SleepManager] TransitionManager não encontrado na cena! " +
                           "Adicione um GameObject com o componente TransitionManager e atribua o TransitionTemplate.prefab.");
            GameClock.Instance.WakeUp();
            GameStateManager.Instance.UnlockPlayer();
            isSleeping = false;
            yield break;
        }

        tm.onTransitionCutPointReached = () =>
        {
            // Tela 100% coberta pelo brush — momento seguro para avançar o dia
            GameClock.Instance.Sleep();
            GameEvents.RaisePlayerSlept();

            if (forced) ApplyForcedSleepPenalties();
        };

        tm.onTransitionEnd = () =>
        {
            GameClock.Instance.WakeUp();
            GameStateManager.Instance.UnlockPlayer();
            GameEvents.RaisePlayerWokeUp();

            if (forced) RespawnAtChurch();

            // Limpa callbacks — TransitionManager é reutilizado entre transições
            tm.onTransitionCutPointReached = null;
            tm.onTransitionEnd             = null;

            isSleeping = false;
            Debug.Log("[SleepManager] Novo dia iniciado.");
        };

        // 3. Dispara o brush
        tm.Transition(sleepTransition, transitionDelay);
    }

    // ─────────────────────────────────────────────────────────────────
    // PENALIDADES DO SONO FORÇADO
    // ─────────────────────────────────────────────────────────────────

    private void ApplyForcedSleepPenalties()
    {
        // TODO: -10% do ouro do jogador, máximo de 1000 gold de perda
        // int penalty = Mathf.Min(Mathf.RoundToInt(EconomyManager.Instance.Gold * 0.1f), 1000);
        // EconomyManager.Instance.SpendGold(penalty);

        // TODO: saúde e energia a 50% ao acordar
        // StatsManager.Instance.SetHealthPercent(0.5f);
        // StatsManager.Instance.SetEnergyPercent(0.5f);

        Debug.Log("[SleepManager] SONO FORÇADO — penalidades pendentes (EconomyManager + StatsManager).");
    }

    private void RespawnAtChurch()
    {
        if (churchSpawnPoint == null)
        {
            Debug.Log("[SleepManager] Respawn na Igreja pendente — churchSpawnPoint não atribuído.");
            return;
        }
        // TODO: PlayerMovement.Instance.Teleport(churchSpawnPoint.position);
        Debug.Log($"[SleepManager] Player teleportado para a Igreja em {churchSpawnPoint.position}.");
    }
}
