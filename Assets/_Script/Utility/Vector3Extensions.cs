using UnityEngine;

public static class Vector3Extensions
{
    /// <summary>
    /// ������ Y���� 0���� �����մϴ�.
    /// </summary>
    public static Vector3 FlattenY(this Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);
    }
}
