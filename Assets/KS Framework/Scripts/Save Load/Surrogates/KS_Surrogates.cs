using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace KS_SavingLoading.Surrogates
{

    public class Vector3Surrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector3 vector = (Vector3)obj;

            info.AddValue("x", vector.x);
            info.AddValue("y", vector.y);
            info.AddValue("z", vector.z);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector3 vector = (Vector3)obj;

            vector.x = (float) info.GetValue("x", typeof(float));
            vector.y = (float)info.GetValue("y", typeof(float));
            vector.z = (float)info.GetValue("z", typeof(float));

            return (obj = vector);
        }
    }

    public class QuaternionSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Quaternion quaternion = (Quaternion)obj;

            info.AddValue("x", quaternion.x);
            info.AddValue("y", quaternion.y);
            info.AddValue("z", quaternion.z);
            info.AddValue("w", quaternion.w);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Quaternion quaternion = (Quaternion)obj;

            quaternion.x = (float)info.GetValue("x", typeof(float));
            quaternion.y = (float)info.GetValue("y", typeof(float));
            quaternion.z = (float)info.GetValue("z", typeof(float));
            quaternion.w = (float)info.GetValue("w", typeof(float));

            return (obj = quaternion);
        }
    }

    public class TransformSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Transform transform = (Transform)obj;

            info.AddValue("pos", transform.position);
            info.AddValue("rot", transform.rotation);
            info.AddValue("scale", transform.localScale);

        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Transform transform = (Transform)obj;

            /*camera.position = (Vector3) info.GetValue("pos", typeof(Vector3));
            camera.rotation = (Quaternion)info.GetValue("rot", typeof(Quaternion));
            camera.localScale = (Vector3)info.GetValue("scale", typeof(Vector3));*/

            return (obj = transform);
        }
    }

    public class ColourSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Color colour = (Color)obj;

            info.AddValue("r", colour.r);
            info.AddValue("g", colour.g);
            info.AddValue("b", colour.b);
            info.AddValue("a", colour.a);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Color col = (Color)obj;

            col.r = (float)info.GetValue("r", typeof(float));
            col.g = (float)info.GetValue("g", typeof(float));
            col.b = (float)info.GetValue("b", typeof(float));
            col.a = (float)info.GetValue("a", typeof(float));

            return (obj = col);
        }
    }

    public class CameraSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Camera camera = (Camera)obj;

            Debug.Log("Cam dept serial: " + camera.depth);

            info.AddValue("depth", camera.depth);
            info.AddValue("cullMask", camera.cullingMask);
            info.AddValue("flags", camera.clearFlags);
            info.AddValue("bgCol", camera.backgroundColor);
            info.AddValue("rotation", camera.transform.rotation);

        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            GameObject o = new GameObject();
            o.AddComponent<Camera>();
            Camera camera = o.GetComponent<Camera>();
            Debug.Log("Cam Test:" + camera.name);
            Debug.Log("Cam depth deserial: " + (int)info.GetValue("depth", typeof(int)));

            camera.depth = (int)info.GetValue("depth", typeof(int));
            camera.cullingMask = (int)info.GetValue("cullMask", typeof(int));
            camera.clearFlags = (CameraClearFlags)info.GetValue("flags", typeof(CameraClearFlags));
            camera.transform.rotation = (Quaternion)info.GetValue("rotation", typeof(Quaternion));

            return (obj = camera);
        }
    }

}