# ClockHUD

**Arquivo:** `Assets/Scripts/UI/ClockHUD.cs`
**Tipo:** `MonoBehaviour`
**Dependências:** `GameClock`, `GameEvents`, `TMPro.TMP_Text`
**Semana:** 1 — Task C-4

---

## Responsabilidade

Exibe horário atual e dia do jogo em tempo real via TextMeshPro. Atualiza a cada minuto de jogo via evento e força atualização imediata em dois casos: entrada na cena e ao acordar.

---

## Campos no Inspector

| Campo | Tipo | Descrição |
|---|---|---|
| `timeText` | `TMP_Text` | Exibe `"HH:MM"` |
| `dayText` | `TMP_Text` | Exibe `"Dia X · Semana Y"` |

---

## Eventos assinados

| Evento | Quando dispara | Ação |
|---|---|---|
| `GameEvents.OnTimeChanged` | A cada minuto de jogo | `UpdateHUD(hora, minuto)` |
| `GameEvents.OnPlayerWokeUp` | Ao fim do fade de acordar | `UpdateHUD` com hora atual do clock |

---

## Lógica de atualização imediata

Dois casos onde o HUD precisa atualizar sem esperar o próximo tick de `OnTimeChanged`:

**1. Entrada na cena (`Start`):**
```csharp
private void Start()
{
    if (GameClock.Instance != null)
        UpdateHUD(GameClock.Instance.CurrentHour, GameClock.Instance.CurrentMinute);
}
```
Sem isso, o HUD ficaria em branco até o primeiro minuto de jogo passar.

**2. Ao acordar (`OnPlayerWokeUp`):**
```csharp
private void OnWokeUp()
{
    if (GameClock.Instance != null)
        UpdateHUD(GameClock.Instance.CurrentHour, GameClock.Instance.CurrentMinute);
}
```
Sem isso, o HUD mostraria o horário anterior ao sono por até 1 minuto após acordar.

---

## Formato de exibição

```csharp
timeText.text = $"{hour:00}:{minute:00}";
dayText.text  = $"Dia {GameClock.Instance.CurrentDay} · Semana {GameClock.Instance.CurrentWeek}";
```

---

## Regras de subscrição

```csharp
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
```

Subscrição em `OnEnable`, cancelamento em `OnDisable` — padrão obrigatório do projeto.

---

## Pendências

| Pendência | Semana |
|---|---|
| Estilo visual final (fonte, tamanho, posição, ícone de sol/lua) | 11 (polish de UI) |
| Exibir dia da semana por nome (ex: "Segunda", "Terça") | opcional — pós-MVP |
| Integrar mês e estação na HUD | pós-MVP |

