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
}