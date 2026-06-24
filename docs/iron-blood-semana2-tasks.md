# Iron & Blood — Tasks da Semana 2
**Sprint:** Semana 2 de 20 | **Fase:** 1 — Fundação
**Foco:** Additive Loading · Inventário · Interação · HUD base · Layout da Ferraria

> ⚠️ Sprint prioritária. Os sistemas de inventário e interação são pré-requisitos diretos da Semana 5 (IA de clientes) e Semana 3 (placa da loja).

---

## Entregável Final da Semana
> O jogador entra na ferraria pela porta (transição com EasyTransitions), abre o inventário, pega um item do baú e arrasta para a prateleira. Os dois containers se comportam de forma diferente. Um toast aparece na HUD ao tentar dormir cedo.

---

## BLOCO A — Arquitetura de Cenas

### TASK A-1 — Reorganizar estrutura de pastas

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | A — Cenas |
| Estimativa | 20 min |
| Depende de | — |

Criar a estrutura de pastas definitiva do projeto antes de qualquer código:

```
Assets/
├── Scenes/
│   ├── Core/          ← mover GameScene.unity para cá
│   ├── World/         ← Exterior.unity (criar vazio)
│   └── Interiors/     ← Ferraria.unity (criar vazio)
├── Scripts/
│   ├── Core/          ← já existe
│   ├── Player/        ← já existe
│   ├── Inventory/     ← criar
│   ├── UI/            ← criar
│   ├── World/         ← criar, mover DayNightVisuals.cs
│   └── Debug/         ← criar, mover ClockDebugger e SleepTester
├── Art/
│   ├── Sprites/
│   │   ├── Player/
│   │   ├── Items/
│   │   └── UI/
│   ├── Tilemaps/
│   │   ├── Ferraria/
│   │   └── Exterior/
│   └── Lighting/
│       ├── NormalMaps/
│       └── Materials/
├── Prefabs/
│   ├── Interactables/
│   ├── UI/
│   └── Containers/
└── ScriptableObjects/
    └── Items/
```

Após mover os scripts, verificar que não há erros de compilação.

**Critério de conclusão:** Estrutura criada. Scripts movidos. Zero erros no console.

---

### TASK A-2 — Criar SceneLoader.cs

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | A — Cenas |
| Estimativa | 50 min |
| Depende de | A-1 |

Criar `Assets/Scripts/Core/SceneLoader.cs`:

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using EasyTransition;

// Gerencia carregamento e descarregamento de cenas additivamente.
// GameScene (Core) nunca é descarregada.
// Apenas uma cena de área (Exterior ou Interior) fica carregada por vez.

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("Transição")]
    [SerializeField] private TransitionSettings transitionSettings;
    [SerializeField] private float transitionDelay = 0f;

    private string currentAreaScene; // cena de área atualmente carregada

    private void Awake() => Instance = this;

    // Chamado pela porta ao pressionar E
    public void LoadArea(string sceneName, Vector2 spawnPosition)
    {
        StartCoroutine(TransitionToArea(sceneName, spawnPosition));
    }

    private IEnumerator TransitionToArea(string sceneName, Vector2 spawnPosition)
    {
        // Bloqueia input do player durante transição
        GameEvents.RaiseSceneTransitionStarted();

        var tm = TransitionManager.Instance();

        tm.onTransitionCutPointReached = () =>
        {
            StartCoroutine(SwapScene(sceneName, spawnPosition));
        };

        tm.onTransitionEnd = () =>
        {
            tm.onTransitionCutPointReached = null;
            tm.onTransitionEnd             = null;
            GameEvents.RaiseSceneTransitionEnded();
        };

        tm.Transition(transitionSettings, transitionDelay);
        yield return null;
    }

    private IEnumerator SwapScene(string sceneName, Vector2 spawnPosition)
    {
        // Descarrega cena anterior se existir
        if (!string.IsNullOrEmpty(currentAreaScene))
        {
            yield return SceneManager.UnloadSceneAsync(currentAreaScene);
        }

        // Carrega nova cena additivamente
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        currentAreaScene = sceneName;

        // Posiciona player no spawn point da cena destino
        PlayerSpawnPoint spawn = FindFirstObjectByType<PlayerSpawnPoint>();
        if (spawn != null)
            PlayerMovement.Instance.Teleport(spawn.transform.position);
        else
            Debug.LogWarning($"[SceneLoader] PlayerSpawnPoint não encontrado em {sceneName}");
    }
}
```

Adicionar ao `GameManager` na `GameScene`.

Adicionar em `GameEvents.cs` (descomentar quando implementar):
```csharp
public static event Action OnSceneTransitionStarted;
public static void RaiseSceneTransitionStarted() => OnSceneTransitionStarted?.Invoke();

