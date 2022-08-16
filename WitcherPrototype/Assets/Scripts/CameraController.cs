using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public Transform target;

   // public Tilemap theMap;
    public float bottomLeftLimitX;
    public float topRightLimitX;
    public float bottomLeftLimitY;
    public float topRightLimitY;

    private float halfHeight;
    private float halfWidth;
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    public int musicToPlay;
    private bool musicStarted;
    // Start is called before the first frame update
    void Start()
    {
        //target = PlayerController.instance.transform;
        target = FindObjectOfType<PlayerController>().transform;

        SpriteRenderer[] renderers = FindObjectsOfType<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.sortingOrder = (int)(renderer.transform.position.y * -100);
        }

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        bottomLeftLimit = new Vector3(bottomLeftLimitX, bottomLeftLimitY, 0);
        topRightLimit = new Vector3(topRightLimitX, topRightLimitY, 0);

        PlayerController.instance.SetBounds(bottomLeftLimit, topRightLimit);

        bottomLeftLimit = new Vector3(bottomLeftLimitX + halfWidth, bottomLeftLimitY + halfHeight, 0);
        topRightLimit = new Vector3(topRightLimitX - halfWidth, topRightLimitY - halfHeight, 0);
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (PlayerController.instance != null)
        {
            if (target == null)
            {

                target = PlayerController.instance.transform;
            }
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

            //keep camera inside bounds

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
            if (!musicStarted)
            {
                musicStarted = true;
                AudioManager.instance.PlayBGM(musicToPlay);
            }
        }
    }
}
