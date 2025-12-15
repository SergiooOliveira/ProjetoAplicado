using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChatGPTIntegration : MonoBehaviour
{
    #region Variables / References

    private PlayerData playerData;

    [Header("UI")]
    public GameObject chatCanvas;
    public TMP_Text npcText;
    public TMP_InputField playerInput;
    public Button sendButton;
    public Button nextButton;
    public TMP_Text counterText;

    private int currentQuestionIndex = 0;
    private string lastPlayerMessage = "";
    private string[] questions;
    private string[] playerAnswers;
    private string finalDecisionValue = null;
    private bool rewardGiven = false;

    private string apiKey = "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";
    private string apiUrl = "https://openrouter.ai/api/v1/chat/completions"; // URL da API

    #endregion

    #region Classes

    [System.Serializable]
    public class Message { public string role; public string content; }

    [System.Serializable]
    public class RequestBody { public string model; public Message[] messages; }

    [System.Serializable]
    public class Choice { public Message message; }

    [System.Serializable]
    public class APIResponse { public Choice[] choices; }

    #endregion

    #region Initialize

    public void StartDialogue(PlayerData player)
    {
        sendButton.onClick.AddListener(OnPlayerSend);
        nextButton.onClick.AddListener(OnNextClicked);
        nextButton.gameObject.SetActive(false);

        // Inicializa perguntas
        questions = new string[]
        {
            "Um viajante faminto rouba pão para alimentar a sua família. Entregaria-o às autoridades ou ajudaria a escondê-lo?",
            "Se alguém que conhecesse estivesse em perigo, mas ajudar significasse abandonar a sua própria busca, o que faria?",
            "Quantas vezes já se aventurou por terras desconhecidas como estas?",
            "Se eu lhe emprestasse algo valioso, daria a sua palavra de que o devolveria?"
        };
        playerAnswers = new string[questions.Length];

        playerData = player;
        currentQuestionIndex = 0;
        ShowQuestion(currentQuestionIndex);
    }

    private void ShowQuestion(int index)
    {
        npcText.text = questions[index];
        playerInput.interactable = true;
        sendButton.interactable = true;
        nextButton.gameObject.SetActive(false);
        UpdateCounter();
    }

    private void UpdateCounter()
    {
        counterText.text = $"{currentQuestionIndex + 1} / {questions.Length}";
    }

    #endregion

    #region AI Response

    private void OnPlayerSend()
    {
        if (string.IsNullOrEmpty(playerInput.text))
            return;

        lastPlayerMessage = playerInput.text;
        playerInput.text = "";
        playerInput.interactable = false;
        sendButton.interactable = false;

        playerAnswers[currentQuestionIndex] = lastPlayerMessage;
        Debug.Log($"📤 Player respondeu: {lastPlayerMessage}");

        string prompt = $"Você é um NPC neutro. Pergunta do NPC: \"{questions[currentQuestionIndex]}\". Resposta do jogador: \"{lastPlayerMessage}\". Responda de forma neutra, curta, como o NPC responderia.";

        StartCoroutine(SendToAI(prompt, aiResponse =>
        {
            npcText.text = aiResponse;
            Debug.Log($"🗨 NPC respondeu: {aiResponse}");
            nextButton.gameObject.SetActive(true);
        }));
    }

    #endregion

    #region Next Action

    private void OnNextClicked()
    {
        nextButton.gameObject.SetActive(false);

        // Se estamos na fase da decisão final
        if (finalDecisionValue != null)
        {
            if (!rewardGiven) // Primeira vez que clica Next → dá recompensa
            {
                if (finalDecisionValue == "1" && playerData != null)
                {
                    GiveRewardsToPlayer();
                    npcText.text = "Toma aqui a tua recompensa 500 Moedas!";
                }
                else
                {
                    npcText.text = "O NPC não vai ajudar.";
                }

                rewardGiven = true;       // Marca que a recompensa já foi entregue
                nextButton.gameObject.SetActive(true); // Mostra Next novamente para fechar
                return;
            }
            else // Segunda vez que clica Next → fecha Canvas
            {
                finalDecisionValue = null;
                rewardGiven = false;
                chatCanvas.SetActive(false); // Fecha Canvas
                return;
            }
        }

        // Continua com perguntas
        currentQuestionIndex++;
        if (currentQuestionIndex < questions.Length)
        {
            ShowQuestion(currentQuestionIndex);
        }
        else
        {
            // Última pergunta -> decisão final
            StartCoroutine(FinalDecision());
        }
    }

    #endregion

    #region Final Decision

    private IEnumerator FinalDecision()
    {
        string finalPrompt = "Com base nas respostas do jogador, decida se o NPC deve ajudá-lo. Responda apenas '0' (não ajuda) ou '1' (ajuda).\n";
        for (int i = 0; i < questions.Length; i++)
            finalPrompt += $"Pergunta: {questions[i]} | Resposta: {playerAnswers[i]}\n";

        yield return SendToAI(finalPrompt, finalDecision =>
        {
            finalDecisionValue = finalDecision.Trim(); // Guardamos a decisão

            Debug.Log("🧾 Decisão final da IA: " + finalDecisionValue);
            npcText.text = finalDecisionValue == "1" ? "Acho que posso confiar em ti. Vou ajudar-te." : "Lamento, mas não posso ajudar-te.";

            sendButton.interactable = false;
            nextButton.gameObject.SetActive(true); // Next agora vai dar recompensa
            playerInput.interactable = false;
        });
    }

    #endregion

    #region Reward If AI Help Player

    private void GiveRewardsToPlayer()
    {
        playerData.AddGold(500);

        Debug.Log("🏆 Player recebeu recompensas!");
    }

    #endregion

    #region AI HTTP Request

    public IEnumerator SendToAI(string prompt, System.Action<string> callback)
    {
        RequestBody body = new RequestBody
        {
            model = "nex-agi/deepseek-v3.1-nex-n1:free",
            messages = new Message[] { new Message { role = "user", content = prompt } }
        };

        string json = JsonUtility.ToJson(body);
        byte[] data = Encoding.UTF8.GetBytes(json);

        UnityWebRequest req = new UnityWebRequest(apiUrl, "POST");
        req.uploadHandler = new UploadHandlerRaw(data);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ ERRO NA API: " + req.error);
            Debug.LogError("❌ Resposta bruta da API: " + req.downloadHandler.text);
            callback("ERRO: " + req.error);
            yield break;
        }

        APIResponse response = JsonUtility.FromJson<APIResponse>(req.downloadHandler.text);
        if (response.choices == null || response.choices.Length == 0)
        {
            Debug.LogError("❌ Resposta da API inválida!");
            callback("ERRO: Resposta inválida.");
            yield break;
        }

        string aiMsg = response.choices[0].message.content;
        callback(aiMsg);
    }

    #endregion
}