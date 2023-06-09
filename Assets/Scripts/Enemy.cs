using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    [SerializeField]
    private player _Player;
    private Animator _anim;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _Player = GameObject.Find ("player").GetComponent<player>();
        _audioSource = GetComponent<AudioSource>();
        if (_Player == null)
        {
            Debug.LogError("The player is Null.");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The Animator is Null");
        }
    }

    // Update is called once per frame
    void Update()
    { 
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

         if (transform.position.y < -5f){
            float randomX = Random.Range(7f, -7f);
            transform.position = new Vector3(randomX, 7, 0);
        }
     }

     private void OnTriggerEnter2D(Collider2D other)
    {

    if (other.tag == "Player")
    {
        // damage player 
        other.transform.GetComponent<player>().Damage();

        _anim.SetTrigger("OnEnemyDeath");
        _speed = 0; 

        _audioSource.Play();
        Destroy(this.gameObject, 2.8f);
        
    }

      if (other.tag == "Laser")
    {
        Destroy(other.gameObject);

            if (_Player != null )
            {
                _Player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;

        _audioSource.Play();
        Destroy(this.gameObject, 2.8f);
      
    }

    }  

}
