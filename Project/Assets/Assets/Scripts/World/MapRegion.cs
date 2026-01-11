using UnityEngine;
using UnityEngine.EventSystems;

public class MapRegion : MonoBehaviour, IPointerClickHandler
{
    public string mapName;

    private MapSelectorUI mapSelector;

    private void Awake()
    {
        mapSelector = Object.FindFirstObjectByType<MapSelectorUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (mapSelector != null)
            mapSelector.OnMapSelect(mapName);
    }
}
