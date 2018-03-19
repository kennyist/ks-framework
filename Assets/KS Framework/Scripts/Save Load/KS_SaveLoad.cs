using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Reflection;
using System;
using System.Runtime.Serialization;
using UnityEngine.SceneManagement;
using KS_SavingLoading.Surrogates;

namespace KS_SavingLoading
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
        public string idParent;

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

    public static class KS_SaveLoad
    {
        public static KS_Scriptable_GameConfig gameConfig;

        public static event VoidHandler OnSave;
        public static event VoidHandler OnLoad;

        private static SurrogateSelector surrogateSelector = new SurrogateSelector();
        private static KS_SaveGame toSave = new KS_SaveGame();

        private static List<string> componentTypesToAdd = new List<string>() {
            "UnityEngine.MonoBehaviour"
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="obj"></param>
        public static void AddObjectToSave(string ID, object obj)
        {
            toSave.SaveData.Add(ID, obj);
        }

        /// <summary>
        /// Create save game file and save Savable Objects
        /// </summary>
        /// <param name="saveName"></param>
        public static void Save(string saveName)
        {
            toSave = new KS_SaveGame
            {
                SceneIndex = SceneManager.GetActiveScene().buildIndex
            };

            // Call event to add data before serialization
            if (OnSave != null)
                OnSave();

            // Setup binary selector and surrogates
            BinaryFormatter bf = new BinaryFormatter();
            AddSurrogates(ref surrogateSelector);
            bf.SurrogateSelector = surrogateSelector;

            // Save all Savable objects to file
            foreach (GameObject obj in GetAllObjects())
            {
                toSave.gameObjects.Add(StoreObject(obj));
            }

            // Serialize save object
            MemoryStream stream = new MemoryStream();
            bf.Serialize(stream, toSave);

            // Save save object to file
            KS_FileHelper.Instance.SaveFile(KS_FileHelper.Folders.Saves, saveName + ".save", stream.GetBuffer());
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
                saveObject.idParent = obj.transform.parent.GetComponent<KS_SaveableObject>().ID;
            }
            else
            {
                saveObject.idParent = null;
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
                        /*Type element;
                        
                        Type elementType = TypeSystem.GetElementType(field.FieldType);
                        //Debug.Log(field.Name + " -> " + elementType);

                        if (elementType.IsSerializable == false)
                        {
                            continue;
                        }

                        if (element.IsSerializable == false)
                        {
                            continue;
                        }*/
                    }

                    /*object[] attributes = field.GetCustomAttributes(typeof(DontSaveField), true);
                    bool stop = false;
                    foreach (Attribute attribute in attributes)
                    {
                        if (attribute.GetType() == typeof(DontSaveField))
                        {
                            //Debug.Log(attribute.GetType().Name.ToString());
                            stop = true;
                            break;
                        }
                    }
                    if (stop == true)
                    {
                        continue;
                    }*/

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

            return new GameObject();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveComponent"></param>
        /// <returns></returns>
        private static object RestoreComponent(KS_SaveObjectComponent saveComponent)
        {

            return new object();
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
            Vector3Surrogate Vector3_SS = new Vector3Surrogate();
            ss.AddSurrogate(typeof(Vector3),
                            new StreamingContext(StreamingContextStates.All),
                            Vector3_SS);
            /**Texture2DSurrogate Texture2D_SS = new Texture2DSurrogate();
            ss.AddSurrogate(typeof(Texture2D),
                            new StreamingContext(StreamingContextStates.All),
                            Texture2D_SS);
            ColorSurrogate Color_SS = new ColorSurrogate();
            ss.AddSurrogate(typeof(Color),
                            new StreamingContext(StreamingContextStates.All),
                            Color_SS);
            GameObjectSurrogate GameObject_SS = new GameObjectSurrogate();
            ss.AddSurrogate(typeof(GameObject),
                            new StreamingContext(StreamingContextStates.All),
                            GameObject_SS);
            TransformSurrogate Transform_SS = new TransformSurrogate();
            ss.AddSurrogate(typeof(Transform),
                            new StreamingContext(StreamingContextStates.All),
                            Transform_SS);*/
            QuaternionSurrogate Quaternion_SS = new QuaternionSurrogate();
            ss.AddSurrogate(typeof(Quaternion),
                            new StreamingContext(StreamingContextStates.All),
                            Quaternion_SS);


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surrogate"></param>
        /// <param name="forType"></param>
        public static void AddSurrogate(ISerializationSurrogate surrogate, Type forType)
        {
            surrogateSelector.AddSurrogate(forType,
                                           new StreamingContext(StreamingContextStates.All),
                                           surrogate);
        }
    }



}