using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUpdater : MonoBehaviour
{
	private TextMeshProUGUI textMesH;     //links
    private TextMeshProUGUI textMesH2;  //rechts
	private int p1;                    //links
	private int p2;                    //rechts

    // Start is called before the first frame update
    void Start()
    {
        textMesH = GameObject.Find("Punktestand_p1").GetComponent<TextMeshProUGUI>();
    	textMesH2 = GameObject.Find("Punktestand_p2").GetComponent<TextMeshProUGUI>();
    
    	p1 = 0;
    	p2 = 0;
    }

    // Update is called once per frame
    /*
    void Update()
    {
		textMesH.text = p1.ToString() + ":" + p2.ToString();
    }
    */

    public void AddP1(){
    	p1++;
    }

    public void AddP2(){
    	p2++;
    }

    public void ScoreAusgeben(){
    	textMesH.text = p1.ToString();
        textMesH2.text = p2.ToString();
       // textMesH.text = textMesH.text.Replace(textMesH.text [0].ToString(), "<color=#ff0000ff>" + textMesH.text [0].ToString() + "</color>");       //rot
        //textMesH2.text = textMesH2.text.Replace(textMesH2.text [0].ToString(), "<color=#0000ffff>" + textMesH2.text [0].ToString() + "</color>");      //blau
        Debug.Log(p1.ToString() + ":" + p2.ToString());
    }
}
