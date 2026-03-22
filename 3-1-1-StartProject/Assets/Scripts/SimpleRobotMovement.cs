using UnityEngine;

public class SimpleRobotMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float turnSpeed = 120f;
    
    // 如果勾选了这个，原地转向时轮子不会因为 RobotWheelController 检测不到位移而不动
    // 但 RobotWheelController 主要检测位移，所以这里只是纯粹的物体旋转
    [Tooltip("Allows the robot to turn in place")]
    public bool canTurnInPlace = true;

    void Update()
    {
        // 获取输入 (W/S 控制前后，A/D 控制左右旋转)
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // 移动逻辑
        // 如果有移动输入，则向前/后移动
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            Vector3 moveDirection = transform.forward * moveInput * moveSpeed * Time.deltaTime;
            transform.position += moveDirection;
        }

        // 转向逻辑
        // 如果有转向输入，且 (允许原地转向 或者 正在移动)
        if (Mathf.Abs(turnInput) > 0.01f)
        {
            if (canTurnInPlace || Mathf.Abs(moveInput) > 0.01f)
            {
                float turnAmount = turnInput * turnSpeed * Time.deltaTime;
                // 对于倒车时，反向旋转操作手感更符合直觉（或者保持不变取决于需求，这里保持不变）
                transform.Rotate(Vector3.up, turnAmount);
            }
        }
    }
}
