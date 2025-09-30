using NUnit.Framework;
using UnityEngine;

/*
 * Chest -> Link to a chest and give a spell / Item
 * NPC -> Responsable for dialogue
 * Shop -> Same as an NPC but with a shop menu after
 */
public enum InteractableType { Chest, NPC, Shop }


public interface IInteractableObject
{
    string IOName { get; }
    Sprite IOIcon { get; }
    InteractableType IOType { get; }
    string[] IODialogue { get; }
    //List<Item> IOShop { get; }

}
