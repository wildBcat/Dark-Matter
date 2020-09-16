using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDemoController : MonoBehaviour
{

    WeaponController[] allTurrets;
    EdgeCollider2D edgeCollider;
    bool repeatFire = false;

    // Start is called before the first frame update
    void Start()
    {
        allTurrets = GameObject.FindObjectsOfType<WeaponController>();

        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));

        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.points = new Vector2[] { bottomLeft,
            new Vector2(bottomLeft.x, topRight.y),
            topRight,
            new Vector2(topRight.x, bottomLeft.y),
            bottomLeft
        };

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                StartStopRepeatFire();
            } else if (Input.GetKey(KeyCode.LeftAlt))
            {
                ExplosionController[] allExplosions = GameObject.FindObjectsOfType<ExplosionController>();
                foreach (ExplosionController oneExpl in allExplosions)
                    oneExpl.StartExplosion();
            } else 
            {
                foreach (WeaponController oneTurret in allTurrets)
                {
                    oneTurret.MakeOneShot();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
            Destroy(collision.gameObject);
    }

    public void StartStopRepeatFire()
    {
        repeatFire = !repeatFire;
        foreach (WeaponController oneTurret in allTurrets)
        {
            if (repeatFire)
            {
                oneTurret.StartRepeateFire();
            }
            else
            {
                oneTurret.StopRepeatFire();
            }
        }

    }

    public void ExplosionAll()
    {
        BaseShipController[] allShips = GameObject.FindObjectsOfType<BaseShipController>();
        foreach (BaseShipController ship in allShips)
        {
            ship.StartExplosion();
        }
    }

    
}
