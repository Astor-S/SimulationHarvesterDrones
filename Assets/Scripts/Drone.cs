using System;
using System.Collections;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Picker _picker;
    [SerializeField] private Base _base;

    private bool _isBusy = false;

    public bool IsBusy => _isBusy;

    public event Action<Resource, Drone> ResourceDelivered;

    public void Init(Base @base) =>
        _base = @base;

    public void SendToResource(Resource resource, Drone drone)
    {
        _isBusy = true;
        _mover.MoveTo(resource.transform);
        StartCoroutine(CollectResource(resource, drone));
    }

    private IEnumerator CollectResource(Resource resource, Drone drone)
    {
        yield return new WaitUntil(() =>
            (transform.position - resource.transform.position).sqrMagnitude <= _picker.PickUpDistance);

        _picker.PickUp(resource);
        _mover.MoveTo(_base.transform);

        yield return new WaitUntil(() =>
            (transform.position - _base.transform.position).sqrMagnitude <= _picker.PickUpDistance);

        ResourceDelivered?.Invoke(resource, drone);
        _picker.Release();
        _isBusy = false;
    }
}