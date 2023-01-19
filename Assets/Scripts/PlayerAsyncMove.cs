using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAsyncMove : MonoBehaviour
{
    [SerializeField] private Transform box;
    [SerializeField] private Transform capsule;
    [SerializeField] private Transform startPoint;
    private CancellationToken cancellationToken;
    private TextMeshProUGUI _text;
    private int myInt;

    private int MyInterger
    {
        get => myInt;
        set => myInt = value;
    }

    private async void Start()
    {
        cancellationToken = this.GetCancellationTokenOnDestroy();

        DOTween.To(() => _text.alpha, x => _text.alpha = x, 0f, 1);
        DOTween.To(() => myInt, x => myInt = x, 100, 1).OnUpdate(() =>
        {
            _text.text = myInt.ToString();
        });

        // var direction = capsule.position - box.position;
        // direction.Normalize();
        Rigidbody rigidbody = FindObjectOfType<Rigidbody>();
        bool isCanBeStoped = true;
        rigidbody.velocity += new Vector3(0, 1, 0);
        rigidbody.Sleep();
        if (isCanBeStoped &&  rigidbody.velocity.magnitude < 0.1f)
            rigidbody.isKinematic = true;

        // await MoveShipToStartPoint();
        // capsule.position = startPoint.position + startPoint.forward * 0.5f;
        // await MoveCapsule();
        // capsule.gameObject.SetActive(false);
        // await MoveShipBack();
        Debug.Log("Log start");
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
        }

        Debug.Log("Log end");
    }

    private async UniTask MoveShipToStartPoint()
    {
        await box.DOMove(startPoint.position, 3).WithCancellation(cancellationToken);
    }

    private async UniTask MoveShipBack()
    {
        await box.DOMove(startPoint.position - startPoint.forward * 3, 1);
    }

    private async UniTask MoveCapsule()
    {
        await capsule.DOMove(capsule.position + capsule.forward * 5, 2);
        await capsule.DOJump(capsule.position, 2, 1, 1).Join(capsule.DORotate(new Vector3(0, 180, 0), 1));
        await capsule.DOMove(capsule.position + capsule.forward * 5, 2);
    }
}