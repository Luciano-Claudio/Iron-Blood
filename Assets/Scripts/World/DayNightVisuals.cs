using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class DayNightVisuals : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;

    [System.Serializable]
    public class ManagedLight
    {
        public Light2D light;

        [Tooltip("Quanto essa luz responde ao ciclo dia/noite.\n1.0 = janela plena\n0.4 = interior de casa (luz filtrada pelas paredes)\n0.2 = entrada de caverna\n0.0 = tocha (ignora o ciclo)")]
        [Range(0f, 1f)] public float cycleStrength = 1f;

        [Tooltip("Intensidade máxima no pico do dia")]
        public float maxIntensity = 1f;

        [Tooltip("Usar a cor do gradiente do céu. Desative para manter cor própria (ex: tocha, interior aquecido)")]
        public bool useSkyColor = true;
    }

    // Aceita qualquer Light2D: Global (interior/exterior), Spot (janelas, cavernas), Freeform...
    // Cada luz tem seu próprio cycleStrength — de 0 (estática) a 1 (100% do ciclo)
    [Header("Luzes controladas pelo ciclo dia/noite")]
    [SerializeField] private List<ManagedLight> managedLights = new();

    [Header("Ciclo de 24h")]
    // Dia: dayStartHour → dayEndHour (t: 0 → 1)
    // Noite fixa: dayEndHour → preDawnHour  (t = 1)
    // Pré-amanhecer: preDawnHour → dayStartHour (t: 1 → 0)
    [SerializeField] private float dayStartHour  = 6f;   // início do dia / fim do amanhecer
    [SerializeField] private float dayEndHour    = 22f;  // início da noite fixa
    [SerializeField] private float preDawnHour   = 3f;   // início do pré-amanhecer

    [Header("Cor da luz por hora do dia")]
    // t=0 (06:00) → t=1 (22:00) — configure no Inspector
    // Sugestão: 06h laranja-quente | 10h branco-amarelado | 12h branco puro | 18h laranja | 20h azul-noite | 22h azul-escuro
    [SerializeField] private Gradient lightGradient;

    [Header("Intensidade por hora do dia")]
    // t=0 (06:00) → t=1 (22:00) — configure no Inspector
    // Sugestão: 0.0→0.3 no amanhecer | 1.0 ao meio-dia | 0.3 às 20h | 0.1 às 22h
    [SerializeField] private AnimationCurve intensityCurve = new AnimationCurve(
        new Keyframe(0.00f, 0.30f),  // 06:00 — amanhecer
        new Keyframe(0.25f, 1.00f),  // 10:00 — dia pleno
        new Keyframe(0.50f, 1.00f),  // 14:00 — dia pleno
        new Keyframe(0.75f, 0.40f),  // 18:00 — entardecer
        new Keyframe(1.00f, 0.10f)   // 22:00 — noite
    );

    private void Start()
    {
        // Aplica o estado visual imediatamente ao entrar na cena
        if (GameClock.Instance != null)
            Apply(GameClock.Instance.CurrentHour, GameClock.Instance.CurrentMinute);
    }

    private void OnEnable()
    {
        GameEvents.OnTimeChanged  += Apply;
        GameEvents.OnPlayerWokeUp += OnWokeUp;
    }

    private void OnDisable()
    {
        GameEvents.OnTimeChanged  -= Apply;
        GameEvents.OnPlayerWokeUp -= OnWokeUp;
    }

    // Atualiza iluminação ao acordar — o clock já está em 06:00 mas ainda não
    // disparou OnTimeChanged, então forçamos a atualização manual aqui
    private void OnWokeUp()
    {
        if (GameClock.Instance != null)
            Apply(GameClock.Instance.CurrentHour, GameClock.Instance.CurrentMinute);
    }

    private void Apply(int hour, int minute)
    {
        float t         = ComputeT(hour + minute / 60f);
        Color skyColor  = lightGradient.Evaluate(t);
        float intensity = intensityCurve.Evaluate(t);

        globalLight.color     = skyColor;
        globalLight.intensity = intensity;

        foreach (var managed in managedLights)
        {
            if (managed.light == null) continue;
            if (managed.useSkyColor) managed.light.color = skyColor;
            managed.light.intensity = intensity * managed.cycleStrength * managed.maxIntensity;
        }
    }

    // Mapeia qualquer hora do dia (0-24) para t 0-1 (amanhecer → noite)
    // 06:00-22:00 → ciclo normal (0→1)
    // 22:00-03:00 → noite fixa (1)
    // 03:00-06:00 → pré-amanhecer gradual (1→0)
    private float ComputeT(float time)
    {
        if (time >= dayEndHour || time < preDawnHour)
            return 1f;

        if (time < dayStartHour)
            return 1f - Mathf.InverseLerp(preDawnHour, dayStartHour, time);

        return Mathf.InverseLerp(dayStartHour, dayEndHour, time);
    }
}
