using FishNet;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public PlayerData playerData;
    
    private PlayerData runTimePlayerData;

    [SerializeField] private GameObject notificationSlotPrefab;
    [SerializeField] private Transform notificationPanel;
    [SerializeField] private Transform spellSpawnPoint;

    private PlayerHUDManager playerHUDManager;
    private SpellManager spellManager;

    private Coroutine manaRegenCoroutine;

    private Dissolve dissolve;
    private bool isDead;

    [SerializeField] private AudioSource effectsSource;
    [SerializeField] private AudioClip deathClip;

    #region Property implementation
    public Transform SpellSpawnPoint => spellSpawnPoint;
    public PlayerHUDManager PlayerHUDManager => playerHUDManager;
    public SpellManager SpellManager => spellManager;
    public bool IsDead => isDead;
    #endregion

    #region Unity Methods
    public void Awake ()
    {
        runTimePlayerData = Instantiate(playerData);
        Initialize();
        playerHUDManager = GetComponentInChildren<PlayerHUDManager>();
        spellManager = GetComponentInChildren<SpellManager>();
        dissolve = GetComponent<Dissolve>();
    }

    public void Start()
    {
        GameManager.Instance.RegisterPlayer(this);
    }

    /// <summary>
    /// Call this method to initialize the player data
    /// </summary>
    public void Initialize()
    {
        runTimePlayerData.CharacterHp.Initialize();
        runTimePlayerData.CharacterMana.Initialize();
        runTimePlayerData.CharacterXp.NewXpMax(GetPlayerLevel());

        // Initialize item list
        foreach (ItemEntry item in runTimePlayerData.CharacterInventory)
        {
            item.item.Initialize();
        }

        // Initialize equipment list

        for (int i = 0; i < runTimePlayerData.CharacterEquipment.Count(); i++)
        {
            EquipmentEntry eq = runTimePlayerData.CharacterEquipment[i];

            eq.equipment.Initialize();

            // Check if its equipped
            if (eq.isEquipped)
            {
                // Check if slot already has something
                // if does unequip
                bool isEquipped = (runTimePlayerData.CharacterEquipedEquipment.Find(e => e.equipment.RunTimeEquipmentData.ItemSlot == eq.equipment.RunTimeEquipmentData.ItemSlot).equipment != null);

                if (isEquipped) runTimePlayerData.UnequipEquipment(eq);
                else runTimePlayerData.EquipEquipment(eq);
            }
        }

        runTimePlayerData.InitializeSpells();
        runTimePlayerData.InitializeEquippedSpells();

        UpdateManaUI();
    }

    /// <summary>
    /// Public getter for run time player data
    /// </summary>
    public PlayerData RunTimePlayerData => runTimePlayerData;

    public int GetPlayerLevel() => runTimePlayerData.CharacterLevel;

    public void DisplayNotification(string name, int amount)
    {
        GameObject newNotificationSlot = Instantiate(notificationSlotPrefab, notificationPanel);

        Transform nameTransform = newNotificationSlot.transform.Find("ItemName");
        Transform amountTransform = newNotificationSlot.transform.Find("ItemAmount");

        TMP_Text tb_name = nameTransform.GetComponent<TMP_Text>();
        TMP_Text tb_amount = amountTransform.GetComponent<TMP_Text>();

        tb_name.text = "+ " + name;
        tb_amount.text = "x" + amount.ToString();

        Destroy(newNotificationSlot, 2f);
    }
    #endregion

    public float GetAffinityBonuses(SpellAffinity spellAffinity)
    {
        float totalBonus = RunTimePlayerData.CharacterEquipedEquipment
            .Where(entry => entry.isEquipped && entry.equipment?.RunTimeEquipmentData?.ItemDamageAffinity != null)
            .SelectMany(entry => entry.equipment.RunTimeEquipmentData.ItemDamageAffinity)
            .Where(ida => ida.SpellAfinity == spellAffinity)
            .Sum(ida => ida.Amount);

        return 1f + (totalBonus / 100f);
    }

    #region Mana Regeneration
    public void UseMana(float amount)
    {
        runTimePlayerData.CharacterMana.ConsumeMana(amount);

        UpdateManaUI();

        if (manaRegenCoroutine != null)
        {
            StopCoroutine(manaRegenCoroutine);
        }

        manaRegenCoroutine = StartCoroutine(RegenManaRoutine());
    }

    private IEnumerator RegenManaRoutine()
    {
        yield return new WaitForSeconds(runTimePlayerData.RegenDelay);

        Stat mana = runTimePlayerData.CharacterMana;

        while (mana.Current < mana.Max)
        {
            mana.IncreaseCurrent((int)runTimePlayerData.RegenAmount);

            UpdateManaUI();

            yield return new WaitForSeconds(runTimePlayerData.RegenTickRate);
        }

        manaRegenCoroutine = null;
    }

    private void UpdateManaUI()
    {
        if (playerHUDManager != null)
        {
            float current = runTimePlayerData.CharacterMana.Current;
            float max = runTimePlayerData.CharacterMana.Max;

            playerHUDManager.SetManaBar(current / max);
        }
    }
    #endregion

    #region Die
    public void OnDamageTaken()
    {
        if (isDead) return;

        if (runTimePlayerData.CharacterHp.Current <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (dissolve != null)
            dissolve.StartDissolve();

        if (effectsSource != null && deathClip != null)
        {
            StartCoroutine(PlayDeathAndReload());
        }
        else
        {
            if (InstanceFinder.IsServerStarted)
                StartCoroutine(RequestMapReload());
        }
    }

    public void Revive()
    {
        isDead = false;

        RunTimePlayerData.CharacterHp.Reset();
        RunTimePlayerData.CharacterMana.Reset();
        RunTimePlayerData.ResetProgression();

        PlayerHUDManager.SetHPBar(1f);
        PlayerHUDManager.SetManaBar(1f);
        PlayerHUDManager.ResetXPBar(0f);

        dissolve?.ResetDissolve();
    }

    private IEnumerator PlayDeathAndReload()
    {
        effectsSource.volume = 1f;
        effectsSource.PlayOneShot(deathClip, 1f);

        yield return new WaitForSeconds(deathClip.length);

        effectsSource.volume = 0.1f;

        if (InstanceFinder.IsServerStarted)
            StartCoroutine(RequestMapReload());
    }

    private IEnumerator RequestMapReload()
    {
        yield return new WaitForSeconds(0.8f);

        BootstrapSceneManager bootstrap = FindFirstObjectByType<BootstrapSceneManager>();

        if (bootstrap != null)
            bootstrap.ReloadCurrentMap();
    }
    #endregion
}