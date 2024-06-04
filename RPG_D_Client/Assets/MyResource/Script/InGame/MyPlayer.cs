using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class MyPlayer : MonoBehaviour
{
    public Rigidbody2D rigid;
    public SPUM_Prefabs spumPlayer;

    public float speed;

    GameObject nameTag;
    Vector2 input;

    bool moveLock;

    private void Start()
    {
        nameTag = Managers.ui.GetLayout<UILayoutNameTag>().AcquireNameTag(transform, "유저이름어쩌구");
    }

    private void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        if (moveLock)
            input = Vector2.zero;

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

        //nameTag.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, 1f, 0f));
    }

    public void SetPlayerMoveLock(bool moveLock)
    {
        this.moveLock = moveLock;
    }
}
