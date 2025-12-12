# 🌌 Projeto Aplicado — Jogo de Magia  

Um **2D Platformer** onde o jogador assume o papel de um aprendiz de mago, explorando mundos misteriosos, enfrentando inimigos e desbloqueando feitiços poderosos.  
O objetivo? Aprender a dominar o **grimório proibido**, escrito pelo primeiro mago que ousou roubar o fogo das estrelas.  

---

## 🎮 Plataforma & Público-Alvo
- **Plataforma:** PC  
- **Público-alvo:** 14–30 anos  

---

## 📖 História
O grimório sagrado, escrito pelo primeiro mago, foi roubado  por um **mago mal intencionado** e está a espalhar o terror por todo o mundo. O objetivo do nosso herói? Derrotar esse mago e recuperar o grimório para o devolver onde deveria estar.

O nosso herói leva debuffs com o passar do tempo que vão acumulando. Existe maneira de limpar tais debuffs?
O jogador pode recuperar fragmentos de alma ao fazer craft a poções. Para além dessa poção o jogador pode fazer poções de vida, ataque e defesa.

O jogador progride por **diversos mundos interligados** (à la *Mario*), desbloqueando feitiços que permitem acesso a novas áreas.  
Cada mundo terá um boss com uma afinidade especifica. Cada afinidade tem uma fraqueza e um imunidade.

Fogo -> Vento -> Água -> Fogo

Fogo é imune a vento e leva mais dano de água. (Podemos também ver outras afinidades como Light and Dark que não têm imunidade uma com a outra mas dão dano neutro)
<img width="1152" height="648" alt="Imunidades" src="https://github.com/user-attachments/assets/07f9c0f5-7d87-4828-ad24-8f7d63b23a8a" />

---

## 🧙‍♂️ Mecânicas Principais
- **Skill Trees:**  
  - 🌟 Evolução de **feitiços** (ataque, defesa, utilidade)  
  - 💪 Progressão de **atributos do jogador**  

- **Varinhas:**  
  - Diferentes tipos, cada uma com **afinidades mágicas**  
  - Influenciam o estilo de combate e estratégia  

- **Inimigos:**  
  - Resistências específicas a certos elementos  
  - Desafiam o jogador a variar magias e táticas  

- **Equipamentos:**  
  - Itens que adicionam **atributos base**  
  - Bônus em magias específicas ou resistências  

---

## ✨ Exemplos de Feitiços

### 🔥 Ataque
- **Fireball**  
- **Raio**  

### 🛡 Defesa
- **Shield**  

### 🌀 Exploração
- **Levitação** (atravessar obstáculos)  
- **Boulder** (quebrar barreiras)  
- **Respiração Aquática** (explorar áreas submersas)  

---

## 🖼 Estilo & Visual
- **2D Platformer** com movimentação horizontal e vertical  
- Animações fluidas, foco em HUD clara e responsiva  
- Ambientes variados e mágicos, inspirados em mundos fantásticos  

---

## 🚀 Objetivo Final
Subir no **sistema de ranks** até se tornar o **Mago Supremo**, dominando o grimório proibido e decidindo o destino do mundo.  

---

## 🤖 Técnicas de Inteligência Artificial

### ✨ Introdução
O jogo possui 3 técnicas de inteligência artificial, que são:
- **A***
- **Máquina de Estados**
- **Integração de API**

### ❓ Motivo
O porquê, de usar estas três mencionadas e não outras?<br>
O **A*** vai ser usado em conjunto com **Máquina de Estados**, que está implementado no inimigo.<br>
E a **API** vai ser usado para interação entre **NPC's**

---

### 💬 Explicação **A***
#### ⭐ O que é o **A***?
O **A*** é um **algoritmo de pathfinding**, ou seja, ele calcula o **melhor caminho** entre um ponto inicial e um ponto destino.<br>
Neste caso usado para os inimigos.

