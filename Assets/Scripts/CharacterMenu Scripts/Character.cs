using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    public Camera DefaultCamera;//Camera for this character the main camera will jump to
    public int ID;//id for this character starting at 0, maybe wont be used if the controller uses names instead, and names correspond to the model name
    public string name;//like Mars and stuff, capitalize the first letter. models will use the same name
    public GameObject model;//not sure if we will need linked here
    public GameObject nameSprite;//^^

    void Start()
    {
        //    :) hi lucas
        
    }



}
