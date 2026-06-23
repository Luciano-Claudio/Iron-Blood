# Iron & Blood — Tasks da Semana 1
**Sprint:** Semana 1 de 20 | **Fase:** 1 — Fundação
**Foco:** Setup do projeto + movimentação do jogador + ciclo de tempo

> ⚠️ Sprint prioritária sem margem de atraso. Não avance para a Semana 2 sem o entregável final validado.

---

## Entregável Final da Semana
> O player se move com WASD em top-down, a câmera o segue, o ciclo dia/noite avança visivelmente com variação de luz, e ao chegar às 21h o jogo exibe um fade e reinicia o dia às 6h.

---

## BLOCO A — Setup do Projeto

### TASK A-1 — Criar o projeto Unity

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | A — Setup |
| Estimativa | 20 min |
| Depende de | — |

- Abrir Unity Hub → New Project
- Versão: **Unity 6.3 LTS**
- Template: **2D (URP)**
- Nome: `IronAndBlood`
- Caminho: sem espaços ou acentos

**Critério de conclusão:** Projeto abre sem erros. URP Pipeline Asset visível em Project Settings → Graphics.

---

### TASK A-2 — Instalar pacotes obrigatórios

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | A — Setup |
| Estimativa | 20 min |
| Depende de | A-1 |

Abrir **Window → Package Manager** e instalar via Unity Registry:

| Pacote | Versão mínima |
|---|---|
| Input System | 1.7+ |
| Cinemachine | 3.x |
| TextMeshPro | 3.0+ |
| 2D Tilemap Extras | 3.1+ |

Após instalar o **Input System** → confirmar **"Yes"** na troca de backend.

Instalar **DOTween** manualmente:
- Baixar em demigiant.com/plugins/dotween
- Importar `.unitypackage`
- Rodar: Tools → Demigiant → DOTween Utility Panel → Setup DOTween

**Critério de conclusão:** Todos os pacotes como "Installed". Sem erros no console.

---

### TASK A-3 — Configurar layers e Physics 2D

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | A — Setup |
| Estimativa | 15 min |
| Depende de | A-1 |

Edit → Project Settings → Tags and Layers:

| Layer | Nome |
|---|---|
| 6 | Player |
| 7 | Enemy |
| 8 | NPC |
| 9 | Interactable |
| 10 | PlayerAttack |
| 11 | VFX |

Physics 2D → Layer Collision Matrix:

| | Player | Enemy | NPC | Interactable |
|---|---|---|---|---|
| **Player** | ✗ | ✓ | ✓ | ✓ |
| **Enemy** | ✓ | ✗ | ✗ | ✗ |
| **PlayerAttack** | ✗ | ✓ | ✗ | ✗ |
| **VFX** | ✗ | ✗ | ✗ | ✗ |

**Critério de conclusão:** Matrix salva. Sem layers duplicadas.

---

### TASK A-4 — Criar cena GameScene e tilemap placeholder

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | A — Setup |
| Estimativa | 20 min |
| Depende de | A-2, A-3 |

- File → New Scene → Basic 2D
- Salvar como `Assets/Scenes/GameScene.unity`
- Criar Tilemap de chão interior (representa o piso da ferraria):
  - GameObject → 2D Object → Tilemap → Rectangular
  - Pintar área de ~25×20 tiles com cor sólida qualquer
  - Nomear: `Floor_Tilemap`
- Adicionar paredes placeholder (segundo Tilemap com TilemapCollider2D)
- Configurar câmera:
  - Adicionar **Cinemachine Camera** à cena
  - Adicionar **Pixel Perfect Camera** na Main Camera
  - PPU: `16` | Reference Resolution: `320 × 180`

**Critério de conclusão:** Cena salva. Tilemap de chão e paredes visíveis. Câmera sem distorção de pixel.

---

## BLOCO B — Player e Movimentação

### TASK B-1 — Criar Input Action Asset

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | B — Player |
| Estimativa | 20 min |
| Depende de | A-2 |

- Project → Create → Input Actions
- Nomear: `PlayerInputActions`
- Salvar em `Assets/Settings/`
- Action Map `Player`:

| Action | Type | Binding |
|---|---|---|
| `Move` | Value / Vector2 | WASD + Arrow Keys |
| `Interact` | Button | F |
| `Attack` | Button | Mouse Left Button |
| `ToggleInventory` | Button | Tab |
| `Sleep` | Button | E (placeholder — cama) |

- Marcar **"Generate C# Class"** → Apply

**Critério de conclusão:** `.inputactions` salvo. Classe C# gerada sem erros.

---

### TASK B-2 — Criar GameObject Player

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | B — Player |
| Estimativa | 15 min |
| Depende de | A-4 |

Hierarquia na GameScene:

