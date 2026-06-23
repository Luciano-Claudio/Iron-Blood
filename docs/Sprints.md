# Iron & Blood — Plano de Sprints
**20 semanas · 1 desenvolvedor · Unity 6.3 LTS**
**Abordagem:** Escopo completo com margem real para dev solo

---

## REGRA GERAL DE COMPORTAMENTO

Se terminar antes da semana acabar, a ordem é sempre:
1. **Avisar o gerente de projeto** antes de avançar
2. **Polir o que foi feito** — testar edge cases, corrigir bugs, melhorar feedback visual
3. **Avançar sozinho** — nunca sem antes passar pelos dois pontos acima

---

## FASE 1 — FUNDAÇÃO (Semanas 1–4)
*Base técnica do jogo. Sem essa fase sólida, tudo que vem depois desmorona.*

---

**Semana 1 — Setup e movimentação do jogador**
`PRIORITÁRIA — SEM ATRASOS`

Configure o projeto Unity 6.3 LTS com URP, Input System, Cinemachine e Pixel Perfect Camera. Implemente o player se movendo em top-down com WASD. Configure o sistema de iluminação dinâmica conectado ao GameClock — você já fez isso antes, então deve ser rápido. Ao final, o player deve se mover, a câmera deve seguir e o ciclo dia/noite deve avançar visivelmente.

> **Se terminar antes:** Não avance. Estresse o ciclo de tempo — teste dormir, acordar, verificar se os eventos de hora/dia/semana disparam corretamente. Esse sistema vai ser inscrito por dezenas de outros scripts.

---

**Semana 2 — Layout da ferraria e sistema de inventário**
`PRIORITÁRIA — SEM ATRASOS`

Construa o layout inicial da ferraria com Tilemaps. Implemente o inventário pessoal do jogador e os containers (baús e prateleiras) com a distinção `accessibleToClients`. Implemente o sistema de Drag & Drop entre inventário pessoal e containers. Ao final, o jogador deve conseguir pegar um item do baú, arrastar para a prateleira e ver a diferença entre os dois tipos de container.

> **Se terminar antes:** Avise-me. Quero revisar a arquitetura do ContainerInventory antes de seguir — ela vai ser usada pela IA dos clientes na Semana 5.

---

**Semana 3 — GameClock completo e placa da loja**
`PRIORITÁRIA — SEM ATRASOS`

Implemente o GameClock com todos os eventos (OnTimeChanged, OnNewDay, OnNewWeek). Implemente o sistema de dormir obrigatório às 21h com fade de tela. Implemente a placa da porta (ShopSign) com abertura e fechamento manual. A loja deve começar fechada todo dia. Ao final, o ciclo completo deve funcionar: acorda às 6h → trabalha → fecha às 21h → dorme → novo dia.

> **Se terminar antes:** Polir o feedback visual e sonoro da placa — é o objeto mais tocado do jogo inteiro. O som de sino ao abrir e a tranca ao fechar precisam ser satisfatórios.

---

**Semana 4 — ItemSO, ScriptableObjects e economia base**
`PRIORITÁRIA — COM MARGEM PEQUENA`

Crie toda a estrutura de ScriptableObjects do jogo: ItemSO, FoodItemSO, CraftingRecipeSO, CookingRecipeSO. Crie os itens do MVP no banco de dados (madeira, ferro, couro, espadas, comidas). Implemente o EconomyManager com AddGold, TrySpend e HUD de moedas. Ao final, todos os itens do MVP devem existir como assets e o dinheiro deve aparecer na HUD.

> **Se terminar antes:** Revise cada ItemSO criado — nome, descrição, ícone placeholder, valor base. Dados errados aqui viram bugs difíceis de rastrear nas semanas 6 e 7.

---

## FASE 2 — LOOP PRINCIPAL DA LOJA (Semanas 5–8)
*O coração do jogo. Quando essa fase terminar, já deve ser possível abrir a loja e vender itens.*

---

**Semana 5 — Mini-game de craft**
`ALTA PRIORIDADE — RISCO TÉCNICO MÉDIO`

