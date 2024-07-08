using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    public void OnDamage(long damage)
    {
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.1f);
    }
}
