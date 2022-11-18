using System;
using UnityEngine;

public class ChoiceIndicators : MonoBehaviour
{
    [SerializeField] private TransformChoice[] choiceTransforms;
    
    [Serializable]
    private struct TransformChoice
    {
        public Transform trans;
        public RectTransform choice;
    }
    
    private void LateUpdate()
    {
        foreach (var choiceTrans in choiceTransforms)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(choiceTrans.trans.position);
            if (screenPos.z > 0f && screenPos.x > Screen.width * .05f && screenPos.x < Screen.width *.95f &&
                screenPos.y > Screen.width * .05f && screenPos.y < Screen.height * .95f)
            {
                choiceTrans.choice.position = screenPos;
            }
            else
            {
                if (screenPos.z < 0)
                    screenPos.z *= -1;
                Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0)/2;
                //make 0,0 center of screen instead of bottom left
                screenPos -= screenCenter;
                
                //find angle from center of screen to screen position
                var angle = Mathf.Atan2(screenPos.y, screenPos.x);
                angle -= 90 * Mathf.Deg2Rad;
                var cos = Mathf.Cos(angle);
                var sin = -Mathf.Sin(angle);

                float slope = cos / sin;
                Vector3 screenBounds = screenCenter * .9f;

                if (cos > 0)
                {
                    screenPos = new Vector3(screenBounds.y / slope, screenBounds.y, 0);
                }
                else
                {
                    screenPos = new Vector3(-screenBounds.y / slope, -screenBounds.y, 0);
                }

                if (screenPos.x > screenBounds.x)
                {
                    screenPos = new Vector3(screenBounds.x, screenBounds.x * slope, 0);
                }
                else if (screenPos.x < -screenBounds.x)
                {
                    screenPos = new Vector3(-screenBounds.x, -screenBounds.x * slope, 0);
                }

                screenPos += screenCenter;
                choiceTrans.choice.position = screenPos;
            }
        }
    }
}
