using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR;
namespace VentaVR.Peace
{
    [RequireComponent(typeof(ARTrackedImageManager))]
    public class DrawImage : MonoBehaviour
    {

        [SerializeField]
        private GameObject[] addprefabs;

        private ARTrackedImageManager trackedImageManager;
        private Dictionary<string, GameObject> spawnPrefabs = new Dictionary<string, GameObject>();

        public Transform ObjectParent;


        private void Awake()
        {
            trackedImageManager = GetComponent<ARTrackedImageManager>();

            for (int i = 0; i < addprefabs.Length; i++)
            {
                GameObject newPrefab = Instantiate(addprefabs[i].gameObject);
                newPrefab.name = addprefabs[i].name;
                newPrefab.transform.parent = ObjectParent;
                spawnPrefabs.Add(newPrefab.name, newPrefab);
                newPrefab.SetActive(false);
            }
        }
        private void OnEnable()
        {
            trackedImageManager.trackedImagesChanged += imageChanged;
        }
        private void OnDisable()
        {
            trackedImageManager.trackedImagesChanged -= imageChanged;
        }

        private void imageChanged(ARTrackedImagesChangedEventArgs eventargs)
        {
            foreach (ARTrackedImage trackedImage in eventargs.added)
            {
                UpdateImage(trackedImage);
            }
            foreach (ARTrackedImage trackedImage in eventargs.updated)
            {
                UpdateImage(trackedImage);
            }

            foreach (ARTrackedImage trackedImage in eventargs.removed)
            {
                spawnPrefabs[trackedImage.name].SetActive(false); // 트랙이미지의 오브젝트 비활성화
            }
        }

        private void UpdateImage(ARTrackedImage trackedImage) // 다넣고 아니면 지우고 
        {
            string name = trackedImage.referenceImage.name;
            Vector3 pos = trackedImage.transform.position;
            Quaternion rot = trackedImage.transform.rotation; // x 90으롷 ㅏ면 정방향 
            rot *= Quaternion.Euler(new Vector3(90, 0, 0));

            GameObject prefab = spawnPrefabs[name];
            prefab.transform.position = pos;

            prefab.transform.rotation = rot;

            prefab.SetActive(true);

        }
    }
}