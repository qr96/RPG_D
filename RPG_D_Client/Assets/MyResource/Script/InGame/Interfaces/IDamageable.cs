using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void OnDamage(long damage);
}

public interface IPushable
{
    public GameObject gameObject { get; }
    public void OnPushed(Vector2 direction);
}

