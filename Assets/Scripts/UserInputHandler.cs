using UnityEngine;
using System.Collections;

public class UserInputHandler : MonoBehaviour {

    #region Public Methods
    public delegate void TapAction(Touch t);
    public static event TapAction OnTap;

    public delegate void PanBeganAction(Touch t);
    public static event PanBeganAction OnPanBegan;

    public delegate void PanHeldAction(Touch t);
    public static event PanHeldAction OnPanHeld;

    public delegate void PanEndedAction(Touch t);
    public static event PanEndedAction OnPanEnded;

    public delegate void AccelerometerChangedAction(Vector3 acceleration);
    public static event AccelerometerChangedAction OnAccelerometerChanged;
    #endregion

    #region Public Variables
    public float tapMaxMovement = 50f;

    public float panMinTime = 0.4f;

    public GameObject optionsMenu;
    #endregion

    #region Private Variables
    private Vector2 movement;

    private bool tapGestureFailed = false;

    private float startTime;

    private bool panGestureRecognized = false;

    private Vector3 defaultAcceleration;
    #endregion

    #region MonoBehvaiours

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                movement = Vector2.zero;
                startTime = Time.time;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                movement += touch.deltaPosition;

                if (!panGestureRecognized && Time.time - startTime > panMinTime)
                {
                    panGestureRecognized = true;
                    tapGestureFailed = true;

                    if (OnPanBegan != null)
                        OnPanHeld(touch);
                }
                else if (panGestureRecognized)
                {
                    if (OnPanHeld != null)
                        OnPanHeld(touch);
                }

                else if (movement.magnitude > tapMaxMovement)
                    tapGestureFailed = true;
            }
            else
            {
                if (panGestureRecognized)
                {
                    if (OnPanEnded != null)
                    {
                        OnPanEnded(touch);
                    }
                }
                else if (!tapGestureFailed)
                {
                    if (OnTap != null)
                    {
                        OnTap(touch);
                    }
                }

                panGestureRecognized = false;
                tapGestureFailed = false;
            }
        }

        if (OnAccelerometerChanged != null)
        {
            Vector3 acceleration = new Vector3(Input.acceleration.x, Input.acceleration.y, -1 * Input.acceleration.z);
            acceleration -= defaultAcceleration;
            OnAccelerometerChanged(acceleration);
        }
    }
    #endregion

    void OnEnable()
    {
        defaultAcceleration = new Vector3(Input.acceleration.x, Input.acceleration.y, -1*Input.acceleration.z);
    }

    public void OnOptionsSelected()
    {
        optionsMenu.SetActive(true);
    }

    public void OnOptionsClosed()
    {
        optionsMenu.SetActive(false);
    }
}
