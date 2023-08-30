using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Texture2D mouseCursorOnDrag;
    [SerializeField] private float panSpeed = 50f;
    [SerializeField] private float panPadding = 10f;
    [SerializeField] private float scrollSpeed = 20f;
    [SerializeField] private float mousesButtonSpeed = 10f;
    [SerializeField] private bool borderMovement = false;
    private float MIN_ZOOM = 5f;
    private float MAX_ZOOM = 20f;
    private float defaultZoom;
    private Vector3 mouseWheelDownPosition;

    private void Start()
    {
        defaultZoom = Camera.main.orthographicSize;

    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == GameConstants.SCENE_GAME)
        {
            Movement();
            Scrolling();
            ResetCameraZoom();
            MousewheelMovement();
        }
    }

    private void MousewheelMovement()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            mouseWheelDownPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetCursorSpriteDrag();
        }

        if (Input.GetKey(KeyCode.Mouse2))
        {
            Vector3 dir = mouseWheelDownPosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Camera.main.transform.Translate(dir * Time.deltaTime * mousesButtonSpeed);
        }

        if (Input.GetKeyUp(KeyCode.Mouse2))
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    private void SetCursorSpriteDrag()
    {
        if(mouseCursorOnDrag == null) return;
        Cursor.SetCursor(mouseCursorOnDrag, new Vector2(mouseCursorOnDrag.width / 2, mouseCursorOnDrag.height / 2), CursorMode.Auto);
    }

    private void ResetCameraZoom()
    {
        if (Input.GetKey(KeyCode.T))
        {
            Camera.main.orthographicSize = defaultZoom;
        }
    }

    private void Movement()
    {
        if (Input.GetKey("w") || ((Input.mousePosition.y >= Screen.height - panPadding) && borderMovement))
        {
            transform.Translate(0, panSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey("s") || ((Input.mousePosition.y <= panPadding) && borderMovement))
        {
            transform.Translate(0, -(panSpeed * Time.deltaTime), 0);
        }
    }

    private void Scrolling()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            float newOrtographicSize = Camera.main.orthographicSize -= scroll * scrollSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(newOrtographicSize, MIN_ZOOM, MAX_ZOOM);
        }
    }
}
