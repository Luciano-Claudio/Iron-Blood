// Interface implementada por qualquer objeto com o qual o jogador pode interagir.
// Exemplos: cama, porta, baú, prateleira, NPC, bancada de craft.
public interface IInteractable
{
    string InteractionLabel { get; } // texto exibido no prompt — ex: "Entrar", "Dormir", "Abrir"
    void Interact();
}
