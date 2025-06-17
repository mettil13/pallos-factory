using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnviromentMove : MonoBehaviour
{
    [SerializeField] float windCycleTime = 20;
    [SerializeField] Light lightComponent;
    [SerializeField] Vector2 movementDirection;

    public void Update()
    {
        UniversalAdditionalLightData lightData = lightComponent.GetUniversalAdditionalLightData();
        Vector2 size = lightData.lightCookieSize;
        Vector2 currentOffset = lightData.lightCookieOffset;
        movementDirection.Normalize();
        Vector2 movementToDo = new Vector2(
            ((movementDirection.x * size.x) / windCycleTime) * Time.deltaTime,
            ((movementDirection.y * size.y) / windCycleTime) * Time.deltaTime
            );
        currentOffset += movementToDo;

        if (Mathf.Abs(currentOffset.x) >= size.x)
            currentOffset.x -= Mathf.Sign(currentOffset.x) * size.x;
        if (Mathf.Abs(currentOffset.y) >= size.y)
            currentOffset.y -= Mathf.Sign(currentOffset.y) * size.y;

        lightData.lightCookieOffset = currentOffset;
    }
}
