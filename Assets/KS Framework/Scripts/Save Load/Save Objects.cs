using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.IO
{
    [System.Serializable]
    public class KS_SaveGame
    {
        public int SceneIndex;
        public List<KS_SaveObject> gameObjects = new List<KS_SaveObject>();
        public Dictionary<string, object> SaveData = new Dictionary<string, object>();
    }

    [System.Serializable]
    public class KS_SaveObject
    {
        public string name;
        public string prefabName;
        public string id;
        public string parentId;

        public bool active;
        public Vector3 position;
        public Vector3 localScale;
        public Quaternion rotation;

        public List<KS_SaveObjectComponent> objectComponents = new List<KS_SaveObjectComponent>();
    }

    [System.Serializable]
    public class KS_SaveObjectComponent
    {
        public string componentName;
        public Dictionary<string, object> fields;
    }
}