using System.Collections;
using UnityEngine;

public class Shift : ToTrigger
{
    [System.Serializable]
    public class ShiftData
    {
        public Transform goToShift;
        public Direction direction;
        public float distance;
        public float optionalOverSeconds;
        public float speed;              
    }

    public ShiftData[] actorShift;
    private int i;

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    private Vector2 CalculateEndPos(ShiftData shiftData)
    {
        Vector2 basePos = shiftData.goToShift.position;

        Vector2 dir = shiftData.direction switch
        {
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            _ => throw new System.ArgumentOutOfRangeException()
        };

        return basePos + dir * shiftData.distance;
    }

    public override IEnumerator TriggerFunction()
    {
        CombatEvents.LockPlayerMovement();

        for (i = 0; i < actorShift.Length; i++)
        {
            ShiftData shiftData = actorShift[i];
            Vector3 startPos = shiftData.goToShift.position;
            Vector3 targetPos = CalculateEndPos(shiftData);

            // Use timed Lerp if optionalOverSeconds > 0
            if (shiftData.optionalOverSeconds > 0f)
            {
                float elapsedTime = 0f;

                while (elapsedTime < shiftData.optionalOverSeconds)
                {
                    float t = elapsedTime / shiftData.optionalOverSeconds;
                    shiftData.goToShift.position = Vector3.Lerp(startPos, targetPos, t);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                shiftData.goToShift.position = targetPos;
            }

            // Otherwise, move with fixed speed until target is reached
            else
            {
                while (Vector3.Distance(shiftData.goToShift.position, targetPos) > 0.01f)
                {
                    shiftData.goToShift.position = Vector3.MoveTowards(
                        shiftData.goToShift.position,
                        targetPos,
                        shiftData.speed * Time.deltaTime
                    );

                    yield return null;
                }

                shiftData.goToShift.position = targetPos;
            }
        }
        CombatEvents.UnlockPlayerMovement();
    }
}
