using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif
//using UnityEditor;

namespace Code.Base
{
    public class PrefabTileGenerator : MonoBehaviour
    {
         public GameObject[] prefab;
        public int width = 10;
        public int length = 10;
        public float space = 1;
        [Space] public bool randomRotate;
        public float minScale = 1;
        public float maxScale = 1;
        public float minPositionDeviation = 0.1f;
        public float maxPositionDeviation = 1;
        public bool withTexture;
        public Texture2D referenceTexture;
#if UNITY_EDITOR
        [ContextMenu("Create")]
        public void Create()
        {
            RemoveAll();
            if (withTexture)
            {
                CreateWithTexture();
                return;
            }

            var pos = Vector3.zero;
            for (var x = 0; x < width; x++)
            for (var y = 0; y < length; y++)
            {
                var transform1 = transform;
                var position = transform1.position;
                pos = new Vector3(position.x + x * space, position.y, position.z + y * space);
                var createdGo = (GameObject) PrefabUtility.InstantiatePrefab(prefab.RandomElement(), transform1);
                createdGo.transform.position = pos;
                createdGo.transform.rotation = Quaternion.identity;
                UseOtherValues(createdGo);
            }
        }

        public void CreateWithTexture()
        {
            var pos = Vector3.zero;
            for (var x = 0; x < referenceTexture.width; x++)
            for (var y = 0; y < referenceTexture.height; y++)
                if (referenceTexture.GetPixel(x, y).r >= 0.95f)
                {
                    var transform1 = transform;
                    var position = transform1.position;
                    pos = new Vector3(position.x + x * space, position.y + y * space, position.z);
                    var createdGo = (GameObject) PrefabUtility.InstantiatePrefab(prefab.RandomElement(), transform1);
                    createdGo.transform.position = pos;
                    createdGo.transform.rotation = Quaternion.identity;
                    UseOtherValues(createdGo);
                }
        }

        private void UseOtherValues(GameObject createdGo)
        {
            createdGo.transform.localScale *= Random.Range(minScale, maxScale);
            if (randomRotate) createdGo.transform.Rotate(0, Random.Range(-360, 360), 0);
            createdGo.transform.position += new Vector3(Random.Range(minPositionDeviation, maxPositionDeviation), 0,
                Random.Range(minPositionDeviation, maxPositionDeviation));
        }

        [ContextMenu("RemoveAll")]
        public void RemoveAll()
        {
            while (transform.childCount > 0)
                for (var i = 0; i < transform.childCount; i++)
                    DestroyImmediate(transform.GetChild(i).gameObject);
        }
#endif
    }
}