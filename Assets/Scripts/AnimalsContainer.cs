using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Animals")]
public class AnimalsContainer : ScriptableObject
{
    
    [SerializeField] public List<Animal> Animals = new List<Animal>() ;
}
