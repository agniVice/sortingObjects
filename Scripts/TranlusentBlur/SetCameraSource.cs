using System;
using LeTai.Asset.TranslucentImage;
using UnityEngine;

[RequireComponent(typeof(TranslucentImage))]
public class SetCameraSource : MonoBehaviour
{
    [SerializeField] private TranslucentImage _translucentImage;
    [SerializeField] private TranslucentSourceType _type;

    private void OnValidate()
    {
        if (_translucentImage == null)
        {
            _translucentImage = transform.GetComponent<TranslucentImage>();
        }
    }

    private void Start()
    {
        _translucentImage.source = TranslucentSourcesManager.instance.GetSource(_type);
    }
}
