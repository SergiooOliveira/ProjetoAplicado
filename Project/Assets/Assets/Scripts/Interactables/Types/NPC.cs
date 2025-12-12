using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{    
    public NPCData npcData;
    private NPCData runtimeData;
   
    public NPCData RuntimeData => runtimeData;

    private GameObject shopPanel;
    private PlayerData playerData;

    public ChatGPTIntegration chatGPT;

    public PlayerData PlayerData => playerData;

    private void Awake()
    {
        runtimeData = Instantiate(npcData);
        Initialize();
    }

    private void Initialize()
    {
        foreach (ItemEntry entry in runtimeData.NPCInventory)
        {
            if (entry.item != null)
                entry.item.Initialize();
        }

        foreach (EquipmentEntry entry in runtimeData.NPCEquipment)
        {
            if (entry.equipment != null)
                entry.equipment.Initialize();
        }

        // Get ItemPanel
        runtimeData.SetPanel(this.transform.Find("Canvas/Menus").gameObject);
    }

    public void Interact(PlayerData playerData)
    {
        this.playerData = playerData;

        bool newState = !runtimeData.NPCItemPanel.activeSelf;
        runtimeData.NPCItemPanel.SetActive(newState);

        // Inicia o processo de interação com perguntas ao ChatGPT
        // StartCoroutine(StartInteraction());
    }

    // // Corrotina para gerenciar as perguntas ao ChatGPT
    // private IEnumerator StartInteraction()
    // {
    //     string[] questions = {
    //         "Um viajante faminto rouba pão para alimentar sua família. Você o entregaria às autoridades ou ajudaria a escondê-lo?",
    //         "Se alguém que você conhece estivesse em perigo, mas ajudar significasse abandonar sua própria busca, o que você faria?",
    //         "Quantas vezes você já se aventurou por terras desconhecidas como estas?",
    //         "Se eu te emprestasse algo valioso, você me daria sua palavra de que devolveria?"
    //     };

    //     // Para armazenar as respostas do jogador
    //     string[] playerAnswers = new string[questions.Length];
        

    //     for (int i = 0; i < questions.Length; i++)
    //     {
    //         // Pergunta ao jogador (aqui podemos enviar as perguntas ao ChatGPT para gerar um cenário mais dinâmico)
    //         Debug.Log($"Perguntando: {questions[i]}");
    //         yield return StartCoroutine(AskQuestionToPlayer(questions[i], i, playerAnswers));
    //     }

    //     // Após todas as perguntas, decide se o NPC vai ajudar ou não com base nas respostas
    //     Debug.Log("Decidindo se o NPC vai ajudar ou não...");
    //     DecideHelp(questions, playerAnswers);
    // }

    // // Função para fazer a pergunta ao ChatGPT e obter a resposta
    // private IEnumerator AskQuestionToPlayer(string question, int index, string[] playerAnswers)
    // {
    //     // Exibe a pergunta para o jogador
    //     Debug.Log("Pergunta: " + question);
    //     runtimeData.NPCItemPanel.SetActive(true);  // Exemplo de ativar o painel de perguntas

    //     // Simula a resposta do jogador para a pergunta
    //     string response = GetManualResponse(index);  // Função para pegar uma resposta predefinida

    //     // Mostra a resposta simulada (resposta manual)
    //     Debug.Log($"Resposta simulada para a pergunta '{question}': {response}");

    //     // Armazenamos a resposta
    //     playerAnswers[index] = response;

    //     // Fecha o painel de perguntas após responder
    //     runtimeData.NPCItemPanel.SetActive(false);

    //     // Aqui você pode adicionar uma pausa ou animação, se necessário
    //     yield return null;
    // }

    // // Função para retornar uma resposta simulada manualmente
    // private string GetManualResponse(int questionIndex)
    // {
    //     // Respostas predefinidas para as perguntas (você pode alterar conforme necessário)
    //     string[] predefinedResponses = new string[]
    //     {
    //         "Meu nome é NPC",  // Resposta para a primeira pergunta
    //         "Estou buscando aventuras",  // Resposta para a segunda pergunta
    //         "Estou aqui para ajudar os heróis",  // Resposta para a terceira pergunta
    //         "Você pode me ajudar com uma missão?"  // Resposta para a quarta pergunta
    //     };

    //     // Retorna a resposta predefinida com base no índice
    //     return predefinedResponses[questionIndex];
    // }

    // // Função para decidir se o NPC vai ajudar baseado nas respostas do jogador
    // private void DecideHelp(string[]questions, string[] playerAnswers)
    // {
    //     Debug.Log("Decidindo se o NPC vai ajudar...");

    //     // Constrói o prompt com as perguntas e respostas
    //     string prompt = "";
        

    //     for (int i = 0; i < questions.Length; i++)
    //     {
    //         prompt += $"Pergunta {i + 1}- {questions[i]} Resposta {i + 1}- {playerAnswers[i]}. ";
    //     }

    //     // Adiciona o final do prompt com a instrução para o ChatGPT
    //     prompt += "Decide se devo ou nao ajudar este jogador. A tua resposta deve ser unicamente 0- se não ajudas. 1- se ajudas";

    //     // Envia o prompt para o ChatGPT e espera a resposta
    //     StartCoroutine(chatGPT.SendMessageToChatGPT(prompt, response =>
    //     {
    //         // Aqui, a resposta do ChatGPT é recebida. O ChatGPT deve retornar '0' ou '1'.
    //         if (response == "1")
    //         {
    //             Debug.Log("NPC vai ajudar o jogador!");
    //         }
    //         else
    //         {
    //             Debug.Log("NPC não vai ajudar o jogador.");
    //         }
    //     }));
    // }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == GameManager.Instance.playerTag)
        {
            if (runtimeData.NPCItemPanel.activeSelf)
            {
                runtimeData.NPCItemPanel.SetActive(false);
            }
        }
    }
}
