using UnityEngine;
using System.Collections.Generic;
using Pathfinding; // Make sure you have this!

public class DoorManager : MonoBehaviour
{
    [System.Serializable]
    public class DoorGroup
    {
        [Header("Enemies to track (drag enemy GameObjects from the Hierarchy)")]
        public List<GameObject> enemies;

        [Header("Doors to deactivate when these enemies are cleared (drag door GameObjects)")]
        public List<GameObject> doors;
        
        [HideInInspector] public bool doorsDeactivated = false;
    }

    [Header("Add as many door groups as needed")]
    public List<DoorGroup> doorGroups;

    void Update()
    {
        foreach (DoorGroup group in doorGroups)
        {
            if (!group.doorsDeactivated && AllEnemiesCleared(group.enemies))
            {
                DeactivateDoors(group.doors);
                group.doorsDeactivated = true;
            }
        }
    }

    bool AllEnemiesCleared(List<GameObject> enemies)
    {
        enemies.RemoveAll(e => e == null);
        return enemies.Count == 0;
    }

    void DeactivateDoors(List<GameObject> doors)
    {
        foreach (GameObject door in doors)
        {
            if (door != null)
            {
                // Grab bounds BEFORE deactivating
                Collider2D col = door.GetComponent<Collider2D>();
                if (col != null)
                {
                    Bounds bounds = col.bounds;

                    // Disable the door visually and physically
                    door.SetActive(false);

                    // Update A* Grid Graph in the area the door occupied
                    GraphUpdateObject guo = new GraphUpdateObject(bounds);
                    AstarPath.active.UpdateGraphs(guo);
                }
                else
                {
                    // Fallback: just disable if no collider
                    door.SetActive(false);
                }
            }
        }

        // 🔁 Full graph rescan since map is small
        AstarPath.active.Scan();
    }
}
