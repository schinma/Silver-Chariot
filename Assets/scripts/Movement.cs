using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public float speed = 1;
    public float rotationForce = 1;
    public GameObject[] _allies;
    public GameObject[] _foods;

    public GameObject deathPrefab;
    public ParticleSystem foodParticle;
    public UnityEvent OnGameOver;
    public UnityEvent OnWin;

    private Rigidbody _rig;
    private Vector3 _velocity;
    private Vector3 _rotator;
    private Transform _trans;

    public Text _distanceText;
    public Text _foodText;
    public Text _alliesText;
    public Slider _foodSlider;

    private SoundsManager _soundManager;

    public Text _endMessage;

    public float _distance = 0;
    private float _xInitPos = 0;
    //public PlayerStat _stat;
    private float _timer;
    [SerializeField]
    private Transform _cameraAncor;
    private bool moving = true;


    // Start is called before the first frame update
    void Start()
    {
        _xInitPos = transform.position.x;

        _timer = 1;
        //_stat = GetComponent<PlayerStat>();
        _rig = GetComponent<Rigidbody>();
        _trans = GetComponent<Transform>();
        _soundManager = GetComponent<SoundsManager>();

        // on desactive les alliés

        for (int i = 0; i < _allies.Length; i++)
        {
            _allies[i].SetActive(false);
        }
        GameManager.identity.stat.initialize();
        for (int i = 0; i < _foods.Length; i++)

        {
            if (i * 10 < GameManager.identity.stat.food)
            {
                _foods[i].SetActive(true);
            }
            else
            {
                _foods[i].SetActive(false);
            }
        }

        if (_cameraAncor == null)
        {
            _cameraAncor = GameObject.Find("CameraAncor").transform;
        }
    }

    void OnEnable()
    {
        //_foodText.text = "Food: " + GameManager.identity.stat.food.ToString();
        _foodSlider.value = GameManager.identity.stat.food;
        _alliesText.text = "Saved: " + GameManager.identity.stat.rescued.ToString();
        for(int i = 0; i < _foods.Length; i++)
        {
            if (i * 10 < GameManager.identity.stat.food)
            {
                _foods[i].SetActive(true);
            } else
            {
                _foods[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //calculer speed en fct de la curve logarithme
        speed = 1 + Mathf.Log(GameManager.identity.stat.rescued) * 1.6f;


        float _yMove = Input.GetAxis("Vertical");
        float _xMovePlayer = Input.GetAxis("Horizontal");

        if (transform.position.x <= _cameraAncor.position.x && _xMovePlayer <= 0)
        {
            _xMovePlayer = 0;
            Death();
        }
        //Debug.Log(_xMovePlayer);

        float _xMove = 1 + _xMovePlayer;

        //movement
        Vector3 _moveVertical = _yMove * transform.forward * speed;
        Vector3 _moveHorizontal = transform.right * speed * _xMove;

        _distance = transform.position.x - _xInitPos;
        _distanceText.text = _distance.ToString("F0") + "m";

        _velocity = (_moveVertical + _moveHorizontal);
        _rotator = new Vector3(0f, -_yMove * rotationForce, 0f);
    }

    public IEnumerator Death()
    {
        if (deathPrefab != null)
        {
            var deathVFX = Instantiate(deathPrefab, transform.position, transform.rotation) as GameObject;
            Destroy(deathVFX, 1);
        }

        yield return new WaitForSeconds(0.6f);

        this.gameObject.SetActive(false);
        OnGameOver.Invoke();

    }

    public void Win()
    {
        _endMessage.text = "With the help of " + GameManager.identity.stat.rescued.ToString() + " persons.";

        Destroy(gameObject);
        OnWin.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Food")
        {
            GameManager.identity.stat.food += 10;
            foodParticle.Play();
            _soundManager.playFoodPick();
            _foodSlider.value = GameManager.identity.stat.food;
            for (int i = 0; i < _foods.Length; i++)
            {
                if (i * 10 < GameManager.identity.stat.food)
                {
                    _foods[i].SetActive(true);
                }
                else
                {
                    _foods[i].SetActive(false);
                }
            }
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Ally")
        {
            GameManager.identity.stat.rescued += 1;
            _alliesText.text = "Saved: " + (GameManager.identity.stat.rescued - 1).ToString();
            other.gameObject.SetActive(false);
            //créer un nouvel alié sur le chariot
            if (GameManager.identity.stat.rescued - 2 < _allies.Length)
              _allies[GameManager.identity.stat.rescued - 2].SetActive(true);
        }
        else if (other.tag == "Obstacle")
        {
            Debug.Log("Obstacle");
            _soundManager.playWallHit();
            StopMoving();
            StartCoroutine(Death());
            //inclu la fin du jeu et tout
        }
        else if (other.tag == "Finish")
        {
            Debug.Log("Done");
            StopMoving();
            Win();
            //inclu la fin du jeu et tout
        }
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            performMovement();
            performRotation();
        }
        //_foodText.text = "Food: " + ((int)GameManager.identity.stat.food).ToString();
        _foodSlider.value = GameManager.identity.stat.food;
        for (int i = 0; i < _foods.Length; i++)
        {
            if (i * 10 < GameManager.identity.stat.food)
            {
                _foods[i].SetActive(true);
            }
            else
            {
                _foods[i].SetActive(false);
            }
        }
    }

    void performMovement()
    {
        _rig.MovePosition(_rig.position + _velocity * Time.fixedDeltaTime);
    }

    void performRotation()
    {
        if ((_rig.rotation * Quaternion.Euler(_rotator)).y > -0.45 && (_rig.rotation * Quaternion.Euler(_rotator)).y < 0.45)
             _rig.MoveRotation(_rig.rotation * Quaternion.Euler(_rotator));
    }

    public void StopMoving()
    {
        moving = false;
    }

    public void StartMoving()
    {
        moving = true;
    }
}
