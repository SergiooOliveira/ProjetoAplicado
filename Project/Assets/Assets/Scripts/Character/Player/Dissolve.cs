using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dissolve : MonoBehaviour
{

    [SerializeField] private float _dissolveTime = 0.75f;

    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;

    private int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");

    private void Start()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        _materials = new Material[_spriteRenderers.Length];
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
        }
    }

    public void StartDissolve()
    {
        StartCoroutine(Vanish());
    }

    private IEnumerator Vanish()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _dissolveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedDissolve = Mathf.Lerp(0, 1f, (elapsedTime / _dissolveTime));

            for (int i = 0; i < _materials.Length; i++)
            {
                _materials[i].SetFloat(_dissolveAmount, lerpedDissolve);
            }

            yield return null;
        }
    }

    public void ResetDissolve()
    {
        StopAllCoroutines();

        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat(_dissolveAmount, 0f);
        }
    }
}
