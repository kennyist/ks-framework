using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Reflection;
using System;
using System.Runtime.Serialization;
using UnityEngine.SceneManagement;
using KS_Core.IO.Surrogates;

namespace KS_Core.IO
{
    /// <summary>
    /// 
    /// </summary>
    public static class KS_SaveLoad
    {
        /// <summary>
        /// Game config <see cref="KS_Scriptable_GameConfig"/>
        /// </summary>
        public static KS_Scriptable_GameConfig gameConfig;
        /// <summary>
        /// Format to save games in
        /// </summary>
        public static string fileFormat = ".save";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SaveData"></param>
        public delegate void OnSaveHandler(ref Dictionary<string, object> SaveData);
        /// <summary>
        /// On Game save
        /// </summary>
        public static event OnSaveHandler OnSave;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="savegame"></param>
        public delegate void OnLoadHandler(KS_SaveGame savegame);
        /// <summary>
        /// On Game Load
        /// </summary>
        public static event OnLoadHandler OnLoad;

        private static List<KeyValuePair<ISerializationSurrogate, Type>> surrogateList = new List<KeyValuePair<ISerializationSurrogate, Type>>();
        private static Dictionary<string, GameObject> prefabDictionary;

        private static List<string> componentTypesToAdd = new List<string>() {
            "UnityEngine.MonoBehaviour"
        };

        /// <summary>
        /// Create save game file and save Savable Objects
        /// </summary>
        /// <param name="saveName"></param>
        public static void Save(string saveName)
        {
            KS_SaveGame save = new KS_SaveGame
            {
                SceneIndex = SceneManager.GetActiveScene().buildIndex
            };

            // Call event to add data before serialization
            if (OnSave != null)
                OnSave(ref save.SaveData);

            // Setup binary selector and surrogates
            BinaryFormatter bf = new BinaryFormatter();
            SurrogateSelector ss = new SurrogateSelector();
            AddSurrogates(ref ss);
            bf.SurrogateSelector = ss;

            // Save all Savable objects to file
            foreach (GameObject obj in GetAllObjects())
            {
                save.gameObjects.Add(StoreObject(obj));
            }

            // Serialize save object
            MemoryStream stream = new MemoryStream();
            bf.Serialize(stream, save);

            // Save save object to file
            KS_FileHelper.Instance.SaveFile(Folders.Saves, saveName + GetFileFormat(), stream.GetBuffer());
        }

        /// <summary>
        /// Find all objects with "KS_SavableObject" script attached
        /// </summary>
        /// <returns>Gameobject list</returns>
        private static List<GameObject> GetAllObjects()
        {
            KS_SaveableObject[] saveableObjs = GameObject.FindObjectsOfType<KS_SaveableObject>() as KS_SaveableObject[];
            List<GameObject> objects = new List<GameObject>();

            foreach (KS_SaveableObject savable in saveableObjs)
            {
                if (string.IsNullOrEmpty(savable.ID))
                {
                    savable.GetID();
                }

                objects.Add(savable.gameObject);
                savable.gameObject.SendMessage("OnSerialize", SendMessageOptions.DontRequireReceiver);
            }

            return objects;
        }

        /// <summary>
        /// Load save game
        /// </summary>
        /// <param name="name">Name of the file (excluding file format)</param>
        /// <returns></returns>
        public static KS_SaveGame Load(string name)
        {
            BinaryFormatter bf = new BinaryFormatter();
            SurrogateSelector ss = new SurrogateSelector();
            AddSurrogates(ref ss);
            bf.SurrogateSelector = ss;

            MemoryStream stream = new MemoryStream();
            byte[] file = KS_FileHelper.Instance.LoadFileToBytes(Folders.Saves, name + GetFileFormat());
            stream.Write(file, 0, file.Length);
            stream.Position = 0;

            KS_SaveGame save = (KS_SaveGame)bf.Deserialize(stream);

            if (OnLoad != null)
                OnLoad(save);

            return save;
        }

