using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;
using TMPro;

public class ChatGPTIntegration : MonoBehaviour
{
    public TMP_Text questionText;
    public TMP_InputField playerInputField;  
    public Button sendButton;

    private string apiKey = "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";  // Substitua pela sua chave de API da OpenAI
    private string apiUrl = "https://openrouter.ai/api/v1/chat/completions"; // URL da API

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class RequestBody
    {
        public string model;
        public Message[] messages;
    }

    public void test()
    {
        Debug.Log("test");
    }

    // Função para enviar uma mensagem ao ChatGPT e obter a resposta
    public IEnumerator SendMessageToChatGPT(string prompt, System.Action<string> callback)
    {
        // Criar o corpo da requisição (RequestBody)
        RequestBody requestBody = new RequestBody
        {
            model = "nex-agi/deepseek-v3.1-nex-n1:free",  // Modelo que você está utilizando
            messages = new Message[]
            {
                //new Message { role = "system", content = "Você é um NPC em um jogo." },
                new Message { role = "user", content = prompt }
            }
        };

        // Converter o objeto para JSON
        string jsonString = JsonUtility.ToJson(requestBody);
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonString);

        // Criar a requisição HTTP
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        
        // Definir o conteúdo da requisição como JSON
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        // Enviar a requisição
        yield return request.SendWebRequest();

        // Verificar a resposta
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Resposta do ChatGPT: " + responseText);

            // Deserializar a resposta JSON e pegar o conteúdo da resposta
            ChatGPTResponse responseObject = JsonUtility.FromJson<ChatGPTResponse>(responseText);
            string response = responseObject.choices[0].message.content;

            // Passar a resposta para o callback
            callback(response);
        }
        else
        {
            Debug.LogError("Erro: " + request.error);
            Debug.LogError("Resposta do servidor: " + request.downloadHandler.text);
            callback("Erro ao obter resposta.");
        }
    }

    // Classe para deserializar a resposta do ChatGPT
    [System.Serializable]
    public class ChatGPTChoice
    {
        public Message message;
    }

    [System.Serializable]
    public class ChatGPTResponse
    {
        public ChatGPTChoice[] choices;
    }

        // Corrotina para gerenciar as perguntas ao ChatGPT
    private IEnumerator StartInteraction()
    {
        string[] questions = {
            "Um viajante faminto rouba pão para alimentar sua família. Você o entregaria às autoridades ou ajudaria a escondê-lo?",
            "Se alguém que você conhece estivesse em perigo, mas ajudar significasse abandonar sua própria busca, o que você faria?",
            "Quantas vezes você já se aventurou por terras desconhecidas como estas?",
            "Se eu te emprestasse algo valioso, você me daria sua palavra de que devolveria?"
        };

        // Para armazenar as respostas do jogador
        string[] playerAnswers = new string[questions.Length];
        

        for (int i = 0; i < questions.Length; i++)
        {
            // Pergunta ao jogador (aqui podemos enviar as perguntas ao ChatGPT para gerar um cenário mais dinâmico)
            Debug.Log($"Perguntando: {questions[i]}");
            yield return StartCoroutine(AskQuestionToPlayer(questions[i], i, playerAnswers));
        }

        // Após todas as perguntas, decide se o NPC vai ajudar ou não com base nas respostas
        Debug.Log("Decidindo se o NPC vai ajudar ou não...");
        DecideHelp(questions, playerAnswers);
    }

    // Função para fazer a pergunta ao ChatGPT e obter a resposta
    private IEnumerator AskQuestionToPlayer(string question, int index, string[] playerAnswers)
    {
        // Exibe a pergunta para o jogador no TextMeshPro
        questionText.text = question;

        // Mostra a pergunta no console também
        Debug.Log("Pergunta: " + question);

        // Aguarda o botão de envio ser clicado
        yield return new WaitUntil(() => sendButton.onClick.GetPersistentEventCount() > 0);

        // Pega a resposta do jogador a partir do InputField
        string playerResponse = playerInputField.text;
        playerAnswers[index] = playerResponse;

        // Mostra a resposta no console e na tela
        Debug.Log($"Resposta do jogador para a pergunta '{question}': {playerResponse}");

        // Limpa o campo de texto
        playerInputField.text = "";

        // Aqui você pode adicionar uma pausa ou animação, se necessário
        yield return null;
    }


    // Função para retornar uma resposta simulada manualmente
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

    // Função para decidir se o NPC vai ajudar baseado nas respostas do jogador
    private void DecideHelp(string[]questions, string[] playerAnswers)
    {
        Debug.Log("Decidindo se o NPC vai ajudar...");

        // Constrói o prompt com as perguntas e respostas
        string prompt = "";
        

        for (int i = 0; i < questions.Length; i++)
        {
            prompt += $"Pergunta {i + 1}- {questions[i]} Resposta {i + 1}- {playerAnswers[i]}. ";
        }

        // Adiciona o final do prompt com a instrução para o ChatGPT
        prompt += "Decide se devo ou nao ajudar este jogador. A tua resposta deve ser unicamente 0- se não ajudas. 1- se ajudas";

        // Envia o prompt para o ChatGPT e espera a resposta
        StartCoroutine(SendMessageToChatGPT(prompt, response =>
        {
            // Aqui, a resposta do ChatGPT é recebida. O ChatGPT deve retornar '0' ou '1'.
            if (response == "1")
            {
                Debug.Log("NPC vai ajudar o jogador!");
            }
            else
            {
                Debug.Log("NPC não vai ajudar o jogador.");
            }
        }));
    }

    // Exemplo de como usar a função para enviar o prompt "Olá" e obter uma resposta
    // private void Start()
    // {
    //     // Envia o prompt "Olá" para o ChatGPT
    //     StartCoroutine(SendMessageToChatGPT("Um viajante faminto rouba pão para alimentar sua família. Você o entregaria às autoridades ou ajudaria a escondê-lo?", (response) =>
    //     {
    //         Debug.Log("Resposta recebida: " + response);
    //     }));
    // }

    public void OnSendButtonClicked()
    {

        Debug.Log("TEste");
        // // Obtém o texto digitado no InputField
        // string playerResponse = playerInputField.text;

        // // Verifica se o jogador digitou algo
        // if (!string.IsNullOrEmpty(playerResponse))
        // {
        //     // Envia a resposta para o ChatGPT
        //     StartCoroutine(SendMessageToChatGPT(playerResponse, (response) =>
        //     {
        //         // Exibe a resposta do ChatGPT na tela
        //         Debug.Log("Resposta da IA: " + response);
                
        //         // Aqui você pode atualizar um TextMeshPro ou qualquer outro componente para mostrar a resposta.
        //         questionText.text = "Resposta da IA: " + response;
        //     }));
            
        //     // Limpa o campo de texto após enviar
        //     playerInputField.text = "";
        // }
        // else
        // {
        //     // Se o campo de entrada estiver vazio, você pode mostrar uma mensagem de erro
        //     Debug.Log("Por favor, insira uma resposta antes de enviar.");
        // }
    }
}
