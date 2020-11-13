using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ShellPool : MonoBehaviour
    {
        public GameObject prefab;
        public int amount;
        public bool populateOnStart = true;
        public bool growOverAmount = true;

        public List<GameObject> Pool { get; } = new List<GameObject>();

        void Start()
        {
            if (populateOnStart && prefab != null && amount > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    var instance = Instantiate(prefab);
                    instance.SetActive(false);
                    Pool.Add(instance);
                }
            }
        }

        public GameObject Instantiate(Vector3 position, Quaternion rotation)
        {
            foreach (var item in Pool)
            {
                if (!item.activeInHierarchy)
                {
                    item.transform.position = position;
                    item.transform.rotation = rotation;
                    item.SetActive(true);
                    return item;
                }
            }

            if (growOverAmount)
            {
                var instance = Instantiate(prefab, position, rotation);
                Pool.Add(instance);
                return instance;
            }

            return null;
        }
    }
}  

