using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SpellChain : MonoBehaviour
{
    [Header("Chain Settings")]
    [SerializeField] private float chainRange = 6f;
    [SerializeField] private float maxBeamDistance = 12f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float tickRate = 0.2f;

    [Header("Visual Settings")]
    [SerializeField] private float textureScrollSpeed = -10f;
    [SerializeField] private float textureTiling = 1f;

    [Header("Charge Setting")]
    [SerializeField] private float timeToMaxCharge = 3f;
    [SerializeField] private float maxMultiplier = 2.0f;

    private LineRenderer lineRenderer;
    private SpellData runtimeSpellData;
    private Player caster;

    private float nextTickTime;
    private float currentChargeTime;

    private Transform currentTarget1;
    private Transform currentTarget2;

    public SpellData RuntimeSpellData => runtimeSpellData;

    public void Initialize(SpellData spell, Player caster)
    {
        this.runtimeSpellData = Instantiate(spell);
        this.runtimeSpellData.Initialize();

        this.caster = caster;
        this.lineRenderer = GetComponent<LineRenderer>();

        // Initial lineRenderer setup
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    private void FixedUpdate()
    {
        if (caster == null) { Destroy(gameObject); return; }

        HandleCharging();

        Vector2 origin = caster.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - origin).normalized;

        UpdateTargets(origin, direction);
        DrawVisuals(origin, direction);
        AnimateTexture();
        HandleTickDamage();
    }

    public void HandleCharging()
    {
        currentChargeTime += Time.fixedDeltaTime;

        float t = Mathf.Clamp01(currentChargeTime / timeToMaxCharge);
        float multiplier = Mathf.Lerp(1f, maxMultiplier, t);

        runtimeSpellData.UpdateDamageMultiplier(multiplier);
    }

    private void UpdateTargets(Vector2 origin, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxBeamDistance, enemyLayer);

        if (hit.collider != null)
        {
            currentTarget1 = hit.transform;

            FindChainTarget(hit.transform.position);
        }
        else
        {
            currentTarget1 = null;
            currentTarget2 = null;
        }
    }

    private void FindChainTarget(Vector2 startPoint)
    {
        Collider2D[] potentialTargets = Physics2D.OverlapCircleAll(startPoint, chainRange, enemyLayer);

        float closestDist = Mathf.Infinity;
        Transform bestCandidate = null;

        foreach (var target in potentialTargets)
        {
            if (target.transform == currentTarget1) continue;

            float dist = Vector2.Distance(startPoint, target.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                bestCandidate = target.transform;
            }
        }

        currentTarget2 = bestCandidate;
    }

    private void DrawVisuals(Vector2 origin, Vector2 direction)
    {
        lineRenderer.SetPosition(0, origin);

        if (currentTarget1 != null)
        {
            lineRenderer.SetPosition(1, currentTarget1.position);

            if (currentTarget2 != null)
            {
                lineRenderer.positionCount = 3;
                lineRenderer.SetPosition(2, currentTarget2.position);
            }
            else
            {
                lineRenderer.positionCount = 2;
            }
        }
        else
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(1, origin + (direction * maxBeamDistance));
        }
    }

    private void HandleTickDamage()
    {
        if (Time.time >= nextTickTime)
        {
            ApplyEffectToTarget(currentTarget1);
            ApplyEffectToTarget(currentTarget2);

            nextTickTime = Time.time + tickRate;
        }
    }

    private void ApplyEffectToTarget(Transform target)
    {
        if (target != null && runtimeSpellData != null)
        {
            Collider2D col = target.GetComponent<Collider2D>();
            if (col != null)
            {
                runtimeSpellData.OnHit(caster, col);
            }
        }
    }

    private void AnimateTexture()
    {
        if (lineRenderer.material != null)
        {
            float textureOffset = Time.fixedTime * textureScrollSpeed;
            lineRenderer.material.mainTextureOffset = new Vector2(textureOffset, 0);
        }
    }
}