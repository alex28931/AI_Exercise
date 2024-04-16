using UnityEngine;


public enum AreaCorner {
    bottomLeft,
    topLeft,
    bottomRight,
    topRight,
    bottomMiddle,
    topMiddle,
    rightMiddle,
    leftMiddle,
    bottomXZero //legacy for crawler enemies
}

/// <summary>
/// Collect of utilities methods for enemies stuff
/// </summary>
public static class EnemyUtility {

    public static LayerMask groundMask;

    static EnemyUtility() {
        groundMask = LayerMask.GetMask("Ground");
    }

    #region AreaMethod

    /// <summary>
    /// Detect if a point is inside the border of the sprite inside of the sprite renderer.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="spriteRenderer"></param>
    /// <returns></returns>
    public static bool PointInsideArea(Vector3 position, SpriteRenderer spriteRenderer) {
        if (spriteRenderer == null) return false;
        return spriteRenderer.bounds.Contains(position);
    }

    /// <summary>
    /// Return the point of the corner of the canvas of the sprite inside the sprite renderer.
    /// </summary>
    /// <param name="corner">Whic corner?</param>
    /// <param name="spriteRenderer"></param>
    /// <returns></returns>
    public static Vector3 GetCornerPosition(AreaCorner corner, SpriteRenderer spriteRenderer) {
        if (spriteRenderer == null) return Vector3.negativeInfinity;
        switch (spriteRenderer.drawMode) {
            case SpriteDrawMode.Simple:
                Vector2 position;
                switch (corner) {
                    case AreaCorner.bottomLeft:
                        position = spriteRenderer.sprite.bounds.min;
                        break;
                    case AreaCorner.topLeft:
                        position = new Vector3(spriteRenderer.sprite.bounds.min.x, spriteRenderer.sprite.bounds.max.y);
                        break;
                    case AreaCorner.bottomRight:
                        position = new Vector3(spriteRenderer.sprite.bounds.max.x, spriteRenderer.sprite.bounds.min.y);
                        break;
                    case AreaCorner.topRight:
                        position = spriteRenderer.sprite.bounds.max;
                        break;
                    default:
                        return Vector3.negativeInfinity;
                }
                return spriteRenderer.transform.TransformPoint(position);
            default:
                switch (corner) {
                    case AreaCorner.bottomLeft:
                        return new Vector3(spriteRenderer.bounds.center.x - spriteRenderer.bounds.extents.x,
                            spriteRenderer.bounds.center.y - spriteRenderer.bounds.extents.y);
                    case AreaCorner.topLeft:
                        return new Vector3(spriteRenderer.bounds.center.x - spriteRenderer.bounds.extents.x,
                            spriteRenderer.bounds.center.y + spriteRenderer.bounds.extents.y);
                    case AreaCorner.bottomRight:
                        return new Vector3(spriteRenderer.bounds.center.x + spriteRenderer.bounds.extents.x,
                            spriteRenderer.bounds.center.y - spriteRenderer.bounds.extents.y);
                    case AreaCorner.topRight:
                        return new Vector3(spriteRenderer.bounds.center.x + spriteRenderer.bounds.extents.x,
                            spriteRenderer.bounds.center.y + spriteRenderer.bounds.extents.y);
                    default:
                        return Vector3.negativeInfinity;
                }
        }
    }

    public static Vector3 GetCornerPosition(AreaCorner corner, Collider2D collider) {
        if (collider == null) return Vector3.negativeInfinity;
        BoxCollider2D boxCollider = collider as BoxCollider2D;
        if (boxCollider == null) return Vector3.negativeInfinity;
        float top = boxCollider.offset.y + (boxCollider.size.y / 2f);
        float btm = boxCollider.offset.y - (boxCollider.size.y / 2f);
        float left = boxCollider.offset.x - (boxCollider.size.x / 2f);
        float right = boxCollider.offset.x + (boxCollider.size.x / 2f);
        switch (corner) {
            case AreaCorner.bottomLeft:
                return collider.transform.TransformPoint(new Vector3(left, btm, 0));
            case AreaCorner.bottomRight:
                return collider.transform.TransformPoint(new Vector3(right, btm, 0));
            case AreaCorner.topLeft:
                return collider.transform.TransformPoint(new Vector3(left, top, 0));
            case AreaCorner.topRight:
                return collider.transform.TransformPoint(new Vector3(right, top, 0));
            case AreaCorner.bottomMiddle:
                return collider.transform.TransformPoint(new Vector3(boxCollider.offset.x, btm, 0));
            case AreaCorner.bottomXZero:
                return collider.transform.TransformPoint(new Vector3(0, btm, 0));
            case AreaCorner.topMiddle:
                return collider.transform.TransformPoint(new Vector3(boxCollider.offset.x, top, 0));
            case AreaCorner.rightMiddle:
                return collider.transform.TransformPoint(new Vector3(right, boxCollider.offset.y, 0));
            case AreaCorner.leftMiddle:
                return collider.transform.TransformPoint(new Vector3(left, boxCollider.offset.y, 0));
            default:
                return Vector3.negativeInfinity;
        }
    }

    public static bool Approximately(float a, float b, float tollerance = 0.1f) {
        return Mathf.Abs(a - b) <= tollerance;
    }

    public static float[] FromObjectArrayToFloatArray(object[] array) {
        float[] floatArray = new float[array.Length];
        for (int i = 0; i < floatArray.Length; i++) {
            floatArray[i] = (float)array[i];
        }
        return floatArray;
    }

    public static bool PointUnderCollider(Vector3 point, BoxCollider2D collider) {
        Vector3 bottomLeft = GetCornerPosition(AreaCorner.bottomLeft, collider);
        Vector3 bottomRight = GetCornerPosition(AreaCorner.bottomRight, collider);
        float minX = bottomLeft.x < bottomRight.x ? bottomLeft.x : bottomRight.x;
        float maxX = bottomLeft.x > bottomRight.x ? bottomLeft.x : bottomRight.x;
        return bottomLeft.y >= point.y && point.x >= minX && point.x <= maxX;
    }

    public static Vector3 PerformVelocityToTarget(Vector3 position, Vector3 targetPosition,
        float startYVel, float gravityScale, float minXVelocity = 0, float maxXVelocity = Mathf.Infinity,
        float gravityMultiplier = 1) {
        float yMax = (startYVel * startYVel) / (gravityMultiplier * Physics2D.gravity.y * gravityScale);
        float flyTime = startYVel / (Physics2D.gravity.y * gravityScale);
        flyTime += yMax / 20f;
        float startXVel = Mathf.Abs(position.x - targetPosition.x) / flyTime;
        startXVel = Mathf.Abs(startXVel);
        startXVel = startXVel > maxXVelocity ? maxXVelocity :
            startXVel < minXVelocity ? minXVelocity : startXVel;
        return new Vector3(startXVel, startYVel, 0);
    }


    public static AnimationCurve AnimationCurveFactory(float amplitude, float frequency) {
        AnimationCurve curve = new AnimationCurve();
        float timeSlice = 1 / frequency;
        float currentTime = 0;
        bool setAmplitude = true;
        while (currentTime <= 1) {
            curve.AddKey(currentTime, setAmplitude ? amplitude : 0);
            currentTime += timeSlice;
            setAmplitude = !setAmplitude;
        }
        return curve;
    }
    #endregion


}
