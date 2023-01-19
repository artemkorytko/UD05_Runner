using UnityEngine;

namespace Runner
{
    public class PhysicsManager : MonoBehaviour
    {
        [SerializeField] private GameObject cubePrefab;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    var point = hit.point;
                    var cube = Instantiate(cubePrefab, point + new Vector3(0, 0.5f, 0), Quaternion.identity);
                    var rnd = Random.Range(0, 2);
                    var isNotDestroy = rnd == 0;
                    cube.layer = LayerMask.NameToLayer(isNotDestroy ? "CubeNotDestroy" : "Cube");
                    cube.GetComponent<MeshRenderer>().material.color = isNotDestroy ? Color.black : Color.white;
                }
                else
                {
                    Debug.Log("No raycast hit");
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                LayerMask layer = LayerMask.GetMask("Cube");
                if (Physics.Raycast(ray, out RaycastHit hit, 100, layer))
                {
                    if (hit.collider.GetComponent<CubeComponent>())
                    {
                        Destroy(hit.collider.gameObject);
                    }
                }
                else
                {
                    Debug.Log("No raycast hit");
                }
            }
            
        }
    }
}