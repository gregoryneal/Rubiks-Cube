using UnityEngine;
using System.Collections.Generic;

public class CubeState : MonoBehaviour {

    private CubePiece[,,] State;
    private CubePiece[,,] SolvedState;
    private int sideLength;
    private float spacing;
    private float width;

    public void Create(int sideLength, float spacing, float width, GameObject cube) {
        this.sideLength = sideLength;
        this.spacing = spacing;
        this.width = width;

        PopulateCube(cube);
    }

    public CubePiece[,,] GetState() {
        return State;
    }

    public void SetCube(CubePiece cubeInstance, Vector3Int position) {
        State[position.x, position.y, position.z] = cubeInstance;
        cubeInstance.SetIndex(position);
    }

    public GameObject[,] GetRelativeCubeFaceGameObjects(RelativeCubeFace face) {
        CubePiece[,] cps = GetRelativeCubeFace(face);
        GameObject[,] gos = new GameObject[sideLength, sideLength];
        for(int x = 0; x < sideLength; ++x) {
            for(int y = 0; y < sideLength; ++y) {
                gos[x, y] = cps[x, y].gameObject;
            }
        }
        return gos;
    }

    public CubePiece[,] GetRelativeCubeFace(RelativeCubeFace face) {
        CubePiece[,] objs = new CubePiece[sideLength, sideLength];
        switch (face)
        {
            case RelativeCubeFace.FRONT:
                for (int x = 0; x < sideLength; x++) {
                    for (int y = 0; y < sideLength; y++) {
                        objs[x, y] = GetCubePiece(new Vector3Int(x, y, 0));
                    }
                }
                break;
            case RelativeCubeFace.BACK:
                for (int x = 0; x < sideLength; x++) {
                    for (int y = 0; y < sideLength; y++) {
                        objs[x, y] = GetCubePiece(new Vector3Int(x, y, sideLength - 1));
                    }
                }
                break;
            case RelativeCubeFace.TOP:
                for (int x = 0; x < sideLength; x++) {
                    for (int z = 0; z < sideLength; z++) {
                        objs[x, z] = GetCubePiece(new Vector3Int(x, sideLength - 1, z));
                    }
                }
                break;
            case RelativeCubeFace.BOTTOM:
                for (int x = 0; x < sideLength; x++) {
                    for (int z = 0; z < sideLength; z++) {
                        objs[x, z] = GetCubePiece(new Vector3Int(x, 0, z));
                    }
                }
                break;
            case RelativeCubeFace.LEFT:
                for (int y = 0; y < sideLength; y++) {
                    for (int z = 0; z < sideLength; z++) {
                        objs[y, z] = GetCubePiece(new Vector3Int(0, y, z));
                    }
                }
                break;
            case RelativeCubeFace.RIGHT:
                for (int y = 0; y < sideLength; y++) {
                    for (int z = 0; z < sideLength; z++) {
                        objs[y, z] = GetCubePiece(new Vector3Int(sideLength - 1, y, z));
                    }
                }
                break;                
        }
        return objs;
    }

    private CubePiece GetCubePiece(Vector3Int index) {
        return State[index.x, index.y, index.z];
    }

    public Vector3Int GetIndex(int hash) {
        for(int x = 0; x < sideLength; ++x) {
            for(int y = 0; y < sideLength; ++y) {
                for(int z = 0; z < sideLength; ++z) {
                    Vector3Int pos = new Vector3Int(x, y, z);
                    CubePiece piece = State[x, y, z];
                    if(piece != null) {
                        if (hash == piece.hash) {
                            return pos;
                        }
                    }
                }
            }
        }
        return Vector3Int.negativeOne;
    }

    private void PopulateCube(GameObject cube) {
        State = new CubePiece[sideLength, sideLength, sideLength];

        for (int x = 0; x < sideLength; x++) {
            for (int y = 0; y < sideLength; y++) {
                for (int z = 0; z < sideLength; z++) {
                    bool onFrontFace = z == 0;
                    bool onBackFace = z == sideLength - 1;
                    bool onLeftFace = x == 0;
                    bool onRightFace = x == sideLength - 1;
                    bool onTopFace = y == sideLength - 1;
                    bool onBottomFace = y == 0;
                    bool onSecondHalf = onRightFace || onTopFace || onBackFace;
                    bool onFirstHalf = onLeftFace || onBottomFace || onFrontFace;
                    if (onFirstHalf || onSecondHalf)
                    {
                        GameObject temp = Instantiate(cube, new Vector3(x * (width + spacing), y * (width + spacing), z * (width + spacing)), Quaternion.identity) as GameObject;
                        CubePiece cp = temp.GetComponent<CubePiece>();
                        SetCube(cp, new Vector3Int(x, y, z));
                        temp.transform.SetParent(transform);
                        cp.SetIndex(new Vector3Int(x, y, z));
                        
                        List<RelativeCubeFace> facesOn = new List<RelativeCubeFace>();
                        if(onFrontFace) {
                            facesOn.Add(RelativeCubeFace.FRONT);
                        }
                        if(onBackFace) {
                            facesOn.Add(RelativeCubeFace.BACK);
                        }
                        if(onLeftFace) {
                            facesOn.Add(RelativeCubeFace.LEFT);
                        }
                        if(onRightFace) {
                            facesOn.Add(RelativeCubeFace.RIGHT);
                        }
                        if(onTopFace) {
                            facesOn.Add(RelativeCubeFace.TOP);
                        }
                        if(onBottomFace) {
                            facesOn.Add(RelativeCubeFace.BOTTOM);
                        }

                        cp.SetCubeFaces(facesOn);

                        cp.BuildCubePiece(width);
                    }
                }
            }
        }

        SolvedState = State.Clone() as CubePiece[,,];
    }

