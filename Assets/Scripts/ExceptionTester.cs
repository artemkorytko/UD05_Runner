using System;
using System.Collections;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Runner;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ExceptionTester : MonoBehaviour
{
    private Button _button;

    private async void Start()
    {
        Debug.Log("Start() start");

        bool isOk = true;
        try
        {
            _button = FindObjectOfType<Button>();
            // _button.onClick.AddListener(ButtonClick);

            int[] intArray = new[] {1, 2, 3, 4};
            //Debug.Log(intArray[-1]);
            throw new PlayerDeadException();
        }
        catch (NullReferenceException e)
        {
            Debug.LogError($"Was NullReferenceException: {e}");
        }
        catch (ArgumentOutOfRangeException e)
        {
            Debug.LogError($"Was ArgumentOutOfRangeException: {e}");
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogError($"Was IndexOutOfRangeException: {e}");
        }
        catch (PlayerDeadException e)
        {
            Debug.LogError($"Was PlayerDeadException: {e}");
        }
        catch (Exception e)
        {
            isOk = false;
            Debug.LogError($"Was exception: {e}");
        }
        finally
        {
            Debug.Log("finally");
        }

        Debug.Log("Start() end");
        var value =  await DoSomethingWithDelay();
        var value1 =  await DoSomethingWithDelay();
        var value2 =  await DoSomethingWithDelay();
        var value3 =  await DoSomethingWithDelay();
        var req = await WaitForSecond();
        StartCoroutine(DoSomethingWithDelayCor());
    }

    private void OnDestroy()
    {
        if (_button)
            _button.onClick.RemoveListener(ButtonClick);
    }

    private async void ButtonClick()
    {
        await WaitForSecond();
        Debug.Log(_button.name);
    }

    private async Task<int> DoSomethingWithDelay()
    {
        await WaitForSecond();
        Debug.Log("DoSomethingWithDelay completed");
        return 100;
    }

    private async Task<HttpWebRequest> WaitForSecond()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        string url = "http://imgur.com/gallery/VcBfl.json";
        HttpWebRequest request = HttpWebRequest.CreateHttp(url);
        await request.GetResponseAsync();
        return request;
    }
    
    private IEnumerator DoSomethingWithDelayCor()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("DoSomethingWithDelayCor completed");
    }
}

public class PlayerDeadException : Exception
{
}