<div align="center">

# ⚒️ Iron & Blood

**Simulador de Ferraria Medieval · Top-Down Pixel Art**

![Unity](https://img.shields.io/badge/Unity-6.3_LTS-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-12.0-purple?logo=csharp)
![Status](https://img.shields.io/badge/Status-Em_Desenvolvimento-orange)
![Fase](https://img.shields.io/badge/Fase-1%20de%206-blue)
![Semana](https://img.shields.io/badge/Sprint-Semana_1-yellow)

[Documentação](./docs/index.html) · [GDD](./docs/GDD.md) · [Sprints](./docs/sprints/plano-sprints.md)

</div>

---

## Sobre o Jogo

**Iron & Blood** é um simulador de gerenciamento de ferraria em Pixel Art com perspectiva top-down. Você joga como **Durin Ironcroft**, um anão que herda a ferraria do pai após sua morte e parte para o Feudo de Cinzas para assumir o negócio — sem saber que as paredes da própria forja guardam segredos que o pai nunca contou.

Durante o dia você fabrica armas, atende clientes, fecha acordos com o feudo e espécies da floresta, contrata funcionários e gerencia seu estoque. À noite, os segredos da ferraria começam a falar.

**Referências:** Graveyard Keeper · Jacksmith · Stardew Valley

---

## Mecânicas Principais

- **Mini-game de fabricação** estilo Jacksmith — encaixe de peças com sistema de notas S→D que afeta preço e reputação
- **IA de clientes** que vasculham prateleiras fisicamente e aceitam alternativas por conta própria
- **Sistema de tempo** com ciclo dia/noite de ~10 minutos, horário de abertura e fechamento manual da loja
- **Coleta em campo** — floresta (madeira, caça), mina (minérios, combate), riacho (pesca, itens secretos)
- **Acordos e relacionamentos** com NPCs do feudo e espécies não-humanas da floresta
- **Narrativa linear com dois finais** — revelada pelos marcos de progresso do jogador, não por cutscenes

---

## Estado Atual

| Sistema | Status |
|---|---|
| Setup do projeto Unity | 🔄 Em andamento |
| Player Movement | ⬜ Pendente |
| GameClock / Dia e Noite | ⬜ Pendente |
| Sistema de Inventário | ⬜ Pendente |
| Mini-game de Craft | ⬜ Pendente |
| IA de Clientes | ⬜ Pendente |
| Floresta e Mina | ⬜ Pendente |
| NPCs e Relacionamentos | ⬜ Pendente |
| Narrativa | ⬜ Pendente |
| Build Final | ⬜ Pendente |

---

## Como Rodar

### Requisitos

- Unity **6.3 LTS**
- .NET / C# 12
- Git

### Pacotes Unity necessários

| Pacote | Versão |
|---|---|
| Input System | 1.7+ |
| Cinemachine | 3.x |
| TextMeshPro | 3.0+ |
| 2D Tilemap Extras | 3.1+ |
| DOTween | última estável |

### Passos

```bash
# 1. Clone o repositório
git clone https://github.com/Luciano-Claudio/Iron-Blood.git

# 2. Abra o Unity Hub → Add → selecione a pasta clonada

# 3. Abra com Unity 6.3 LTS
# Os pacotes serão instalados automaticamente via Package Manager

# 4. Abra a cena principal
# Assets/Scenes/GameScene.unity

# 5. Play ▶
```

> **Atenção:** Não use versões do Unity diferentes da 6.3 LTS. A API do Cinemachine 3.x e do Input System variam entre versões e podem causar erros de compilação.

---

## Estrutura do Repositório

```
Iron-Blood/
├── Assets/
│   ├── Scenes/
│   ├── Scripts/
│   │   ├── Core/          ← GameClock, SleepManager, EconomyManager
│   │   ├── Player/        ← PlayerMovement, PlayerCombat
│   │   ├── Shop/          ← ShopSign, CustomerAI, CustomerSpawner
│   │   ├── Inventory/     ← PlayerInventory, ContainerInventory
│   │   ├── Crafting/      ← CraftingMinigame, CraftingRecipeSO
│   │   ├── NPCs/          ← DialogueManager, NPCRelationship
│   │   ├── World/         ← MineralNode, TreeNode, FishingSystem
│   │   ├── Narrative/     ← NarrativeManager, TutorialManager
│   │   └── UI/            ← HUDManager, InventoryUI, ClockHUD
│   ├── ScriptableObjects/
│   │   ├── Items/
│   │   ├── Recipes/
│   │   └── NPCs/
│   ├── Art/
│   │   ├── Sprites/
│   │   ├── Tilemaps/
│   │   └── VFX/
│   └── Audio/
├── docs/
│   ├── index.html         ← Índice da documentação
│   ├── GDD.md             ← Game Design Document completo
│   ├── sprints/           ← Tasks semanais
│   └── systems/           ← Documentação técnica por sistema
├── .gitignore
└── README.md
```

---

## Branches

| Branch | Uso |
|---|---|
| `main` | Código estável — recebe merge ao fim de cada fase |
| `develop` | Branch de trabalho — integração entre semanas |
| `feature/semana-XX` | Branch da sprint atual |

**Fluxo:** `feature/semana-XX` → PR → `develop` → PR → `main` (ao fim de cada fase)

---

## Roadmap

- [x] Fase 0 — Repositório e documentação base
- [ ] **Fase 1** — Fundação (Semanas 1–4)
- [ ] Fase 2 — Loop da Loja (Semanas 5–8)
- [ ] Fase 3 — Mundo e Coleta (Semanas 9–11)
- [ ] Fase 4 — Social e Econômico (Semanas 12–14)
- [ ] Fase 5 — Comida e Narrativa (Semanas 15–18)
- [ ] Fase 6 — Polish e Build (Semanas 19–20)

---

## Documentação

A documentação completa está em [`/docs`](./docs/index.html), incluindo:

- [Game Design Document](./docs/GDD.md) — especificação completa de todos os sistemas
- [Plano de Sprints](./docs/sprints/plano-sprints.md) — 20 semanas de desenvolvimento
- [Documentação técnica](./docs/systems/) — um arquivo por sistema implementado

---

## Desenvolvedor

**Luciano** — Dev solo
Universidade Estadual da Paraíba (UEPB) · Ciência da Computação

---

<div align="center">

*Forjado com C# e muita estamina.*

</div>