using System;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Runner
{
    public class PhisicsManager : MonoBehaviour
    {
        [SerializeField] private GameObject _cubePrefab;


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100f))
                { 
                    var point = hit.point;
                    var cube= Instantiate(_cubePrefab, point + new Vector3(0, 0.5f,0), Quaternion.identity);
                    var random = Random.Range(0, 2);
                    var isNotDestroy = random == 0;
                    cube.layer = LayerMask.NameToLayer(isNotDestroy ? "Cube" : "CubeNoDestroy");
                    cube.GetComponent<MeshRenderer>().material.color = isNotDestroy ? Color.black : Color.white;
                }
                else
                {
                    Debug.Log("null");
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                LayerMask layerMask = LayerMask.GetMask("Cube");
                
                if (Physics.Raycast(ray, out hit,100f, layerMask))
                {
                    if (hit.collider.GetComponent<CubeComponent>())
                    {
                        Destroy(hit.collider.gameObject);
                    }
                }
                else
                {
                    Debug.Log("null");
                }
            }
            
        }
    }
}