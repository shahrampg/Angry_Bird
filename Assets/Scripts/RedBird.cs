using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RedBird : MonoBehaviour
{
    Vector2 start_position;
    Vector2 init_position;

    // initial bird angle
    float bird_init_angle = 25.0f;
    float bird_fly_angle = 25.0f;

    bool IS_Infly = false;
    bool TimerTimeOut = false;

    public float init_force = 200;
    float force = 200;

    static int Score = 0;
    public Text Score_Value_Text;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().rotation = bird_init_angle;
        init_position = GetComponent<Rigidbody2D>().position;
    }

    // Update is called once per frame
    void Update()
    {
        if (IS_Infly)
        {
            bird_fly_angle -= 0.15f;
            GetComponent<Rigidbody2D>().rotation = bird_fly_angle;
        }

        if (TimerTimeOut)
        {
            Reset_bird_position();
            TimerTimeOut = false;
        }
    }

    private void OnMouseDrag()
    {
        Vector3 mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mouseposition.x, mouseposition.y, transform.position.z);
    }

    private void OnMouseDown()
    {
        start_position = GetComponent<Rigidbody2D>().position;
    }

    private void OnMouseUp()
    {
        IS_Infly = true;
        GetComponent<Rigidbody2D>().isKinematic = false;

        Vector2 current_position = GetComponent<Rigidbody2D>().position;
        Vector2 direction = start_position - current_position;
        force *= direction.magnitude;

        direction.Normalize();
        GetComponent<Rigidbody2D>().AddForce(direction * force);
        force = init_force;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        IS_Infly = false;

        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Box")
        {
            // Distroy enemy
            collision.gameObject.SetActive(false);

            // increase score and display it
            Score++;
            Score_Value_Text.text = Score.ToString();
        }

        TimerCount(3);
    }

    // Async Timer
    private async void TimerCount(int milsec)
    {
        await Task.Delay(milsec * 1000);
        TimerTimeOut = true;
    }

    private void Reset_bird_position()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().position = init_position;

        GetComponent<Rigidbody2D>().freezeRotation = true;
        GetComponent<Rigidbody2D>().freezeRotation = false;

        GetComponent<Rigidbody2D>().rotation = bird_init_angle;
        bird_fly_angle = bird_init_angle;
    }
}