```
Player (GameObject)
├── Rigidbody2D
│     ├── Body Type: Dynamic
│     ├── Gravity Scale: 0
│     ├── Collision Detection: Continuous
│     └── Freeze Rotation Z: ✓
├── CapsuleCollider2D
│     ├── Direction: Vertical
│     └── Size: (0.5, 0.8)
├── SpriteRenderer
│     └── Sprite: placeholder (quadrado colorido)
└── Tag: "Player" | Layer: "Player"
```

Configurar Cinemachine Camera:
- Follow: `Player`
- Body: Framing Transposer → Dead Zone: `0`

**Critério de conclusão:** Player visível. Câmera centralizada ao dar Play.

---

### TASK B-3 — Implementar PlayerMovement.cs

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | B — Player |
| Estimativa | 40 min |
| Depende de | B-1, B-2 |

Criar `Assets/Scripts/Player/PlayerMovement.cs`:

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private float moveSpeed = 4f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public Vector2 LastMoveDirection { get; private set; } = Vector2.down;

    private void Awake()
        => rb = GetComponent<Rigidbody2D>();

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if (moveInput != Vector2.zero)
            LastMoveDirection = moveInput.normalized;
    }

    private void FixedUpdate()
        => rb.linearVelocity = moveInput * moveSpeed;
}
```

Adicionar ao Player:
- `PlayerMovement`
- `PlayerInput` → apontar para `PlayerInputActions` → Behavior: **Send Messages**

**Armadilha:** No Iron & Blood o player NÃO rotaciona para o cursor — é um gerenciador de loja, não um jogo de mira. A direção é controlada só pelo WASD. Não implementar AimController aqui.

**Critério de conclusão:** Player se move com WASD. Para imediatamente ao soltar as teclas. Não atravessa paredes.

---

## BLOCO C — Sistema de Tempo

### TASK C-1 — Implementar GameClock.cs

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | C — Tempo |
| Estimativa | 50 min |
| Depende de | A-1 |

Criar `Assets/Scripts/Core/GameClock.cs`:

```csharp
using UnityEngine;
using System;

public class GameClock : MonoBehaviour
{
    public static GameClock Instance;

    [Header("Configuração")]
    public float realSecondsPerGameHour = 0.833f; // 10min real = 12h jogo
    public int startHour = 6;

    [Header("Estado — somente leitura")]
    public int CurrentHour    { get; private set; }
    public int CurrentMinute  { get; private set; }
    public int CurrentDay     { get; private set; } = 1;
    public int CurrentWeek    => (CurrentDay - 1) / 7 + 1;
    public bool IsSleeping    { get; private set; }

    private float timer;

    public event Action<int, int> OnTimeChanged;  // hora, minuto
    public event Action<int>      OnNewDay;        // nº do dia
    public event Action<int>      OnNewWeek;       // nº da semana

    private void Awake()
    {
        Instance = this;
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

            if (CurrentHour >= 21)
                SleepManager.Instance.ForceSleep();
        }
        OnTimeChanged?.Invoke(CurrentHour, CurrentMinute);
    }

    public void Sleep()
    {
        if (IsSleeping) return;
        IsSleeping = true;

        int prevWeek = CurrentWeek;
        CurrentDay++;
        CurrentHour   = startHour;
        CurrentMinute = 0;
        timer         = 0f;

        OnNewDay?.Invoke(CurrentDay);
        if (CurrentWeek != prevWeek)
            OnNewWeek?.Invoke(CurrentWeek);
    }

    public void WakeUp() => IsSleeping = false;

    // Utilitário para HUD
    public string GetTimeString()
        => $"{CurrentHour:00}:{CurrentMinute:00}";
}
```

Criar GameObject `GameManager` na cena e adicionar `GameClock`.

**Critério de conclusão:** Script compila. No Inspector, CurrentHour e CurrentDay aparecem e atualizam em Play Mode.

---

### TASK C-2 — Implementar SleepManager.cs

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | C — Tempo |
| Estimativa | 35 min |
| Depende de | C-1 |

Criar `Assets/Scripts/Core/SleepManager.cs`:

```csharp
using UnityEngine;
using System.Collections;

public class SleepManager : MonoBehaviour
{
    public static SleepManager Instance;

    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake() => Instance = this;

    // Chamado pelo GameClock às 21h
    public void ForceSleep()
    {
        StartCoroutine(SleepSequence());
    }

    // Chamado pelo jogador ao interagir com a cama
    public void TrySleep()
    {
        if (GameClock.Instance.CurrentHour < 18)
        {
            HUDManager.Instance.ShowToast("Ainda é cedo para dormir.");
            return;
        }
        StartCoroutine(SleepSequence());
    }

