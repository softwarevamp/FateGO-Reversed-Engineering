using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public static class GameObjectExtensions
{
    [CompilerGenerated]
    private static Func<Transform, GameObject> <>f__am$cache0;

    public static void AddEulerAngleX(this GameObject self, float x)
    {
        self.transform.Rotate(x, 0f, 0f, Space.World);
    }

    public static void AddEulerAngleY(this GameObject self, float y)
    {
        self.transform.Rotate(0f, y, 0f, Space.World);
    }

    public static void AddEulerAngleZ(this GameObject self, float z)
    {
        self.transform.Rotate(0f, 0f, z, Space.World);
    }

    public static void AddLocalEulerAngleX(this GameObject self, float x)
    {
        self.transform.Rotate(x, 0f, 0f, Space.Self);
    }

    public static void AddLocalEulerAngleY(this GameObject self, float y)
    {
        self.transform.Rotate(0f, y, 0f, Space.Self);
    }

    public static void AddLocalEulerAngleZ(this GameObject self, float z)
    {
        self.transform.Rotate(0f, 0f, z, Space.Self);
    }

    public static void AddLocalPosition(this GameObject self, Vector2 v)
    {
        self.transform.localPosition = new Vector3(self.transform.localPosition.x + v.x, self.transform.localPosition.y + v.y, self.transform.localPosition.z);
    }

    public static void AddLocalPosition(this GameObject self, Vector3 v)
    {
        Transform transform = self.transform;
        transform.localPosition += v;
    }

    public static void AddLocalPosition(this GameObject self, float x, float y)
    {
        self.transform.localPosition = new Vector3(self.transform.localPosition.x + x, self.transform.localPosition.y + y, self.transform.localPosition.z);
    }

    public static void AddLocalPosition(this GameObject self, float x, float y, float z)
    {
        self.transform.localPosition = new Vector3(self.transform.localPosition.x + x, self.transform.localPosition.y + y, self.transform.localPosition.z + z);
    }

    public static void AddLocalPositionX(this GameObject self, float x)
    {
        self.transform.localPosition = new Vector3(self.transform.localPosition.x + x, self.transform.localPosition.y, self.transform.localPosition.z);
    }

    public static void AddLocalPositionY(this GameObject self, float y)
    {
        self.transform.localPosition = new Vector3(self.transform.localPosition.x, self.transform.localPosition.y + y, self.transform.localPosition.z);
    }

    public static void AddLocalPositionZ(this GameObject self, float z)
    {
        self.transform.localPosition = new Vector3(self.transform.localPosition.x, self.transform.localPosition.y, self.transform.localPosition.z + z);
    }

    public static void AddLocalScale(this GameObject self, float f)
    {
        self.transform.localScale = new Vector3(self.transform.localScale.x + f, self.transform.localScale.y + f, self.transform.localScale.z + f);
    }

    public static void AddLocalScale(this GameObject self, Vector2 v)
    {
        self.transform.localScale = new Vector3(self.transform.localScale.x + v.x, self.transform.localScale.y + v.y, self.transform.localScale.z);
    }

    public static void AddLocalScale(this GameObject self, Vector3 v)
    {
        Transform transform = self.transform;
        transform.localScale += v;
    }

    public static void AddLocalScale(this GameObject self, float x, float y)
    {
        self.transform.localScale = new Vector3(self.transform.localScale.x + x, self.transform.localScale.y + y, self.transform.localScale.z);
    }

    public static void AddLocalScale(this GameObject self, float x, float y, float z)
    {
        self.transform.localScale = new Vector3(self.transform.localScale.x + x, self.transform.localScale.y + y, self.transform.localScale.z + z);
    }

    public static void AddLocalScaleX(this GameObject self, float x)
    {
        self.transform.localScale = new Vector3(self.transform.localScale.x + x, self.transform.localScale.y, self.transform.localScale.z);
    }

    public static void AddLocalScaleY(this GameObject self, float y)
    {
        self.transform.localScale = new Vector3(self.transform.localScale.x, self.transform.localScale.y + y, self.transform.localScale.z);
    }

    public static void AddLocalScaleZ(this GameObject self, float z)
    {
        self.transform.localScale = new Vector3(self.transform.localScale.x, self.transform.localScale.y, self.transform.localScale.z + z);
    }

    public static void AddPosition(this GameObject self, Vector2 v)
    {
        self.transform.position = new Vector3(self.transform.position.x + v.x, self.transform.position.y + v.y, self.transform.position.z);
    }

    public static void AddPosition(this GameObject self, Vector3 v)
    {
        Transform transform = self.transform;
        transform.position += v;
    }

    public static void AddPosition(this GameObject self, float x, float y)
    {
        self.transform.position = new Vector3(self.transform.position.x + x, self.transform.position.y + y, self.transform.position.z);
    }

    public static void AddPosition(this GameObject self, float x, float y, float z)
    {
        self.transform.position = new Vector3(self.transform.position.x + x, self.transform.position.y + y, self.transform.position.z + z);
    }

    public static void AddPositionX(this GameObject self, float x)
    {
        self.transform.position = new Vector3(self.transform.position.x + x, self.transform.position.y, self.transform.position.z);
    }

    public static void AddPositionY(this GameObject self, float y)
    {
        self.transform.position = new Vector3(self.transform.position.x, self.transform.position.y + y, self.transform.position.z);
    }

    public static void AddPositionZ(this GameObject self, float z)
    {
        self.transform.position = new Vector3(self.transform.position.x, self.transform.position.y, self.transform.position.z + z);
    }

    public static Transform Find(this GameObject self, string name) => 
        self.transform.Find(name);

    public static T FindComponentWithLog<T>(this GameObject self, string name) where T: Component
    {
        GameObject obj2 = self.FindGameObjectWithLog(name);
        if (obj2 == null)
        {
            return null;
        }
        return obj2.GetComponentWithLog<T>();
    }

    public static GameObject FindDeep(this GameObject self, string name, bool includeInactive = false)
    {
        foreach (Transform transform in self.GetComponentsInChildren<Transform>(includeInactive))
        {
            if (transform.name == name)
            {
                return transform.gameObject;
            }
        }
        return null;
    }

    public static GameObject FindDeepWithLog(this GameObject self, string name, bool includeInactive = false) => 
        self.FindDeep(name, includeInactive);

    public static GameObject FindGameObject(this GameObject self, string name) => 
        self.transform.Find(name).gameObject;

    public static GameObject FindGameObjectWithLog(this GameObject self, string name)
    {
        Transform transform = self.transform.Find(name);
        return transform?.gameObject;
    }

    public static Transform FindWithLog(this GameObject self, string name) => 
        self.transform.Find(name);

    public static Transform GetChild(this GameObject self, int index) => 
        self.transform.GetChild(index);

    public static GameObject[] GetChildren(this GameObject self, bool includeInactive = false)
    {
        <GetChildren>c__AnonStoreyAC yac = new <GetChildren>c__AnonStoreyAC {
            self = self
        };
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = c => c.gameObject;
        }
        return yac.self.GetComponentsInChildren<Transform>(includeInactive).Where<Transform>(new Func<Transform, bool>(yac.<>m__198)).Select<Transform, GameObject>(<>f__am$cache0).ToArray<GameObject>();
    }

    public static T GetComponentInChildrenWithLog<T>(this GameObject self) where T: Component => 
        self.GetComponentInChildren<T>();

    public static T[] GetComponentsInChildrenWithLog<T>(this GameObject self) where T: Component => 
        self.GetComponentsInChildren<T>();

    public static T[] GetComponentsInChildrenWithLog<T>(this GameObject self, bool includeInactive) where T: Component => 
        self.GetComponentsInChildren<T>(includeInactive);

    public static T[] GetComponentsInChildrenWithoutSelf<T>(this GameObject self) where T: Component
    {
        <GetComponentsInChildrenWithoutSelf>c__AnonStoreyAD<T> yad = new <GetComponentsInChildrenWithoutSelf>c__AnonStoreyAD<T> {
            self = self
        };
        return yad.self.GetComponentsInChildren<T>().Where<T>(new Func<T, bool>(yad.<>m__19A)).ToArray<T>();
    }

    public static T[] GetComponentsInChildrenWithoutSelfWithLog<T>(this GameObject self) where T: Component => 
        self.GetComponentsInChildrenWithoutSelf<T>();

    public static T[] GetComponentsWithLog<T>(this GameObject self) where T: Component => 
        self.GetComponents<T>();

    public static T GetComponentWithLog<T>(this GameObject self) where T: Component => 
        self.GetComponent<T>();

    public static Vector3 GetEulerAngles(this GameObject self) => 
        self.transform.eulerAngles;

    public static float GetEulerAngleX(this GameObject self) => 
        self.transform.eulerAngles.x;

    public static float GetEulerAngleY(this GameObject self) => 
        self.transform.eulerAngles.y;

    public static float GetEulerAngleZ(this GameObject self) => 
        self.transform.eulerAngles.z;

    public static Vector3 GetLocalEulerAngles(this GameObject self) => 
        self.transform.localEulerAngles;

    public static float GetLocalEulerAngleX(this GameObject self) => 
        self.transform.localEulerAngles.x;

    public static float GetLocalEulerAngleY(this GameObject self) => 
        self.transform.localEulerAngles.y;

    public static float GetLocalEulerAngleZ(this GameObject self) => 
        self.transform.localEulerAngles.z;

    public static Vector3 GetLocalPosition(this GameObject self) => 
        self.transform.localPosition;

    public static float GetLocalPositionX(this GameObject self) => 
        self.transform.localPosition.x;

    public static float GetLocalPositionY(this GameObject self) => 
        self.transform.localPosition.y;

    public static float GetLocalPositionZ(this GameObject self) => 
        self.transform.localPosition.z;

    public static Vector3 GetLocalScale(this GameObject self) => 
        self.transform.localScale;

    public static float GetLocalScaleX(this GameObject self) => 
        self.transform.localScale.x;

    public static float GetLocalScaleY(this GameObject self) => 
        self.transform.localScale.y;

    public static float GetLocalScaleZ(this GameObject self) => 
        self.transform.localScale.z;

    public static Transform GetParent(this GameObject self) => 
        self.transform.parent;

    public static Vector3 GetPosition(this GameObject self) => 
        self.transform.position;

    public static float GetPositionX(this GameObject self) => 
        self.transform.position.x;

    public static float GetPositionY(this GameObject self) => 
        self.transform.position.y;

    public static float GetPositionZ(this GameObject self) => 
        self.transform.position.z;

    public static GameObject GetRoot(this GameObject self)
    {
        Transform root = self.transform.root;
        return root?.gameObject;
    }

    public static bool HasChild(this GameObject self) => 
        (0 < self.transform.childCount);

    public static bool HasComponent<T>(this GameObject self) where T: Component => 
        (self.GetComponent<T>() != null);

    public static bool HasParent(this GameObject self) => 
        (self.transform.parent != null);

    public static bool IsNotNullOrInactive(this GameObject self) => 
        !self.IsNullOrInactive();

    public static bool IsNullOrInactive(this GameObject self) => 
        (((self == null) || !self.activeInHierarchy) || !self.activeSelf);

    public static void LookAt(this GameObject self, GameObject target)
    {
        self.transform.LookAt(target.transform);
    }

    public static void LookAt(this GameObject self, Transform target)
    {
        self.transform.LookAt(target);
    }

    public static void LookAt(this GameObject self, Vector3 worldPosition)
    {
        self.transform.LookAt(worldPosition);
    }

    public static void LookAt(this GameObject self, GameObject target, Vector3 worldUp)
    {
        self.transform.LookAt(target.transform, worldUp);
    }

    public static void LookAt(this GameObject self, Transform target, Vector3 worldUp)
    {
        self.transform.LookAt(target, worldUp);
    }

    public static void LookAt(this GameObject self, Vector3 worldPosition, Vector3 worldUp)
    {
        self.transform.LookAt(worldPosition, worldUp);
    }

    public static void RemoveComponent<T>(this GameObject self) where T: Component
    {
        UnityEngine.Object.Destroy(self.GetComponent<T>());
    }

    public static void RemoveComponents<T>(this GameObject self) where T: Component
    {
        T[] components = self.GetComponents<T>();
        for (int i = 0; i < components.Length; i++)
        {
            Component component = components[i];
            UnityEngine.Object.Destroy(component);
        }
    }

    public static void ResetEulerAngles(this GameObject self)
    {
        self.transform.eulerAngles = Vector3.zero;
    }

    public static void ResetLocalEulerAngles(this GameObject self)
    {
        self.transform.localEulerAngles = Vector3.zero;
    }

    public static void ResetLocalPosition(this GameObject self)
    {
        self.transform.localPosition = Vector3.zero;
    }

    public static void ResetLocalScale(this GameObject self)
    {
        self.transform.localScale = Vector3.one;
    }

    public static void ResetPosition(this GameObject self)
    {
        self.transform.position = Vector3.zero;
    }

    public static T SafeGetComponent<T>(this GameObject self) where T: Component
    {
        T component = self.GetComponent<T>();
        if (component != null)
        {
            return component;
        }
        return self.AddComponent<T>();
    }

    public static void SafeSetParent(this GameObject self, Component parent)
    {
        self.SafeSetParent(parent.gameObject);
    }

    public static void SafeSetParent(this GameObject self, GameObject parent)
    {
        Transform transform = self.transform;
        Vector3 localPosition = transform.localPosition;
        Quaternion localRotation = transform.localRotation;
        Vector3 localScale = transform.localScale;
        transform.parent = parent.transform;
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
        transform.localScale = localScale;
        self.layer = parent.layer;
    }

    public static void SetEulerAngles(this GameObject self, Vector3 v)
    {
        self.transform.eulerAngles = v;
    }

    public static void SetEulerAngleX(this GameObject self, float x)
    {
        self.transform.eulerAngles = new Vector3(x, self.transform.eulerAngles.y, self.transform.eulerAngles.z);
    }

    public static void SetEulerAngleY(this GameObject self, float y)
    {
        self.transform.eulerAngles = new Vector3(self.transform.eulerAngles.x, y, self.transform.eulerAngles.z);
    }

    public static void SetEulerAngleZ(this GameObject self, float z)
    {
        self.transform.eulerAngles = new Vector3(self.transform.eulerAngles.x, self.transform.eulerAngles.y, z);
    }

    public static void SetLayerRecursively(this GameObject self, int layer)
    {
        self.layer = layer;
        Transform transform = self.transform;
        int index = 0;
        int childCount = transform.childCount;
        while (index < childCount)
        {
            transform.GetChild(index).gameObject.SetLayerRecursively(layer);
            index++;
        }
    }

    public static void SetLocalEulerAngle(this GameObject self, Vector3 v)
    {
        self.transform.localEulerAngles = v;
    }

    public static void SetLocalEulerAngleX(this GameObject self, float x)
    {
        self.transform.localEulerAngles = new Vector3(x, self.transform.localEulerAngles.y, self.transform.localEulerAngles.z);
    }

    public static void SetLocalEulerAngleY(this GameObject self, float y)
    {
        self.transform.localEulerAngles = new Vector3(self.transform.localEulerAngles.x, y, self.transform.localEulerAngles.z);
    }

    public static void SetLocalEulerAngleZ(this GameObject self, float z)
    {
        self.transform.localEulerAngles = new Vector3(self.transform.localEulerAngles.x, self.transform.localEulerAngles.y, z);
    }

    public static void SetLocalPosition(this GameObject self, Vector2 v)
    {
        self.transform.localPosition = new Vector3(v.x, v.y, self.transform.localPosition.z);
    }

    public static void SetLocalPosition(this GameObject self, Vector3 v)
    {
        self.transform.localPosition = v;
    }

    public static void SetLocalPosition(this GameObject self, float x, float y)
    {
        self.transform.localPosition = new Vector3(x, y, self.transform.localPosition.z);
    }

    public static void SetLocalPosition(this GameObject self, float x, float y, float z)
    {
        self.transform.localPosition = new Vector3(x, y, z);
    }

    public static void SetLocalPositionX(this GameObject self, float x)
    {
        self.transform.localPosition = new Vector3(x, self.transform.localPosition.y, self.transform.localPosition.z);
    }

    public static void SetLocalPositionY(this GameObject self, float y)
    {
        self.transform.localPosition = new Vector3(self.transform.localPosition.x, y, self.transform.localPosition.z);
    }

    public static void SetLocalPositionZ(this GameObject self, float z)
    {
        self.transform.localPosition = new Vector3(self.transform.localPosition.x, self.transform.localPosition.y, z);
    }

    public static void SetLocalScale(this GameObject self, float f)
    {
        self.transform.localScale = new Vector3(f, f, f);
    }

    public static void SetLocalScale(this GameObject self, Vector2 v)
    {
        self.transform.localScale = new Vector3(v.x, v.y, self.transform.localScale.z);
    }

    public static void SetLocalScale(this GameObject self, Vector3 v)
    {
        self.transform.localScale = v;
    }

    public static void SetLocalScale(this GameObject self, float x, float y)
    {
        self.transform.localScale = new Vector3(x, y, self.transform.localScale.z);
    }

    public static void SetLocalScale(this GameObject self, float x, float y, float z)
    {
        self.transform.localScale = new Vector3(x, y, z);
    }

    public static void SetLocalScaleX(this GameObject self, float x)
    {
        self.transform.localScale = new Vector3(x, self.transform.localScale.y, self.transform.localScale.z);
    }

    public static void SetLocalScaleY(this GameObject self, float y)
    {
        self.transform.localScale = new Vector3(self.transform.localScale.x, y, self.transform.localScale.z);
    }

    public static void SetLocalScaleZ(this GameObject self, float z)
    {
        self.transform.localScale = new Vector3(self.transform.localScale.x, self.transform.localScale.y, z);
    }

    public static void SetParent(this GameObject self, Component parent)
    {
        self.transform.parent = parent.transform;
    }

    public static void SetParent(this GameObject self, GameObject parent)
    {
        self.transform.parent = parent.transform;
    }

    public static void SetPosition(this GameObject self, Vector2 v)
    {
        self.transform.position = new Vector3(v.x, v.y, self.transform.position.z);
    }

    public static void SetPosition(this GameObject self, Vector3 v)
    {
        self.transform.position = v;
    }

    public static void SetPosition(this GameObject self, float x, float y)
    {
        self.transform.position = new Vector3(x, y, self.transform.position.z);
    }

    public static void SetPosition(this GameObject self, float x, float y, float z)
    {
        self.transform.position = new Vector3(x, y, z);
    }

    public static void SetPositionX(this GameObject self, float x)
    {
        self.transform.position = new Vector3(x, self.transform.position.y, self.transform.position.z);
    }

    public static void SetPositionY(this GameObject self, float y)
    {
        self.transform.position = new Vector3(self.transform.position.x, y, self.transform.position.z);
    }

    public static void SetPositionZ(this GameObject self, float z)
    {
        self.transform.position = new Vector3(self.transform.position.x, self.transform.position.y, z);
    }

    [CompilerGenerated]
    private sealed class <GetChildren>c__AnonStoreyAC
    {
        internal GameObject self;

        internal bool <>m__198(Transform c) => 
            (c != this.self.transform);
    }

    [CompilerGenerated]
    private sealed class <GetComponentsInChildrenWithoutSelf>c__AnonStoreyAD<T> where T: Component
    {
        internal GameObject self;

        internal bool <>m__19A(T c) => 
            (this.self != c.gameObject);
    }
}

