using System;
using UnityEngine;

public class CopySetActive : MonoBehaviour
{
    [SerializeField] private GameObject _objectToEffect;

    private void OnEnable()
    {
        if (_objectToEffect == null)
            return;
        
        _objectToEffect.SetActive(true);
    }

    private void OnDisable()
    {
        if (_objectToEffect == null)
            return;
        
        _objectToEffect.SetActive(false);
    }
}