    public bool IsSolved() {
        return State == SolvedState;
    }

    //The parameter newFrontFace is the face of the unrotated cube that
    //will be the new front face of the rotated cube
    public void RotateCube(RelativeCubeFace newFrontFace) {
        CubePiece[,,] StateCopy = State.Clone() as CubePiece[,,];
        switch (newFrontFace) {
            case RelativeCubeFace.TOP:
                for(int x = 0; x < sideLength; ++x) {
                    for(int y = 0; y < sideLength; ++y) {
                        for(int z = 0; z < sideLength; ++z) {
                            CubePiece t = StateCopy[x, y, z];
                            if(t != null) {
                                SetCube(StateCopy[x, sideLength - z - 1, y], new Vector3Int(x, y, z));
                            }
                        }
                    }
                }
                break;
            case RelativeCubeFace.BOTTOM:
                for(int x = 0; x < sideLength; ++x) {
                    for(int y = 0; y < sideLength; ++y) {
                        for(int z = 0; z < sideLength; ++z) {
                            CubePiece t = StateCopy[x, y, z];
                            if(t != null) {
                                SetCube(StateCopy[x, z, sideLength - y - 1], new Vector3Int(x, y, z));
                            }
                        }
                    }
                }
                break;
            case RelativeCubeFace.LEFT:
                for(int x = 0; x < sideLength; ++x) {
                    for(int y = 0; y < sideLength; ++y) {
                        for(int z = 0; z < sideLength; ++z) {
                            CubePiece t = StateCopy[x, y, z];
                            if(t != null) {
                                SetCube(StateCopy[z, y, sideLength - x - 1], new Vector3Int(x, y, z));
                            }
                        }
                    }
                }
                break;
            case RelativeCubeFace.RIGHT:
                for(int x = 0; x < sideLength; ++x) {
                    for(int y = 0; y < sideLength; ++y) {
                        for(int z = 0; z < sideLength; ++z) {
                            CubePiece t = StateCopy[x, y, z];
                            if(t != null) {
                                SetCube(StateCopy[sideLength - z - 1, y, x], new Vector3Int(x, y, z));
                            }
                        }
                    }
                }
                break;
        }
    }

    //Rotates a relative cube face by 90 degress in the RotationDirection
    public void RotateRelativeCubeFace(RelativeCubeFace face, RotationDirection direction) {
        CubePiece[,] cubeFace = GetRelativeCubeFace(face);
        int sideLength = cubeFace.GetLength(0);
        CubePiece[,] temp = new CubePiece[sideLength, sideLength];
        if(direction == RotationDirection.CW) {
            for(int x = 0; x < sideLength; x++) {
                for(int y = 0; y < sideLength; y++) {
                    CubePiece t = cubeFace[sideLength - y - 1, x];
                    if (t != null) {
                        temp[x, y] = t;
                    }
                }
            }
        }
        else {
            for(int x = 0; x < sideLength; x++) {
                for(int y = 0; y < sideLength; y++) {
                    CubePiece t = cubeFace[y, sideLength - x - 1];
                    if (t != null) {
                        temp[x, y] = t;
                    }
                }
            }
        }

        ReplaceCubeFace(face, temp);
    }

    private void ReplaceCubeFace(RelativeCubeFace faceToReplace, CubePiece[,] newFace) {
        switch (faceToReplace)
        {
            case RelativeCubeFace.FRONT:
                for (int x = 0; x < sideLength; x++) {
                    for (int y = 0; y < sideLength; y++) {
                        SetCube(newFace[x, y], new Vector3Int(x, y, 0));
                    }
                }
                break;
            case RelativeCubeFace.BACK:
                for (int x = 0; x < sideLength; x++) {
                    for (int y = 0; y < sideLength; y++) {
                        SetCube(newFace[x, y], new Vector3Int(x, y, sideLength - 1));
                    }
                }
                break;
            case RelativeCubeFace.TOP:
                for (int x = 0; x < sideLength; x++) {
                    for (int z = 0; z < sideLength; z++) {
                        SetCube(newFace[x, z], new Vector3Int(x, sideLength - 1, z));
                    }
                }
                break;
            case RelativeCubeFace.BOTTOM:
                for (int x = 0; x < sideLength; x++) {
                    for (int z = 0; z < sideLength; z++) {
                        SetCube(newFace[x, z], new Vector3Int(x, 0, z));
                    }
                }
                break;
            case RelativeCubeFace.LEFT:
                for (int y = 0; y < sideLength; y++) {
                    for (int z = 0; z < sideLength; z++) {
                        SetCube(newFace[y, z], new Vector3Int(0, y, z));
                    }
                }
                break;
            case RelativeCubeFace.RIGHT:
                for (int y = 0; y < sideLength; y++) {
                    for (int z = 0; z < sideLength; z++) {
                        SetCube(newFace[y, z], new Vector3Int(sideLength - 1, y, z));
                    }
                }
                break;
        }
    }
}