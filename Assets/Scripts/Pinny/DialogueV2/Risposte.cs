using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Risposta", menuName = "Dialoghi/Frasi/Risposta")]
public class Risposte : ScriptableObject
{
    public List<string> risposte = new List<string>() {"risp1","risp2","risp3"};

    public FrasiDiPinny pinnyReply;

    internal string GetReply()
    {
        return risposte[Random.Range(0, risposte.Count)];
    }
}
