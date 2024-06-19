using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    public Rigidbody2D rigid;
    public SPUM_Prefabs spumPlayer;

    public float speed;

    Transform nameTag;
    Vector2 input;

    private void Update()
    {
        input = Managers.input.GetPlayerInputAxis();

        if (input.sqrMagnitude > 1)
            input = input.normalized;

        rigid.velocity = new Vector2(input.x * speed * Time.fixedDeltaTime, input.y * speed * Time.fixedDeltaTime);

        if (input.magnitude > 0)
            spumPlayer.PlayAnimation(1);
        else
            spumPlayer.PlayAnimation(0);

        if (input.x > 0)
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        else if (input.x < 0)
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10f);

        if (nameTag != null)
            nameTag.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, 1f, 0f));
    }

    public void SetNameTag(string name)
    {
        if (nameTag != null)
        {
            Managers.ui.GetLayout<UILayoutNameTag>().RemoveNameTag(gameObject);
            nameTag = null;
        }
        
        nameTag = Managers.ui.GetLayout<UILayoutNameTag>().AcquireNameTag(gameObject, name);
    }
}
