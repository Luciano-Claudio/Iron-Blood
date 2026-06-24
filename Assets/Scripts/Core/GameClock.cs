using UnityEngine;

// ESTRUTURA DO CALENDÁRIO DE IRON & BLOOD
// ─────────────────────────────────────────────────────────────────
// 1 dia    = 24 horas (00:00 → 23:59). O dia vira naturalmente à meia-noite.
// 1 semana = 6 dias
// 1 mês    = 5 semanas = 30 dias
// 1 ano    = 4 meses   = 120 dias
//
// REGRA DE SONO (implementada no SleepManager, não aqui):
//   - O jogador pode dormir voluntariamente interagindo com a cama (tecla E)
//   - Ao dormir, o clock pula para 06:00 do dia seguinte
//   - No futuro: se o jogador não dormir até certa hora (ex: 02:00),
//     o sono é forçado automaticamente com penalidade de energia/stamina
//
// IDEIA FUTURA — ESTAÇÕES E CLIMA:
//   - Cada um dos 4 meses representa uma estação (Primavera, Verão, Outono, Inverno)
//   - GameEvents.OnNewMonth é o gancho para trocar a estação ativa
//   - Clima (chuva, neve, sol forte) pode ser gerado por seed dentro de cada estação
//   - Clima afeta: velocidade de coleta na floresta, fluxo de clientes, buffs de comida
// ─────────────────────────────────────────────────────────────────

public class GameClock : MonoBehaviour
{
    public static GameClock Instance;

    // Constantes do calendário — alterar aqui reflete em todo o jogo
    public const int DaysPerWeek   = 6;
    public const int WeeksPerMonth = 5;
    public const int MonthsPerYear = 4;
    public const int DaysPerMonth  = DaysPerWeek * WeeksPerMonth;   // 30
    public const int DaysPerYear   = DaysPerMonth * MonthsPerYear;  // 120

    [Header("Configuração")]
    // 25s por hora de jogo = 10 minutos reais por dia completo (24h)
    public float realSecondsPerGameHour = 25f;
    // Dia 1 começa às 06:00 — hora de chegada do Durin à ferraria
    public int startHour = 6;

    [Header("Estado")]
    public int  CurrentDay    { get; private set; } = 1;
    public int  CurrentHour   { get; private set; }
    public int  CurrentMinute { get; private set; }
    // Pausado durante a animação de fade do sono (SleepManager define isso)
    public bool IsSleeping    { get; private set; }

    // Derivados do dia absoluto — não armazenados, calculados on demand
    public int DayOfWeek    => (CurrentDay - 1) % DaysPerWeek + 1;                  // 1–6
    public int DayOfMonth   => (CurrentDay - 1) % DaysPerMonth + 1;                 // 1–30
    public int WeekOfMonth  => ((CurrentDay - 1) % DaysPerMonth) / DaysPerWeek + 1; // 1–5
    public int MonthOfYear  => (CurrentMonth - 1) % MonthsPerYear + 1;              // 1–4
    public int CurrentWeek  => (CurrentDay - 1) / DaysPerWeek + 1;                  // semana absoluta
    public int CurrentMonth => (CurrentDay - 1) / DaysPerMonth + 1;                 // mês absoluto
    public int CurrentYear  => (CurrentDay - 1) / DaysPerYear + 1;                  // ano absoluto

    private float timer;

    private void Awake()
    {
        Instance      = this;
        CurrentHour   = startHour;
        CurrentMinute = 0;
    }

    private void Update()
    {
        if (IsSleeping) return;

        timer += Time.deltaTime;
        float minuteDuration = realSecondsPerGameHour / 60f;

        if (timer >= minuteDuration)
        {
            timer -= minuteDuration;
            AdvanceMinute();
        }
    }

    private void AdvanceMinute()
    {
        CurrentMinute++;
        if (CurrentMinute >= 60)
        {
            CurrentMinute = 0;
            CurrentHour++;

            // Virada de meia-noite — o dia avança naturalmente se o jogador não dormiu
            if (CurrentHour >= 24)
            {
                CurrentHour = 0;
                AdvanceDay();
            }
        }
        GameEvents.RaiseTimeChanged(CurrentHour, CurrentMinute);
    }

    private void AdvanceDay()
    {
        int prevWeek  = CurrentWeek;
        int prevMonth = CurrentMonth;
        int prevYear  = CurrentYear;

        CurrentDay++;

        GameEvents.RaiseNewDay(CurrentDay);
        if (CurrentWeek  != prevWeek)  GameEvents.RaiseNewWeek(CurrentWeek);
        if (CurrentMonth != prevMonth) GameEvents.RaiseNewMonth(CurrentMonth);
        if (CurrentYear  != prevYear)  GameEvents.RaiseNewYear(CurrentYear);
    }

    // Chamado pelo SleepManager no início da sequência de sono.
    // Pausa o clock imediatamente — antes da animação e do fade.
    // Bloqueia também o movimento do player (PlayerMovement verifica IsSleeping).
    public void PauseForSleep()
    {
        IsSleeping = true;
        timer      = 0f;
    }

    // Chamado no cut point do fade (tela coberta) — avança o dia.
    // PauseForSleep já foi chamado antes, então IsSleeping já é true.
    public void Sleep()
    {
        CurrentHour   = 6;
        CurrentMinute = 0;
        timer         = 0f;
        AdvanceDay();
    }

    public void WakeUp() => IsSleeping = false;

    public string GetTimeString() => $"{CurrentHour:00}:{CurrentMinute:00}";
}
