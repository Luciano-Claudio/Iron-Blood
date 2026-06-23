# GAME DESIGN DOCUMENT
# Iron & Blood — Simulador de Ferraria Medieval
**Versão:** 1.0 — MVP
**Engine:** Unity 6.3 LTS | **Linguagem:** C# | **Perspectiva:** Top-Down 2D Pixel Art
**Referência principal:** Graveyard Keeper + Jacksmith
**Dev:** Solo | **Prazo MVP:** 3 meses

---

## ÍNDICE

1. [Resumo Executivo e Core Loop](#1-resumo-executivo-e-core-loop)
2. [Narrativa e Protagonista](#2-narrativa-e-protagonista)
3. [Sistema de Tempo e Ciclo Diário](#3-sistema-de-tempo-e-ciclo-diário)
4. [Sistema de Inventário](#4-sistema-de-inventário)
5. [A Ferraria — Layout e Expansão](#5-a-ferraria--layout-e-expansão)
6. [Mini-game de Craft (Estilo Jacksmith)](#6-mini-game-de-craft-estilo-jacksmith)
7. [Sistema de Abertura e Fechamento da Loja](#7-sistema-de-abertura-e-fechamento-da-loja)
8. [Tutorial — Marta e o Primeiro Dia](#8-tutorial--marta-e-o-primeiro-dia)
9. [Sistema de Clientes e IA de Compra](#9-sistema-de-clientes-e-ia-de-compra)
10. [Coleta — Floresta e Mina](#10-coleta--floresta-e-mina)
11. [Sistema de Acordos e Relacionamentos](#11-sistema-de-acordos-e-relacionamentos)
12. [Funcionários e Automação](#12-funcionários-e-automação)
13. [Vendedor Semanal de Minérios](#13-vendedor-semanal-de-minérios)
14. [Progressão Narrativa — Segredos da Ferraria](#14-progressão-narrativa--segredos-da-ferraria)
15. [Combate Simples](#15-combate-simples)
16. [Escopo MVP — 3 Meses Solo](#16-escopo-mvp--3-meses-solo)

---

## 1. RESUMO EXECUTIVO E CORE LOOP

### 1.1 Identidade do Jogo

| Atributo | Valor |
|---|---|
| Gênero | Shop Management / Tycoon narrativo |
| Perspectiva | Top-Down 2D Pixel Art |
| Referências | Graveyard Keeper, Jacksmith, Stardew Valley |
| Protagonista | Anão ferreiro — narrativa linear com fim definido |
| Plataforma | PC (Windows/Linux) |
| Tom | Medieval, humano, com mistério crescente |

### 1.2 Premissa

Você é **Durin**, um anão que nunca conheceu bem o pai — um ferreiro recluso que vivia numa pequena aldeia chamada **Feudo de Cinzas**. Após a morte do pai, você herda a ferraria e parte para assumir o negócio. O que você não esperava é que o pai guardava segredos dentro das próprias paredes da forja. Segredos que só se revelam quando você constrói as ferramentas certas.

### 1.3 Core Loop — Macro

```
[MANHÃ — 6h]
      │
      ▼
Verificar demanda da loja
(clientes chegam ao longo do dia)
      │
      ▼
Craftar itens na bancada
(mini-game Jacksmith)
      │
      ▼
Colocar itens nos baús/prateleiras da loja
      │
      ▼
Sair para coletar (floresta/mina) OU conversar com NPCs OU fechar acordos
      │
      ▼
[NOITE — 21h] Dormir obrigatório → avança para o próximo dia
      │
      ▼
[Semana nova] → Vendedor de minérios aparece
              → Contratos de acordos vencem ou renovam
              → Novo evento narrativo pode desbloquear
```

### 1.4 Core Loop — Micro (por dia)

```
Acorda às 6h
    │
    ├─ Loja abre automaticamente
    │   Clientes entram, compram do estoque dos baús
    │
    ├─ Jogador crafta na bancada
    │   Mini-game de montagem de peças
    │   Item ganha nota (D → S) baseada na precisão
    │
    ├─ Jogador sai para floresta/mina
    │   Coleta madeira/minérios
    │   Combate simples se for à mina
    │
    ├─ Jogador conversa com NPCs
    │   Relacionamentos avançam
    │   Acordos podem ser iniciados
    │
    └─ 21h → Dorme → Dia avança
```

### 1.5 Pilares de Design

**Pilar 1 — A Ferraria é Viva:** A loja não é um menu — é um espaço físico top-down onde o jogador anda, interage com bancadas, baús e clientes em tempo real.

**Pilar 2 — Crafting com Peso:** Cada item fabricado tem uma nota. Um cliente satisfeito com uma arma S-rank volta. Um insatisfeito com D-rank pode reclamar publicamente, afastando clientes.

**Pilar 3 — Narrativa por Conquista:** A história não é contada em cutscenes longas. Ela se revela quando o jogador constrói itens específicos, entra em cômodos bloqueados ou atinge marcos de relacionamento.

**Pilar 4 — Economia Real:** Sem dinheiro infinito. Cada decisão de compra de minério, contratação de funcionário ou desconto em acordo tem impacto real no fluxo de caixa semanal.

---

## 2. NARRATIVA E PROTAGONISTA

### 2.1 Durin — O Protagonista

**Durin Ironcroft** é um anão de meia idade que passou a vida longe do pai, **Aldric Ironcroft**, sem nunca entender bem o motivo do distanciamento. Quando Aldric morre, um mensageiro entrega a Durin uma chave enferrujada e uma carta curta:

> *"A ferraria é sua agora. Não venda. Não destrua nada. Você vai entender quando chegar."*

Durin não é um herói. É um filho confuso tentando entender o pai que nunca conheceu — e pagar as contas no processo.

### 2.2 O Feudo de Cinzas

Uma aldeia pequena, cercada por floresta densa e uma mina ao norte. O feudo tem:

| Local | Função no jogo |
|---|---|
| **A Ferraria** (sede do jogador) | Crafting, loja, moradia |
| **A Praça** | NPCs circulam, eventos acontecem |
| **A Floresta** | Coleta de madeira, encontros com espécies |
| **A Mina** | Minérios raros, monstros, segredos |
| **A Taverna** | Conversar com NPCs, ouvir rumores, recrutar |
| **O Celeiro do Feudo** | Sede do acordo com o Feudo |
| **A Floresta Profunda** | Desbloqueada narrativamente — espécies não-humanas |

### 2.3 NPCs Principais

| NPC | Papel | Arco narrativo |
|---|---|---|
| **Marta** | Vizinha idosa, conhecia Aldric | Revela fragmentos do passado do pai |
| **Ser Edwyn** | Cavaleiro do Feudo, cliente frequente | Representa os interesses do Senhor local |
| **Pip** | Menino da vila, curioso | Primeiro a mencionar "os sons estranhos da ferraria" |
| **Thresh** | Goblin comerciante da floresta | Primeiro acordo com espécie não-humana |
| **A Sombra** | Figura encapuzada, aparece na semana 3 | Núcleo do mistério — ligada ao pai |

### 2.4 Estrutura Narrativa — Atos

**Ato 1 — O Herdeiro (semanas 1-3 do jogo):**
Durin chega, abre a ferraria, conhece o feudo. A loja está em mau estado — poucos clientes, estoque vazio. Aprende o básico do ofício. Marta começa a contar histórias do pai.

**Ato 2 — O Ferreiro (semanas 4-8):**
A loja cresce. Acordos com o Feudo e com Thresh. Primeiro funcionário contratado. Pip encontra uma porta trancada no fundo da ferraria. A Sombra aparece pela primeira vez.

**Ato 3 — O Segredo (semanas 9+):**
Itens específicos craftados revelam mecanismos escondidos na ferraria. Cômodos secretos. O passado de Aldric se revela — ele não era apenas um ferreiro. O jogador decide o que fazer com o que encontrou.

**Fim:** Determinado pela escolha final do jogador em relação ao segredo do pai. Dois finais possíveis no MVP.

---

## 3. SISTEMA DE TEMPO E CICLO DIÁRIO

### 3.1 Estrutura do Dia

| Período | Horário | O que acontece |
|---|---|---|
| Manhã | 6h — 12h | Loja aberta, primeiros clientes, melhor hora para craftar |
| Tarde | 12h — 18h | Pico de clientes, coleta na floresta/mina |
| Anoitecer | 18h — 21h | Últimos clientes, NPCs na taverna, eventos noturnos |
| Noite | 21h — 6h | Dorme obrigatório (ou paga penalidade de energia) |

**Duração:** ~10 minutos por dia de jogo (5 min dia / 5 min noite — ajustável por testes).
**Início:** Todo dia começa às 6h após dormir.

### 3.2 Implementação — GameClock.cs

```csharp
public class GameClock : MonoBehaviour
{
    public static GameClock Instance;

    [Header("Configuração")]
    public float realSecondsPerGameHour = 0.833f; // 10min real = 12h de jogo
    public int startHour = 6;

    [Header("Estado")]
    public int CurrentHour   { get; private set; }
    public int CurrentMinute { get; private set; }
    public int CurrentDay    { get; private set; } = 1;
    public int CurrentWeek   => (CurrentDay - 1) / 7 + 1;

    private float timer;
    private bool isSleeping;

    public event Action<int, int> OnTimeChanged;  // hora, minuto
    public event Action<int>      OnNewDay;        // dia
    public event Action<int>      OnNewWeek;       // semana

    private void Awake() => Instance = this;

    private void Start()
    {
        CurrentHour = startHour;
        CurrentMinute = 0;
    }

    private void Update()
    {
        if (isSleeping) return;

        timer += Time.deltaTime;
        if (timer >= realSecondsPerGameHour / 60f)
        {
            timer = 0f;
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

            // 21h → força dormir
            if (CurrentHour >= 21)
                SleepManager.Instance.ForceSleep();
        }
        OnTimeChanged?.Invoke(CurrentHour, CurrentMinute);
    }

    public void Sleep()
    {
        isSleeping = true;
        int previousWeek = CurrentWeek;

        CurrentDay++;
        CurrentHour = startHour;
        CurrentMinute = 0;
        timer = 0f;

        OnNewDay?.Invoke(CurrentDay);
        if (CurrentWeek != previousWeek)
            OnNewWeek?.Invoke(CurrentWeek);

        // Fade in/out de tela + variação de luz
        StartCoroutine(WakeUpSequence());
    }

    private IEnumerator WakeUpSequence()
    {
        yield return new WaitForSeconds(1.5f); // fade de tela
        isSleeping = false;
    }
}
```

### 3.3 Sistema de Iluminação por Hora

O jogador mencionou que já implementou variação de sombras/arte por período — o `GameClock` expõe os eventos necessários:

```csharp
// DayNightVisuals.cs — conecta ao GameClock
public class DayNightVisuals : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Gradient lightColorByHour; // configurado no Inspector

    private void OnEnable()
        => GameClock.Instance.OnTimeChanged += UpdateLighting;

    private void UpdateLighting(int hour, int minute)
    {
        float t = (hour - 6f) / 15f; // normaliza 6h-21h para 0-1
        globalLight.color = lightColorByHour.Evaluate(t);
    }
}
```

---

## 4. SISTEMA DE INVENTÁRIO

### 4.1 Filosofia

Existem **dois tipos de inventário** no jogo:

| Tipo | Quem acessa | O que armazena |
|---|---|---|
| **Inventário Pessoal do Jogador** | Só o jogador | Materiais coletados, itens em trânsito |
| **Inventário de Objeto** (baú/prateleira) | Jogador + clientes | Itens craftados à venda, materiais armazenados |

Não existe "inventário da loja" global — o estoque é físico, distribuído nos baús e prateleiras colocados na ferraria. Um baú no fundo guarda madeira. Uma prateleira perto da porta exibe espadas. **Se não está num recipiente acessível aos clientes, não está à venda.**

### 4.2 Inventário Pessoal

```csharp
[Serializable]
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    [Header("Configuração")]
    public int maxSlots = 20;

    private List<ItemStack> items = new();

    public event Action OnInventoryChanged;

    public bool AddItem(ItemSO item, int quantity = 1)
    {
        // Tenta empilhar em stack existente
        var existing = items.FirstOrDefault(s => s.item == item);
        if (existing != null)
        {
            existing.quantity += quantity;
            OnInventoryChanged?.Invoke();
            return true;
        }

        // Novo slot
        if (items.Count >= maxSlots) return false;
        items.Add(new ItemStack { item = item, quantity = quantity });
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool RemoveItem(ItemSO item, int quantity = 1)
    {
        var stack = items.FirstOrDefault(s => s.item == item);
        if (stack == null || stack.quantity < quantity) return false;

        stack.quantity -= quantity;
        if (stack.quantity <= 0) items.Remove(stack);

        OnInventoryChanged?.Invoke();
        return true;
    }

    public int GetQuantity(ItemSO item)
        => items.FirstOrDefault(s => s.item == item)?.quantity ?? 0;
}

[Serializable]
public class ItemStack
{
    public ItemSO item;
    public int quantity;
}
```

### 4.3 Inventário de Objeto (Baú / Prateleira)

```csharp
// ContainerInventory.cs — componente em baús, prateleiras, caixas
public class ContainerInventory : MonoBehaviour, IInteractable
{
    [Header("Configuração")]
    public string containerName = "Baú";
    public int maxSlots = 10;
    public bool accessibleToClients = false; // prateleiras = true

    private List<ItemStack> items = new();

    public event Action OnContentsChanged;

    // Chamado pelo jogador ao interagir (tecla F)
    public void Interact()
        => InventoryUI.Instance.OpenContainer(this);

    // Chamado por clientes ao buscar itens
    public ItemStack FindItem(ItemSO requested, ItemSO[] acceptable = null)
    {
        if (!accessibleToClients) return null;

        // Procura o item exato primeiro
        var exact = items.FirstOrDefault(s => s.item == requested && s.quantity > 0);
        if (exact != null) return exact;

        // Procura alternativas aceitáveis
        if (acceptable != null)
            return items.FirstOrDefault(
                s => acceptable.Contains(s.item) && s.quantity > 0);

        return null;
    }

    public bool TakeItem(ItemSO item, int quantity = 1)
    {
        var stack = items.FirstOrDefault(s => s.item == item);
        if (stack == null || stack.quantity < quantity) return false;

        stack.quantity -= quantity;
        if (stack.quantity <= 0) items.Remove(stack);

        OnContentsChanged?.Invoke();
        return true;
    }

    public bool StoreItem(ItemSO item, int quantity = 1)
    {
        var existing = items.FirstOrDefault(s => s.item == item);
        if (existing != null)
        {
            existing.quantity += quantity;
            OnContentsChanged?.Invoke();
            return true;
        }
        if (items.Count >= maxSlots) return false;
        items.Add(new ItemStack { item = item, quantity = quantity });
        OnContentsChanged?.Invoke();
        return true;
    }
}
```

### 4.4 Itens — ScriptableObject Base

```csharp
[CreateAssetMenu(menuName = "IronBlood/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Identidade")]
    public string itemName;
    public Sprite icon;
    public string description;

    [Header("Classificação")]
    public ItemCategory category;
    // Enum: Weapon, Armor, Tool, Material, Consumable, Key

    [Header("Economia")]
    public int baseValue;         // preço base de venda
    public int craftingValue;     // valor ao ser craftado com nota S

    [Header("Craft")]
    public bool isCraftable;
    public CraftingRecipe recipe; // null se não craftável
}
```

### 4.5 UI de Inventário

A UI de inventário abre em overlay sem pausar o jogo:
- **Inventário pessoal:** painel lateral esquerdo, sempre acessível com `I` ou `Tab`
- **Inventário de container:** painel lateral direito, abre ao interagir com o objeto
- **Transferência:** arrastar item de um painel para o outro (Drag & Drop)
- O tempo **continua passando** enquanto o inventário está aberto — decisão intencional de pressão temporal

---

## 5. A FERRARIA — LAYOUT E EXPANSÃO

### 5.1 Layout Inicial

A ferraria começa pequena e em mau estado. Layout fixo — **não há reposicionamento livre de objetos**. Expansões desbloqueiam novos cômodos pré-definidos.

```
┌─────────────────────────────────────┐
│  [Porta da Rua]                     │
│                                     │
│  [Prateleira 1]   [Prateleira 2]    │  ← Estoque visível aos clientes
│                                     │
│  [Balcão de Atendimento]            │  ← Ponto onde clientes pagam
│                                     │
│  [Bancada de Forja]                 │  ← Mini-game de craft
│                                     │
│  [Baú de Materiais]                 │  ← Estoque de matéria-prima
│                                     │
│  [Porta dos Fundos] (trancada)      │  ← Narrativa — desbloqueada no Ato 2
└─────────────────────────────────────┘
```

### 5.2 Expansões Disponíveis (MVP)

| Expansão | Custo | Desbloqueia | Semana Narrativa |
|---|---|---|---|
| **Segunda Bancada** | 500 moedas | Crafting paralelo (funcionário usa) | Semana 2 |
| **Depósito nos Fundos** | 800 moedas + 20 madeira | +2 baús de armazenamento | Semana 3 |
| **Vitrine Externa** | 1200 moedas + 10 madeira | Clientes compram mesmo com loja fechada | Semana 5 |
| **Forja Avançada** | 2000 moedas + 15 minério | Receitas de nível 2 desbloqueadas | Semana 7 |
| **Cômodo Secreto** | Narrativo (chave mestra) | Revelação do Ato 3 | Semana 9+ |

### 5.3 Objetos Interativos da Ferraria

| Objeto | Interação | Efeito |
|---|---|---|
| Bancada de Forja | F → abre mini-game | Crafta item |
| Prateleira | F → abre ContainerInventory (accessibleToClients=true) | Gerencia estoque de venda |
| Baú | F → abre ContainerInventory (accessibleToClients=false) | Armazenamento privado |
| Balcão | Automático | Clientes pagam aqui |
| Cama | F → dorme | Avança o dia |
| Porta dos Fundos | F → trancada até narrativa | Ato 2 |

---

## 6. MINI-GAME DE CRAFT (ESTILO JACKSMITH)

### 6.1 Filosofia

O crafting não é instantâneo nem apenas "esperar um timer". O jogador **fabrica fisicamente** o item, peça por peça, arrastando componentes para posições-alvo. A qualidade final depende da precisão de cada encaixe.

### 6.2 Fluxo do Mini-game

```
1. Jogador interage com a Bancada de Forja
2. Seleciona receita (ex: Espada Curta)
3. Tela de craft abre (overlay, tempo pausado durante o mini-game)
4. Peças aparecem à esquerda: Lâmina, Guarda, Cabo
5. Jogador arrasta cada peça para a silhueta da arma
6. Cada peça tem uma zona de encaixe perfeito (área verde)
7. Quanto mais próximo do centro da zona → nota mais alta
8. Após todas as peças encaixadas → item criado com nota final
```

### 6.3 Sistema de Notas

| Nota | Precisão média das peças | Efeito no item |
|---|---|---|
| **S** | 95-100% | +30% ao baseValue, cliente satisfeito, pode virar item especial |
| **A** | 80-94% | +15% ao baseValue |
| **B** | 60-79% | Valor base normal |
| **C** | 40-59% | -10% ao baseValue |
| **D** | 0-39% | -30% ao baseValue, chance de cliente reclamar |

```csharp
public class CraftingMinigame : MonoBehaviour
{
    [SerializeField] private CraftingRecipe currentRecipe;
    private List<PiecePlacement> placements = new();
    private bool isOpen;

    public void Open(CraftingRecipe recipe)
    {
        currentRecipe = recipe;
        isOpen = true;
        Time.timeScale = 0f; // pausa o jogo durante o mini-game
        SpawnPieces(recipe);
    }

    public void Close()
    {
        isOpen = false;
        Time.timeScale = 1f;
    }

    // Chamado quando o jogador solta uma peça (IDropHandler)
    public void OnPiecePlaced(CraftPiece piece, Vector2 dropPosition)
    {
        float distance = Vector2.Distance(dropPosition, piece.targetPosition);
        float maxDistance = piece.targetRadius;

        float accuracy = Mathf.Clamp01(1f - (distance / maxDistance));
        placements.Add(new PiecePlacement { piece = piece, accuracy = accuracy });

        // Feedback visual: verde (próximo) → amarelo → vermelho
        piece.ShowAccuracyFeedback(accuracy);

        if (placements.Count >= currentRecipe.pieces.Count)
            FinalizeCraft();
    }

    private void FinalizeCraft()
    {
        float avgAccuracy = placements.Average(p => p.accuracy);
        CraftGrade grade = GetGrade(avgAccuracy);

        ItemSO result = currentRecipe.resultItem;
        int finalValue = Mathf.RoundToInt(result.baseValue * GetValueMultiplier(grade));

        // Cria o item com nota e valor calculados
        CraftedItem craftedItem = new CraftedItem
        {
            item      = result,
            grade     = grade,
            saleValue = finalValue
        };

        PlayerInventory.Instance.AddCraftedItem(craftedItem);
        Close();
        UIManager.Instance.ShowCraftResult(grade, finalValue);
    }

    private CraftGrade GetGrade(float accuracy) => accuracy switch
    {
        >= 0.95f => CraftGrade.S,
        >= 0.80f => CraftGrade.A,
        >= 0.60f => CraftGrade.B,
        >= 0.40f => CraftGrade.C,
        _        => CraftGrade.D
    };

    private float GetValueMultiplier(CraftGrade grade) => grade switch
    {
        CraftGrade.S => 1.30f,
        CraftGrade.A => 1.15f,
        CraftGrade.B => 1.00f,
        CraftGrade.C => 0.90f,
        CraftGrade.D => 0.70f,
        _            => 1.00f
    };
}
```

### 6.4 Receitas — MVP

| Item | Peças necessárias | Materiais | Nível |
|---|---|---|---|
| Espada Curta | Lâmina, Guarda, Cabo | Ferro ×2, Madeira ×1 | 1 |
| Espada Longa | Lâmina, Lâmina, Guarda, Cabo | Ferro ×3, Madeira ×1 | 1 |
| Machado | Cabeça, Cabo | Ferro ×2, Madeira ×2 | 1 |
| Escudo | Placa, Alça, Reforço | Ferro ×3, Madeira ×2 | 1 |
| Adaga | Lâmina, Cabo | Ferro ×1, Madeira ×1 | 1 |
| Espada de Aço | Lâmina, Guarda, Cabo, Ornamento | Aço ×3, Madeira ×1 | 2 |
| Armadura Leve | Peitoral, Spaulders, Cinto | Ferro ×5, Couro ×2 | 2 |
| Chave Mestra | Haste, Dente, Anel | Aço ×2, Cristal ×1 | Narrativo |

---

## 7. SISTEMA DE ABERTURA E FECHAMENTO DA LOJA

### 7.1 Filosofia

A loja não abre automaticamente. É uma decisão consciente do jogador. Isso cria um ritmo natural — você abre quando está pronto para atender, fecha quando vai sair para coletar ou explorar. Clientes não entram em loja fechada, mas também não ficam parados esperando do lado de fora: eles simplesmente não aparecem.

### 7.2 Objeto de Controle — Placa da Porta

Na entrada da ferraria existe uma **placa pendurada na porta**, interativa. Ela tem dois estados visuais:

| Estado | Visual da Placa | Efeito |
|---|---|---|
| **Aberta** | Placa verde: "ABERTO" | Clientes começam a aparecer conforme o fluxo do dia |
| **Fechada** | Placa vermelha: "FECHADO" | Nenhum cliente novo entra. Clientes dentro terminam a compra e saem. |

O jogador interage com a placa pressionando **F** ao se aproximar.

### 7.3 Implementação — ShopSign.cs

```csharp
// ShopSign.cs
public class ShopSign : MonoBehaviour, IInteractable
{
    [Header("Visuais")]
    [SerializeField] private Sprite spriteOpen;
    [SerializeField] private Sprite spriteClosed;
    [SerializeField] private SpriteRenderer signRenderer;

    [Header("Estado")]
    public bool IsOpen { get; private set; } = false;

    public static ShopSign Instance;

    public event Action<bool> OnShopStateChanged; // true = abriu, false = fechou

    private void Awake() => Instance = this;

    public void Interact()
    {
        SetShopState(!IsOpen);
    }

    public void SetShopState(bool open)
    {
        IsOpen = open;
        signRenderer.sprite = open ? spriteOpen : spriteClosed;
        AudioManager.Instance.PlaySFX(open ? "ShopOpen" : "ShopClose");
        OnShopStateChanged?.Invoke(IsOpen);

        // Notifica o spawner de clientes
        CustomerSpawner.Instance.SetActive(IsOpen);

        // Exibe tooltip rápido na HUD
        HUDManager.Instance.ShowToast(open ? "Loja aberta" : "Loja fechada");
    }
}
```

### 7.4 Regras de Comportamento

**Ao abrir a loja:**
- `CustomerSpawner` começa a gerar clientes com base na reputação e hora do dia
- A placa muda visualmente para verde
- Som de sino tocando (feedback auditivo claro)

**Ao fechar a loja:**
- Nenhum novo cliente é gerado
- Clientes já dentro da loja completam sua ação atual e saem normalmente — **não são teletransportados**
- A placa muda para vermelho
- Som de tranca batendo

**Ao dormir (21h):**
- A loja é **fechada automaticamente** antes do fade de tela
- Um aviso aparece na HUD se o jogador for dormir com a loja aberta: *"Você fechou a loja antes de dormir."*

**Ao acordar (6h):**
- A loja começa **sempre fechada** — o jogador decide quando abrir
- Isso é ensinado no tutorial pelo Marta no Dia 1

### 7.5 Integração com o CustomerSpawner

```csharp
// CustomerSpawner.cs
public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance;

    [SerializeField] private GameObject[] customerPrefabs;
    [SerializeField] private Transform spawnPoint; // fora da porta da loja

    private bool isActive = false;
    private float spawnTimer;

    // Intervalo entre clientes baseado na hora e reputação
    private float SpawnInterval =>
        Mathf.Lerp(120f, 30f, ReputationManager.Instance.ReputationScore / 100f)
        * HourMultiplier();

    private float HourMultiplier()
    {
        int hour = GameClock.Instance.CurrentHour;
        // Pico ao meio-dia, baixo de manhã cedo e à noite
        return hour switch
        {
            >= 6 and < 9   => 2.0f,  // poucos clientes cedo
            >= 9 and < 14  => 1.0f,  // fluxo normal
            >= 14 and < 18 => 0.7f,  // pico da tarde
            >= 18 and < 21 => 1.5f,  // diminuindo
            _              => 99f    // noite — sem clientes
        };
    }

    public void SetActive(bool active) => isActive = active;

    private void Update()
    {
        if (!isActive) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnCustomer();
            spawnTimer = SpawnInterval;
        }
    }

    private void SpawnCustomer()
    {
        var prefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }
}
```

### 7.6 Decisão de Design — Por Que Não Abrir Automaticamente

A abertura manual força o jogador a desenvolver uma rotina. Um dia típico bem gerenciado pode ser:

```
6h  — Acorda → vai à bancada → crafta 3 espadas
7h  — Coloca espadas na prateleira → abre a loja
8h  — Atende clientes enquanto crafta mais
12h — Fecha a loja → vai à floresta coletar madeira
15h — Volta → reabre a loja
18h — Fecha → vai à taverna falar com Ser Edwyn
20h — Volta → dorme
```

Sem a placa, o jogador não teria controle sobre esse ritmo e se sentiria reativo ao jogo em vez de agente dele.

---

## 8. TUTORIAL — MARTA E O PRIMEIRO DIA

### 8.1 Filosofia do Tutorial

O tutorial não é uma tela de instruções. É uma conversa com Marta — a vizinha idosa que era amiga próxima do pai de Durin. Ela estava esperando pela chegada do herdeiro. Ela conhece a ferraria melhor do que Durin e tem interesse pessoal em vê-la prosperar.

**Regras do tutorial:**
- Nunca pausa o jogo com painéis de texto bloqueantes
- Cada instrução é dada **no momento exato em que é relevante**
- O jogador pode ignorar Marta e descobrir as coisas por conta própria — o tutorial não é obrigatório, mas é natural seguir
- Marta não repete instruções, mas o jogador pode falar com ela novamente para ouvir de novo

### 8.2 Estrutura do Dia 1

O Dia 1 começa às 6h com Durin chegando à ferraria pela primeira vez. Marta já está lá, esperando na frente da porta.

```
[SEQUÊNCIA DO DIA 1]

06:00 — Durin chega à ferraria
        Marta está na porta
        → Diálogo de abertura (não pulável na 1ª vez)

06:05 — Marta guia Durin para dentro
        → Apresenta a ferraria (câmera pan pelos objetos principais)
        → Explica a placa da porta: "Seu pai nunca abria sem antes ter algo pra vender."

06:10 — Marta aponta para a bancada
        → Explica o mini-game de craft brevemente
        → Sugere craftar uma Espada Curta (materiais já estão no baú)

[JOGADOR CRAFTA A ESPADA]

06:20 — Marta comenta a nota obtida
        → Explica o sistema de notas (S→D) e o impacto no preço

06:25 — Marta guia para a prateleira
        → Explica que itens na prateleira são visíveis aos clientes
        → "Coloca aí. Quando você abrir a loja, alguém vai comprar."

[JOGADOR COLOCA ITEM NA PRATELEIRA]

06:30 — Marta vai até a placa da porta
        → "Isso aqui é o coração do negócio. Aberto, os clientes entram.
           Fechado, você tem paz pra trabalhar. Aprende a usar direito."
        → Instrui o jogador a abrir a loja

[JOGADOR ABRE A LOJA]

07:00 — Primeiro cliente entra (scripted — sempre o mesmo no Dia 1)
        Marta observa de longe
        → Após a venda: "Viu? Simples assim."

07:30 — Marta leva Durin para fora
        Tour pelo Feudo:
        → Praça: "É aqui que o feudo respira."
        → Taverna: "Quer contratar alguém ou ouvir fofoca? É aqui."
        → Igreja: "Padre Orim. Homem estranho. Mas cura bem."
        → Floresta: "Madeira ali. Não vá longe no primeiro dia."
        → Mina (de longe): "A mina. Não vá hoje. Sério."
        → Vendedor de Minérios (já na praça): "Ah, o Bram. Aparece toda semana.
           Compra o que precisar antes que ele vá embora."

08:30 — Marta volta à ferraria com Durin
        → Entrega a carta do pai (gatilho narrativo do Ato 1)
        → "Ele deixou isso pra você. Eu guardei por muito tempo."
        → Durin lê a carta. Marta sai em silêncio.

[FIM DO TUTORIAL]
```

### 8.3 Diálogos de Marta — Dia 1

```csharp
// TutorialDialogue_Day1.cs
// Sequência de diálogos controlada por TutorialManager

public static readonly string[][] MartaDay1Dialogues = {
    // 0 — Chegada
    new[] {
        "Durin. Eu reconheceria essa testa larga em qualquer lugar.",
        "Seu pai me disse que você viria um dia. Só não disse quando.",
        "Entra. Tem muito que eu preciso te mostrar."
    },
    // 1 — A placa
    new[] {
        "Essa placa aqui é a primeira coisa que você toca toda manhã.",
        "Aberta — os clientes entram. Fechada — você tem paz.",
        "Seu pai fechava quando ia pra mina. Nunca deixava aberta sem ter olhos na loja."
    },
    // 2 — A bancada
    new[] {
        "Aqui é onde o trabalho de verdade acontece.",
        "Seu pai tinha mãos de ouro. Espero que você herdou isso também.",
        "A qualidade do que você faz define o preço que você cobra. Lembra disso."
    },
    // 3 — A prateleira
    new[] {
        "O que está na prateleira, o cliente vê.",
        "O que está no baú, é seu. Ninguém mexe.",
        "Simples assim."
    },
    // 4 — Tour: Taverna
    new[] {
        "Taverna do Gordo Oswin. Melhor cerveja do feudo, pior cozinha.",
        "É onde você vai achar gente pra trabalhar pra você quando crescer.",
        "E onde vai ouvir o que o feudo tá precisando."
    },
    // 5 — Tour: Igreja
    new[] {
        "Padre Orim.",
        "Ele cuida dos feridos. Cobra bem, mas é bom.",
        "Conhecia seu pai. Os dois tinham uma relação... complicada."
    },
    // 6 — Tour: Floresta
    new[] {
        "Madeira ali. Limpa, fácil, perto.",
        "Não vá além das árvores marcadas hoje. Você ainda não conhece o lugar."
    },
    // 7 — Tour: Mina
    new[] {
        "A mina.",
        "Tem o que você precisa pra fazer armas boas. Tem também o que te mata.",
        "Não hoje."
    },
    // 8 — Vendedor de minérios
    new[] {
        "Esse é Bram. Aparece toda semana, segunda de manhã.",
        "Vende minérios sem fazer perguntas. Preço justo.",
        "Se você não quer ir à mina, compra dele. Ele não falta."
    },
    // 9 — A carta
    new[] {
        "Seu pai deixou isso comigo. Pediu pra guardar até você chegar.",
        "Eu fiz o que ele pediu. Como sempre fiz.",
        "Lê quando estiver sozinho."
    }
};
```

### 8.4 TutorialManager.cs

```csharp
// TutorialManager.cs
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    private bool tutorialCompleted;
    private int currentStep = 0;

    private readonly TutorialStep[] steps = new TutorialStep[]
    {
        new TutorialStep { id = "arrival",      trigger = TutorialTrigger.OnDay1Start },
        new TutorialStep { id = "sign",         trigger = TutorialTrigger.OnEnterShop },
        new TutorialStep { id = "workbench",    trigger = TutorialTrigger.OnApproachWorkbench },
        new TutorialStep { id = "shelf",        trigger = TutorialTrigger.OnCraftComplete },
        new TutorialStep { id = "open_shop",    trigger = TutorialTrigger.OnItemPlacedOnShelf },
        new TutorialStep { id = "first_sale",   trigger = TutorialTrigger.OnShopOpened },
        new TutorialStep { id = "town_tour",    trigger = TutorialTrigger.OnFirstSaleComplete },
        new TutorialStep { id = "the_letter",   trigger = TutorialTrigger.OnTourComplete },
    };

    public void AdvanceStep(TutorialTrigger trigger)
    {
        if (tutorialCompleted) return;
        if (currentStep >= steps.Length) { CompleteTutorial(); return; }
        if (steps[currentStep].trigger != trigger) return;

        DialogueManager.Instance.PlayMartaDialogue(steps[currentStep].id);
        currentStep++;
    }

    private void CompleteTutorial()
    {
        tutorialCompleted = true;
        SaveSystem.Instance.SetFlag("tutorial_done");
    }
}

public enum TutorialTrigger
{
    OnDay1Start,
    OnEnterShop,
    OnApproachWorkbench,
    OnCraftComplete,
    OnItemPlacedOnShelf,
    OnShopOpened,
    OnFirstSaleComplete,
    OnTourComplete
}
```

### 8.5 Regras de Ouro do Tutorial

| Regra | Implementação |
|---|---|
| Nunca bloquear o jogador | Diálogos são exibidos como balões — o jogador pode se mover enquanto Marta fala |
| Nunca repetir automaticamente | Cada step dispara uma única vez |
| Sempre ter contexto visual | Marta aponta (seta animada) para o objeto que está explicando |
| Permitir re-leitura | Falar com Marta após o tutorial exibe um menu "Me conta de novo sobre..." |
| Vendedor no Dia 1 garantido | `WeeklyVendorSpawner` força spawn de Bram no Dia 1 independente da semana |

---

## 9. SISTEMA DE CLIENTES E IA DE COMPRA

### 7.1 Comportamento do Cliente

Clientes chegam durante o dia com um **pedido em mente** e uma **lista de alternativas aceitáveis**. Eles percorrem as prateleiras acessíveis (`accessibleToClients = true`) antes de falar com o jogador.

```
Cliente entra na loja
      │
      ▼
Verifica prateleiras por conta própria
      │
      ├─ Encontrou o item exato? → Pega, vai ao balcão, paga
      │
      ├─ Encontrou alternativa aceitável? → Avalia preço
      │   ├─ Preço OK → Pega, paga (satisfação parcial)
      │   └─ Muito caro → Reclama ou vai embora
      │
      └─ Não encontrou nada → Vai ao balcão
            ├─ Jogador pode mostrar item do inventário pessoal
            ├─ Jogador pode prometer para amanhã (cliente volta)
            └─ Cliente vai embora sem comprar
```

### 7.2 Implementação — CustomerAI.cs

```csharp
public class CustomerAI : MonoBehaviour
{
    [Header("Pedido")]
    public ItemSO desiredItem;
    public ItemSO[] acceptableAlternatives;
    public int maxAcceptablePrice;
    public float patience = 30f; // segundos antes de ir embora

    private CustomerState state = CustomerState.Entering;
    private float patienceTimer;

    private void Update()
    {
        patienceTimer += Time.deltaTime;
        if (patienceTimer >= patience && state != CustomerState.Leaving)
            StartLeaving(CustomerMood.Impatient);
    }

    public void StartShopping()
    {
        state = CustomerState.Browsing;
        // Move para a primeira prateleira acessível
        var shelves = FindObjectsOfType<ContainerInventory>()
            .Where(c => c.accessibleToClients)
            .OrderBy(c => Vector2.Distance(transform.position, c.transform.position));

        StartCoroutine(BrowseShelves(shelves.ToList()));
    }

    private IEnumerator BrowseShelves(List<ContainerInventory> shelves)
    {
        foreach (var shelf in shelves)
        {
            // Move até a prateleira
            yield return MoveTo(shelf.transform.position);
            yield return new WaitForSeconds(0.5f); // "olhando"

            // Tenta encontrar o item
            var found = shelf.FindItem(desiredItem, acceptableAlternatives);
            if (found != null)
            {
                int price = CalculatePrice(found.item);
                if (price <= maxAcceptablePrice)
                {
                    yield return BuyItem(shelf, found, price);
                    yield break;
                }
                else
                {
                    // Muito caro — reação visual
                    ShowReaction(CustomerReaction.TooExpensive);
                }
            }
        }

        // Nada encontrado — vai ao balcão
        yield return GoToCounter();
    }

    private IEnumerator BuyItem(ContainerInventory shelf,
                                ItemStack stack, int price)
    {
        state = CustomerState.Buying;
        shelf.TakeItem(stack.item);
        EconomyManager.Instance.AddGold(price);

        bool isExact = stack.item == desiredItem;
        CustomerMood mood = isExact ? CustomerMood.Happy : CustomerMood.Neutral;

        ShowReaction(isExact
            ? CustomerReaction.Satisfied
            : CustomerReaction.Acceptable);

        yield return MoveTo(ShopDoor.Instance.transform.position);
        ReputationManager.Instance.RegisterSale(mood);
        Destroy(gameObject);
    }

    private void StartLeaving(CustomerMood mood)
    {
        state = CustomerState.Leaving;
        if (mood == CustomerMood.Impatient)
            ReputationManager.Instance.RegisterNoSale();
        StartCoroutine(LeaveCoroutine());
    }
}

public enum CustomerState  { Entering, Browsing, Buying, Leaving }
public enum CustomerMood   { Happy, Neutral, Impatient, Angry }
public enum CustomerReaction { Satisfied, Acceptable, TooExpensive, NotFound }
```

### 7.3 Reputação

```csharp
public class ReputationManager : MonoBehaviour
{
    public static ReputationManager Instance;

    public int ReputationScore { get; private set; } = 50; // 0-100

    public void RegisterSale(CustomerMood mood)
    {
        ReputationScore += mood switch
        {
            CustomerMood.Happy   => 2,
            CustomerMood.Neutral => 1,
            _                    => 0
        };
        ReputationScore = Mathf.Clamp(ReputationScore, 0, 100);
    }

    public void RegisterNoSale()
    {
        ReputationScore = Mathf.Max(0, ReputationScore - 1);
    }

    // Reputação alta → mais clientes por dia
    public int GetDailyCustomerCount()
        => Mathf.RoundToInt(Mathf.Lerp(2, 12, ReputationScore / 100f));
}
```

---

## 10. COLETA — FLORESTA E MINA

### 8.1 A Floresta

Área acessível a pé durante o dia. Sem combate — apenas coleta.

| Recurso | Quantidade por visita | Tempo de coleta |
|---|---|---|
| Madeira Comum | 3-6 | 3s por árvore |
| Madeira Densa | 1-2 | 6s por árvore |
| Ervas | 2-4 | 2s por planta |
| Cogumelos | 1-3 | 2s por cogumelo |

**Encontros na floresta:** NPCs de espécies não-humanas aparecem em locais fixos após marcos narrativos. Thresh (goblin) aparece a partir da semana 3 do jogo.

### 8.2 A Mina

Área com múltiplos andares. Quanto mais fundo, melhores os minérios e mais fortes os monstros.

| Andar | Minério disponível | Nível dos monstros |
|---|---|---|
| 1 | Ferro, Cobre | Fraco (2-3 hits para matar) |
| 2 | Ferro, Carvão, Prata | Médio (4-6 hits) |
| 3 | Aço bruto, Prata | Forte (8-10 hits) |
| 4 | Cristal, Aço bruto | Muito forte (15+ hits) |
| 5 | Cristal Raro | Boss do andar — narrativo |

**Regra de design:** O jogador não precisa ir à mina — o Vendedor Semanal fornece o básico. A mina é para quem quer mais recursos, mais rápido, com risco real.

### 8.3 Coleta de Minério

```csharp
public class MineralNode : MonoBehaviour, IInteractable
{
    public ItemSO mineralType;
    public int quantity = 3;
    public float mineTime = 2f; // segundos para extrair

    private bool isDepleted;

    public void Interact()
    {
        if (isDepleted) return;
        StartCoroutine(MineCoroutine());
    }

    private IEnumerator MineCoroutine()
    {
        // Animação de mineração
        yield return new WaitForSeconds(mineTime);

        PlayerInventory.Instance.AddItem(mineralType, quantity);
        isDepleted = true;
        GetComponent<SpriteRenderer>().color = Color.gray; // visual de esgotado

        // Respawn em X dias
        GameClock.Instance.OnNewDay += TryRespawn;
    }

    private void TryRespawn(int day)
    {
        // Respawn após 3 dias
        if (day % 3 == 0)
        {
            isDepleted = false;
            GetComponent<SpriteRenderer>().color = Color.white;
            GameClock.Instance.OnNewDay -= TryRespawn;
        }
    }
}
```

---


---

## 10.1-EXT — SISTEMAS DE COLETA EXPANDIDOS

> Esta seção expande a Seção 10 (Coleta — Floresta e Mina) com os sistemas de animais, couro, NPCs fixos da cidade, comida, cozinha e pesca.

---

### ANIMAIS E COURO NA FLORESTA

#### Animais Disponíveis

A floresta contém animais caçáveis além das árvores. O abate é simples — o jogador se aproxima e interage (sem mini-game, sem combate elaborado). O animal foge se o jogador demorar.

| Animal | Drop | Quantidade | Tempo de Respawn |
|---|---|---|---|
| Veado | Couro Bruto ×2, Carne Crua ×2 | — | 2 dias |
| Coelho | Couro Bruto ×1, Carne Crua ×1 | — | 1 dia |
| Javali | Couro Bruto ×3, Carne Crua ×3 | — | 3 dias |
| Raposa | Couro Fino Bruto ×1 | — | 4 dias |

**Couro Bruto ≠ Couro Refinado.** O couro bruto não pode ser usado em receitas — precisa ser processado pelo Curtidor antes.

---

### O CURTIDOR — NPC FIXO DA CIDADE

**Nome:** Vorn
**Localização:** Barracão ao lado da praça — presente todos os dias
**Função:** Refina Couro Bruto em Couro Refinado mediante pagamento

| Serviço | Custo | Tempo |
|---|---|---|
| Couro Bruto → Couro Refinado | 5 moedas/unidade | Instantâneo |
| Couro Fino Bruto → Couro Fino Refinado | 15 moedas/unidade | Instantâneo |

```csharp
// TannerNPC.cs
public class TannerNPC : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        int rawHide      = PlayerInventory.Instance.GetQuantity(ItemDB.RawHide);
        int rawFineHide  = PlayerInventory.Instance.GetQuantity(ItemDB.RawFineHide);

        if (rawHide == 0 && rawFineHide == 0)
        {
            DialogueManager.Instance.ShowLine("Vorn",
                "Sem couro pra curtir, sem negócio pra fazer.");
            return;
        }

        TannerUI.Instance.Open(rawHide, rawFineHide);
    }

    public void ProcessHide(int rawQty, int fineQty)
    {
        int cost = (rawQty * 5) + (fineQty * 15);
        if (!EconomyManager.Instance.TrySpend(cost)) return;

        PlayerInventory.Instance.RemoveItem(ItemDB.RawHide, rawQty);
        PlayerInventory.Instance.RemoveItem(ItemDB.RawFineHide, fineQty);
        PlayerInventory.Instance.AddItem(ItemDB.RefinedHide, rawQty);
        PlayerInventory.Instance.AddItem(ItemDB.RefinedFineHide, fineQty);

        DialogueManager.Instance.ShowLine("Vorn", "Pronto. Bom couro.");
    }
}
```

---

### O LENHADOR — NPC FIXO DA CIDADE

**Nome:** Burl
**Localização:** Beira da praça, presente todos os dias
**Função:** Vende madeira com estoque limitado por dia

| Item | Preço | Estoque diário |
|---|---|---|
| Madeira Comum | 8 moedas | 10 unidades |
| Madeira Densa | 18 moedas | 4 unidades |

O estoque de Burl **reseta todo dia às 6h** junto com o ciclo diário. Isso força o jogador a decidir entre comprar (rápido, mas caro) ou coletar na floresta (de graça, mas consome tempo). Com funcionário Lenhador contratado, a compra com Burl se torna redundante para madeira comum.

```csharp
// WoodsellerNPC.cs
public class WoodsellerNPC : MonoBehaviour, IInteractable
{
    private int dailyCommonStock  = 10;
    private int dailyDenseStock   = 4;

    private void OnEnable()
        => GameClock.Instance.OnNewDay += ResetStock;

    private void ResetStock(int day)
    {
        dailyCommonStock = 10;
        dailyDenseStock  = 4;
    }

    public void Interact()
        => WoodsellerUI.Instance.Open(dailyCommonStock, dailyDenseStock);

    public void BuyWood(WoodType type, int quantity)
    {
        int price = type == WoodType.Common ? 8 : 18;
        int cost  = price * quantity;

        if (!EconomyManager.Instance.TrySpend(cost)) return;

        if (type == WoodType.Common) dailyCommonStock -= quantity;
        else                          dailyDenseStock  -= quantity;

        PlayerInventory.Instance.AddItem(
            type == WoodType.Common ? ItemDB.CommonWood : ItemDB.DenseWood,
            quantity);
    }
}
```

---

### SISTEMA DE COMIDA E BUFFS

#### Filosofia

Comida é uma necessidade diária. O jogador precisa comer **uma vez por dia** (antes de dormir ou ao acordar). Não comer resulta em penalidade leve no dia seguinte — sem drama, sem game over.

**Penalidade por não comer:** -20% de velocidade de movimento e stamina no dia seguinte.

Comidas simples (cozinhadas em casa ou compradas baratas) apenas suprem a necessidade. Comidas boas — da taverna ou de ingredientes raros — concedem **buffs que duram o dia inteiro**.

#### Categorias de Comida

| Categoria | Fonte | Efeito |
|---|---|---|
| **Básica** | Cozinha própria (ingredientes simples) | Apenas supre a necessidade diária |
| **Boa** | Taverna / ingredientes raros cozinhados | Buff de 1 dia (varia por prato) |
| **Rara** | Pesca (itens especiais) / receitas secretas | Buff poderoso de 1 dia |

#### Tabela de Buffs por Prato

| Prato | Fonte | Buff | Duração |
|---|---|---|---|
| Ensopado de Carne | Taverna (20 moedas) | +25% velocidade | 1 dia |
| Sopa de Peixe | Cozinha (Peixe ×2) | +30% stamina máx | 1 dia |
| Assado de Javali | Cozinha (Carne Javali ×2) | +40 HP máx | 1 dia |
| Pão com Mel | Taverna (12 moedas) | +15% velocidade craft (nota sobe 1 grau mínimo) | 1 dia |
| Filé Raro | Pesca (item raro) | +20% dano de combate | 1 dia |
| Caldo do Chef | Taverna (50 moedas) | Todos os buffs em 50% | 1 dia |

**"Nota sobe 1 grau mínimo"** significa: se o jogador tiraria D, tira C. Se tiraria C, tira B. Não muda notas já altas.

#### Implementação — FoodSystem.cs

```csharp
// FoodSystem.cs
public class FoodSystem : MonoBehaviour
{
    public static FoodSystem Instance;

    public bool AteToday { get; private set; }
    public List<FoodBuff> ActiveBuffs { get; private set; } = new();

    public event Action<FoodBuff> OnBuffApplied;
    public event Action           OnBuffsCleared;

    private void OnEnable()
        => GameClock.Instance.OnNewDay += ProcessNewDay;

    private void ProcessNewDay(int day)
    {
        // Aplica penalidade se não comeu
        if (!AteToday)
            StatModifierManager.Instance.ApplyPenalty(
                StatPenalty.HungryPenalty); // -20% vel e stamina

        // Limpa buffs do dia anterior
        ActiveBuffs.Clear();
        AteToday = false;
        OnBuffsCleared?.Invoke();
    }

    public void Eat(FoodItemSO food)
    {
        AteToday = true;

        // Remove penalidade de fome se estava ativa
        StatModifierManager.Instance.RemovePenalty(StatPenalty.HungryPenalty);

        if (food.buff != null)
        {
            ActiveBuffs.Add(food.buff);
            StatModifierManager.Instance.ApplyBuff(food.buff);
            OnBuffApplied?.Invoke(food.buff);
            HUDManager.Instance.ShowToast($"Buff ativo: {food.buff.displayName}");
        }

        PlayerInventory.Instance.RemoveItem(food, 1);
    }
}

[CreateAssetMenu(menuName = "IronBlood/FoodItem")]
public class FoodItemSO : ItemSO
{
    public FoodBuff buff; // null = comida básica, sem buff
}

[Serializable]
public class FoodBuff
{
    public string displayName;
    public BuffType type;
    public float value; // ex: 0.25 = +25%

    public enum BuffType
    {
        SpeedBoost,
        StaminaBoost,
        HPBoost,
        CraftBoost,
        DamageBoost,
        AllStats
    }
}
```

---

### SISTEMA DE COZINHA

#### Filosofia

A cozinha é simples — estilo Minecraft. Sem mini-game, sem risco de queimar. O jogador coloca os ingredientes no slot correto e aguarda o tempo de preparo. É um sistema de conversão com espera, não de habilidade.

#### Objeto — Fogão (Kitchen.cs)

```csharp
// Kitchen.cs
public class Kitchen : MonoBehaviour, IInteractable
{
    [SerializeField] private CookingRecipeSO[] knownRecipes;
    private CookingRecipeSO activeRecipe;
    private float cookTimer;
    private bool isCooking;

    public void Interact()
    {
        if (isCooking)
        {
            KitchenUI.Instance.ShowProgress(activeRecipe, cookTimer);
            return;
        }
        KitchenUI.Instance.Open(knownRecipes);
    }

    public void StartCooking(CookingRecipeSO recipe)
    {
        // Verifica ingredientes
        foreach (var ing in recipe.ingredients)
            if (PlayerInventory.Instance.GetQuantity(ing.item) < ing.quantity)
            {
                HUDManager.Instance.ShowToast("Ingredientes insuficientes.");
                return;
            }

        // Consome ingredientes
        foreach (var ing in recipe.ingredients)
            PlayerInventory.Instance.RemoveItem(ing.item, ing.quantity);

        activeRecipe = recipe;
        cookTimer    = recipe.cookTimeSeconds;
        isCooking    = true;
        StartCoroutine(CookCoroutine());
    }

    private IEnumerator CookCoroutine()
    {
        while (cookTimer > 0f)
        {
            cookTimer -= Time.deltaTime;
            yield return null;
        }

        PlayerInventory.Instance.AddItem(activeRecipe.resultFood, 1);
        isCooking    = false;
        activeRecipe = null;
        AudioManager.Instance.PlaySFX("CookingDone");
        HUDManager.Instance.ShowToast("Comida pronta!");
    }
}

[CreateAssetMenu(menuName = "IronBlood/CookingRecipe")]
public class CookingRecipeSO : ScriptableObject
{
    public string recipeName;
    public List<Ingredient> ingredients;
    public FoodItemSO resultFood;
    public float cookTimeSeconds = 30f; // tempo real — ajustar por testes
}
```

#### Receitas de Cozinha — MVP

| Receita | Ingredientes | Tempo | Resultado |
|---|---|---|---|
| Carne Assada | Carne Crua ×1 | 30s | Carne Assada (básica) |
| Sopa de Peixe | Peixe ×2 | 45s | Sopa de Peixe (buff stamina) |
| Assado de Javali | Carne Javali ×2 | 60s | Assado de Javali (buff HP) |
| Ensopado | Carne Crua ×1, Erva ×1 | 40s | Ensopado (básico melhorado) |

---

### SISTEMA DE PESCA

#### Local

O **Riacho do Feudo** fica a leste da cidade, a caminho da floresta. É um local de descanso — sem monstros, sem pressão. Totalmente opcional.

#### Mecânica de Pesca

Simples e contemplativa:
1. Jogador se aproxima da beira do riacho com a vara equipada
2. Pressiona F → lança a linha
3. Aguarda (barra de progresso discreta)
4. Quando o peixe morder, um ícone pisca — jogador pressiona F novamente
5. Se apertar no tempo certo → pesca o item
6. Se demorar → o peixe foge, tenta novamente

Sem mini-game elaborado. A "habilidade" é apenas o timing do segundo F.

#### Tabela de Drops da Pesca

| Item | Probabilidade | Tipo |
|---|---|---|
| Peixe Comum | 50% | Ingrediente (cozinha) |
| Peixe Raro | 15% | Ingrediente (buff raro) |
| Bota Velha | 10% | Lixo (vender por 1 moeda) |
| Couro Bruto | 8% | Material |
| Espada Enferrujada | 5% | Pode ser reparada na bancada — item narrativo |
| Carta Molhada | 5% | Lore — história paralela |
| Anel Misterioso | 4% | Item narrativo — gatilho de história paralela |
| Tesouro Pequeno | 3% | 10-30 moedas |

**Espada Enferrujada:** Pode ser levada à bancada para restauração. Quando restaurada, revela uma inscrição — fragmento de história paralela sobre o dono anterior. Não obrigatório, totalmente colecionável.

**Carta Molhada / Anel Misterioso:** Itens de lore que expandem o mundo sem interferir na narrativa principal. O jogador pode ignorar completamente.

#### O Pescador — NPC Fixo do Riacho

**Nome:** Old Finn
**Localização:** Beira do riacho — presente todos os dias
**Função:** Vende peixe (para quem não quer pescar) e conta histórias

| Item | Preço | Estoque diário |
|---|---|---|
| Peixe Comum ×3 | 15 moedas | 5 lotes |
| Peixe Raro ×1 | 35 moedas | 2 unidades |

Old Finn tem o maior banco de histórias paralelas do jogo — cada dia que o jogador visita, ele conta um fragmento diferente sobre o riacho, o feudo antigo, ou coisas que "pescou sem querer". É o narrador ambiental do mundo exterior.

```csharp
// FishermanNPC.cs
public class FishermanNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string[] dailyStories;
    private int dailyCommonStock = 5;
    private int dailyRareStock   = 2;

    public void Interact()
    {
        // Exibe menu: Comprar peixe | Ouvir história
        FishermanUI.Instance.Open(
            dailyCommonStock,
            dailyRareStock,
            GetTodayStory());
    }

    private string GetTodayStory()
    {
        int index = GameClock.Instance.CurrentDay % dailyStories.Length;
        return dailyStories[index];
    }
}
```

---

## 11. SISTEMA DE ACORDOS E RELACIONAMENTOS

### 9.1 Filosofia

Acordos são **eventos únicos de negociação** — o jogador vende um lote, recebe pagamento, e precisa manter o relacionamento para que futuras oportunidades apareçam. Não existe contrato eterno.

### 9.2 Fluxo de um Acordo

```
1. Relacionamento com NPC atinge nível mínimo (ex: "Conhecido" → "Aliado")
2. NPC propõe um acordo específico:
   "Preciso de 10 espadas até a próxima semana. Pago 800 moedas."
3. Jogador aceita ou recusa
4. Se aceitar: prazo aparece na UI, lote reservado no baú de entregas
5. Na data combinada: jogador entrega o lote → recebe pagamento
6. Relação melhora ou piora dependendo da qualidade das armas entregues
7. Após X semanas, NPC pode propor novo acordo (maior volume, melhor preço)
```

### 9.3 Níveis de Relacionamento

| Nível | Nome | Desbloqueado por |
|---|---|---|
| 0 | Desconhecido | Estado inicial |
| 1 | Conhecido | 3 interações / compras |
| 2 | Aliado | 1 acordo concluído |
| 3 | Parceiro | 3 acordos concluídos |
| 4 | Amigo | Evento narrativo específico |

```csharp
public class NPCRelationship
{
    public NPCData npc;
    public int level;           // 0-4
    public int interactionCount;
    public List<Agreement> agreementHistory;

    public void RegisterInteraction()
    {
        interactionCount++;
        if (interactionCount >= 3 && level == 0) level = 1;
    }

    public void RegisterAgreementCompleted(float qualityScore)
    {
        agreementHistory.Add(new Agreement
        {
            completed = true,
            quality   = qualityScore,
            day       = GameClock.Instance.CurrentDay
        });

        if (agreementHistory.Count(a => a.completed) >= 1 && level == 1) level = 2;
        if (agreementHistory.Count(a => a.completed) >= 3 && level == 2) level = 3;
    }
}
```

### 9.4 Acordos Disponíveis no MVP

| NPC | Acordo | Requisito | Recompensa |
|---|---|---|---|
| Ser Edwyn (Feudo) | 10 espadas/semana | Rel. nível 1 | 800 moedas |
| Ser Edwyn (Feudo) | 5 armaduras | Rel. nível 2 | 2000 moedas |
| Thresh (Goblin) | Trocar madeira por cristais | Rel. nível 1 | Cristais raros |
| Thresh (Goblin) | Fornecer facas para a tribo | Rel. nível 2 | Mapa da Floresta Profunda |
| Marta (Vizinha) | Consertar ferramentas do feudo | Rel. nível 1 | Info sobre o pai (narrativo) |

---

## 12. FUNCIONÁRIOS E AUTOMAÇÃO

### 10.1 Contratação

Funcionários são contratados na Taverna após certo nível de relacionamento ou pagamento direto.

| Funcionário | Função | Custo semanal | Onde contratar |
|---|---|---|---|
| **Lenhador** | Coleta madeira automaticamente (1x/dia) | 50 moedas | Taverna |
| **Aprendiz de Ferreiro** | Crafta itens simples na 2ª bancada (nota máx: B) | 80 moedas | Taverna |
| **Carregador** | Transfere itens entre baús automaticamente | 40 moedas | Taverna |

**Não existe minerador contratável** — a mina é domínio exclusivo do jogador e do vendedor semanal, por design narrativo.

### 10.2 Lógica de Funcionário

```csharp
public class WorkerAI : MonoBehaviour
{
    public WorkerType type;
    public int weeklyCost;
    private bool isPaid;

    private void OnEnable()
        => GameClock.Instance.OnNewWeek += ProcessWeeklyPayment;

    private void ProcessWeeklyPayment(int week)
    {
        if (EconomyManager.Instance.Gold >= weeklyCost)
        {
            EconomyManager.Instance.SpendGold(weeklyCost);
            isPaid = true;
        }
        else
        {
            isPaid = false;
            // Worker fica idle até ser pago
            UIManager.Instance.ShowWarning($"{workerName} não foi pago esta semana.");
        }
    }

    private void Update()
    {
        if (!isPaid) return;
        // Executa comportamento por tipo
        ExecuteWork();
    }
}
```

---

## 13. VENDEDOR SEMANAL DE MINÉRIOS

### 11.1 Funcionamento

Todo início de semana (segunda-feira in-game), um vendedor aparece na Praça e fica disponível até o fim do segundo dia da semana.

**Estoque do vendedor:**
- Itens variam a cada semana (gerado com seed da semana)
- Sempre inclui ferro e madeira (básicos)
- Tem chance de trazer minérios raros dependendo da semana do jogo

### 11.2 Decisão de Compra

O jogador deve avaliar:
1. Quanto estoque tem no baú de materiais
2. Quantos pedidos estão pendentes (clientes e acordos)
3. Se há minério raro que não conseguiria na mina facilmente
4. Quanto dinheiro tem disponível sem comprometer o fluxo de caixa

Essa é uma decisão central do loop de gerenciamento — o vendedor não espera.

```csharp
[Serializable]
public class WeeklyVendorStock
{
    public List<VendorItem> items;

    public static WeeklyVendorStock Generate(int weekSeed)
    {
        Random.InitState(weekSeed);
        var stock = new WeeklyVendorStock { items = new List<VendorItem>() };

        // Básicos sempre disponíveis
        stock.items.Add(new VendorItem { item = ItemDB.Iron,   quantity = 20, price = 15 });
        stock.items.Add(new VendorItem { item = ItemDB.Wood,   quantity = 20, price = 8  });

        // Raro baseado na semana
        if (weekSeed % 3 == 0)
            stock.items.Add(new VendorItem
            {
                item     = ItemDB.Silver,
                quantity = Random.Range(3, 8),
                price    = 45
            });

        return stock;
    }
}
```

---

## 14. PROGRESSÃO NARRATIVA — SEGREDOS DA FERRARIA

### 12.1 Sistema de Gatilhos

A história avança por **conquistas do jogador**, não por tempo. O jogador inevitavelmente vai atingi-las no fluxo normal do jogo, mas o ritmo é controlado por ele.

```csharp
public class NarrativeTrigger
{
    public string triggerID;
    public TriggerCondition condition;
    public NarrativeEvent onTriggered;
    public bool hasTriggered;
}

public enum TriggerCondition
{
    ItemCrafted,          // craftou X item
    AgreementCompleted,   // completou acordo com NPC Y
    DayReached,           // chegou ao dia N
    RoomEntered,          // entrou num cômodo específico
    ItemDelivered,        // entregou item para NPC
    ReputationReached     // reputação >= N
}
```

### 12.2 Gatilhos Narrativos do MVP

| Gatilho | Condição | Evento |
|---|---|---|
| **A Carta** | Dia 1 | Marta entrega carta do pai — introduz o mistério |
| **O Barulho** | Dia 3 noite | Som vindo da porta dos fundos (só Durin ouve) |
| **O Mapa** | Acordo com Thresh nível 2 | Mapa revela câmara abaixo da ferraria |
| **A Chave** | Craftar Chave Mestra | Porta dos fundos abre |
| **O Diário** | Entrar no cômodo secreto | Diário do pai — Ato 3 começa |
| **A Escolha** | Ler última página do diário | Apresenta os dois finais possíveis |

### 12.3 Os Dois Finais

**Final A — O Herdeiro:**
Durin decide selar o segredo do pai, destruir o diário e continuar vivendo no feudo. A ferraria prospera. A Sombra desaparece. Créditos mostram o feudo décadas depois, com uma estátua de Durin na praça.

**Final B — A Verdade:**
Durin decide revelar o segredo do pai para o feudo. As consequências são imprevisíveis — alguns fogem, alguns ficam. A Sombra revela sua identidade. A ferraria continua, mas o feudo nunca mais é o mesmo.

---

## 15. COMBATE SIMPLES

### 13.1 Filosofia

O combate não é o foco do jogo. É uma barreira de risco na mina — simples o suficiente para não frustrar, presente o suficiente para dar peso à exploração.

### 13.2 Mecânica

- **Ataque:** pressionar M1 executa um arco de espada na frente do jogador (180°, alcance 1.5u)
- **Sem combo, sem habilidades, sem hotbar** — apenas o arco e o timing
- **Sem roll/dash** — o jogador se afasta andando normalmente
- **HP limitado** — se morrer na mina, perde os itens coletados naquela visita e volta à ferraria

```csharp
public class PlayerCombat : MonoBehaviour
{
    [Header("Espada")]
    public float damage = 10f;
    public float arcAngle = 180f;
    public float arcRadius = 1.5f;
    public float attackCooldown = 0.6f;

    private float cooldownTimer;

    private void Update()
    {
        if (cooldownTimer > 0f) cooldownTimer -= Time.deltaTime;
    }

    public void OnAttack(InputValue value)
    {
        if (!value.isPressed || cooldownTimer > 0f) return;

        cooldownTimer = attackCooldown;
        PerformArcAttack();
        GamefeelManager.Instance.TriggerShake(0.8f, 1.2f, 0.1f);
    }

    private void PerformArcAttack()
    {
        Vector2 facing = transform.up;
        var hits = Physics2D.OverlapCircleAll(transform.position, arcRadius);

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            Vector2 dir = ((Vector2)hit.transform.position
                           - (Vector2)transform.position).normalized;
            float angle = Vector2.Angle(facing, dir);

            if (angle <= arcAngle * 0.5f)
                hit.GetComponent<EnemyHealth>()?.TakeDamage(damage);
        }
    }
}
```

---

## 14. ESCOPO MVP — 3 MESES SOLO

### 14.1 Cronograma Revisado

| Semana | Foco | Entregável |
|---|---|---|
| 1 | Setup Unity + Player top-down + GameClock | Player anda, dia avança, HUD de tempo funcional |
| 2 | Layout da ferraria + sistema de inventário + containers | Baús funcionais, inventário pessoal, transferência por drag & drop |
| 3 | Mini-game de craft completo | Craftar espada curta com nota, item vai para inventário |
| 4 | Sistema de clientes + prateleiras + balcão | Cliente entra, compra da prateleira, reputação funciona |
| 5 | Floresta — coleta de madeira + NPCs básicos | Coletar madeira, Marta com 3 linhas de diálogo |
| 6 | Mina — andares 1 e 2 + combate simples | Minar ferro, matar goblin, perder itens ao morrer |
| 7 | Vendedor semanal + sistema de economia | Comprar minérios, fluxo de caixa semanal funcional |
| 8 | Acordos + sistema de relacionamentos | Acordo com Ser Edwyn, entrega de lote, pagamento |
| 9 | Funcionários + automação | Lenhador e Aprendiz funcionando |
| 10 | Progressão narrativa — Atos 1 e 2 | Carta da Marta, barulho, Thresh, porta dos fundos |
| 11 | Expansões da ferraria + Ato 3 + dois finais | Chave mestra, cômodo secreto, diário, escolha final |
| 12 | Polish, áudio, balanceamento, build | Build Windows, demo no itch.io |

### 14.2 Scripts C# a Desenvolver

#### Core
- `GameClock.cs`
- `DayNightVisuals.cs`
- `SleepManager.cs`
- `EconomyManager.cs`
- `ReputationManager.cs`
- `SaveSystem.cs`
- `GamefeelManager.cs`

#### Inventário
- `ItemSO.cs`
- `ItemStack.cs`
- `PlayerInventory.cs`
- `ContainerInventory.cs`
- `InventoryUI.cs`
- `DragDropHandler.cs`
- `CraftedItem.cs`

#### Ferraria
- `WorkbenchInteractable.cs`
- `CraftingMinigame.cs`
- `CraftPiece.cs`
- `CraftingRecipeSO.cs`
- `ShopDoor.cs`
- `ExpansionManager.cs`

#### Clientes
- `CustomerAI.cs`
- `CustomerSpawner.cs`
- `CustomerData.cs`

#### Mundo
- `PlayerMovement.cs`
- `PlayerCombat.cs`
- `AimController.cs`
- `MineralNode.cs`
- `TreeNode.cs`
- `MineFloorManager.cs`

#### NPCs
- `NPCRelationship.cs`
- `NPCDialogue.cs`
- `AgreementSystem.cs`
- `WeeklyVendor.cs`
- `WorkerAI.cs`
- `NarrativeTrigger.cs`
- `NarrativeManager.cs`

#### Inimigos (mina)
- `EnemyAI_Mine.cs`
- `EnemyHealth.cs`

### 14.3 Assets a Adquirir

| Asset | Fonte | Uso |
|---|---|---|
| Top-Down RPG Tileset (interior) | Itch.io (LimeZu / Szadi) | Ferraria, taverna, mina |
| Top-Down RPG Tileset (exterior) | Itch.io (LimeZu) | Feudo, floresta |
| Character Pack (anão + NPCs) | Itch.io (Pixel Frog) | Durin + NPCs do feudo |
| Monster Pack | Itch.io | Criaturas da mina |
| UI Kit Medieval Pixel | Itch.io | HUD, inventário, mini-game |
| SFX Pack (forja, madeira, moedas) | Kenney.nl (CC0) | Sons do loop principal |
| Music Pack (medieval ambience) | Itch.io (Patrick de Arteaga) | Trilha da ferraria e do feudo |
| Particle VFX Pack | Unity Asset Store | Faíscas da forja, poeira da mina |

### 14.4 O Que NÃO Está no MVP

| Feature | Motivo |
|---|---|
| Reposicionamento livre de objetos | Complexidade desnecessária — layout fixo é suficiente |
| Minerador contratável | Decisão narrativa e de design — mina é risco do jogador |
| Mais de 2 finais | Escopo narrativo controlado |
| Sistema de combate elaborado | Combate é barreira, não o foco |
| Mais de 3 espécies não-humanas | Thresh é suficiente para o MVP |
| Sistema de preço dinâmico | Preços fixos com multiplicador de nota é suficiente |
| Multiplayer | Fora de escopo |

---

*GDD v1.0 — Iron & Blood · Unity 6.3 LTS · Dev solo · MVP 3 meses*
*Inspirado em Graveyard Keeper e Jacksmith. Narrativa centrada em herança, identidade e escolha.*
