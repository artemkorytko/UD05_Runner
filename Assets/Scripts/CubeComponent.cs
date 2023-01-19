using System;
using UnityEngine;

namespace Runner
{
    public class CubeComponent : MonoBehaviour
    {
        [SerializeField] private Transform ball;
        [SerializeField] private Transform otherObject;
        
        
        // [SerializeField] private ColorConfig _colorConfig;
        //
        // private void Awake()
        // {
        //     GetComponent<MeshRenderer>().material.color = _colorConfig.GetRandomColor();
        // }

        private void LateUpdate()
        {
           
        }

        private void OnTriggerEnter(Collider other)
        {
            Camera.main.transform.SetParent(otherObject);
        }

        private void OnTriggerExit(Collider other)
        {
            Camera.main.transform.SetParent(ball);
            transform.parent = ball;
            transform.parent = null;
            Camera.main.transform.SetParent(null);
        }
    }
}