Implemente o mini-game de fabricação estilo Jacksmith completo: abertura da tela, spawn das peças, drag & drop das peças para as silhuetas, cálculo de precisão por peça, nota final S→D e multiplicador de valor. O tempo deve pausar durante o mini-game. Ao final, deve ser possível craftar uma Espada Curta com nota variando conforme a precisão.

> **Se terminar antes:** Playtest obsessivo do mini-game. Ajuste o tamanho das zonas de encaixe — muito grande fica trivial, muito pequeno fica frustrante. Esse é o sistema mais jogado do game, precisa estar com o feeling certo antes de seguir.

---

**Semana 6 — IA dos clientes**
`ALTA PRIORIDADE — RISCO TÉCNICO ALTO`

Implemente o CustomerAI completo: entrada na loja, navegação pelas prateleiras, busca por item exato e alternativas, compra no balcão, pagamento e saída. Implemente o CustomerSpawner conectado ao ShopSign e ao GameClock. Implemente o ReputationManager. Ao final, abrir a loja deve gerar clientes que entram, vasculham prateleiras, compram e saem sozinhos.

> **Se terminar antes:** Avise-me antes de qualquer coisa. A IA de clientes é o sistema mais complexo do MVP — quero revisar o comportamento com você antes de seguir para a integração com crafting.

---

**Semana 7 — Integração craft + loja + economia**
`ALTA PRIORIDADE`

Integre os sistemas das semanas 5 e 6: craftar item → colocar na prateleira → cliente compra → dinheiro entra → comprar material → craftar de novo. Implemente o fluxo completo sem quebras. Adicione feedback visual de venda (número flutuante de moedas, reação do cliente). Ao final, o loop básico da loja deve ser jogável de ponta a ponta.

> **Se terminar antes:** Jogue 30 minutos seguidos. Anote tudo que incomoda — ritmo, feedback, clareza. Corrija o que conseguir antes da próxima semana.

---

**Semana 8 — Tutorial com Marta (Dia 1)**
`PRIORIDADE MÉDIA — COM MARGEM`

Implemente o TutorialManager com todos os steps do Dia 1. Escreva e implemente os diálogos de Marta com o sistema de balões não-bloqueantes. Implemente o tour pelo feudo com a câmera acompanhando. Garanta que o vendedor Bram aparece forçado no Dia 1. Ao final, um jogador novo deve conseguir aprender o jogo inteiro só seguindo o Dia 1.

> **Se terminar antes:** Dê o jogo para alguém que nunca viu jogar sem você explicar nada. Observe onde trava. Corrija o tutorial baseado nisso.

---

## FASE 3 — MUNDO E COLETA (Semanas 9–11)
*Expande o jogo para fora da ferraria. Dá ao jogador razões para sair.*

---

**Semana 9 — Floresta: coleta de madeira e caça**
`PRIORIDADE MÉDIA`

Construa o tilemap da floresta. Implemente os nós de árvore (TreeNode) com timer de respawn. Implemente os animais caçáveis (Veado, Coelho, Javali, Raposa) com drops de couro e carne. Implemente Burl (lenhador da cidade) com estoque diário limitado. Implemente Vorn (curtidor) com pagamento por refinamento. Ao final, o ciclo madeira/couro deve ser completo — coletar, refinar, usar no craft.

> **Se terminar antes:** Teste a decisão de comprar vs coletar — o preço do Burl deve tornar a coleta vantajosa mas não obrigatória. Ajuste os valores se necessário.

---

**Semana 10 — Mina: andares e combate simples**
`PRIORIDADE MÉDIA — RISCO TÉCNICO BAIXO`

Construa os tilemaps dos andares 1 e 2 da mina. Implemente os nós de minério (MineralNode) com respawn em dias. Implemente o combate simples — arco de espada em 180° ao pressionar M1, sem combo, sem habilidades. Implemente EnemyAI simples (persegue e ataca). Implemente a penalidade de morte na mina (perde itens coletados, volta à ferraria). Ao final, entrar na mina deve ser arriscado e recompensador.

> **Se terminar antes:** Polir o gamefeel do combate — freeze frame leve no impacto, screenshake sutil. Mesmo sendo simples, precisa ser satisfatório.

