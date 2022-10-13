using UnityEngine;

public static class VectorConverter
{
    public static Vector3 ConvertVector2ToVector3(Vector2 vector, bool convertYtoZ)
    {
        if(convertYtoZ) return new Vector3(vector.x, 0, vector.y);
        
        return (Vector3) vector;
    }
}