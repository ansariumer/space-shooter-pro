using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    // public or private refernce 
    // data type (int, float, bool, string)
    // every variable has a name 
    // optional value assinged
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    
    [SerializeField]
    private GameObject _shieldVisualizer; 
    [SerializeField]
    private GameObject _rightEngine,_leftEngine;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laserSoundClip;

    private AudioSource _audioSource;





    // Start is called before the first frame update
    void Start()
    {     
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null )
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip; 
        }
    }

    // Update is called once per frame
    void Update()
    {
      CalculateMovement();

      if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
      {
        FireLaser();
      }
    }
    
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput *_speed * Time.deltaTime);

        // if player position on y is >= 0
        // y position = 0
        // eles if positionon is less than -2.78f
        // y position = 2.78f

        if (transform.position.y >= 0){
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -2.78f){
            transform.position = new Vector3(transform.position.x, -2.78f, 0);
        }

        // if player position on x is >=9.3f
        // x position = - 9.3f
        // if player position on x is <= -9.3f
        // x position = 9.3f  

        if (transform.position.x >= 9.3f){
            transform.position = new Vector3(-9.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -9.3f){
            transform.position = new Vector3(9.3f, transform.position.y, 0);
        }
    }

    void FireLaser ()
    {  
        _canFire = Time.time + _fireRate;

        if(_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }

        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.04f, 0) , Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        // if shield is active
        // do nothing
        // deactive shield 
        // return
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;    

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
            
        }
        else if(_lives == 1)
        {
           _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1 )
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

}
