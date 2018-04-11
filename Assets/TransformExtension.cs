using UnityEngine;

public static class TransformExtension
{
    public static Quaternion? ToMouseRotation(this Transform transf, float offSet = 0f)
    {
        if (InputController.ICtrl.PointerPosition == null)
            return null;

        Vector2 pointerPos = InputController.ICtrl.PointerPosition.Value;
        Vector2 direction = new Vector2(pointerPos.x - transf.position.x, pointerPos.y - transf.position.y);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, angle + offSet);
    }
}

public static class Vector2Extension
{
    public static Quaternion? ToMouseRotation(this Vector2 position, float offSet = 0f)
    {
        if (InputController.ICtrl.PointerPosition == null)
            return null;

        Vector2 pointerPos = InputController.ICtrl.PointerPosition.Value;
        Vector2 direction = new Vector2(pointerPos.x - position.x, pointerPos.y - position.y);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, angle + offSet);
    }
}
