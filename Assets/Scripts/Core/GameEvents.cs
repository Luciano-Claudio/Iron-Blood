using System;

// ═══════════════════════════════════════════════════════════════════════════════
// GAME EVENTS — Iron & Blood
// Central de todos os eventos do jogo.
//
// COMO USAR:
//   Assinar:   GameEvents.OnTimeChanged += MinhaFuncao;
//   Cancelar:  GameEvents.OnTimeChanged -= MinhaFuncao;
//   Disparar:  GameEvents.RaiseTimeChanged(hora, minuto);
//              (só o sistema responsável pelo evento deve disparar)
//
// REGRA: Nunca assine em Awake(). Use OnEnable()/Start() e cancele em OnDisable().
// ═══════════════════════════════════════════════════════════════════════════════

public static class GameEvents
{
    // ───────────────────────────────────────────────────────────────────────────
    // TEMPO E CALENDÁRIO
    // Disparado por: GameClock
    // ───────────────────────────────────────────────────────────────────────────

    public static event Action<int, int> OnTimeChanged;
    public static void RaiseTimeChanged(int hour, int minute) => OnTimeChanged?.Invoke(hour, minute);

    public static event Action<int> OnNewDay;
    public static void RaiseNewDay(int day) => OnNewDay?.Invoke(day);

    public static event Action<int> OnNewWeek;
    public static void RaiseNewWeek(int week) => OnNewWeek?.Invoke(week);

    // Use para trocar estação no futuro — Mês 1 Primavera | 2 Verão | 3 Outono | 4 Inverno
    public static event Action<int> OnNewMonth;
    public static void RaiseNewMonth(int month) => OnNewMonth?.Invoke(month);

    public static event Action<int> OnNewYear;
    public static void RaiseNewYear(int year) => OnNewYear?.Invoke(year);


    // ───────────────────────────────────────────────────────────────────────────
    // SONO
    // Disparado por: SleepManager — Task C-2
    // ───────────────────────────────────────────────────────────────────────────

    // Dispara quando o fade de sono começa (clock já está pausado)
    public static event Action OnPlayerSlept;
    public static void RaisePlayerSlept() => OnPlayerSlept?.Invoke();

    // Dispara quando o fade de acordar termina (clock já está rodando)
    public static event Action OnPlayerWokeUp;
    public static void RaisePlayerWokeUp() => OnPlayerWokeUp?.Invoke();


    // ───────────────────────────────────────────────────────────────────────────
    // LOJA
    // Disparado por: ShopSign — Semana 3
    // ───────────────────────────────────────────────────────────────────────────

    // public static event Action OnShopOpened;
    // public static void RaiseShopOpened() => OnShopOpened?.Invoke();

    // public static event Action OnShopClosed;
    // public static void RaiseShopClosed() => OnShopClosed?.Invoke();


    // ───────────────────────────────────────────────────────────────────────────
    // INVENTÁRIO
    // Disparado por: PlayerInventory, ContainerInventory — Semana 2
    // ───────────────────────────────────────────────────────────────────────────

    // public static event Action OnInventoryChanged;
    // public static void RaiseInventoryChanged() => OnInventoryChanged?.Invoke();

    // public static event Action<ItemSO, int> OnItemAdded;
    // public static void RaiseItemAdded(ItemSO item, int qty) => OnItemAdded?.Invoke(item, qty);

    // public static event Action<ItemSO, int> OnItemRemoved;
    // public static void RaiseItemRemoved(ItemSO item, int qty) => OnItemRemoved?.Invoke(item, qty);


    // ───────────────────────────────────────────────────────────────────────────
    // ECONOMIA
    // Disparado por: EconomyManager — Semana 4
    // ───────────────────────────────────────────────────────────────────────────

    // public static event Action<int> OnGoldChanged;
    // public static void RaiseGoldChanged(int total) => OnGoldChanged?.Invoke(total);


    // ───────────────────────────────────────────────────────────────────────────
    // CRAFT
    // Disparado por: CraftingMinigame — Semana 5
    // ───────────────────────────────────────────────────────────────────────────

