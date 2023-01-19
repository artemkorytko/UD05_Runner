using System;
using System.Collections;
using UnityEngine;

namespace Runner
{
    public class TimeScaller : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(SlowMotion());
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Time.timeScale = 5;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Time.timeScale = 1;
            }
        }

        private IEnumerator SlowMotion()
        {
            while (Time.timeScale > 0.2f)
            {
                Time.timeScale -= Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            while (Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime;
                yield return null;
            }
        }
    }
}