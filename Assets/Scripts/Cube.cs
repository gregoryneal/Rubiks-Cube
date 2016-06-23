using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {

    public float spacing = 1f;
    [Range(2, 15)]
    public int side_length = 3;
    public GameObject cube;
    private float width;
    private Vector3 cube_centroid;
    private const float rotation_time = 0.2f;
    private bool rotating = false;
    private CubeState State;
    public GameObject emptyRotator;

    private Quaternion downRotation = Quaternion.Euler(-Vector3.right * 90);
    private Quaternion upRotation = Quaternion.Euler(Vector3.right * 90);
    private Quaternion leftRotation = Quaternion.Euler(Vector3.up * 90);
    private Quaternion rightRotation = Quaternion.Euler(-Vector3.up * 90);

    void Update() {
        if (!rotating) {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                State.RotateCube(RelativeCubeFace.TOP);
                StartCoroutine(RotateCube(transform.rotation, downRotation * transform.rotation));
                goto end_of_input;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                State.RotateCube(RelativeCubeFace.BOTTOM);
                StartCoroutine(RotateCube(transform.rotation, upRotation * transform.rotation));
                goto end_of_input;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                State.RotateCube(RelativeCubeFace.RIGHT);
                StartCoroutine(RotateCube(transform.rotation, leftRotation * transform.rotation));
                goto end_of_input;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                State.RotateCube(RelativeCubeFace.LEFT);
                StartCoroutine(RotateCube(transform.rotation, rightRotation * transform.rotation));
                goto end_of_input;
            }
            //Rotate top slice clockwise
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)){
                State.RotateRelativeCubeFace(RelativeCubeFace.TOP, RotationDirection.CW);
                StartCoroutine(RotateFace(RelativeCubeFace.TOP, RotationDirection.CW));
                goto end_of_input;
            }
            //Rotate top slice counterclockwise
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftShift)) {
                State.RotateRelativeCubeFace(RelativeCubeFace.TOP, RotationDirection.CCW);
                StartCoroutine(RotateFace(RelativeCubeFace.TOP, RotationDirection.CCW));
                goto end_of_input;
            }
            //Rotate bottom slice clockwise
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S)){
                State.RotateRelativeCubeFace(RelativeCubeFace.BOTTOM, RotationDirection.CW);
                StartCoroutine(RotateFace(RelativeCubeFace.BOTTOM, RotationDirection.CW));
                goto end_of_input;
            }
            //Rotate bottom slice counterclockwise
            if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.LeftShift)) {
                State.RotateRelativeCubeFace(RelativeCubeFace.BOTTOM, RotationDirection.CCW);
                StartCoroutine(RotateFace(RelativeCubeFace.BOTTOM, RotationDirection.CCW));
                goto end_of_input;
            }
            //Rotate right slice clockwise
            if (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.LeftShift)) {
                State.RotateRelativeCubeFace(RelativeCubeFace.RIGHT, RotationDirection.CCW);
                StartCoroutine(RotateFace(RelativeCubeFace.RIGHT, RotationDirection.CW));
                goto end_of_input;
            }
            //Rotate right slice counterclockwise
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftShift)) {
                State.RotateRelativeCubeFace(RelativeCubeFace.RIGHT, RotationDirection.CW);
                StartCoroutine(RotateFace(RelativeCubeFace.RIGHT, RotationDirection.CCW));
                goto end_of_input;
            }
            //Rotate left slice clockwise
            if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.LeftShift)) {
                State.RotateRelativeCubeFace(RelativeCubeFace.LEFT, RotationDirection.CCW);
                StartCoroutine(RotateFace(RelativeCubeFace.LEFT, RotationDirection.CW));
                goto end_of_input;
            }
            //Rotate left slice counterclockwise
            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftShift)) {
                State.RotateRelativeCubeFace(RelativeCubeFace.LEFT, RotationDirection.CW);
                StartCoroutine(RotateFace(RelativeCubeFace.LEFT, RotationDirection.CCW));
                goto end_of_input;
            }
            //Rotate front face clockwise
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.E)) {
                State.RotateRelativeCubeFace(RelativeCubeFace.FRONT, RotationDirection.CW);
                StartCoroutine(RotateFace(RelativeCubeFace.FRONT, RotationDirection.CW));
                goto end_of_input;
            }
            //Rotate front face counterclockwise
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Q)) {
                State.RotateRelativeCubeFace(RelativeCubeFace.FRONT, RotationDirection.CCW);
                StartCoroutine(RotateFace(RelativeCubeFace.FRONT, RotationDirection.CCW));
                goto end_of_input;
            }
        }
        end_of_input:;

        Debug.Log(State.IsSolved());
    }

    private IEnumerator RotateCube(Quaternion from, Quaternion to, float time = rotation_time) {
        rotating = true;
        float t = 0;
        while (t <= time) {
            transform.rotation = Quaternion.Slerp(from, to, t / time);
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }
        transform.rotation = to;
        rotating = false;
    }

    private IEnumerator RotateFace(RelativeCubeFace face, RotationDirection direction, float time = rotation_time) {
        rotating = true;
        float t = 0;
        GameObject[,] objs = State.GetRelativeCubeFaceGameObjects(face);
        Vector3 centerOfSlice = cube_centroid;
        Vector3 rotationAxis = Vector3.up;
        GameObject rotator = Instantiate(emptyRotator, centerOfSlice, Quaternion.identity) as GameObject;
        rotator.transform.SetParent(transform);
        SetSliceParent(objs, rotator.transform);

        if(face == RelativeCubeFace.RIGHT || face == RelativeCubeFace.LEFT) {
            rotationAxis = Vector3.right;
        }
        if(face == RelativeCubeFace.FRONT) {
            rotationAxis = -Vector3.forward;
        }

        Quaternion from = rotator.transform.rotation;
        Quaternion to = Quaternion.Euler((int)direction * rotationAxis * 90) * from;

        while (t <= time) {
            rotator.transform.rotation = Quaternion.Slerp(from, to, t / time);
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
        }

        rotator.transform.rotation = to;
        SetSliceParent(objs, transform);
        Destroy(rotator.gameObject);
        rotating = false;
    }

    private void SetSliceParent(GameObject[,] objects, Transform newParent) {
        for(int x = 0; x < objects.GetLength(0); ++x) {
            for(int y = 0; y < objects.GetLength(1); ++y) {
                if(objects[x, y] != null) {
                    objects[x, y].transform.SetParent(newParent);
                }
            }
        }
    }

    void Start() {
        if (cube)
        {   
            width = 1f;
            State = GetComponent<CubeState>();
            State.Create(side_length, spacing, width, cube);
            cube_centroid = GetCenterOfRubiks();
            CenterCube();
            cube_centroid = GetCenterOfRubiks();
        }
        else {
            throw new MissingReferenceException("Please set the cube gameobject!");
        }
    }

    private void CenterCube() {
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).transform.position -= cube_centroid;
        }
    }

    private Vector3 GetCenterOfRubiks() {
        int n = transform.childCount;
        Vector3 sum = Vector3.zero;
        for(int i = 0; i < n; i++)
        {
            sum += transform.GetChild(i).transform.position;
        }
        return 1.0f * sum / n;
    }

    private Vector3 GetCenterOfSlice(GameObject[,] objects) {
        int count = 0;
        Vector3 sum = Vector3.zero;

        for(int x = 0; x < objects.GetLength(0); ++x) {
            for(int y = 0; y < objects.GetLength(1); ++y) {
                if (objects[x, y] != null) {
                    sum += objects[x, y].transform.position;
                    count++;
                }
            }
        }

        return 1.0f * sum / count;
    }

    public CubeState GetCubeState() {
        return State;
    }
}
