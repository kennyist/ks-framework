using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Reflection;
using System;
using System.Runtime.Serialization;
using UnityEngine.SceneManagement;

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

        private static List<string> componentTypesToAdd = new List<string>() {
            "UnityEngine.MonoBehaviour"
        };

        /// <summary>
        /// Create save game file and save Savable Objects
        /// </summary>
        /// <param name="saveName"></param>
        public static void Save(string saveName)
        {
            if (OnSave != null)
                OnSave();

            KS_SaveGame save = new KS_SaveGame
            {
                SceneIndex = SceneManager.GetActiveScene().buildIndex
            };

            BinaryFormatter bf = new BinaryFormatter();

            // 1. Construct a SurrogateSelector object
            SurrogateSelector ss = new SurrogateSelector();
            // 2. Add the ISerializationSurrogates to our new SurrogateSelector
            AddSurrogates(ref ss);
            // 3. Have the formatter use our surrogate selector
            bf.SurrogateSelector = ss;

            Debug.Log(GetAllObjects().Count);

            foreach (GameObject obj in GetAllObjects())
            {
                save.gameObjects.Add(StoreObject(obj));
            }

            MemoryStream stream = new MemoryStream();
            bf.Serialize(stream, save);

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

        private static bool IsFieldACollection(Type type)
        {
            // Has IEnumerable or ICollection interfaces?
            bool ienum = (type.GetInterface("IEnumerable") != null);
            bool collection = (type.GetInterface("ICollection") != null);

            if (ienum || collection) return true;
            return false;
        }

        public static GameObject RestoreGameObject(KS_SaveObject saveObject)
        {

            return new GameObject();
        }

        private static object RestoreComponent(KS_SaveObjectComponent saveComponent)
        {

            return new object();
        }

        private static void AddSurrogates(ref SurrogateSelector ss)
        {
            Vector3Surrogate Vector3_SS = new Vector3Surrogate();
            ss.AddSurrogate(typeof(Vector3),
                            new StreamingContext(StreamingContextStates.All),
                            Vector3_SS);
            Texture2DSurrogate Texture2D_SS = new Texture2DSurrogate();
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
                            Transform_SS);
            QuaternionSurrogate Quaternion_SS = new QuaternionSurrogate();
            ss.AddSurrogate(typeof(Quaternion),
                            new StreamingContext(StreamingContextStates.All),
                            Quaternion_SS);

        }

        private static void AddSurrogate(Type surrogate)
        {
            object toAdd = Activator.CreateInstance(surrogate);

            surrogateSelector.AddSurrogate(typeof()

            Vector3Surrogate Vector3_SS = new Vector3Surrogate();
            ss.AddSurrogate(typeof(Vector3),
                            new StreamingContext(StreamingContextStates.All),
                            Vector3_SS);
            Texture2DSurrogate Texture2D_SS = new Texture2DSurrogate();
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
                            Transform_SS);
            QuaternionSurrogate Quaternion_SS = new QuaternionSurrogate();
            ss.AddSurrogate(typeof(Quaternion),
                            new StreamingContext(StreamingContextStates.All),
                            Quaternion_SS);

        }
    }



}