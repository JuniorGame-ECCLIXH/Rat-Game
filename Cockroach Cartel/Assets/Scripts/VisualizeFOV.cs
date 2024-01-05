using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyFOV))]

public class VisualizeFOV : Editor
{
    private void OnSceneGUI()
    {
        //Visualizes the radius that an enemy can see the player from
        EnemyFOV fov = (EnemyFOV)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.fovRadius);

        //Gets the view angle of the enemies FOV
        Vector3 viewAngle1 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.fovAngle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.fovAngle / 2);

        //Visualizes the enemies fov view angle
        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle1 * fov.fovRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle2 * fov.fovRadius);

        //If the enemy can see the player draw a line towards the player
        if(fov.canSeePlayer) 
        {
            Handles.color = Color.red;
            Handles.DrawLine(fov.transform.position, fov.player.transform.position);
        }
    }

    //Calculates the angle that the enemy can view from based on the enemies FOV view angle
    private Vector3 DirectionFromAngle(float eulerY, float degreeAngle)
    {
        degreeAngle += eulerY;

        return new Vector3(Mathf.Sin(degreeAngle * Mathf.Deg2Rad), 0, Mathf.Cos(degreeAngle * Mathf.Deg2Rad));
    }
}
