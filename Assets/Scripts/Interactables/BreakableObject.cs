using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    
    public List<GameObject> pieces = new List<GameObject>();

    public void Unbreak()
    {
        if (pieces.Count > 0)
        {
            foreach (var variable in pieces)
            {
                variable.SetActive(true);
            }
        }   
    }
    
    public void Break()
    {
        if (pieces.Count > 0)
        {
            foreach (var variable in pieces)
            {
                variable.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }    
}
