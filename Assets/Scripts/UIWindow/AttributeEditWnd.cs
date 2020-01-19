using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocol;
public class AttributeEditWnd : WindowRoot
{
    public InputField hpInput;
    public InputField adInput;
    public Button okBtn;

    void Start()
    {
        okBtn.onClick.AddListener(SetAttribute);
    }

    private void SetAttribute()
    {
        string hp = hpInput.text;
        string ad = adInput.text;
        PlayerData data = PlayerUtil.LoadLocalPlayerData();
        if(!string.IsNullOrEmpty(hp))
        {
            int hpNum = Int32.Parse(hp);
            if(hpNum > 0 && hpNum < 9999)
            {
                data.hp = hpNum;
            }
        }
        if(!string.IsNullOrEmpty(ad))
        {
            int adNum = Int32.Parse(ad);
            if(adNum >0 || adNum < 999)
            {
                data.ad = adNum;
            }
        }
        PlayerUtil.SetLocalPlayerData(data);
        SetWndState(false);
    }
}
