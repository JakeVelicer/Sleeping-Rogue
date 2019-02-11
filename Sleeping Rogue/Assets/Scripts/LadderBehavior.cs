using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderBehavior : MonoBehaviour
{
    public Collider2D Collider;
    public LayerMask PlayerLayer;

    public float RayCastCenterPointOffset;


    // Start is called before the first frame update
    void Start()
    {
        Collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo1 = Physics2D.Raycast(new Vector2 (transform.position.x - 1, transform.position.y + RayCastCenterPointOffset), Vector2.up, 5, PlayerLayer);
        RaycastHit2D hitInfo2 = Physics2D.Raycast(new Vector2 (transform.position.x, transform.position.y + RayCastCenterPointOffset), Vector2.up, 5, PlayerLayer);
        RaycastHit2D hitInfo3 = Physics2D.Raycast(new Vector2 (transform.position.x + 1, transform.position.y + RayCastCenterPointOffset), Vector2.up, 5, PlayerLayer);
        
        if ((hitInfo1.collider != null || hitInfo2.collider != null || hitInfo3.collider != null)
        && Input.GetAxis("Vertical") >= 0)
        {
            Collider.enabled = true;
        }
        else
        {
            Collider.enabled = false;
        }
    }
}
