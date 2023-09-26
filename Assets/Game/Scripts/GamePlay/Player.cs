using UnityEngine;

public class Player : Character
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private LayerMask brickBrigeLayer;
    [SerializeField] private LayerMask wallLayer;
    private bool isControl = true;
    private Vector3 moveVector;
    private RaycastHit hit;
    private RaycastHit hitWall;
    void Start()
    {

    }
    public override void Control()
    {
        moveVector = Vector3.zero;
        moveVector.x = joystick.Horizontal * moveSpeed * Time.deltaTime;
        moveVector.z = joystick.Vertical * moveSpeed * Time.deltaTime;
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            Vector3 direction = Vector3.RotateTowards(transform.forward, moveVector, moveSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(direction);
            ChangeAnim(Constansts.RunAnim);
        }
        else if (joystick.Horizontal == 0 || joystick.Vertical == 0)
        {
            ChangeAnim(Constansts.IdleAnim);
        }
        Ray ray = new(transform.position, new Vector3(joystick.Horizontal, 0, joystick.Vertical));
        if (Physics.Raycast(ray, out hitWall, 1f, wallLayer))
        {
            moveSpeed = 0f;
            Debug.Log(hitWall.collider.name);
        }
        else moveSpeed = 5f;
        Debug.DrawRay(transform.position, new Vector3(joystick.Horizontal, 0, joystick.Vertical), UnityEngine.Color.red);
        rb.MovePosition(rb.position + moveVector);
    }
    public void SetJoyStick(FloatingJoystick joystick)
    {
        this.joystick = joystick;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
