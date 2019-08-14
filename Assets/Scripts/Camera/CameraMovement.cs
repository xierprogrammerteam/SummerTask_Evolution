using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Collider target;//目标碰撞体
    public Camera camera;//摄像机对象
    public LayerMask obstacleLayers = -1;//用于指定函数FixedUpdate中球形探测的碰撞层
    public LayerMask groundLayers = -1;//用于指定函数FixedUpdate中确定摄像机是否着地的线性探测的碰撞层
    public float groundedCheckOffset = 0.7f;
    public float rotationUpdateSpeed = 60.0f;
    public float lookUpSpeed = 20.0f;
    public float distanceUpdateSpeed = 10.0f;
    public float followUpdateSpeed = 10.0f;
    public float maxForwardAngle = 80.0f;
    public float minDistance = 0.1f;
    public float maxDistance = 10.0f;
    public float zoomSpeed = 1.0f;
    public bool requireLock = true;
    public bool controlLock = true;
    private const float movementThreshold = 0.1f;
    private const float rotationThreshold = 0.1f;
    private const float groundedDistance = 0.5f;
    private Vector3 lastStationaryPosition;//target对象上次停驻的位置
    private float optimalDistance;
    private float targetDistance;
    private bool grounded = false;

    // Use this for initialization
    void Start()
    {
        if (target == null)
            target = GetComponent<Collider>();
        if (camera == null && Camera.main != null)
            camera = Camera.main;
        if (target == null)
        {
            Debug.LogError("target未赋值");
            enabled = false;
            return;
        }
        if (camera == null)
        {
            Debug.LogError("camera未赋值");
            enabled = false;
            return;
        }
        //将lastStationaryPosition设置为对象target的位置
        lastStationaryPosition = target.transform.position;
        //targetDistance和optimalDistance均设定成对象target到camera的距离
        targetDistance = optimalDistance = (camera.transform.position - target.transform.position).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        //Input.GetAxis("MouseScrollWheel")表示鼠标滚轮输入量,向上滑动为正值，向下滑动为负值
        //将optimalDistance设置为optimalDistance + Input.GetAxis("MouseScrollWheel") * -zoomSpeed * Time.deltaTime,且其值限制在minDistance和maxDistance之间
        optimalDistance = Mathf.Clamp(optimalDistance + Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed * Time.deltaTime,
            minDistance, maxDistance);
    }

    //ViewRadius取值为fieldOfViewRadius和doubleCharacterRadius中较小者
    private float ViewRadius
    {
        get
        {
            //fieldOfViewRadius表示视角半径
            float fieldOfViewRadius = (optimalDistance * Mathf.Tan(camera.fieldOfView / 2.0f) * Mathf.Deg2Rad);
            float doubleCharacterRadius = Mathf.Max(target.bounds.extents.x, target.bounds.extents.z) * 2.0f;
            return Mathf.Min(doubleCharacterRadius, fieldOfViewRadius);
        }
    }

    //根据target对象的方向修正后的摄像机位置
    private Vector3 SnappedCamerForward
    {
        get
        {
            Vector3 f = camera.transform.forward;
            Vector2 planeForward = new Vector2(f.x, f.z);
            planeForward = new Vector2(target.transform.forward.x, target.transform.forward.z).normalized *
                           planeForward.magnitude;
            return new Vector3(planeForward.x, f.y, planeForward.y);
        }
    }

    void FixedUpdate()
    {
        grounded = Physics.Raycast(camera.transform.position + target.transform.up * -groundedCheckOffset,
            target.transform.up * -1, groundedDistance, groundLayers);
        //target对象到摄像机的矢量
        Vector3 inverseLineOfSight = camera.transform.position - target.transform.position;
        RaycastHit hit;
        if (Physics.SphereCast(target.transform.position, ViewRadius, inverseLineOfSight, out hit, optimalDistance,
            obstacleLayers))
        {
            targetDistance = Mathf.Min((hit.point - target.transform.position).magnitude, optimalDistance);
        }
        else targetDistance = optimalDistance;
        //targetDistance的大小根据球形探测的结果而定
    }

    void FollowUpdate()
    {
        //摄像机到目标对象的矢量
        Vector3 cameraForward = target.transform.position - camera.transform.position;
        cameraForward = new Vector3(cameraForward.x, 0.0f, cameraForward.z);
        //cameraForward与目标对象间的夹角;若rotationAmount小于rotationTreshold,则更新lastStationaryRosition为target对象的位置
        float rotationAmount = Vector3.Angle(cameraForward, target.transform.forward);
        if (rotationAmount < rotationThreshold)
            lastStationaryPosition = target.transform.position;
        rotationAmount *= followUpdateSpeed * Time.deltaTime;
        //若cameraForward与target对象右侧的夹角小于target对象左侧的夹角,则将rotationAmount乘以-1
        if (Vector3.Angle(cameraForward, target.transform.right) <
            Vector3.Angle(cameraForward, target.transform.right * -1.0f))
            rotationAmount *= -1.0f;
        //将摄像机绕y轴旋转rotationAmount
        camera.transform.RotateAround(target.transform.position, Vector3.up, rotationAmount);
    }

    void FreedUpdate()
    {
        float rotationAmout;
        //单击鼠标右键时调用FollowUpdate旋转摄像机;否则根据MouseX输入轴的输入位来确定rotationAmout,然后将摄像机绕Y轴旋转rotationAmout
        if (Input.GetMouseButton(1))
            FollowUpdate();
        else
        {
            rotationAmout = Input.GetAxis("Mouse X") * rotationUpdateSpeed * Time.deltaTime;
            camera.transform.RotateAround(target.transform.position, Vector3.up, rotationAmout);
        }
        rotationAmout = Input.GetAxis("Mouse Y") * -1.0f * lookUpSpeed * Time.deltaTime;
        bool lookFromBelow = Vector3.Angle(camera.transform.forward, target.transform.up * -1) >
                             Vector3.Angle(camera.transform.forward, target.transform.up);
        if (grounded && lookFromBelow)
            camera.transform.RotateAround(camera.transform.position, camera.transform.right, rotationAmout);
        else
        {
            camera.transform.RotateAround(target.transform.position, camera.transform.right, rotationAmout);
            camera.transform.LookAt(target.transform.position);
            float forwardAngle = Vector3.Angle(target.transform.forward, SnappedCamerForward);
            if (forwardAngle > maxForwardAngle)
                camera.transform.RotateAround(target.transform.position, camera.transform.right,
                    lookFromBelow ? forwardAngle - maxForwardAngle : maxForwardAngle - forwardAngle);
        }
    }

    //更新摄像机位置
    void DistanceUpdate()
    {
        //target对象到摄像机的方向
        Vector3 dir = (camera.transform.position - target.transform.position).normalized;
        //target对象的目标位置，其值为当前位置沿着dir方向向前移动targetDistance
        Vector3 targetPosition = target.transform.position + dir * targetDistance;
        camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition,
            Time.deltaTime * distanceUpdateSpeed);
    }

    void LateUpdate()
    {
        //当鼠标操作摄像机时，调用FreeUpdate来更新摄像机的方位;反之则通过调用FollowUpdate来更新摄像机的方位
        if (//(Input.GetMouseButton(0) || Input.GetMouseButton(1)) &&
            (!requireLock || controlLock || Cursor.lockState == CursorLockMode.Locked))
        {
            if (controlLock)
                Cursor.lockState = CursorLockMode.Locked;
            FreedUpdate();
            lastStationaryPosition = target.transform.position;
        }
        else
        {
            if (controlLock)
                Cursor.lockState = CursorLockMode.None;
            Vector3 movement = target.transform.position - lastStationaryPosition;
            if (new Vector2(movement.x, movement.z).magnitude > movementThreshold)
                FollowUpdate();
        }
        DistanceUpdate();
    }
}
