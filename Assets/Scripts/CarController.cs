using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeuralNetwork;

public class CarController : MonoBehaviour
{
    public bool feed_forward = false;
    public DFFNeuralNetwork neural_net;
    private Camera input;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private bool isBreaking;
    private float steerAngle;

    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    public float maxSteeringAngle = 30f;
    public float motorForce = 50f;
    public float brakeForce = 0f;

    public float checkpoint_score = 0.0F;
    private List<string> checkpoints_encountered = new List<string>();


    private void FixedUpdate()
    {

        gameObject.GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, 0f, 1f);
        //GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput(float[] nnInput)
    {
        //horizontalInput = Input.GetAxis("Horizontal");
        //verticalInput = Input.GetAxis("Vertical");
        //isBreaking = Input.GetKey(KeyCode.Space);
        horizontalInput = nnInput[0];
        verticalInput = nnInput[1];
        isBreaking = (nnInput[2] > 0f) ? true : false;
    }

    private void HandleSteering()
    {
        steerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steerAngle;
        frontRightWheelCollider.steerAngle = steerAngle;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        brakeForce = isBreaking ? 3000f : 0f;
        frontLeftWheelCollider.brakeTorque = brakeForce;
        frontRightWheelCollider.brakeTorque = brakeForce;
        rearLeftWheelCollider.brakeTorque = brakeForce;
        rearRightWheelCollider.brakeTorque = brakeForce;
    }

    private void UpdateWheels()
    {
        UpdateWheelPos(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPos(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelPos(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheelPos(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateWheelPos(WheelCollider wheelCollider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        trans.rotation = rot;
        trans.position = pos;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (checkpoints_encountered.Contains(collision.gameObject.name))
        {
            Debug.Log("Collided with previous checkpoint");
            checkpoint_score += -0.5f;
        } else
        {
            if (collision.gameObject.name == "Checkpoint 1")
            {
                Debug.Log("Collided with checkpoint 1");
                checkpoints_encountered.Add("Checkpoint 1");
                checkpoint_score += 1f;
            }
            if (collision.gameObject.name == "Checkpoint 2")
            {
                Debug.Log("Collided with checkpoint 2");
                checkpoints_encountered.Add("Checkpoint 2");
                checkpoint_score += 1f;
            }
            if (collision.gameObject.name == "Checkpoint 3")
            {
                Debug.Log("Collided with checkpoint 3");
                checkpoints_encountered.Add("Checkpoint 3");
                checkpoint_score += 1f;
            }
            if (collision.gameObject.name == "Checkpoint 4")
            {
                Debug.Log("Collided with checkpoint 4");
                checkpoints_encountered.Add("Checkpoint 4");
                checkpoint_score += 1f;
            }
            if (collision.gameObject.name == "Checkpoint 5")
            {
                Debug.Log("Collided with checkpoint 5");
                checkpoints_encountered.Add("Checkpoint 5");
                checkpoint_score += 1f;
            }
            if (collision.gameObject.name == "Checkpoint 6")
            {
                Debug.Log("Collided with checkpoint 6");
                checkpoints_encountered.Add("Checkpoint 6");
                checkpoint_score += 1f;
            }
            if (collision.gameObject.name == "Checkpoint 7")
            {
                Debug.Log("Collided with checkpoint 7");
                checkpoints_encountered.Add("Checkpoint 7");
                checkpoint_score += 1f;
            }
            if (collision.gameObject.name == "Checkpoint 8")
            {
                Debug.Log("Collided with checkpoint 8");
                checkpoints_encountered.Add("Checkpoint 8");
                checkpoint_score += 2f;
            }
        }
    }

    void Start()
    {
        this.input = camera_obj.GetComponent<Camera>();
        this.input.targetTexture = new RenderTexture(50, 50, 24);
        // setting frame rate to 30 frames per second
        Application.targetFrameRate = 30;
    }

    void Update()
    {
        Texture2D frame = new Texture2D(50, 50, TextureFormat.RGB24, false);
        this.input.Render();
        RenderTexture.active = this.input.targetTexture;
        frame.ReadPixels(new Rect(0, 0, 50, 50), 0, 0);

        float frame_mat = new float[2500];
        int index = 0;
        for (int x = 0; x < 50; x++)
        {
            for (int y = 0; y < 50; y++)
            {
                frame_mat[index] = frame.GetPixel(x, y).grayscale;
                index++;
            }
        }
        float[] inf = this.neural_net.feedForward(frame_mat);
    }

    public void InsertBrain(DFFNeuralNetwork nnet)
    {
        this.neural_net = nnet;
    }

    void Update()
    {
        //Run model by taking screenshot and then passing it in to the model
        //retrieve output from model and then 

    }
}
