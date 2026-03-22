using UnityEngine;

public class RobotWheelController : MonoBehaviour
{
    [Header("Basic Settings")]
    public float movementThreshold = 0.01f;
    
    [Header("Animator Mode")]
    public bool useAnimator = true;
    public Animator animator;
    public string movingParameterName = "IsMoving";

    [Header("Transform Rotation Mode")]
    public bool useTransformRotation = false;
    public Transform[] wheels;
    public Vector3 rotationAxis = Vector3.right;
    public float rotationSpeedMultiplier = 360f; // Degrees per unit of movement

    private Vector3 lastPosition;
    private bool wasMoving;

    void Start()
    {
        if (useAnimator && animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        lastPosition = transform.position;
    }

    void Update()
    {
        // 1. 计算移动速度
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        float speed = distanceMoved / Time.deltaTime;
        bool isMoving = speed > movementThreshold;

        // 2. Animator 控制 (仅在状态改变时设置)
        if (useAnimator && animator != null)
        {
            if (isMoving != wasMoving)
            {
                animator.SetBool(movingParameterName, isMoving);
                wasMoving = isMoving;
            }
        }

        // 3. Transform 旋转控制 (实时计算旋转量)
        if (useTransformRotation && wheels != null && wheels.Length > 0)
        {
            if (distanceMoved > 0.0001f)
            {
                float rotationAmount = distanceMoved * rotationSpeedMultiplier;
                
                // 根据移动方向判断正反转
                Vector3 movementDirection = (transform.position - lastPosition).normalized;
                // 使用点乘判断是前进还是后退 (假设物体的前方是 transform.forward)
                float dot = Vector3.Dot(transform.forward, movementDirection);
                
                // 如果是后退 (点乘结果为负)，反转旋转方向
                if (dot < -0.1f) 
                {
                    rotationAmount = -rotationAmount;
                }

                // 应用旋转到每个车轮
                foreach (var wheel in wheels)
                {
                    if (wheel != null)
                    {
                        // 注意：这里假设车轮绕 X 轴旋转，如果不是，请修改 rotationAxis
                        wheel.Rotate(rotationAxis, rotationAmount, Space.Self);
                    }
                }
            }
        }

        lastPosition = transform.position;
    }
}
