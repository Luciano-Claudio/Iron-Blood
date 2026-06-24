# SleepManager

**Arquivo:** `Assets/Scripts/Core/SleepManager.cs`
**Tipo:** `MonoBehaviour` Singleton
**Dependências:** `GameClock`, `GameEvents`, `EasyTransition.TransitionManager`
**Semana:** 1 — Task C-2

---

## Responsabilidade

Orquestra a sequência completa de sono: pausa o clock, executa animação, dispara a transição de tela, avança o dia no momento certo e retoma o jogo. Não gerencia tempo — isso é responsabilidade do `GameClock`.

---

## Casos de sono

### Caso 1 — Voluntário

Disponível a partir das 18:00. Acionado pela cama ao pressionar E (Interact).

```
Cama.Interact()
  └─ SleepManager.TrySleep()
       └─ Valida hora (>= 18:00)
       └─ SleepSequence(forced: false)
            1. GameClock.PauseForSleep()     ← clock para, player trava
            2. WaitForSecondsRealtime(2f)    ← animação placeholder
            3. tm.Transition(sleepTransition) ← brush cobre a tela
            4. onTransitionCutPointReached:
               └─ GameClock.Sleep()          ← dia avança, pula para 06:00
               └─ GameEvents.RaisePlayerSlept()
            5. onTransitionEnd:
               └─ GameClock.WakeUp()         ← clock volta
               └─ GameEvents.RaisePlayerWokeUp()
               └─ limpa callbacks
               └─ isSleeping = false
```

### Caso 2 — Forçado

Ativado automaticamente em `forcedSleepHour` (padrão 02:00, configurável no Inspector). Mesma sequência do Caso 1, mais penalidades no cut point e respawn na Igreja ao acordar.

**Avisos antes do sono forçado:**

| Antecedência | Ação |
|---|---|
| 10 minutos | `ShowSleepWarning(10)` → TODO: ícone ZZZ no HUD |
| 5 minutos | `ShowSleepWarning(5)` → TODO: ícone ZZZ no HUD |
| 0 minutos | `ForceSleep()` |

**Detecção via `GameEvents.OnTimeChanged`:**
```csharp
int current = hour * 60 + minute;
int forced  = forcedSleepHour * 60;
if (current == forced - 10) ShowSleepWarning(10);
if (current == forced - 5)  ShowSleepWarning(5);
if (current == forced)      ForceSleep();
```

---

## Regras de implementação

| Regra | Descrição |
|---|---|
| R1 | EasyTransitions usa `WaitForSecondsRealtime` internamente — imune a `Time.timeScale` |
| R2 | `GameClock` não conhece o fade — só expõe `PauseForSleep()`, `Sleep()`, `WakeUp()` |
| R3 | Clock e movimento pausados no instante zero, antes da animação de 2s |
| R4 | Flag `isSleeping` bloqueia `TrySleep()`, `ForceSleep()` e `CheckSleepTime()` durante a sequência |

---

## Integração com EasyTransitions

```csharp
var tm = TransitionManager.Instance();

tm.onTransitionCutPointReached = () => { /* avança o dia */ };
tm.onTransitionEnd             = () => { /* acorda */ };

tm.Transition(sleepTransition, transitionDelay);
```

**Importante:** os callbacks são limpos em `onTransitionEnd` para evitar acúmulo entre transições. O `TransitionManager` é reutilizado — não instanciar um novo.

Transição configurada: **Brush** (`Assets/EasyTransitions/Transitions/Brush/Brush.asset`). Atribuir no Inspector em `sleepTransition`.

---

## Integração com outros sistemas

| Sistema | Relação |
|---|---|
| `GameClock` | Chama `PauseForSleep()`, `Sleep()`, `WakeUp()`. Lê `CurrentHour`. |
| `GameEvents` | Assina `OnTimeChanged` para detectar hora forçada. Dispara `RaisePlayerSlept()` e `RaisePlayerWokeUp()`. |
| `PlayerMovement` | Não há referência direta — `PlayerMovement` verifica `GameClock.IsSleeping` de forma autônoma. |
| `EasyTransition.TransitionManager` | Chamado via `Instance()` dentro da Coroutine. |

---

## Inspector

| Campo | Tipo | Descrição |
|---|---|---|
| `forcedSleepHour` | `int` | Hora do sono forçado (padrão: 2 = 02:00) |
| `sleepTransition` | `TransitionSettings` | Asset Brush do EasyTransitions |
| `transitionDelay` | `float` | Delay antes de iniciar o brush (padrão: 0) |
| `churchSpawnPoint` | `Transform` | Ponto de respawn da Igreja — atribuir na Semana 2 |

---

## Scripts temporários relacionados

**`SleepTester.cs`** — deletar quando a cama for implementada.
Tecla configurada como `Teste` no `PlayerInputActions`. Chama `SleepManager.Instance.TrySleep()`.
Para testar sono forçado: alterar `forcedSleepHour` para 1 no Inspector.

---

## Pendências

| Pendência | Sistema responsável | Semana |
|---|---|---|
| Ícone ZZZ no HUD nos avisos | `HUDManager` | 4 |
| Toast "Ainda é cedo para dormir" | `HUDManager` | 4 |
| Penalidade: -10% ouro (máx. 1.000) | `EconomyManager` | 4 |
| Saúde e energia a 50% ao acordar | `StatsManager` | 5 |
| Respawn na Igreja (sono forçado) | `churchSpawnPoint` | 2 |
| Pausa de combate durante sono | `CombatManager` | 10 |
| Inimigos ignoram player durante sono | `EnemyAIManager` | 10 |
| Animação real de dormir | `PlayerAnimator` | 7 |

