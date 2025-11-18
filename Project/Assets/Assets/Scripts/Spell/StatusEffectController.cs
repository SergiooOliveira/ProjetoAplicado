using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectController : MonoBehaviour
{
    private List<ActiveStatusEffect> activeEffects = new();

    // ---------------- DEBUFFS ----------------
    public void ApplyDebuff(DebuffType type, float value, float duration)
    {
        // Verifica se já existe efeito semelhante ativo
        var existing = activeEffects.Find(e => e.Type.Equals(type));
        if (existing != null)
        {
            // Refresh duração
            existing.Timer = duration;
            return;
        }

        // Cria novo efeito
        ActiveStatusEffect newEffect = new ActiveStatusEffect(type, value, duration);
        activeEffects.Add(newEffect);

        // Aplica imediatamente o efeito
        ApplyDebuffEffect(type, value);

        Debug.Log($"<Color=red>{gameObject.name}</Color><Color=blue> recebeu debuff {type} por {duration}s</color>");
    }

    private void ApplyDebuffEffect(DebuffType type, float value)
    {
        switch (type)
        {
            case DebuffType.Slow:
                // Exemplo:
                // var enemy = GetComponent<Enemy>();
                // if (enemy != null) enemy.ModifySpeed(-value);
                break;
        }
    }

    private void RemoveDebuffEffect(ActiveStatusEffect effect)
    {
        // Só processa se o tipo for um DebuffType
        if (effect.Type is DebuffType)
        {
            switch ((DebuffType)effect.Type)
            {
                case DebuffType.Slow:
                    // Reverte a velocidade
                    // var enemy = GetComponent<Enemy>();
                    // if (enemy != null) enemy.ModifySpeed(effect.Value);
                    break;
            }
        }

        Debug.Log($"{gameObject.name} perdeu debuff {effect.Type}");
    }

    // ---------------- BUFFS ----------------
    public void ApplyBuff(BuffType type, float value, float duration)
    {
        var existing = activeEffects.Find(e => e.Type.Equals(type));
        if (existing != null)
        {
            existing.Timer = duration;
            return;
        }

        ActiveStatusEffect newEffect = new ActiveStatusEffect(type, value, duration);
        activeEffects.Add(newEffect);

        ApplyBuffEffect(type, value);

        Debug.Log($"{gameObject.name} recebeu buff {type} por {duration}s");
    }

    private void ApplyBuffEffect(BuffType type, float value)
    {
        switch (type)
        {
            case BuffType.ArmorBreaker:
                // var player = GetComponent<Player>();
                // if (player != null) player.ModifyDamage(value);
                break;
        }
    }

    public void RemoveBuff(BuffType type)
    {
        ActiveStatusEffect effect = activeEffects.Find(e => e.Type.Equals(type));
        if (effect != null)
        {
            // Reverter
            if (effect.Type is BuffType)
            {
                switch ((BuffType)effect.Type)
                {
                    case BuffType.ArmorBreaker:
                        // GetComponent<Player>()?.ModifyDamage(-effect.Value);
                        break;
                }
            }

            activeEffects.Remove(effect);
            Debug.Log($"{gameObject.name} perdeu buff {type}");
        }
    }

    // ---------------- UPDATE / TICK ----------------
    private void FixedUpdate()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].Timer -= Time.deltaTime;
            if (activeEffects[i].Timer <= 0)
            {
                // Separar remoção de buffs/debuffs para aplicar reverter corretamente
                if (activeEffects[i].Type is DebuffType)
                    RemoveDebuffEffect(activeEffects[i]);
                else if (activeEffects[i].Type is BuffType)
                    RemoveBuff((BuffType)activeEffects[i].Type);

                activeEffects.RemoveAt(i);
            }
        }
    }

    // ---------------- HELPERS (ex.: regen) ----------------
    //private IEnumerator RegenHealth(float rate)
    //{
    //    // Este exemplo começa uma coroutine que cura enquanto o buff existir.
    //    // Nota: para parar a corrotina quando o efeito termina, seríamos mais explícitos
    //    // (ex.: guardar a coroutine em ActiveStatusEffect). Aqui é apenas um exemplo simplificado.
    //    while (true)
    //    {
    //        var player = GetComponent<Player>();
    //        if (player != null)
    //            player.Heal(rate * Time.deltaTime);

    //        yield return null;
    //    }
    //}
}

public class ActiveStatusEffect
{
    // Guardamos o tipo como Enum para suportar tanto BuffType quanto DebuffType
    public Enum Type;
    public float Value;
    public float Timer;

    public ActiveStatusEffect(Enum type, float value, float timer)
    {
        Type = type;
        Value = value;
        Timer = timer;
    }

    // para facilidade, podes adicionar construtores de conveniência:
    public ActiveStatusEffect(DebuffType type, float value, float timer)
    {
        Type = type;
        Value = value;
        Timer = timer;
    }

    public ActiveStatusEffect(BuffType type, float value, float timer)
    {
        Type = type;
        Value = value;
        Timer = timer;
    }
}