public static event Action OnSceneTransitionEnded;
public static void RaiseSceneTransitionEnded() => OnSceneTransitionEnded?.Invoke();
```

Adicionar `public static PlayerMovement Instance` e método `Teleport(Vector2 pos)` no `PlayerMovement.cs`:
```csharp
public static PlayerMovement Instance;
private void Awake() { Instance = this; rb = GetComponent<Rigidbody2D>(); }
public void Teleport(Vector2 position) => transform.position = position;
```

**Critério de conclusão:** Script compila sem erros. Singleton acessível.

---

### TASK A-3 — Criar PlayerSpawnPoint.cs e configurar cenas

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | A — Cenas |
| Estimativa | 30 min |
| Depende de | A-2 |

Criar `Assets/Scripts/World/PlayerSpawnPoint.cs`:

```csharp
using UnityEngine;

// Marca o ponto onde o player aparece ao entrar nesta cena.
// Cada cena de área tem exatamente um PlayerSpawnPoint.
public class PlayerSpawnPoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        Gizmos.DrawIcon(transform.position, "sv_icon_dot3_pix16_gizmo", true);
    }
}
```

Configurar as cenas:
- `Exterior.unity` → criar GameObject `PlayerSpawnPoint` na entrada do feudo
- `Ferraria.unity` → criar GameObject `PlayerSpawnPoint` na entrada da ferraria (lado interno)
- Ambas as cenas devem ter **Build Settings** configurados (File → Build Settings → Add Open Scenes)

**Critério de conclusão:** Duas cenas no Build Settings. Cada uma com um `PlayerSpawnPoint` visível via Gizmo verde.

---

### TASK A-4 — Criar DoorInteractable.cs

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | A — Cenas |
| Estimativa | 40 min |
| Depende de | A-3, B-1 |

Criar `Assets/Scripts/World/DoorInteractable.cs`:

```csharp
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("Destino")]
    [SerializeField] private string targetScene;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer doorRenderer;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;

    public string InteractionLabel => "Entrar";

    public void Interact()
    {
        SceneLoader.Instance.LoadArea(targetScene, Vector2.zero);
    }
}
```

Criar dois prefabs em `Assets/Prefabs/Interactables/`:
- `Door_ToFerraria.prefab` → `targetScene = "Ferraria"`
- `Door_ToExterior.prefab` → `targetScene = "Exterior"`

Posicionar na `Exterior.unity` e `Ferraria.unity` respectivamente.

**Critério de conclusão:** Interagir com a porta executa a transição e o player aparece no spawn point da cena destino.

---

## BLOCO B — Sistema de Interação

### TASK B-1 — Criar IInteractable e PlayerInteractor

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | B — Interação |
| Estimativa | 45 min |
| Depende de | A-1 |

Criar `Assets/Scripts/Inventory/IInteractable.cs`:

```csharp
// Interface implementada por qualquer objeto com o qual o jogador pode interagir.
// Exemplos: cama, porta, baú, prateleira, NPC, bancada de craft.
public interface IInteractable
{
    string InteractionLabel { get; } // texto exibido no prompt (ex: "Abrir", "Entrar", "Dormir")
    void Interact();
}
```

Criar `Assets/Scripts/Player/PlayerInteractor.cs`:

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteractor : MonoBehaviour
{
    public static PlayerInteractor Instance;

    private IInteractable currentInteractable;
    public IInteractable CurrentInteractable => currentInteractable;

    public event System.Action<IInteractable> OnInteractableEntered;
    public event System.Action                OnInteractableExited;

    private void Awake() => Instance = this;

    // Chamado pelo PlayerInput via Send Messages
    public void OnInteract(InputValue value)
    {
        if (!value.isPressed) return;
        if (GameClock.Instance.IsSleeping) return;
        currentInteractable?.Interact();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            currentInteractable = interactable;
            OnInteractableEntered?.Invoke(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable)
            && interactable == currentInteractable)
        {
            currentInteractable = null;
            OnInteractableExited?.Invoke();
        }
    }
}
```

