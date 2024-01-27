using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "FraseDipenny", menuName = "Dialoghi/Frasi/FraseDiPenny")]
public class FrasiDiPinny : ScriptableObject
{
    public List<FrasiPenny> frasi = new List<FrasiPenny>();

    public bool hasReply = false;

    public Risposte risposta1;
    public Risposte risposta2;
    public Risposte risposta3;

}

[Serializable]
public class FrasiPenny
{
    public List<string> frase;
    public Sprite sprite;

    public string GetPrhase()
    {
        return frase[Random.Range(0, frase.Count)];
    }
}