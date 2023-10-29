using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [Header("位置相关")]
    public Transform playerTarget;
    public Transform mapCamera;
    [Header("移动时间相关")]
    public float moveTime;

    private void LateUpdate()
    {
        moveTime = 1.5f;
        if(playerTarget != null)
        {
            if(playerTarget.position != transform.position)
            {
                Vector3 tempPosition = new Vector3(transform.position.x, playerTarget.position.y + 6f, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, tempPosition, moveTime * Time.deltaTime);
            }
        }        
    }
}
