using UnityEngine;
using TMPro;

public class ClockHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text dayText;

    private void Start()
    {
        // Atualiza imediatamente ao entrar na cena — não espera o próximo tick
        if (GameClock.Instance != null)
            UpdateHUD(GameClock.Instance.CurrentHour, GameClock.Instance.CurrentMinute);
    }

    private void OnEnable()
    {
        GameEvents.OnTimeChanged  += UpdateHUD;
        GameEvents.OnPlayerWokeUp += OnWokeUp;
    }

    private void OnDisable()
    {
        GameEvents.OnTimeChanged  -= UpdateHUD;
        GameEvents.OnPlayerWokeUp -= OnWokeUp;
    }

    // Força atualização ao acordar — clock já está em 06:00 mas OnTimeChanged
    // só dispara no próximo minuto
    private void OnWokeUp()
    {
        if (GameClock.Instance != null)
            UpdateHUD(GameClock.Instance.CurrentHour, GameClock.Instance.CurrentMinute);
    }

    private void UpdateHUD(int hour, int minute)
    {
        timeText.text = $"{hour:00}:{minute:00}";
        dayText.text  = $"Dia {GameClock.Instance.CurrentDay} · Semana {GameClock.Instance.CurrentWeek}";
    }
}