#### 🔍 Como Funciona?
O **A*** tenta encontrar o **caminho mais curto** e **mais barato** entre dois pontos num grid.<br>
Ele faz isto usando algumas informações:
- **G Cost** : O custo real para ir do ponto inicial até a célula atual.<br>
Ex: Distância percorrida.
- **H Cost (heurística)** : Uma estimativa da distância restante até o destino.<br>
A heurística mais usada em 2D é a **distância Manhattan** (|dx| + |dy|).
- **F Cost** : É o valor usado pelo **A*** para decidir onde continuar a procura.<br>
**F = G + H** - Ele sempre escolhe o próximo **nó** com o **menor F**

#### ✔️ Vantagens do **A***
- **Rápido** e eficiente para grids 2D.
- Sempre encontra o **melhor caminho possível**.
- Fácil de personalizar (custos diferentes para terrenos, evitar zonas).

---

### 💬 Explicação **Máquina de Estados**
#### 🧠 O que é uma **Máquina de Estados**?
Uma **máquina de estados finitos** é uma forma organizada de controlar o comportamento de um personagem.<br>
Em vez de misturar tudo num único script, divide-se o comportamento em **estados**, e o inimigo só executa **um estado por vez**.

#### 🔍 Como Funciona?
- **Estados** : Ações ou comportamentos.<br>
**Patrulha** / **Perseguição** / **Atacar**
- **Transições** : Regras que dizem quando trocar de um estado para outro.<br>
Quando o **Jogador** estiver próximo -> muda para **Perseguição**
- **Estado atual** : O que o inimigo está realemente a fazer no momento.

#### 🎮 Como funciona dentro do jogo?
1º **Patrulha**<br>
<br>
O inimigo:
- Cria uma área de patrulha predefinida.
- Aleatoriamente escolhe pontos para se movimentar nessa área.
- Move-se usando **A***.

2º **Perseguição**<br>
<br>
O inimigo:
- Usa **A*** para seguir o **Jogador**.
- Verifica a distância.

3º **Ataque**<br>
<br>
O inimigo:
- Para de usar **A***.
- Executa animação de ataque.
- Causa dano ao **Jogador**.

---

### 💬 Explicação Integração com uma **API IA**
#### 🧠 O que é essa **API IA**?
No projeto estamos a usar um modelo de **IA** gratuito, que é o **DeepSeek**

#### ❓ Motivo
O motivo pela qual estamos a usar **DeepSeek**, é que é gratuito, inicalmente tentamos implementar **GPT**, mas é necessário pagar no mínimo **5€**, para ter créditos para usar.

#### 🤖 O que significa **Integrar uma API de IA** num **NPC**?
Significa que os **NPC's não tem falas fixas**.<br>
Em vez disso, quando o **Jogador** fala com o **NPC**:
- O Unity envia o texto do **Jogador** para **API de Inteligência Artificial**.
- A **IA** gera uma resposta.
- O Unity recebe essa resposta e o **NPC "fala" ao Jogador**.

Ou seja, o **NPC** passa a ser um personagem **dinâmico**, capaz de conversar de forma flexível, não limitada a diálogos pré-programados.

#### 🎮 Como funciona dentro do jogo?
- O **Jogador** interage com o **NPC**.<br>
Pressiona a tecla "E"
- O Unity envia um **pedido HTTP para a API**.<br>
  **Envia**: "Olá, quem es tu?"
- A **API** processa com **IA**.<br>
**Resposta**: "Eu sou o Guardião. Precisas de Ajuda?"
- Unity recebe a resposta.<br>
Texto no ecrã

#### 🧠 Fluxo completo

Jogador -> Unity -> API AI -> Unity -> NPC responde

---

### 🔍 Conclusão das 3 **IAs**
Estamos a usar 3 tipos diferentes de **IA**, e cada uma desempenha um papel essencial e distinto.

