using System.Collections;

public class Vector3Int {

    public int x { get; private set; }
    public int y { get; private set; }
    public int z { get; private set; }

    public static Vector3Int zero = new Vector3Int(0, 0, 0);
    public static Vector3Int negativeOne = new Vector3Int(-1, -1, -1);

    public Vector3Int(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override string ToString() {
        return "(" + x + ", " + y + ", " + z + ")";
    }
}