---

**Semana 11 — Riacho: pesca e Old Finn**
`PRIORIDADE BAIXA — SISTEMA SECUNDÁRIO`

Construa o tilemap do riacho. Implemente a mecânica de pesca (lançar → aguardar → timing do segundo F). Implemente a tabela de drops com probabilidades. Implemente Old Finn com estoque diário de peixe e sistema de histórias rotativas. Ao final, pescar deve ser funcional e relaxante — sem pressão, sem risco.

> **Se terminar antes:** Esta semana tem margem real. Se sobrar tempo, comece a preparar os tilemaps dos andares 3 e 4 da mina para a fase seguinte.

---

## FASE 4 — SISTEMAS SOCIAIS E ECONÔMICOS (Semanas 12–14)
*Dá profundidade ao loop — razões para cultivar relacionamentos e pensar no longo prazo.*

---

**Semana 12 — NPCs, diálogos e relacionamentos**
`PRIORIDADE ALTA`

Implemente o sistema de relacionamentos com os 5 níveis (Desconhecido → Amigo). Implemente o DialogueManager com sistema de balões. Escreva e implemente os diálogos de Marta, Ser Edwyn, Thresh e Old Finn para os níveis 1 e 2 de relacionamento. Ao final, conversar com NPCs deve avançar o relacionamento e desbloquear novas falas progressivamente.

> **Se terminar antes:** Revise os textos dos diálogos — tom, personalidade, consistência com o lore. Diálogos fracos quebram a imersão mais do que qualquer bug visual.

---

**Semana 13 — Sistema de acordos**
`PRIORIDADE ALTA`

Implemente o AgreementSystem completo: proposta de acordo, aceitação, prazo, baú de entregas, conclusão e pagamento. Implemente os acordos de Ser Edwyn (espadas) e Thresh (troca por cristais). Implemente o impacto da nota das armas entregues na satisfação do acordo. Ao final, fechar um acordo com Ser Edwyn e entregar o lote deve funcionar do início ao fim.

> **Se terminar antes:** Teste o que acontece quando o jogador aceita um acordo mas não consegue entregar no prazo. Defina e implemente essa penalidade — relacionamento cai, sem punição catastrófica.

---

**Semana 14 — Vendedor semanal, funcionários e automação**
`PRIORIDADE MÉDIA`

Implemente o WeeklyVendor (Bram) com estoque gerado por seed semanal. Garanta que Bram aparece no Dia 1 pelo tutorial e semanalmente depois. Implemente os três funcionários (Lenhador, Aprendiz, Carregador) com pagamento semanal automático e comportamento idle quando não pagos. Implemente a segunda bancada desbloqueável para o Aprendiz usar. Ao final, contratar um Lenhador deve reduzir visivelmente a necessidade de ir à floresta.

> **Se terminar antes:** Teste o fluxo de caixa semanal com funcionários — o jogo deve criar pressão econômica real sem ser injusto. Ajuste salários se necessário.

---

## FASE 5 — COMIDA, EXPANSÃO E NARRATIVA (Semanas 15–18)
*Camadas finais de profundidade. A história ganha corpo.*

---

**Semana 15 — Sistema de comida, cozinha e buffs**
`PRIORIDADE MÉDIA`

Implemente o FoodSystem com necessidade diária e penalidade por não comer. Implemente o fogão (Kitchen) com receitas, timer de preparo e resultado automático. Implemente todos os buffs da tabela do GDD conectados ao StatModifierManager. Implemente a Taverna com estoque de comidas compráveis. Ao final, comer antes de dormir deve ser um hábito natural do jogador, e visitar a taverna para buffs deve ser uma decisão válida.

> **Se terminar antes:** Verifique se os buffs são perceptíveis — +25% de velocidade precisa ser notável para valer o custo. Ajuste valores se o jogador não sentir diferença.

---

**Semana 16 — Expansões da ferraria**
`PRIORIDADE MÉDIA`