#### ⭐ **A*** - Pathfinding
O **A*** é responsável por descobrir como o inimigo se move no mapa.
- Calcula o melhor caminho até ao **Jogador** ou até ao pontos de **Patrulha**.
- Evita paredes, obstáculos e caminhos bloqueados.
- Garante que a movimentação do inimigo é **inteligente e eficiente**.

#### 🧠 **Máquina de Estados**
Define **quando** e **porquê** o inimigo muda de ação.
- **Patrulha** -> anda dentro de uma área.
- **Perseguição** -> usa o **A*** para seguir o **Jogador**.
- **Ataque** -> para de mover e ataca, quando próximo do **Jogador**.
- **Patrulha** -> volta a patrulha quando perde o **Jogador**

#### 🤖 **Integração com API de IA**
A integração com uma **API de IA**, permite interagir com o  **NPC** de forma **dinâmica**.
- O **NPC** entende e responde ao que o jogador diz.
- Gera diálogos naturais e variáveis.
- Cria interação mais viva e realista.

#### 🔗 Como estas três se unem

| Tipo de IA | O que faz | Exemplo |
|----------|----------|----------|
| **A***  | Caminho  | "Por onde devo ir para chegar ao **Jogador**?"  |
| **Máquina de Estados**  | Decisão  | "Devo **Patrulhar**, **Perseguir** ou **Atacar**?"  |
| **API de IA**  | Conversa  | "O que devo dizer ao **Jogador**?"  |

## 🎯 Conclusão Final
Com estas três técnicas de **IA**, cria-se uma nova **experiência e jogabilidade**, pois trazem **dinamismo e variedade**, principalmente na interação entre o **Jogador** e o **NPC**.

---

## Prints / Código

### ⭐ **A*** - Pathfinding
<img width="1919" height="960" alt="image" src="https://github.com/user-attachments/assets/357f860c-6036-48bf-86b5-b9ffa28cdfa5" />

---

### 🧠 **Máquina de Estados**
#### 🧟 Core do Inimigo

Sensor do inimigo, quando o **Jogador** estiver proximo tem realizar algum estado.
```C#
#region Senses

void FindClosestPlayer()
{
    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, sightRange, playerLayer);
    Transform closest = null;
    float closestDist = Mathf.Infinity;
    foreach (var h in hits)
    {
        float d = Vector2.Distance(transform.position, h.transform.position);
        if (d < closestDist)
        {
            closestDist = d;
            closest = h.transform;
        }
    }

    player = closest;
    playerInSightRange = closest != null;
}

#endregion
```
Este código vai estar sempre a correr é o que vai fazer com que o inimigo passe de **Patrulha** -> **Perseguição** -> **Atacar**.
```C#
private IEnumerator AI_Tick()
{
    while (true)
    {
        // If you use server/NetworkBehaviour
        // if (!IsServer) { yield return null; continue; }

        FindClosestPlayer();

        if (movement != null)
        {
            movement.SetPlayerInSight(playerInSightRange);
            movement.SetAttacking(isAttacking);
        }

        if (playerInSightRange && player != null)
        {
            float dist = Vector2.Distance(transform.position, player.position);
            if (dist <= attackRange)
            {
                if (!isAttacking)
                {
                    movement.SetTarget(null);
                    movement.StopMovement();
                    AttackPlayer();
                }
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            Patrolling();
        }

        UpdateAnimator(); // speed, grounded, etc.

        // Tick every 0.1s (10x per second, much lighter than Update)
        yield return new WaitForSeconds(0.1f);
    }
}
```

Que estados o inimigo tem que realizar.
```C#
 #region Movement / High-Level Actions

 public void Patrolling()
 {
     if (isAttacking || playerInSightRange)
         return;

     // Example: mover.MoveTo(waypoint) or idle - keep simple here
     if (movement != null)
         movement.SetTarget(null); // no target -> do patrol inside mover if it supports it
 }

 public void ChasePlayer()
 {
     if (player == null) return;
     if (movement != null)
         movement.SetTarget(player);
 }
 
 #endregion
 ```

