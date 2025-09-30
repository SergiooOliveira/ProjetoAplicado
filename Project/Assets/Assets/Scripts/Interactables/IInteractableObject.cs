using UnityEngine;

/*
 * Chest -> Link to a chest and give a spell / Item
 * NPC -> Responsable for dialogue
 * Shop -> Same as an NPC but with a shop menu after
 */

public enum InteractableType { Chest, NPC, Shop }


public interface IInteractableObject
{
    string Name { get; }
}