Implemente o sistema de expansões com verificação de recursos e moedas. Implemente as 4 expansões do MVP (Segunda Bancada, Depósito, Vitrine Externa, Forja Avançada). Construa os tilemaps dos cômodos expandidos. Implemente as receitas de nível 2 desbloqueadas pela Forja Avançada. Ao final, comprar uma expansão deve mudar visualmente a ferraria e abrir novas possibilidades reais de jogo.

> **Se terminar antes:** Jogue desde o início simulando uma run completa até a Forja Avançada. Verifique se o ritmo de progressão está satisfatório — nem rápido demais, nem lento demais.

---

**Semana 17 — Narrativa: Atos 1 e 2**
`PRIORIDADE ALTA — NÃO PODE ATRASAR`

Implemente o NarrativeManager com todos os gatilhos do GDD. Implemente os eventos do Ato 1 (a carta, o barulho noturno) e do Ato 2 (o mapa de Thresh, a porta dos fundos, a primeira aparição da Sombra). Escreva todos os textos narrativos — diário da carta, textos ambientais da ferraria. Ao final, jogar naturalmente deve revelar os dois primeiros atos sem que o jogador precise buscar a história.

> **Se terminar antes:** Leia todos os textos narrativos em voz alta. Textos que soam estranhos em voz alta soam estranhos para o jogador. Revise ritmo e tom.

---

**Semana 18 — Narrativa: Ato 3, cômodo secreto e dois finais**
`PRIORIDADE ALTA — NÃO PODE ATRASAR`

Implemente a Chave Mestra como receita narrativa especial. Construa o cômodo secreto com tilemap e objetos de lore. Implemente o diário do pai com múltiplas páginas. Implemente a tela de escolha final e os dois finais com cenas de encerramento distintas. Implemente os créditos. Ao final, o jogo deve ter começo, meio e fim funcionando de ponta a ponta.

> **Se terminar antes:** Jogue os dois finais completos. Verifique se o peso emocional da escolha está claro. Se a decisão não parecer difícil, a narrativa não fez seu trabalho.

---

## FASE 6 — POLISH E BUILD (Semanas 19–20)
*Semanas inegociáveis. Zero features novas.*

---

**Semana 19 — Polish visual, áudio e balanceamento**
`ZERO FEATURES NOVAS`

Integre todos os SFX e músicas. Substitua todos os sprites placeholder pelos assets finais comprados. Ajuste valores de economia (preços, salários, custos de expansão) baseado em playtests completos. Ajuste timing do mini-game de craft. Corrija todos os bugs identificados nas fases anteriores. Polir feedback visual de vendas, craft e interações com NPCs.

> **Se terminar antes:** Playtest completo do jogo do zero ao fim. Cronometre quanto tempo leva para atingir cada marco (primeira venda, primeiro acordo, primeiro funcionário, cômodo secreto). Ajuste se alguma fase estiver muito longa ou muito curta.

---

**Semana 20 — Build final e publicação**
`SEMANA DE ENTREGA`

Gere a build Windows sem erros. Teste a build fora do editor em pelo menos dois computadores diferentes. Crie a página no itch.io com descrição, screenshots e o GDD como documento público. Publique a demo. Envie para o Gamalyzr para nova análise de viabilidade com o produto real em mãos.

> **Após publicar:** Jogue com alguém que nunca viu o jogo. Observe sem falar nada. O que você ver nessa sessão vai definir a primeira lista de correções pós-lançamento.

---

## RESUMO DAS FASES

| Fase | Semanas | Entregável da fase |
|---|---|---|
| 1 — Fundação | 1–4 | Player, inventário, ciclo de tempo, placa da loja |
| 2 — Loop da Loja | 5–8 | Craft + clientes + economia + tutorial funcionando |
| 3 — Mundo e Coleta | 9–11 | Floresta, mina e riacho jogáveis |
| 4 — Social e Econômico | 12–14 | Relacionamentos, acordos, funcionários |
| 5 — Comida e Narrativa | 15–18 | Comida, expansões, história completa com 2 finais |
| 6 — Polish e Build | 19–20 | Jogo publicado no itch.io |

---

*Iron & Blood · Plano de Sprints v1.0 · 20 semanas · Dev solo*