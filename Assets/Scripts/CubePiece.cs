using UnityEngine;
using System.Collections.Generic;

public class CubePiece : MonoBehaviour {

    public int hash;
    public Vector3Int index;
    public CubeState state;
    private List<RelativeCubeFace> initCubeFaces = new List<RelativeCubeFace>();
    public GameObject quad;
    private bool facesBuilt = false;
    private List<GameObject> faces = new List<GameObject>();

    void Start() {
        state = GetComponentInParent<Cube>().GetCubeState();
        hash = gameObject.GetInstanceID().GetHashCode();
    }

    public void SetIndex(Vector3Int newIndex) {
        index = newIndex;
    }

    public void SetCubeFaces(List<RelativeCubeFace> faces) {
        initCubeFaces = faces;
    }

    void OnMouseDown() {
        Debug.Log(index);
    }

    public void BuildCubePiece(float width) {
        if(!facesBuilt) {
            CubeMaterials materials = GetComponentInParent<CubeMaterials>();
            GameObject temp;
            if (initCubeFaces.Contains(RelativeCubeFace.FRONT)) {
                temp = Instantiate(quad, transform.position + (new Vector3(0, 0, -width/2f)), Quaternion.identity) as GameObject;
                temp.transform.SetParent(transform);
                faces.Add(temp);
                temp.GetComponent<MeshRenderer>().material = materials.white;
            }
            else {
                temp = Instantiate(quad, transform.position + (new Vector3(0, 0, -width/2f)), Quaternion.identity) as GameObject;
                temp.transform.SetParent(transform);
                faces.Add(temp);
                temp.GetComponent<MeshRenderer>().material = materials.black;
            }
            if(initCubeFaces.Contains(RelativeCubeFace.BACK)) {
                temp = Instantiate(quad, transform.position + (new Vector3(0, 0, width/2f)), Quaternion.Euler(0, 180f, 0)) as GameObject;
                temp.transform.SetParent(transform);
                faces.Add(temp);
                temp.GetComponent<MeshRenderer>().material = materials.yellow;
            }
            else {
                temp = Instantiate(quad, transform.position + (new Vector3(0, 0, width/2f)), Quaternion.Euler(0, 180f, 0)) as GameObject;
                temp.transform.SetParent(transform);
                faces.Add(temp);
                temp.GetComponent<MeshRenderer>().material = materials.black;
            }
            if(initCubeFaces.Contains(RelativeCubeFace.LEFT)) {
                temp = Instantiate(quad, transform.position + (new Vector3(-width/2f, 0, 0)), Quaternion.Euler(0, 90f, 0)) as GameObject;
                temp.transform.SetParent(transform);
                faces.Add(temp);
                temp.GetComponent<MeshRenderer>().material = materials.green;
            }
            else {
                temp = Instantiate(quad, transform.position + (new Vector3(-width/2f, 0, 0)), Quaternion.Euler(0, 90f, 0)) as GameObject;
                temp.transform.SetParent(transform);
                faces.Add(temp);
                temp.GetComponent<MeshRenderer>().material = materials.black;
            }
            if(initCubeFaces.Contains(RelativeCubeFace.RIGHT)) {
                temp = Instantiate(quad, transform.position + (new Vector3(width/2f, 0, 0)), Quaternion.Euler(0, 270f, 0)) as GameObject;
                temp.transform.SetParent(transform);
                faces.Add(temp);
                temp.GetComponent<MeshRenderer>().material = materials.blue;
            }
            else {
                temp = Instantiate(quad, transform.position + (new Vector3(width/2f, 0, 0)), Quaternion.Euler(0, 270f, 0)) as GameObject;
                temp.transform.SetParent(transform);
                faces.Add(temp);
                temp.GetComponent<MeshRenderer>().material = materials.black;
            }
            if(initCubeFaces.Contains(RelativeCubeFace.TOP)) {
                temp = Instantiate(quad, transform.position + (new Vector3(0, width/2f, 0)), Quaternion.Euler(90f, 0, 0)) as GameObject;
                temp.transform.SetParent(transform);
                faces.Add(temp);
                temp.GetComponent<MeshRenderer>().material = materials.orange;
            }
            else {
                temp = Instantiate(quad, transform.position + (new Vector3(0, width/2f, 0)), Quaternion.Euler(90f, 0, 0)) as GameObject;
                temp.transform.SetParent(transform);
                faces.Add(temp);
                temp.GetComponent<MeshRenderer>().material = materials.black;
            }
            if(initCubeFaces.Contains(RelativeCubeFace.BOTTOM)) {
                temp = Instantiate(quad, transform.position + (new Vector3(0, -width/2f, 0)), Quaternion.Euler(270f, 0, 0)) as GameObject;
                temp.transform.SetParent(transform);
                faces.Add(temp);
                temp.GetComponent<MeshRenderer>().material = materials.red;
            }
            else {
                temp = Instantiate(quad, transform.position + (new Vector3(0, -width/2f, 0)), Quaternion.Euler(270f, 0, 0)) as GameObject;
                temp.transform.SetParent(transform);
                faces.Add(temp);
                temp.GetComponent<MeshRenderer>().material = materials.black;
            }

            facesBuilt = true;
        }
    }
}