#### 🧟 Movimentação do Inimigo
Vai escolher pontos aleatórios dentro da área de **Patrulha** para mover-se até esse ponto.
```C#
private void ChooseNewPatrolPoint()
{
    float randomX = patrolCenter.x + Random.Range(-patrolRange, patrolRange);
    patrolTarget = new Vector2(randomX, patrolCenter.y);
}
```

Quando o **Jogador** se aproximar do inimigo vai atualizar o **caminho**, vai entrar no estado **Perseguição**.
```C#
#region Pathfinding

void UpdatePath()
{
    if (seeker == null || rb == null) return;
    if (target == null)
    {
        // If no target, you could compute path to patrol waypoints or stop requesting paths
        path = null;
        return;
    }

    if (seeker.IsDone())
        seeker.StartPath(rb.position, target.position, OnPathComplete);
}

void OnPathComplete(Path p)
{
    if (!p.error)
    {
        path = p;
        currentWaypoint = 0;
    }
}

#endregion

```

Estado em que o inimigo esta a **Patrulhar**
```C#
#region Patrol

private void HandlePatrol()
{
    if (!initializedPatrol) return;

    float distToTarget = Vector2.Distance(rb.position, patrolTarget);

    if (distToTarget < 0.2f)
    {
        if (!waiting)
            StartCoroutine(WaitAndChooseNewPatrol());
        return;
    }

    Vector2 dir = (patrolTarget - rb.position).normalized;
    TryJumpPatrolObstacle(dir);
    rb.linearVelocity = new Vector2(dir.x * patrolSpeed, rb.linearVelocity.y);

    if (dir.x > 0.1f)
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    else if (dir.x < -0.1f)
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
}

private IEnumerator WaitAndChooseNewPatrol()
{
    waiting = true;
    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    yield return new WaitForSeconds(patrolWaitTime);
    ChooseNewPatrolPoint();
    waiting = false;
}

#endregion
```

Quando o inimigo esta em **Perseguição**, pode encontrar obstáculos pelo caminho, e é necessario fazer com que o inimigo salte.
```C#
#region Jump Logic

private void TryJumpPatrolObstacle(Vector2 dir)
{
    bool grounded = IsGrounded();

    if (!grounded) return;

    // Raycast origin: at foot height
    Vector2 origin = rb.position + Vector2.up * -0.05f;

    // Pure horizontal direction, ignoring y
    Vector2 horizontalDir = new Vector2(Mathf.Sign(dir.x), 0f);

    // Raycast length, adjusts according to obstacle
    float distance = 1f;

    // Draw for debug
    Debug.DrawRay(origin, horizontalDir * distance, Color.red);

    RaycastHit2D hit = Physics2D.Raycast(origin, horizontalDir, distance, groundLayer);

    if (hit.collider != null)
    {
        // Ceiling check, keep it higher than the obstacle
        Vector2 ceilingCheck = rb.position + Vector2.up * 1f;
        Collider2D overlap = Physics2D.OverlapBox(ceilingCheck, new Vector2(0.3f, 0.5f), 0f, groundLayer);

        if (overlap == null)
        {
            Vector2 v = rb.linearVelocity;
            v.y = jumpVelocity;
            rb.linearVelocity = v;
            lastJumpTime = Time.time;
        }
    }
}

#endregion
```

---

### 🤖 **Integração com API de IA**

O cérebro da **API**, este código é responsável por fazer requisições **HTTP a API**.
```C#
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
```

O **Jogador** envia a mensagem, o **NPC** responde, e a **IA** processa a resposta para devolver outra.
```C#
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
```

Se a decisão final for **1(True)** o **Jogador** vai ser recompensado, se for **0(False)** o **Jogador** não irá receber nada.
```C#
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
```