Adicionar ao Player:
- `PlayerInteractor` (com Collider2D trigger de raio ~1u)
- O Collider2D do interactor deve estar numa camada que detecta `Interactable`

**Critério de conclusão:** Aproximar do baú registra o interactable. Afastar limpa. Log no console confirma.

---

### TASK B-2 — Criar InteractionPrompt.cs

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | B — Interação |
| Estimativa | 35 min |
| Depende de | B-1 |

Criar `Assets/Scripts/UI/InteractionPrompt.cs`:

```csharp
using UnityEngine;
using TMPro;

// Exibe o ícone [E] sobre o objeto interagível, não sobre o player.
// Segue a posição do objeto em world space convertida para screen space.
public class InteractionPrompt : MonoBehaviour
{
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TMP_Text labelText;
    [SerializeField] private Vector3 worldOffset = new Vector3(0f, 1f, 0f);

    private Transform targetTransform;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        promptPanel.SetActive(false);
    }

    private void OnEnable()
    {
        PlayerInteractor.Instance.OnInteractableEntered += Show;
        PlayerInteractor.Instance.OnInteractableExited  += Hide;
    }

    private void OnDisable()
    {
        PlayerInteractor.Instance.OnInteractableEntered -= Show;
        PlayerInteractor.Instance.OnInteractableExited  -= Hide;
    }

    private void Show(IInteractable interactable)
    {
        // Busca o Transform do objeto que implementa IInteractable
        if (interactable is MonoBehaviour mb)
            targetTransform = mb.transform;

        labelText.text = $"[E] {interactable.InteractionLabel}";
        promptPanel.SetActive(true);
    }

    private void Hide()
    {
        promptPanel.SetActive(false);
        targetTransform = null;
    }

    private void LateUpdate()
    {
        if (targetTransform == null) return;

        Vector3 worldPos   = targetTransform.position + worldOffset;
        Vector3 screenPos  = mainCamera.WorldToScreenPoint(worldPos);
        transform.position = screenPos;
    }
}
```

Criar prefab `InteractionPrompt` em `Assets/Prefabs/UI/` e adicionar ao Canvas da GameScene.

**Critério de conclusão:** Prompt aparece sobre o objeto ao se aproximar. Desaparece ao afastar. Texto exibe o `InteractionLabel` correto.

---

## BLOCO C — Sistema de Inventário

### TASK C-1 — Criar ItemSO e ItemStack

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | C — Inventário |
| Estimativa | 40 min |
| Depende de | A-1 |

Criar `Assets/Scripts/Inventory/ItemSO.cs`:

```csharp
using UnityEngine;

public enum ItemCategory
{
    Weapon, Armor, Tool, Material, Consumable, Key
}

[CreateAssetMenu(menuName = "IronBlood/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Identidade")]
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;

    [Header("Classificação")]
    public ItemCategory category;

    [Header("Economia")]
    public int baseValue;

    [Header("Stack")]
    public bool isStackable = true;
    public int maxStackSize = 99;
}
```

Criar `Assets/Scripts/Inventory/ItemStack.cs`:

