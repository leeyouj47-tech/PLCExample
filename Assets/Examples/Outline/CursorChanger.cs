using UnityEngine;
using UnityEngine.InputSystem;

public class CursorChanger : MonoBehaviour
{
    //마우스 커서 선언
    public Texture2D leftCursor;
    public Texture2D rightCursor;
    public Texture2D middleCursor;

    //커서 위치
    public Vector2 leftHotSpot;
    public Vector2 rightHotSpot;
    public Vector2 middleHotSpot;

    public void OnOrbit(InputValue value)
    {
        Cursor.SetCursor(value.isPressed ? leftCursor : null, leftHotSpot, CursorMode.Auto);
    }

    public void OnFreelook(InputValue value)
    {
        Cursor.SetCursor(value.isPressed ? rightCursor : null, rightHotSpot, CursorMode.Auto);
    }

    public void OnPan(InputValue value)
    {
        Cursor.SetCursor(value.isPressed ? middleCursor : null, middleHotSpot, CursorMode.Auto);
    }

}
