using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    [SerializeField] bool useCameraShake = false;
    [SerializeField] bool useCameraLeaning = false;
    [SerializeField] bool useHeadbob = false;

    [Header("Camera Shake")]
    public float shakeSpeed;
    private Vector3 Rotation;
    private Vector3 Position;
    private Vector3 shakeRotation;
    private Vector3 shakePosition;
    private Vector3 systemPosition;

    [Header("Camera Leaning")]
    [Range(5, 15)]
    public float maxLeanAngle = 15f;
    public float currentLeanAngleX = 0f;
    public float currentLeanAngleY = 0f;
    [Range(0, 15)]
    public float leanSpeed = 5f;
    float targetLeanAngleX;
    float targetLeanAngleY;
    Quaternion targetRotation;
    Vector2 input;

    [Header("Camera Headbob")]
    public float walkingBobbingSpeed = 14f;
    public float runningBobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;
    float defaultPosY = 0;
    float timer = 0;

    PlayerMovement playerMovement; 
    public static CameraEffect Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        playerMovement = GetComponentInParent<PlayerMovement>();
        systemPosition = transform.localPosition;
        defaultPosY = transform.localPosition.y;
    }

    private void Update() {

        if(useCameraShake)
        {
            //Camera Shake
            shakeRotation = Vector3.Lerp(shakeRotation, Vector3.zero, Time.deltaTime * shakeSpeed);
            Rotation = Vector3.Slerp(Rotation, shakeRotation, Time.deltaTime * shakeSpeed);
            transform.localRotation = Quaternion.Euler(Rotation);
        }

        if(useCameraLeaning)
        {
            // Camera Lean
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            CameraLeaning();
        }
        
        if(useHeadbob)
        {
            //Camera Headbob
            Headbob();
        }
    }

    public void Shake(float shakeX, float shakeY, float shakeZ) {
        shakePosition -= new Vector3(0, 0, 0);
        shakeRotation += new Vector3(shakeX, Random.Range(-shakeY, shakeY), Random.Range(-shakeZ, shakeZ));
    }

    void CameraLeaning()
    {
        targetLeanAngleX = -input.x * maxLeanAngle;
        targetLeanAngleY = input.y * maxLeanAngle;
        currentLeanAngleX = Mathf.Lerp(currentLeanAngleX, targetLeanAngleX, Time.deltaTime * leanSpeed);
        currentLeanAngleY = Mathf.Lerp(currentLeanAngleY, targetLeanAngleY, Time.deltaTime * leanSpeed);
        Mathf.Abs(currentLeanAngleX);
        Mathf.Abs(currentLeanAngleY);
        targetRotation = Quaternion.Euler(currentLeanAngleY, transform.localRotation.eulerAngles.y, currentLeanAngleX);
        transform.localRotation = targetRotation;
    }

    void Headbob()
    {
        if (playerMovement.movementState == PlayerMovement.MovementState.Walking)
        {
            if (playerMovement.movementState == PlayerMovement.MovementState.Running)
            {
                timer += Time.deltaTime * runningBobbingSpeed;
            }
            else
            {
                timer += Time.deltaTime * walkingBobbingSpeed;
            }
            transform.localPosition = new Vector3(transform.localPosition.x,
            defaultPosY + Mathf.Sin(timer) * bobbingAmount, transform.localPosition.z);
        }
        else
        {
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x,
            Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed),
            transform.localPosition.z);
        }
    }

    public float SetDefaultY(float _newValue, float _lerpTime)
    {
        return defaultPosY = Mathf.Lerp(defaultPosY, _newValue, _lerpTime * Time.deltaTime);
    }
}
