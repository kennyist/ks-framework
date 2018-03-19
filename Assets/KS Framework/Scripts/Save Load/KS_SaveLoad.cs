using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Reflection;
using System;
using System.Runtime.Serialization;

namespace KS_SavingLoading
{

    [System.Serializable]
    public class KS_SaveGame
    {
        public List<KS_SaveObject> gameObjects = new List<KS_SaveObject>();
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

        public List<ObjectComponent> objectComponents = new List<ObjectComponent>();
    }

    public static class KS_SaveLoad
    {

        public static KS_Scriptable_GameConfig gameConfig;

        public static event VoidHandler OnSave;
        public static event VoidHandler OnLoad;

        private static List<string> componentTypesToAdd = new List<string>() {
            "UnityEngine.MonoBehaviour"
        };

        public static void Save(string saveName)
        {
            if (OnSave != null)
                OnSave();

            KS_SaveGame save = new KS_SaveGame();

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
                save.gameObjects.Add(ConvertObjectToSaveObject(obj));
            }

            MemoryStream stream = new MemoryStream();
            bf.Serialize(stream, save);

            KS_FileHelper.Instance.SaveFile(KS_FileHelper.Folders.Saves, saveName + ".save", stream.GetBuffer());

            Debug.Log("Saved Game: " + saveName);
        }

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

        private static KS_SaveObject ConvertObjectToSaveObject(GameObject obj)
        {
            KS_SaveObject saveObject = new KS_SaveObject();
            KS_SaveableObject saveable = obj.GetComponent<KS_SaveableObject>();

            saveObject.name = obj.name;
            saveObject.prefabName = saveable.prefab;
            saveObject.id = saveable.ID;

            if (obj.transform.parent != null && obj.transform.parent.GetComponent<ObjectIdentifier>() == true)
            {
                saveObject.idParent = obj.transform.parent.GetComponent<ObjectIdentifier>().id;
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
                saveObject.objectComponents.Add(ConvertComponent(component_filtered));
            }

            saveObject.position = obj.transform.position;
            saveObject.localScale = obj.transform.localScale;
            saveObject.rotation = obj.transform.rotation;
            saveObject.active = obj.activeSelf;

            return saveObject;
        }

        private static ObjectComponent ConvertComponent(object component)
        {
            ObjectComponent newObjectComponent = new ObjectComponent();
            newObjectComponent.fields = new Dictionary<string, object>();

            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var componentType = component.GetType();
            FieldInfo[] fields = componentType.GetFields(flags);

            newObjectComponent.componentName = componentType.ToString();

            foreach (FieldInfo field in fields)
            {

                if (field != null)
                {
                    if (field.FieldType.IsSerializable == false)
                    {
                        //Debug.Log(field.Name + " (Type: " + field.FieldType + ") is not marked as serializable. Continue loop.");
                        continue;
                    }
                    if (TypeSystem.IsEnumerableType(field.FieldType) == true || TypeSystem.IsCollectionType(field.FieldType) == true)
                    {
                        Type elementType = TypeSystem.GetElementType(field.FieldType);
                        //Debug.Log(field.Name + " -> " + elementType);

                        if (elementType.IsSerializable == false)
                        {
                            continue;
                        }
                    }

                    object[] attributes = field.GetCustomAttributes(typeof(DontSaveField), true);
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
                    }

                    newObjectComponent.fields.Add(field.Name, field.GetValue(component));
                    //Debug.Log(field.Name + " (Type: " + field.FieldType + "): " + field.GetValue(component));
                }
            }
            return newObjectComponent;
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
    }
}