        /// <summary>
        /// Convert Gameobject to saveobject
        /// </summary>
        /// <param name="obj">Gameobject to convert</param>
        /// <returns>Converted Save object</returns>
        private static KS_SaveObject StoreObject(GameObject obj)
        {
            KS_SaveObject saveObject = new KS_SaveObject();
            KS_SaveableObject saveable = obj.GetComponent<KS_SaveableObject>();

            saveObject.name = obj.name;
            saveObject.prefabName = saveable.prefab;
            saveObject.id = saveable.ID;

            if (obj.transform.parent != null && obj.transform.parent.GetComponent<KS_SaveableObject>() == true)
            {
                saveObject.parentId = obj.transform.parent.GetComponent<KS_SaveableObject>().ID;
            }
            else
            {
                saveObject.parentId = null;
            }

            List<object> components_filtered = new List<object>();

            object[] components_raw = obj.GetComponents<Component>() as object[];
            foreach (object component_raw in components_raw)
            {
                if (componentTypesToAdd.Contains(component_raw.GetType().BaseType.FullName))
                {
                    components_filtered.Add(component_raw);
                }
            }

            foreach (object component_filtered in components_filtered)
            {
                saveObject.objectComponents.Add(StoreComponent(component_filtered));
            }

            saveObject.position = obj.transform.position;
            saveObject.localScale = obj.transform.localScale;
            saveObject.rotation = obj.transform.rotation;
            saveObject.active = obj.activeSelf;

            return saveObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private static KS_SaveObjectComponent StoreComponent(object component)
        {
            // Setup saveable componenet
            KS_SaveObjectComponent newObjectComponent = new KS_SaveObjectComponent
            {
                fields = new Dictionary<string, object>()
            };

            // Get component type
            var type = component.GetType();
            // Get Component Public, private and instance fields
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            // Save componenet type
            newObjectComponent.componentName = type.ToString();

            // Add Each Field
            foreach (FieldInfo field in fields)
            {

                if (field != null) // If field null go to next
                {
                    if (field.FieldType.IsSerializable == false) // If field is not serializable skip field
                    {
                        continue;
                    }

                    if (IsFieldACollection(field.FieldType))    // Is field a collection type?
                    {

                        Type element = GetElementType(field.FieldType);

                        if (element.IsSerializable == false)    // If isnt serializable skip
                        {
                            continue;
                        }
                    }

                    newObjectComponent.fields.Add(field.Name, field.GetValue(component));
                }
            }
            return newObjectComponent;
        }

        /// <summary>
        /// Restore object from a save files SaveObject
        /// </summary>
        /// <param name="saveObject">KS_SaveObject object</param>
        /// <returns>GameObject from KS_saveObject</returns>
        public static GameObject RestoreGameObject(KS_SaveObject saveObject)
        {
            if (prefabDictionary == null || prefabDictionary.Count <= 0)
            {
                GetAllPrefabs();
            }

            if (!prefabDictionary.ContainsKey(saveObject.prefabName))
            {
                Debug.LogWarning("Prefab \"" + saveObject.prefabName + "\" not found, Is it in resources folder?");
                return null;
            }

            GameObject retObject = GameObject.Instantiate(prefabDictionary[saveObject.prefabName], saveObject.position, saveObject.rotation);

            // Fill values
            retObject.name = saveObject.name;
            retObject.transform.localScale = saveObject.localScale;
            retObject.SetActive(saveObject.active);

            // Add savable object if not present
            if (!retObject.GetComponent<KS_SaveableObject>())
            {
                retObject.AddComponent<KS_SaveableObject>();
            }

            // Set savable object fields
            KS_SaveableObject saveable = retObject.GetComponent<KS_SaveableObject>();
            saveable.ID = saveObject.id;
            saveable.ParentID = saveObject.parentId;

            // Restore componenets
            foreach(KS_SaveObjectComponent oc in saveObject.objectComponents)
            {
                RestoreComponent(oc, ref retObject);
            }

            return retObject;
        }

        /// <summary>
        /// Unpack saved objects componenet
        /// </summary>
        /// <param name="saveComponent"></param>
        /// <param name="go"></param>
        private static void RestoreComponent(KS_SaveObjectComponent saveComponent, ref GameObject go)
        {
            // Does component exist
            if (!go.GetComponent(saveComponent.componentName))
            {
                Type componentType = Type.GetType(saveComponent.componentName);
                go.AddComponent(componentType);
            }

            object component = go.GetComponent(saveComponent.componentName) as object;

            Type type = component.GetType();

            foreach(KeyValuePair<string, object> kvp in saveComponent.fields)
            {
                var field = type.GetField(kvp.Key,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetField);

                if(field != null)
                {
                    object value = kvp.Value;
                    field.SetValue(component, value);
                }
            }
        }

        /// <summary>
        /// Checks if field has IEnumerable or ICollection interfaces 
        /// </summary>
        /// <param name="type">Field type</param>
        /// <returns>True if either IEnumerable or ICollection exists</returns>
        private static bool IsFieldACollection(Type type)
        {
            // Has IEnumerable or ICollection interfaces?
            bool ienum = (type.GetInterface("IEnumerable") != null);
            bool collection = (type.GetInterface("ICollection") != null);

            if (ienum || collection) return true;
            return false;
        }


        private static void AddSurrogates(ref SurrogateSelector ss)
        {
            Vector3Surrogate vector = new Vector3Surrogate();
            ss.AddSurrogate(typeof(Vector3),
                            new StreamingContext(StreamingContextStates.All),
                            vector);
            QuaternionSurrogate quaternion = new QuaternionSurrogate();
            ss.AddSurrogate(typeof(Quaternion),
                            new StreamingContext(StreamingContextStates.All),
                            quaternion);
            TransformSurrogate transform = new TransformSurrogate();
            ss.AddSurrogate(typeof(Transform),
                            new StreamingContext(StreamingContextStates.All),
                            transform);
            ColourSurrogate colour = new ColourSurrogate();
            ss.AddSurrogate(typeof(Color),
                            new StreamingContext(StreamingContextStates.All),
                            colour);
            CameraSurrogate camera = new CameraSurrogate();
            ss.AddSurrogate(typeof(Camera),
                            new StreamingContext(StreamingContextStates.All),
                            camera);

            foreach(KeyValuePair<ISerializationSurrogate, Type> kv in surrogateList)
            {
                ss.AddSurrogate(kv.Value,
                                new StreamingContext(StreamingContextStates.All),
                                kv.Key);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surrogate"></param>
        /// <param name="forType"></param>
        public static void AddSurrogate(ISerializationSurrogate surrogate, Type forType)
        {
            surrogateList.Add(new KeyValuePair<ISerializationSurrogate, Type>(surrogate, forType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seqType"></param>
        /// <returns></returns>
        private static Type GetElementType(Type seqType)
        {
            Type ienum = FindIEnumerable(seqType);
            if (ienum == null) return seqType;
            return ienum.GetGenericArguments()[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seqType"></param>
        /// <returns></returns>
        private static Type FindIEnumerable(Type seqType)
        {
            if (seqType == null || seqType == typeof(string))
                return null;
            if (seqType.IsArray)
                return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
            if (seqType.IsGenericType)
            {
                foreach (Type arg in seqType.GetGenericArguments())
                {
                    Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                    if (ienum.IsAssignableFrom(seqType))
                    {
                        return ienum;
                    }
                }
            }
            Type[] ifaces = seqType.GetInterfaces();
            if (ifaces != null && ifaces.Length > 0)
            {
                foreach (Type iface in ifaces)
                {
                    Type ienum = FindIEnumerable(iface);
                    if (ienum != null) return ienum;
                }
            }
            if (seqType.BaseType != null && seqType.BaseType != typeof(object))
            {
                return FindIEnumerable(seqType.BaseType);
            }
            return null;
        }

        /// <summary>
        /// Get all prefabs in resources folder
        /// </summary>
        private static void GetAllPrefabs()
        {
            prefabDictionary = new Dictionary<string, GameObject>();

            foreach(GameObject o in Resources.LoadAll<GameObject>(""))
            {
                if (o.GetComponent<KS_SaveableObject>())
                {
                    prefabDictionary.Add(o.name, o);
                }
            }
        }

        private static string GetFileFormat()
        {
            if (gameConfig)
            {
                return gameConfig.saveFileFormat;
            }

            return fileFormat;
        }
    }
}