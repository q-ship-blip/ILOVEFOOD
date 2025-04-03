using UnityEngine;
using System.Collections.Generic;

public class DoorManager : MonoBehaviour
{
    [System.Serializable]
    public class DoorGroup
    {
        [Header("Enemies to track (drag enemy GameObjects from the Hierarchy)")]
        public List<GameObject> enemies;

        [Header("Doors to deactivate when these enemies are cleared (drag door GameObjects)")]
        public List<GameObject> doors;
        
        // Used internally to ensure this group's doors are only deactivated once.
        [HideInInspector] public bool doorsDeactivated = false;
    }

    [Header("Add as many door groups as needed")]
    public List<DoorGroup> doorGroups;

    void Update()
    {
        // Check each door group and deactivate doors if all enemies are cleared
        foreach (DoorGroup group in doorGroups)
        {
            if (!group.doorsDeactivated && AllEnemiesCleared(group.enemies))
            {
                DeactivateDoors(group.doors);
                group.doorsDeactivated = true;
            }
        }
    }

    // Checks if all enemy GameObjects in the list have been destroyed
    bool AllEnemiesCleared(List<GameObject> enemies)
    {
        // Remove any destroyed (null) enemy references from the list
        enemies.RemoveAll(e => e == null);
        return enemies.Count == 0;
    }

    // Deactivates all door GameObjects in the list
    void DeactivateDoors(List<GameObject> doors)
    {
        foreach (GameObject door in doors)
        {
            if (door != null)
                door.SetActive(false);
        }
    }
}
