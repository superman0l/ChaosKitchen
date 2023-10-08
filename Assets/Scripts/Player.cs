using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private float rotateSpeed = 10.0f;
    [SerializeField] private Transform KitchenObjectPos;

    private bool is_Walking;
    private GameInput gameInput;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    Vector3 hitnormal;

    public EventHandler OnGrabObject;//setobject调用时触发该事件
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start(){
        gameInput = GameObject.Find("GameInput").GetComponent<GameInput>();
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void Update(){
        HandleMovement();
        HandleIntersections();
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.isGamePlaying()) return;
        selectedCounter?.IntersectAlternate(this);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.isGamePlaying()) return;
        selectedCounter?.Intersect(this);
    }

    private void HandleIntersections()
    {
        float rayLength = 1.3f;
        RaycastHit hitinfo;
        LayerMask countersMask = LayerMask.GetMask("Counters");
        if (Physics.Raycast(transform.position, transform.forward, out hitinfo, rayLength, countersMask) &&
            hitinfo.transform.TryGetComponent(out BaseCounter baseCounter))//layermask 6 Counters 
        {
            SetSelectedCounter(baseCounter);
        }
        else SetSelectedCounter(null);
    }

    private void HandleMovement()
    {
        // 处理所有移动函数
        Vector2 inputVector = gameInput.GetComponent<GameInput>().getMoveVectorNormalized();
        if (inputVector != Vector2.zero)
        {
            is_Walking = true;
        }
        else
        {
            is_Walking = false;
        }

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        Vector3 forwardDir = moveDir;

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        RaycastHit hitinfo;
        if (Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, out hitinfo, moveDistance))
        {
            if (hitinfo.normal != Vector3.zero) hitnormal = hitinfo.normal;
            /*
            if (Physics.Raycast(transform.position, moveDir, out hitinfo))
            {
                hitnormal = hitinfo.normal;
                Debug.Log(hitinfo.normal);
            }*/
            
            moveDir -= Vector3.Dot(hitnormal, moveDir) * hitnormal;
        }

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        transform.forward = Vector3.Slerp(transform.forward, forwardDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs{
            selectedCounter = selectedCounter
        });
    }

    public bool isWalking(){
        return is_Walking;
    }

    public Transform getKitchenObjectPos()
    {
        return KitchenObjectPos;
    }

    public void setKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        OnGrabObject?.Invoke(this, EventArgs.Empty);
    }

    public KitchenObject getKitchenObject()
    {
        return kitchenObject;
    }

    public void clearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool hasKitchenObject()
    {
        return kitchenObject != null;
    }
}
