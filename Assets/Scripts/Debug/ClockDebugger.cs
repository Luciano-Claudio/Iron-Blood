using UnityEngine;

// Script temporário de validação — deletar após confirmar que os eventos disparam corretamente
public class ClockDebugger : MonoBehaviour
{
    [Header("Configuração")]
    [Tooltip("Ativado: loga só uma vez por hora. Desativado: loga todo minuto.")]
    public bool logHourlyOnly = false;

    private void Start()
    {
        GameEvents.OnTimeChanged  += OnTimeChanged;
        GameEvents.OnNewDay       += OnNewDay;
        GameEvents.OnNewWeek      += OnNewWeek;
        GameEvents.OnNewMonth     += OnNewMonth;
        GameEvents.OnNewYear      += OnNewYear;
        GameEvents.OnPlayerSlept  += OnPlayerSlept;
        GameEvents.OnPlayerWokeUp += OnPlayerWokeUp;
    }

    private void OnDisable()
    {
        GameEvents.OnTimeChanged  -= OnTimeChanged;
        GameEvents.OnNewDay       -= OnNewDay;
        GameEvents.OnNewWeek      -= OnNewWeek;
        GameEvents.OnNewMonth     -= OnNewMonth;
        GameEvents.OnNewYear      -= OnNewYear;
        GameEvents.OnPlayerSlept  -= OnPlayerSlept;
        GameEvents.OnPlayerWokeUp -= OnPlayerWokeUp;
    }

    private void OnTimeChanged(int hour, int minute)
    {
        if (logHourlyOnly && minute != 0) return;
        Debug.Log($"[Tempo] {hour:00}:{minute:00} | Dia {GameClock.Instance.DayOfWeek}/Sem | " +
                  $"Dia {GameClock.Instance.DayOfMonth}/Mês | " +
                  $"Sem {GameClock.Instance.WeekOfMonth} | " +
                  $"Mês {GameClock.Instance.MonthOfYear} | Ano {GameClock.Instance.CurrentYear}");
    }

    private void OnNewDay(int day)
        => Debug.Log($"[NOVO DIA] Dia absoluto {day} — " +
                     $"Dia {GameClock.Instance.DayOfWeek} da semana, " +
                     $"Dia {GameClock.Instance.DayOfMonth} do mês");

    private void OnNewWeek(int week)
        => Debug.Log($"[NOVA SEMANA] Semana {GameClock.Instance.WeekOfMonth} do Mês {GameClock.Instance.MonthOfYear}");

    private void OnNewMonth(int month)
        => Debug.Log($"[NOVO MÊS] Mês {GameClock.Instance.MonthOfYear} do Ano {GameClock.Instance.CurrentYear}");

    private void OnNewYear(int year)
        => Debug.Log($"[NOVO ANO] Ano {year}");

    private void OnPlayerSlept()
        => Debug.Log($"[SONO] Player dormiu — agora é Dia {GameClock.Instance.CurrentDay} às {GameClock.Instance.GetTimeString()}");

    private void OnPlayerWokeUp()
        => Debug.Log($"[SONO] Player acordou — {GameClock.Instance.GetTimeString()} | Dia {GameClock.Instance.CurrentDay}");
}