```csharp
using System;
using UnityEngine;

[Serializable]
public class ItemStack
{
    public ItemSO item;
    public int quantity;

    public ItemStack(ItemSO item, int quantity)
    {
        this.item     = item;
        this.quantity = quantity;
    }

    public bool IsEmpty => quantity <= 0;
}
```

Criar os primeiros `ItemSO` em `Assets/ScriptableObjects/Items/`:
- `Iron_SO` — Material, stackable, baseValue 15
- `Wood_SO` — Material, stackable, baseValue 8
- `Sword_Short_SO` — Weapon, não stackable, baseValue 50

**Critério de conclusão:** 3 assets criados no Project. Menu "IronBlood/Item" disponível.

---

### TASK C-2 — Criar PlayerInventory.cs

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | C — Inventário |
| Estimativa | 50 min |
| Depende de | C-1 |

Criar `Assets/Scripts/Inventory/PlayerInventory.cs`:

```csharp
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    [Header("Configuração")]
    [SerializeField] private int maxSlots = 20;

    private List<ItemStack> items = new();

    public IReadOnlyList<ItemStack> Items => items;

    private void Awake() => Instance = this;

    public bool AddItem(ItemSO item, int quantity = 1)
    {
        if (item.isStackable)
        {
            var existing = items.FirstOrDefault(s => s.item == item);
            if (existing != null)
            {
                int canAdd = item.maxStackSize - existing.quantity;
                int adding = Mathf.Min(quantity, canAdd);
                existing.quantity += adding;

                if (adding < quantity)
                    return TryAddNewSlot(item, quantity - adding);

                GameEvents.RaiseInventoryChanged();
                return true;
            }
        }
        return TryAddNewSlot(item, quantity);
    }

    private bool TryAddNewSlot(ItemSO item, int quantity)
    {
        if (items.Count >= maxSlots) return false;
        items.Add(new ItemStack(item, quantity));
        GameEvents.RaiseInventoryChanged();
        return true;
    }

    public bool RemoveItem(ItemSO item, int quantity = 1)
    {
        var stack = items.FirstOrDefault(s => s.item == item);
        if (stack == null || stack.quantity < quantity) return false;

        stack.quantity -= quantity;
        if (stack.quantity <= 0) items.Remove(stack);

        GameEvents.RaiseInventoryChanged();
        return true;
    }

    public int GetQuantity(ItemSO item)
        => items.Where(s => s.item == item).Sum(s => s.quantity);

    public bool HasItem(ItemSO item, int quantity = 1)
        => GetQuantity(item) >= quantity;
}
```

Adicionar ao GameObject `Player` na GameScene.

Descomentar em `GameEvents.cs`:
```csharp
public static event Action OnInventoryChanged;
public static void RaiseInventoryChanged() => OnInventoryChanged?.Invoke();
```

**Critério de conclusão:** `AddItem`, `RemoveItem` e `GetQuantity` funcionando. `OnInventoryChanged` disparando.

---

### TASK C-3 — Criar ContainerInventory.cs

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | C — Inventário |
| Estimativa | 50 min |
| Depende de | C-1, B-1 |

Criar `Assets/Scripts/Inventory/ContainerInventory.cs`:

