using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueData
{
    public bool haveReplyOptions = false;
    public string pinnyText;
    public OptionLink option1;
    public OptionLink option2;
    public OptionLink option3;

    public DialogueData(bool haveReplyOptions, string pinnyText, OptionLink option1, OptionLink option2, OptionLink option3)
    {
        this.haveReplyOptions = haveReplyOptions;
        this.pinnyText = pinnyText;
        this.option1 = option1;
        this.option2 = option2;
        this.option3 = option3;
    }
}


[Serializable]
public class  OptionLink
{
    public string text;
    public int targetIndex;
}