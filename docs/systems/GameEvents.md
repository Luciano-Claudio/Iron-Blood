# GameEvents

**Arquivo:** `Assets/Scripts/Core/GameEvents.cs`
**Tipo:** Classe estática — sem herança, sem instância
**Semana:** 1 — implementado como fundação da arquitetura de eventos

---

## Responsabilidade

Central única de todos os eventos do jogo. Nenhum sistema se comunica diretamente com outro — toda comunicação passa por aqui. Um sistema dispara um evento via `Raise*`, outros sistemas reagem assinando `On*`.

---

## Padrão de uso

```csharp
// Assinar (em OnEnable ou Start)
GameEvents.OnTimeChanged += MinhaFuncao;

// Cancelar (sempre em OnDisable)
GameEvents.OnTimeChanged -= MinhaFuncao;

// Disparar (só o sistema dono do evento chama o Raise)
GameEvents.RaiseTimeChanged(hora, minuto);
```

**Regra inviolável:** nunca assinar em `Awake()`. Usar `OnEnable()` ou `Start()` e sempre cancelar em `OnDisable()`. Listeners não cancelados causam erros quando o GameObject é destruído e referências mortas em cena.

---

## Regra de propriedade dos eventos

Cada evento tem um dono — o único sistema autorizado a chamar o `Raise` correspondente.

| Evento | Dono (quem dispara) | Quem consome |
|---|---|---|
| `OnTimeChanged` | `GameClock` | `SleepManager`, `DayNightVisuals`, `ClockHUD`, sistemas futuros |
| `OnNewDay` | `GameClock` | Sistemas de reset diário (loja, NPCs, estoque) |
| `OnNewWeek` | `GameClock` | `WeeklyVendor`, crafting por runs |
| `OnNewMonth` | `GameClock` | Sistema de estações (futuro) |
| `OnNewYear` | `GameClock` | Progressão narrativa (futuro) |
| `OnPlayerSlept` | `SleepManager` | Sistemas que precisam saber que o dia virou via sono |
| `OnPlayerWokeUp` | `SleepManager` | `DayNightVisuals`, `ClockHUD` (atualização imediata) |

---

## Eventos ativos (Semana 1)

### Tempo e Calendário

```csharp
// Dispara a cada minuto de jogo
public static event Action<int, int> OnTimeChanged;     // (hora, minuto)
public static void RaiseTimeChanged(int hour, int minute)

// Dispara quando o dia vira (meia-noite natural ou ao dormir)
public static event Action<int> OnNewDay;               // (dia absoluto)
public static void RaiseNewDay(int day)

// Dispara no primeiro dia de cada nova semana
public static event Action<int> OnNewWeek;              // (semana absoluta)
public static void RaiseNewWeek(int week)

// Dispara no primeiro dia de cada novo mês
// Gancho para sistema de estações — Mês 1 Primavera | 2 Verão | 3 Outono | 4 Inverno
public static event Action<int> OnNewMonth;             // (mês absoluto)
public static void RaiseNewMonth(int month)

// Dispara no primeiro dia de cada novo ano
public static event Action<int> OnNewYear;              // (ano absoluto)
public static void RaiseNewYear(int year)
```

### Sono

```csharp
// Dispara no cut point do fade (clock já pausado, dia já avançado)
public static event Action OnPlayerSlept;
public static void RaisePlayerSlept()

// Dispara quando o fade de acordar termina (clock já rodando)
public static event Action OnPlayerWokeUp;
public static void RaisePlayerWokeUp()
```

---

## Eventos comentados — semanas futuras

Todos os eventos futuros estão declarados e documentados no arquivo como comentários. Antes de implementar um novo sistema, descomentar o evento correspondente e verificar se o tipo de dado está correto.

| Evento | Sistema responsável | Semana |
|---|---|---|
| `OnShopOpened / OnShopClosed` | `ShopSign` | 3 |
| `OnInventoryChanged / OnItemAdded / OnItemRemoved` | `PlayerInventory` | 2 |
| `OnGoldChanged` | `EconomyManager` | 4 |
| `OnItemCrafted / OnCraftStarted / OnCraftCancelled` | `CraftingMinigame` | 5 |
| `OnSaleCompleted / OnNoSale / OnReputationChanged` | `CustomerAI` | 6 |
| `OnAgreementStarted / OnAgreementCompleted / OnAgreementFailed` | `AgreementSystem` | 8 |
| `OnWorkerHired / OnWorkerFired / OnWorkerUnpaid` | `WorkerAI` | 9 |
| `OnNarrativeTrigger` | `NarrativeManager` | 10 |
| `OnPlayerDied / OnPlayerDamageTaken` | `PlayerCombat` | 10 |
| `OnSeasonChanged / OnWeatherChanged` | `WeatherSystem` | pós-MVP |

---

## Script Execution Order

`GameClock` roda em **-100** via Script Execution Order — garante que `GameClock.Instance` esteja disponível antes de qualquer outro script tentar usar `GameEvents`. Configurar em Edit → Project Settings → Script Execution Order.

---

## Decisões de design

**Por que classe estática e não ScriptableObject de eventos?**
Em projetos solo de longa duração, a classe estática elimina a necessidade de arrastar referências no Inspector e simplifica o acesso de qualquer ponto do código. O custo é que os eventos não persistem entre cenas — mas como o projeto usa uma única cena principal (GameScene), isso não é problema.

**Por que métodos `Raise*` separados do evento?**
Evita o padrão `EventName?.Invoke()` espalhado pelo código. O `Raise` centraliza a chamada nula-segura e serve como ponto único de breakpoint para debug.