```csharp
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ContainerInventory : MonoBehaviour, IInteractable
{
    [Header("Configuração")]
    public string containerName = "Baú";
    public int maxSlots = 10;

    [Header("Comportamento")]
    // true = prateleira (clientes podem ver e comprar)
    // false = baú (só o jogador acessa)
    public bool accessibleToClients = false;

    private List<ItemStack> items = new();
    public IReadOnlyList<ItemStack> Items => items;

    public string InteractionLabel => $"Abrir {containerName}";

    // Chamado pelo PlayerInteractor ao pressionar E
    public void Interact()
        => InventoryUI.Instance.OpenContainer(this);

    public bool StoreItem(ItemSO item, int quantity = 1)
    {
        var existing = items.FirstOrDefault(s => s.item == item);
        if (existing != null && item.isStackable)
        {
            existing.quantity += quantity;
            return true;
        }
        if (items.Count >= maxSlots) return false;
        items.Add(new ItemStack(item, quantity));
        return true;
    }

    public bool TakeItem(ItemSO item, int quantity = 1)
    {
        var stack = items.FirstOrDefault(s => s.item == item);
        if (stack == null || stack.quantity < quantity) return false;

        stack.quantity -= quantity;
        if (stack.quantity <= 0) items.Remove(stack);
        return true;
    }

    // Usado pela IA de clientes na Semana 6
    public ItemStack FindItem(ItemSO requested, ItemSO[] alternatives = null)
    {
        if (!accessibleToClients) return null;

        var exact = items.FirstOrDefault(s => s.item == requested && s.quantity > 0);
        if (exact != null) return exact;

        if (alternatives != null)
            return items.FirstOrDefault(
                s => alternatives.Contains(s.item) && s.quantity > 0);

        return null;
    }
}
```

Criar dois prefabs em `Assets/Prefabs/Containers/`:
- `Bau.prefab` → `accessibleToClients = false`, `containerName = "Baú"`
- `Prateleira.prefab` → `accessibleToClients = true`, `containerName = "Prateleira"`

**Critério de conclusão:** Interagir com o baú abre o UI. Interagir com a prateleira também abre. Os dois têm `InteractionLabel` diferentes.

---

## BLOCO D — UI de Inventário

### TASK D-1 — Criar HUDManager.cs

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | D — UI |
| Estimativa | 35 min |
| Depende de | A-1 |

Criar `Assets/Scripts/UI/HUDManager.cs`:

```csharp
using UnityEngine;
using TMPro;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("Toast")]
    [SerializeField] private GameObject toastPanel;
    [SerializeField] private TMP_Text toastText;
    [SerializeField] private float toastDuration = 2.5f;

    private Coroutine toastCoroutine;

    private void Awake() => Instance = this;

    public void ShowToast(string message)
    {
        if (toastCoroutine != null) StopCoroutine(toastCoroutine);
        toastCoroutine = StartCoroutine(ToastCoroutine(message));
    }

    private IEnumerator ToastCoroutine(string message)
    {
        toastText.text = message;
        toastPanel.SetActive(true);
        yield return new WaitForSeconds(toastDuration);
        toastPanel.SetActive(false);
    }
}
```

Criar o painel de toast no Canvas da GameScene. Com isso, os comentários `// TODO: HUDManager.Instance.ShowToast()` do `SleepManager` podem ser descomentados.

**Critério de conclusão:** `ShowToast("Ainda é cedo para dormir.")` aparece na tela ao tentar dormir antes das 18h.

---

### TASK D-2 — Criar InventoryUI.cs

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | D — UI |
| Estimativa | 60 min |
| Depende de | C-2, C-3 |

Criar `Assets/Scripts/UI/InventoryUI.cs`:

```csharp
using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [Header("Painéis")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject containerPanel;
    [SerializeField] private Transform playerSlotsParent;
    [SerializeField] private Transform containerSlotsParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject inventorySlotPrefab;

    private ContainerInventory currentContainer;
    private bool isOpen;

    private void Awake() => Instance = this;

    private void OnEnable()
        => GameEvents.OnInventoryChanged += RefreshPlayerInventory;

    private void OnDisable()
        => GameEvents.OnInventoryChanged -= RefreshPlayerInventory;

    // Chamado pelo PlayerInput (tecla I)
    public void OnToggleInventory(UnityEngine.InputSystem.InputValue value)
    {
        if (!value.isPressed) return;
        if (isOpen) CloseAll();
        else OpenPlayerInventory();
    }

    public void OpenPlayerInventory()
    {
        isOpen = true;
        inventoryPanel.SetActive(true);
        RefreshPlayerInventory();
    }

    public void OpenContainer(ContainerInventory container)
    {
        currentContainer = container;
        isOpen = true;
        inventoryPanel.SetActive(true);
        containerPanel.SetActive(true);
        RefreshPlayerInventory();
        RefreshContainer();
    }

    public void CloseAll()
    {
        isOpen = false;
        inventoryPanel.SetActive(false);
        containerPanel.SetActive(false);
        currentContainer = null;
    }

    private void RefreshPlayerInventory()
    {
        foreach (Transform child in playerSlotsParent)
            Destroy(child.gameObject);

        foreach (var stack in PlayerInventory.Instance.Items)
        {
            var slot = Instantiate(inventorySlotPrefab, playerSlotsParent);
            slot.GetComponent<InventorySlotUI>().Setup(stack, isPlayerSlot: true);
        }
    }

    private void RefreshContainer()
    {
        foreach (Transform child in containerSlotsParent)
            Destroy(child.gameObject);

        if (currentContainer == null) return;

        foreach (var stack in currentContainer.Items)
        {
            var slot = Instantiate(inventorySlotPrefab, containerSlotsParent);
            slot.GetComponent<InventorySlotUI>().Setup(stack, isPlayerSlot: false);
        }
    }
}
```

