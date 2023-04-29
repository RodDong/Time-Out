using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneToJson : MonoBehaviour
{
    List<ObjectPosScaleRotateChild> objectPosScaleRotates = new List<ObjectPosScaleRotateChild>();
    string path = Application.dataPath + "/levelTransformInfo.json";
    [SerializeField] GameObject[] allObjects;
    void Start()
    {
        foreach (var gameObject in allObjects)
        {
            objectPosScaleRotates.Add(ConvertObjectToJson(gameObject));
        }

        System.IO.File.WriteAllText(path, JsonHelper.ToJson(objectPosScaleRotates.ToArray(), true));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    ObjectPosScaleRotateChild ConvertObjectToJson(GameObject gameObject)
    {
        if (gameObject.activeInHierarchy)
        {
            if (gameObject.transform.childCount == 0)
            {
                ObjectPosScaleRotateChild objectPosScaleRotate = new ObjectPosScaleRotateChild();
                objectPosScaleRotate.name = gameObject.name;
                objectPosScaleRotate.pos = gameObject.transform.position;
                objectPosScaleRotate.rot = gameObject.transform.eulerAngles;
                objectPosScaleRotate.scale = gameObject.transform.localScale;
                return objectPosScaleRotate;
            }
            else
            {
                ObjectPosScaleRotateChild objectPosScaleRotateChild = new ObjectPosScaleRotateChild();
                objectPosScaleRotateChild.name = gameObject.name;
                objectPosScaleRotateChild.pos = gameObject.transform.position;
                objectPosScaleRotateChild.rot = gameObject.transform.eulerAngles;
                objectPosScaleRotateChild.scale = gameObject.transform.localScale;
                List<Transform> children = gameObject.GetComponentsInChildren<Transform>().ToList<Transform>();
                children.RemoveAt(0); //remove parent itself
                foreach (var child in children)
                {

                    ObjectPosScaleRotateChild childPosScaleRotate = new ObjectPosScaleRotateChild();
                    childPosScaleRotate = ConvertObjectToJson(child.gameObject);
                    objectPosScaleRotateChild.child.Add(childPosScaleRotate);
                }
                return objectPosScaleRotateChild;
            }
        }
        return null;
    }
}

[Serializable] public class ObjectPosScaleRotateChild
{
    public string name;
    public Vector3 pos;
    public Vector3 rot;
    public Vector3 scale;
    public List<ObjectPosScaleRotateChild> child = new List<ObjectPosScaleRotateChild>();
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}