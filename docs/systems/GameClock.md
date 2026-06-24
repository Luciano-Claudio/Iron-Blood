# GameClock

**Arquivo:** `Assets/Scripts/Core/GameClock.cs`
**Tipo:** `MonoBehaviour` Singleton
**Script Execution Order:** -100
**Semana:** 1 — Task C-1

---

## Responsabilidade

Gerencia o tempo do jogo e o calendário. Avança minutos, dispara eventos de calendário e expõe o estado atual do tempo para qualquer sistema. Não sabe nada sobre UI, sono, fade ou qualquer outro sistema — só conta o tempo e dispara eventos.

---

## Calendário

```
1 semana = 6 dias
1 mês    = 5 semanas  = 30 dias
1 ano    = 4 meses    = 120 dias
```

Cada mês representa uma estação. Gancho: `GameEvents.OnNewMonth`.

| Mês | Estação |
|---|---|
| 1 | Primavera |
| 2 | Verão |
| 3 | Outono |
| 4 | Inverno |

**Constantes públicas** — alterar aqui reflete em todo o jogo:

```csharp
public const int DaysPerWeek   = 6;
public const int WeeksPerMonth = 5;
public const int MonthsPerYear = 4;
public const int DaysPerMonth  = 30;   // DaysPerWeek * WeeksPerMonth
public const int DaysPerYear   = 120;  // DaysPerMonth * MonthsPerYear
```

---

## Velocidade do tempo

```csharp
public float realSecondsPerGameHour = 25f;
```

| Configuração | Resultado |
|---|---|
| 25s / hora de jogo | 10 minutos reais = 1 dia completo (24h) |
| 1 minuto de jogo | ~0.416 segundos reais |

Ajustável no Inspector sem recompilar.

---

## Estado e propriedades

### Armazenadas
```csharp
public int  CurrentDay    { get; private set; } = 1;
public int  CurrentHour   { get; private set; }
public int  CurrentMinute { get; private set; }
public bool IsSleeping    { get; private set; }
```

### Calculadas on demand (não armazenadas)
```csharp
public int DayOfWeek    => (CurrentDay - 1) % DaysPerWeek + 1;                  // 1–6
public int DayOfMonth   => (CurrentDay - 1) % DaysPerMonth + 1;                 // 1–30
public int WeekOfMonth  => ((CurrentDay - 1) % DaysPerMonth) / DaysPerWeek + 1; // 1–5
public int MonthOfYear  => (CurrentMonth - 1) % MonthsPerYear + 1;              // 1–4
public int CurrentWeek  => (CurrentDay - 1) / DaysPerWeek + 1;                  // semana absoluta
public int CurrentMonth => (CurrentDay - 1) / DaysPerMonth + 1;                 // mês absoluto
public int CurrentYear  => (CurrentDay - 1) / DaysPerYear + 1;                  // ano absoluto
```

---

## API pública

```csharp
// Pausa o clock imediatamente — usado pelo SleepManager no início da sequência.
// Bloqueia Update() via IsSleeping = true. PlayerMovement verifica IsSleeping.
public void PauseForSleep()

// Avança o dia para o seguinte e pula para 06:00.
// Chamado pelo SleepManager no cut point do fade (tela 100% coberta).
// PauseForSleep() deve ter sido chamado antes.
public void Sleep()

// Retoma o clock após o fade de acordar.
// Chamado pelo SleepManager em onTransitionEnd.
public void WakeUp()

// Retorna o horário formatado: "HH:MM"
public string GetTimeString()
```

---

## Fluxo interno

```
Update()
  └─ timer += Time.deltaTime
  └─ se timer >= minuteDuration → AdvanceMinute()

AdvanceMinute()
  └─ CurrentMinute++
  └─ se CurrentMinute >= 60 → CurrentHour++
  └─ se CurrentHour >= 24  → AdvanceDay()
  └─ RaiseTimeChanged(hora, minuto)

AdvanceDay()
  └─ CurrentDay++
  └─ RaiseNewDay()
  └─ se semana mudou  → RaiseNewWeek()
  └─ se mês mudou     → RaiseNewMonth()
  └─ se ano mudou     → RaiseNewYear()
```

---

## Separação de responsabilidades — PauseForSleep vs Sleep

Dois métodos distintos por design intencional:

| Método | Quando chamar | O que faz |
|---|---|---|
| `PauseForSleep()` | Início da sequência de sono | Para o clock imediatamente. Player para de se mover. |
| `Sleep()` | Cut point do fade (tela coberta) | Avança o dia, pula para 06:00, dispara eventos de calendário. |

Isso garante que o player não veja o dia virar — o calendário só avança quando a tela está preta.

---

## Integração com outros sistemas

| Sistema | Como usa o GameClock |
|---|---|
| `SleepManager` | Chama `PauseForSleep()`, `Sleep()`, `WakeUp()`. Lê `CurrentHour` para validar hora mínima de sono. |
| `PlayerMovement` | Verifica `IsSleeping` em `OnMove()` e `FixedUpdate()` — zera velocidade durante o sono. |
| `DayNightVisuals` | Assina `GameEvents.OnTimeChanged`. Lê `CurrentHour/Minute` no `Start()` e após acordar. |
| `ClockHUD` | Assina `GameEvents.OnTimeChanged`. Lê `CurrentDay`, `CurrentWeek`. |
| `SleepManager` | Assina `GameEvents.OnTimeChanged` para detectar hora do sono forçado. |

---

## Decisões de design

**Dia começa às 06:00:** narrativamente justificado como hora de chegada de Durin à ferraria no Dia 1. Todos os dias subsequentes também começam às 06:00 após dormir.

**Virada natural à meia-noite:** se o jogador não dormir, o dia avança normalmente às 00:00. O sono forçado às 02:00 (SleepManager) impede que o jogador fique acordado indefinidamente.

**Propriedades calculadas:** `DayOfWeek`, `DayOfMonth` etc. não são armazenadas para evitar estado redundante. O custo de calcular é insignificante; o risco de dessincronizar estado armazenado é real.

---

## Pendências

| Pendência | Sistema responsável | Semana |
|---|---|---|
| Estações por mês | `WeatherSystem` | pós-MVP |
| Clima por seed dentro de cada estação | `WeatherSystem` | pós-MVP |
| Efeitos do clima em coleta e clientes | `ForestArea`, `CustomerSpawner` | pós-MVP |

