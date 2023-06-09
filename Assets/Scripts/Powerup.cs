using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    // ID for Powerups
    // 0 = TripleShot
    // 1 = Speed
    // 2 = Shield
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;


    // Update is called once per frame
    void Update()
    {  
        // move down at a speed of 3 (adjust in the inspactor)
        // when we leave the screen destroy this object  
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -3.66f)
        {
            Destroy(this.gameObject);
        }

    }

   private void OnTriggerEnter2D(Collider2D other)
   {
        if (other.tag == "Player")
        {
            player Player = other.transform.GetComponent<player>();  

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (Player != null)
            {  switch (powerupID)
                {
                    case 0:
                        Player.TripleShotActive();
                        break;
                    case 1:
                        Player.SpeedBoostActive();
                        break;
                    case 2:
                        Player.ShieldActive();
                        break;
                    default:
                        Debug.Log("Default value");
                        break;  
                        


                }
            }
            
            Destroy(this.gameObject);
        }
   }
}