**Critério de conclusão:** Tecla I abre o inventário do player. Interagir com container abre os dois painéis lado a lado.

---

### TASK D-3 — Criar DragDropHandler.cs

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Alta |
| Bloco | D — UI |
| Estimativa | 60 min |
| Depende de | D-2 |

Criar `Assets/Scripts/UI/InventorySlotUI.cs` e `Assets/Scripts/UI/DragDropHandler.cs`:

```csharp
// InventorySlotUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text quantityText;

    public ItemStack Stack { get; private set; }
    public bool IsPlayerSlot { get; private set; }

    private GameObject dragProxy;
    private CanvasGroup canvasGroup;

    public void Setup(ItemStack stack, bool isPlayerSlot)
    {
        Stack       = stack;
        IsPlayerSlot = isPlayerSlot;
        iconImage.sprite  = stack.item.icon;
        quantityText.text = stack.quantity > 1 ? stack.quantity.ToString() : "";
    }

    public void OnBeginDrag(PointerEventData e)
    {
        dragProxy = Instantiate(gameObject, transform.root);
        dragProxy.GetComponent<CanvasGroup>().blocksRaycasts = false;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.4f;
    }

    public void OnDrag(PointerEventData e)
    {
        if (dragProxy != null)
            dragProxy.transform.position = e.position;
    }

    public void OnEndDrag(PointerEventData e)
    {
        Destroy(dragProxy);
        if (canvasGroup != null) canvasGroup.alpha = 1f;
    }

    public void OnDrop(PointerEventData e)
    {
        if (e.pointerDrag == null) return;
        var source = e.pointerDrag.GetComponent<InventorySlotUI>();
        if (source == null) return;

        DragDropHandler.Instance.HandleDrop(source, this);
    }
}
```

```csharp
// DragDropHandler.cs
using UnityEngine;

public class DragDropHandler : MonoBehaviour
{
    public static DragDropHandler Instance;
    private void Awake() => Instance = this;

    public void HandleDrop(InventorySlotUI source, InventorySlotUI target)
    {
        // Player → Container
        if (source.IsPlayerSlot && !target.IsPlayerSlot)
        {
            MovePlayerToContainer(source.Stack);
            return;
        }

        // Container → Player
        if (!source.IsPlayerSlot && target.IsPlayerSlot)
        {
            MoveContainerToPlayer(source.Stack);
            return;
        }

        // Player → Player (reordenação futura)
        // Container → Container (não implementado no MVP)
    }

    private void MovePlayerToContainer(ItemStack stack)
    {
        var container = InventoryUI.Instance.CurrentContainer;
        if (container == null) return;

        if (!container.StoreItem(stack.item, stack.quantity)) return;
        PlayerInventory.Instance.RemoveItem(stack.item, stack.quantity);
        InventoryUI.Instance.Refresh();
    }

    private void MoveContainerToPlayer(ItemStack stack)
    {
        if (!PlayerInventory.Instance.AddItem(stack.item, stack.quantity)) return;

        var container = InventoryUI.Instance.CurrentContainer;
        container?.TakeItem(stack.item, stack.quantity);
        InventoryUI.Instance.Refresh();
    }
}
```

