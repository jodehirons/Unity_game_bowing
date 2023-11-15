using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera类，用于处理与Camera移动相关的逻辑
/// </summary>
public class CameraSystem : MonoBehaviour
{
    [Header("位置相关")]
    public Transform playerTarget;
    public Transform mapCamera;
    [Header("移动时间相关")]
    public float moveTime;

    /// <summary>
    /// 在每一帧更新后调用，用于处理Camera的移动
    /// </summary>
    private void LateUpdate()
    {
        moveTime = 1.5f;
        if(playerTarget != null)
        {
            if(playerTarget.position != mapCamera.position)
            {
                Vector3 tempPosition = new Vector3(mapCamera.position.x, playerTarget.position.y + 2f, mapCamera.position.z);
                mapCamera.position = Vector3.Lerp(mapCamera.position, tempPosition, moveTime * Time.deltaTime);
            }
        }        
    }
}
