using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public string frase;
    public Sprite sprite;
}