    // public static event Action<CraftedItem> OnItemCrafted;
    // public static void RaiseItemCrafted(CraftedItem item) => OnItemCrafted?.Invoke(item);

    // public static event Action OnCraftStarted;
    // public static void RaiseCraftStarted() => OnCraftStarted?.Invoke();

    // public static event Action OnCraftCancelled;
    // public static void RaiseCraftCancelled() => OnCraftCancelled?.Invoke();


    // ───────────────────────────────────────────────────────────────────────────
    // CLIENTES E REPUTAÇÃO
    // Disparado por: CustomerAI, ReputationManager — Semana 6
    // ───────────────────────────────────────────────────────────────────────────

    // public static event Action<CustomerMood> OnSaleCompleted;
    // public static void RaiseSaleCompleted(CustomerMood mood) => OnSaleCompleted?.Invoke(mood);

    // public static event Action OnNoSale;
    // public static void RaiseNoSale() => OnNoSale?.Invoke();

    // public static event Action<int> OnReputationChanged;
    // public static void RaiseReputationChanged(int value) => OnReputationChanged?.Invoke(value);


    // ───────────────────────────────────────────────────────────────────────────
    // ACORDOS E RELACIONAMENTOS
    // Disparado por: AgreementSystem — Semana 8
    // ───────────────────────────────────────────────────────────────────────────

    // public static event Action<string> OnAgreementStarted;
    // public static void RaiseAgreementStarted(string id) => OnAgreementStarted?.Invoke(id);

    // public static event Action<string> OnAgreementCompleted;
    // public static void RaiseAgreementCompleted(string id) => OnAgreementCompleted?.Invoke(id);

    // public static event Action<string> OnAgreementFailed;
    // public static void RaiseAgreementFailed(string id) => OnAgreementFailed?.Invoke(id);


    // ───────────────────────────────────────────────────────────────────────────
    // FUNCIONÁRIOS
    // Disparado por: WorkerAI — Semana 9
    // ───────────────────────────────────────────────────────────────────────────

    // public static event Action<WorkerType> OnWorkerHired;
    // public static void RaiseWorkerHired(WorkerType type) => OnWorkerHired?.Invoke(type);

    // public static event Action<WorkerType> OnWorkerFired;
    // public static void RaiseWorkerFired(WorkerType type) => OnWorkerFired?.Invoke(type);

    // public static event Action<WorkerType> OnWorkerUnpaid;
    // public static void RaiseWorkerUnpaid(WorkerType type) => OnWorkerUnpaid?.Invoke(type);


    // ───────────────────────────────────────────────────────────────────────────
    // NARRATIVA
    // Disparado por: NarrativeManager — Semana 10
    // ───────────────────────────────────────────────────────────────────────────

    // public static event Action<string> OnNarrativeTrigger;
    // public static void RaiseNarrativeTrigger(string id) => OnNarrativeTrigger?.Invoke(id);


    // ───────────────────────────────────────────────────────────────────────────
    // COMBATE
    // Disparado por: PlayerCombat, EnemyHealth — Semana 10
    // ───────────────────────────────────────────────────────────────────────────

    // public static event Action OnPlayerDied;
    // public static void RaisePlayerDied() => OnPlayerDied?.Invoke();

    // public static event Action<float> OnPlayerDamageTaken;
    // public static void RaisePlayerDamageTaken(float dmg) => OnPlayerDamageTaken?.Invoke(dmg);


    // ───────────────────────────────────────────────────────────────────────────
    // CLIMA E ESTAÇÕES — pós-MVP
    // Disparado por: WeatherSystem (futuro)
    // Cada mês = 1 estação: Mês 1 Primavera | 2 Verão | 3 Outono | 4 Inverno
    // Clima afeta: velocidade de coleta, fluxo de clientes, eficácia de buffs de comida
    // ───────────────────────────────────────────────────────────────────────────

    // public static event Action<Season> OnSeasonChanged;
    // public static void RaiseSeasonChanged(Season s) => OnSeasonChanged?.Invoke(s);

    // public static event Action<WeatherType> OnWeatherChanged;
    // public static void RaiseWeatherChanged(WeatherType w) => OnWeatherChanged?.Invoke(w);
}