Adicionar `CurrentContainer` e `Refresh()` ao `InventoryUI`:
```csharp
public ContainerInventory CurrentContainer => currentContainer;
public void Refresh() { RefreshPlayerInventory(); RefreshContainer(); }
```

**Critério de conclusão:** Arrastar item do baú para o inventário do player move o item. Arrastar do inventário para a prateleira move o item. A UI atualiza em tempo real.

---

## BLOCO E — Arte e Iluminação (se sobrar tempo)

### TASK E-1 — Normal Maps e iluminação interior da Ferraria

| Campo | Valor |
|---|---|
| Coluna | A Fazer |
| Prioridade | Média |
| Bloco | E — Arte |
| Estimativa | 2–3h |
| Depende de | A-3 |

- Estudar/relembrar Normal Maps na Unity (30min)
- Aplicar Normal Maps nos tilesets da ferraria
- Criar material `Sprite-Lit-Default` para os sprites da ferraria
- Adicionar luzes interiores:
  - `Point Light 2D` na forja (laranja-quente, cycleStrength 0 — estática)
  - `Point Light 2D` nas velas/lanternas (amarelo, estática)
  - `Spot Light 2D` nas janelas (conectar ao DayNightVisuals via ManagedLight)
- Ajustar `cycleStrength` do Global Light Interior baseado nas novas luzes

> Esta task é **opcional nesta semana**. Se o tempo não permitir, entra na Semana 3 sem impacto no escopo crítico.

**Critério de conclusão:** Luzes da forja e velas visíveis. Normal maps reagindo à iluminação. Interior visivelmente diferente do exterior ao entrar pela porta.

---

## Checklist de Validação Final

- [ ] Estrutura de pastas criada e scripts reorganizados
- [ ] `Exterior.unity` e `Ferraria.unity` no Build Settings
- [ ] Porta da ferraria leva ao interior com transição
- [ ] Porta do interior leva ao exterior com transição
- [ ] Player aparece no spawn point correto em cada cena
- [ ] Prompt [E] aparece sobre objetos interagíveis
- [ ] Prompt desaparece ao afastar
- [ ] Tecla I abre/fecha inventário do player
- [ ] Interagir com baú abre inventário + painel do container
- [ ] Interagir com prateleira abre inventário + painel do container
- [ ] Arrastar item do baú para inventário move o item
- [ ] Arrastar item do inventário para prateleira move o item
- [ ] `ShowToast` funcionando (testar dormindo antes das 18h)
- [ ] `GameEvents.OnInventoryChanged` disparando nas transferências
- [ ] `accessibleToClients` diferencia baú de prateleira
- [ ] Zero erros de compilação

---

## Armadilhas desta Semana

**"FindFirstObjectByType é lento"**
→ Aceitável aqui — só é chamado no cut point da transição, não em Update.

**"Additive Loading não limpa o estado da cena anterior"**
→ Objetos com DontDestroyOnLoad da GameScene persistem corretamente. Objetos de área (NPCs, containers) são destruídos com UnloadSceneAsync automaticamente.

**"Drag & Drop não funciona fora do Canvas"**
→ O `EventSystem` deve existir na cena. Se não existir, criar GameObject → UI → Event System.

**"IInteractable com dois colliders sobrepostos"**
→ Se o player tem um collider de física e um trigger de interação, garantir que são dois Collider2D distintos com configurações diferentes. O trigger do `PlayerInteractor` não deve ter a flag `Is Trigger` no collider principal.

**"Container abre mas mostra vazio"**
→ Verificar se o `ContainerInventory` tem itens adicionados via Inspector (arrastar ItemSO nos campos de teste).

---
*Semana 2 · Iron & Blood · Unity 6.3 LTS · Gerado em 24/06/2026*
