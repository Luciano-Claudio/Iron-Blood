# DayNightVisuals

**Arquivo:** `Assets/Scripts/Core/DayNightVisuals.cs`
**Tipo:** `MonoBehaviour`
**Dependências:** `GameClock`, `GameEvents`, `UnityEngine.Rendering.Universal.Light2D`
**Semana:** 1 — Task C-3

---

## Responsabilidade

Controla a iluminação dinâmica da cena com base no horário do jogo. Gerencia qualquer número de `Light2D` com parâmetros individuais de resposta ao ciclo. Não conhece o `GameClock` diretamente — reage apenas aos eventos de `GameEvents`.

---

## Ciclo de 24 horas

O ciclo é dividido em 3 zonas para evitar transição brusca na virada da meia-noite:

```
06:00 → 22:00   Ciclo completo      t: 0.0 → 1.0
22:00 → 03:00   Noite fixa          t: 1.0 (constante)
03:00 → 06:00   Pré-amanhecer       t: 1.0 → 0.0 (gradual)
```

**Cálculo de t:**
```csharp
private float ComputeT(float time)
{
    if (time >= dayEndHour || time < preDawnHour)  return 1f;
    if (time < dayStartHour)
        return 1f - Mathf.InverseLerp(preDawnHour, dayStartHour, time);
    return Mathf.InverseLerp(dayStartHour, dayEndHour, time);
}
```

---

## Arquitetura ManagedLight

Qualquer `Light2D` da cena (Global, Spot, Freeform) pode entrar na lista `managedLights` com parâmetros individuais:

```csharp
[System.Serializable]
public class ManagedLight
{
    public Light2D light;
    [Range(0f, 1f)] public float cycleStrength; // 0 = estática, 1 = 100% do ciclo
    public float maxIntensity;                  // teto absoluto no pico do dia
    public bool useSkyColor;                    // herda cor do gradiente ou mantém cor própria
}
```

**Fórmula de intensidade:**
```
light.intensity = intensityCurve(t) × cycleStrength × maxIntensity
```

---

## Separação Interior / Exterior

Implementada via Sorting Layers — sem scripts adicionais.

| Light2D | Target Sorting Layers | cycleStrength | Efeito |
|---|---|---|---|
| Global Light Exterior | Default, Player | 1.0 | Segue 100% o ciclo |
| Global Light Interior | Interior | 0.35 | ~28% do ciclo ao meio-dia, quase zero à noite |

**Sprites de interiores** (chão, móveis, paredes internas) devem usar Sorting Layer `Interior` para receber apenas a luz interior.

---

## Valores de referência — Spot Lights

| Tipo de abertura | cycleStrength | maxIntensity | useSkyColor |
|---|---|---|---|
| Janela grande | 0.85 | 0.7 | true |
| Janela pequena | 0.6 | 0.5 | true |
| Entrada de caverna | 0.25 | 0.6 | false |
| Tocha / Vela | 0.0 | — | false |

---

## Configuração do Gradiente (Inspector)

`lightGradient` — sugestão de cores por hora (t: 0 = 06:00, t = 1 = 22:00):

| t | Hora | Cor sugerida |
|---|---|---|
| 0.00 | 06:00 | Laranja-quente (amanhecer) |
| 0.25 | 10:00 | Branco-amarelado |
| 0.50 | 14:00 | Branco puro (meio-dia) |
| 0.75 | 18:00 | Laranja-dourado (entardecer) |
| 0.90 | 20:00 | Azul-acinzentado |
| 1.00 | 22:00 | Azul-escuro (noite) |

---

## Configuração da Curva de Intensidade (Inspector)

`intensityCurve` — valores padrão inicializados no código:

| Keyframe (t) | Hora | Intensidade |
|---|---|---|
| 0.00 | 06:00 | 0.30 |
| 0.25 | 10:00 | 1.00 |
| 0.50 | 14:00 | 1.00 |
| 0.75 | 18:00 | 0.40 |
| 1.00 | 22:00 | 0.10 |

---

## Integração com outros sistemas

| Sistema | Relação |
|---|---|
| `GameEvents.OnTimeChanged` | Dispara `Apply(hora, minuto)` a cada minuto |
| `GameEvents.OnPlayerWokeUp` | Força `Apply()` imediato — clock já está em 06:00 mas `OnTimeChanged` ainda não disparou |
| Sistemas futuros | Qualquer script pode assinar `GameEvents.OnTimeChanged` para controlar suas próprias `Light2D` sem modificar este script |

---

## Fluxo de execução

```
Start()
  └─ Apply(CurrentHour, CurrentMinute)   ← estado visual correto ao entrar na cena

OnTimeChanged(hora, minuto)
  └─ Apply(hora, minuto)                 ← atualização a cada minuto

OnPlayerWokeUp()
  └─ Apply(CurrentHour, CurrentMinute)   ← atualização imediata ao acordar

Apply(hora, minuto)
  └─ t = ComputeT(hora + minuto/60)
  └─ globalLight.color     = lightGradient.Evaluate(t)
  └─ globalLight.intensity = intensityCurve.Evaluate(t)
  └─ para cada ManagedLight:
       └─ se useSkyColor → light.color = skyColor
       └─ light.intensity = intensity × cycleStrength × maxIntensity
```

---

## Pendências

| Pendência | Semana |
|---|---|
| Ajuste fino de `cycleStrength 0.35` do interior (após luzes de forja e velas existirem) | 16 |
| `AutoLight.cs` para postes e janelas iluminadas automaticamente por horário | semana correspondente |
| Normal maps nos sprites para profundidade de iluminação 2D | pós-MVP |

