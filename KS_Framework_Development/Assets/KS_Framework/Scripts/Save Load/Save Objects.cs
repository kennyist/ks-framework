using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.IO
{
    /// <summary>
    /// Save game container
    /// </summary>
    [System.Serializable]
    public class KS_SaveGame
    {
        /// <summary>
        /// Loaded Level at save
        /// </summary>
        public int SceneIndex;
        /// <summary>
        /// Saved game objects from level
        /// </summary>
        public List<KS_SaveObject> gameObjects = new List<KS_SaveObject>();
        /// <summary>
        /// Other stored data from objects
        /// </summary>
        public Dictionary<string, object> SaveData = new Dictionary<string, object>();
    }

    /// <summary>
    /// Game objects save container
    /// </summary>
    [System.Serializable]
    public class KS_SaveObject
    {
        /// <summary>
        /// Game objects name
        /// </summary>
        public string name;
        /// <summary>
        /// Game objects prefab name
        /// </summary>
        public string prefabName;
        /// <summary>
        /// Game objects UID
        /// </summary>
        public string id;
        /// <summary>
        /// Game objects parent
        /// </summary>
        public string parentId;

        /// <summary>
        /// Game objects active state at save
        /// </summary>
        public bool active;
        /// <summary>
        /// Game objects position at save
        /// </summary>
        public Vector3 position;
        /// <summary>
        /// Game objects scale at save
        /// </summary>
        public Vector3 localScale;
        /// <summary>
        /// Game objects rotation at save
        /// </summary>
        public Quaternion rotation;

        /// <summary>
        /// game objects packed components
        /// </summary>
        public List<KS_SaveObjectComponent> objectComponents = new List<KS_SaveObjectComponent>();
    }

    /// <summary>
    /// Game objects component container
    /// </summary>
    [System.Serializable]
    public class KS_SaveObjectComponent
    {
        /// <summary>
        /// Component name
        /// </summary>
        public string componentName;
        /// <summary>
        /// Components feild data
        /// </summary>
        public Dictionary<string, object> fields;
    }
}