    private IEnumerator SleepSequence()
    {
        // Fade out
        yield return StartCoroutine(Fade(0f, 1f));

        GameClock.Instance.Sleep();
        yield return new WaitForSecondsRealtime(1.5f);

        // Fade in
        yield return StartCoroutine(Fade(1f, 0f));

        GameClock.Instance.WakeUp();
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            fadeCanvas.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }
        fadeCanvas.alpha = to;
    }
}
```

Criar na cena:
- Canvas com `CanvasGroup` preto cobrindo a tela toda → `fadeCanvas`
- Adicionar `SleepManager` no `GameManager`

**Critério de conclusão:** Às 21h ocorre fade para preto, dia avança, fade de volta. CurrentDay incrementa corretamente.

---

### TASK C-3 — Implementar DayNightVisuals.cs

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | C — Tempo |
| Estimativa | 30 min |
| Depende de | C-1 |

Criar `Assets/Scripts/Core/DayNightVisuals.cs`:

```csharp
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightVisuals : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;

    [Header("Gradiente de cor por hora (6h → 21h)")]
    [SerializeField] private Gradient lightGradient;

    [Header("Intensidade por hora (6h → 21h)")]
    [SerializeField] private AnimationCurve intensityCurve;

    private void OnEnable()
        => GameClock.Instance.OnTimeChanged += UpdateLighting;

    private void OnDisable()
        => GameClock.Instance.OnTimeChanged -= UpdateLighting;

    private void UpdateLighting(int hour, int minute)
    {
        // Normaliza 6h-21h para 0-1
        float t = Mathf.InverseLerp(6f, 21f, hour + minute / 60f);
        globalLight.color     = lightGradient.Evaluate(t);
        globalLight.intensity = intensityCurve.Evaluate(t);
    }
}
```

Configurar no Inspector:
- `lightGradient`: manhã (amarelo claro) → meio-dia (branco) → tarde (laranja) → noite (azul escuro)
- `intensityCurve`: começa em 0.6, sobe para 1.0 ao meio-dia, desce para 0.2 à noite

Adicionar `Light2D` global na cena (GameObject → Light → Global Light 2D).

**Armadilha:** Você já fez sistema de iluminação antes — adapte o que já conhece ao invés de reescrever do zero. O `DayNightVisuals` só precisa receber o evento do `GameClock` e aplicar os valores.

**Critério de conclusão:** A iluminação muda visivelmente conforme o dia avança. Noite é notavelmente mais escura que o meio-dia.

---

### TASK C-4 — HUD de tempo e dia

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Média |
| Bloco | C — Tempo |
| Estimativa | 25 min |
| Depende de | C-1 |

Criar HUD simples com TextMeshPro mostrando:
- Horário atual (ex: `14:30`)
- Dia atual (ex: `Dia 3`)
- Semana atual (ex: `Semana 1`)

```csharp
// ClockHUD.cs
public class ClockHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text dayText;

    private void OnEnable()
        => GameClock.Instance.OnTimeChanged += UpdateHUD;

    private void UpdateHUD(int hour, int minute)
    {
        timeText.text = $"{hour:00}:{minute:00}";
        dayText.text  = $"Dia {GameClock.Instance.CurrentDay} " +
                        $"· Semana {GameClock.Instance.CurrentWeek}";
    }
}
```

**Critério de conclusão:** HUD exibe hora e dia atualizando em tempo real durante o Play Mode.

---

## Checklist de Validação Final

Antes de encerrar a semana, confirmar cada item:

- [ ] Projeto Unity 6.3 LTS criado sem erros
- [ ] Input System instalado e backend trocado
- [ ] Layers configuradas corretamente
- [ ] Player se move com WASD e para ao soltar as teclas
- [ ] Player não atravessa paredes do tilemap
- [ ] Câmera segue o player sem distorção de pixel
- [ ] GameClock avança hora e minuto visivelmente
- [ ] Às 21h ocorre fade para preto e o dia avança
- [ ] Ao acordar, começa às 6h do dia seguinte
- [ ] CurrentDay e CurrentWeek incrementam corretamente
- [ ] Iluminação muda conforme o horário
- [ ] HUD exibe hora e dia em tempo real
- [ ] Zero erros de compilação no console

---

## Armadilhas Conhecidas desta Semana

**"Cinemachine no Unity 6 tem API diferente"**
→ O componente se chama `CinemachineCamera`, não `CinemachineVirtualCamera`. Documentação online pode estar desatualizada.

**"Light2D não aparece na cena"**
→ Verificar se o projeto está usando URP. Light2D só funciona com URP ativo.

**"O fade não funciona na build"**
→ Usar `Time.unscaledDeltaTime` no Fade — o fade precisa funcionar mesmo se o TimeScale estiver alterado.

**"Player desliza ao soltar WASD"**
→ O `linearVelocity` sendo setado direto no FixedUpdate já resolve. Se ainda deslizar, verificar se há outro script alterando a velocidade.

**"GameClock não dispara eventos"**
→ Verificar se os listeners usam `OnEnable`/`OnDisable` para se inscrever e desinscrever. Listeners que não se desinscrevem causam erros quando o objeto é destruído.

---
*Semana 1 · Iron & Blood · Unity 6.3 LTS · Gerado em 23/06/2026*
