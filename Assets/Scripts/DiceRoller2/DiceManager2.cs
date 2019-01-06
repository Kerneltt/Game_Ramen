using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager2 : MonoBehaviour {
    Dice[] dices;
    float force = 100;
    [SerializeField]
    GameObject[] die;
    [SerializeField]
    LayerMask mask;
    [SerializeField]
    GameObject collorpickerIMG;
    [SerializeField]
    Texture2D collors;
    [SerializeField]
    GameObject collorpickerButton;
    GameObject sellectedDice;
    public Vector2 startPos;
    public Vector2 direction;
    bool doubletapTimer=false;
    int tapcounter=0;
    int trayindex =0;
    Vector3 cameraDirection;

    [SerializeField]
    List<GameObject> trays;
    [SerializeField]
    GameObject currentTray;
    [SerializeField]
    GameObject trayfab;
    bool boolDirection;  

    //fling
    Vector2 startPosF, endPosf, directionf;
    float touchTimeStart, TouchTimeFinish, timeInterval;
    [SerializeField]
    float throwzx = 50;
    Rigidbody rb;

    // Use this for initialization
    void Start () {
        for (int i = 1; i < 20; i++){
            GameObject newBoard =Instantiate(trayfab);
            float newx = i * 17f;
            newBoard.transform.position = new Vector3(newx, 0, 0);
            trays.Add(newBoard); 
        }

        currentTray = trays[0];
        trayindex = 0;
        cameraDirection = new Vector3(currentTray.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }

    public void Setcollor()
    {
        RectTransform rt = collorpickerIMG.GetComponent<RectTransform>();        
        float width = rt.rect.width;
        float height = rt.rect.height;
        Touch touch = Input.GetTouch(0);        
        print("X: " + (collorpickerIMG.transform.InverseTransformPoint(touch.position).x));
        print("Y:" +  (collorpickerIMG.transform.InverseTransformPoint(touch.position).y));
        print("X trans: " + ((((collorpickerIMG.transform.InverseTransformPoint(touch.position).x +width/2)* collors.width) + (width / 2)) / width));
        print("Y trans:" + (((((collorpickerIMG.transform.InverseTransformPoint(touch.position).y +height/2)* collors.height) + (height / 2)) / height)));
        int x = (int)((((collorpickerIMG.transform.InverseTransformPoint(touch.position).x + width / 2) * collors.width) + (width / 2)) / width);
        int y = (int)((((collorpickerIMG.transform.InverseTransformPoint(touch.position).y + height / 2) * collors.height) + (height / 2)) / height);
        collorpickerButton.GetComponent<Image>().color = collors.GetPixel(x, y);
        


    }
    public void CancelTap()
    {
        doubletapTimer = false;
        tapcounter = 0;
    }
    public void SpawnDice(int dice)
    {
        CancelTap();
        if (currentTray.GetComponentsInChildren<Dice>().Length<20)
        {
            print("creatingDice");
            GameObject newdice = Instantiate(die[dice]);
            newdice.GetComponentInChildren<Renderer>().material.color = collorpickerButton.GetComponent<Image>().color;
            newdice.transform.SetParent(currentTray.transform);
            newdice.transform.localPosition = new Vector3(0, 5, 0);
        }        
    }
/*
    public void FixedUpdate(){        
        float smoothSpeed = 0.125f; 
        Vector3 offset; 
        if(boolDirection){
            offset = new Vector3(17f, 0, 0); 
        }
        else{
            offset = new Vector3(-17f, 0, 0); 
        } 

        Vector3 desiredPosition = currentTray.transform.position + offset;
        print(desiredPosition);
        // print(currentTray.transform.position.x);
        // print(currentTray.transform.position.y);
        // print(currentTray.transform.position.z); 

        Camera.main.transform.Translate(currentTray.transform.position * Time.deltaTime);
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 20.7f, 0); 
    }
   */
    // Update is called once per frame
    void Update () {

        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, cameraDirection,  50 * Time.deltaTime);

        Vector3 dir  = Vector3.zero;
        dir.x = -Input.acceleration.x;
        dir.z = Input.acceleration.z;
        dir.y = Input.acceleration.y;
        //print(dir);
        //Unlock all dice
        if (tapcounter>0)
        {
            tapcounter--;
            if (tapcounter==0 && doubletapTimer==true)
            {
                doubletapTimer = false;
                foreach (Dice dice in currentTray.GetComponentsInChildren<Dice>())
                {
                    dice.UnlockDice();
                }
                print("Unlockall");
            }
        }
        //Troww all dice
        if (dir.sqrMagnitude > 5)
        {
            dices =currentTray.GetComponentsInChildren<Dice>();
            dir.Normalize();
            foreach (Dice diceToRoll in dices)
            {                
                diceToRoll.RollDice();
               // diceToRoll.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
            }
        }

        //tilt dice on tray
        if (dir.x>1)
        {
            /*
            foreach (Dice dice in currentTray.GetComponentsInChildren<Dice>())
            {
                dice.gameObject.GetComponent<Rigidbody>().AddForce
            }
            */
        }
        
        if (Input.touchCount>0)
        {            
            if ((Input.GetTouch(0).phase == TouchPhase.Began))
            {
                //detect dice touch or not
                Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit raycastHit;
                if (Physics.Raycast(raycast, out raycastHit, 20f, mask))
                {

                    //dice detected
                    if (raycastHit.collider.tag == "Dice" && raycastHit.collider.gameObject.GetComponent<Dice>()!=null)
                    {
                        //lock/unlock dice
                        startPos = Input.GetTouch(0).position;
                        if (raycastHit.collider.gameObject.GetComponent<Dice>().locked)
                        {
                            raycastHit.collider.gameObject.GetComponent<Dice>().UnlockDice();
                        }
                        else
                        {
                            raycastHit.collider.gameObject.GetComponent<Dice>().LockDice();
                        }
                        //show interest in dice
                        sellectedDice = raycastHit.collider.gameObject;
                        touchTimeStart = Time.time;
                        startPosF = Input.GetTouch(0).position;
                        rb = sellectedDice.GetComponent<Rigidbody>();
                    }
                    //dice undetected
                    else
                    {
                        //roll all dice on double tap
                        if (doubletapTimer == true)
                        {
                            dices = currentTray.GetComponentsInChildren<Dice>();                            
                            foreach (Dice diceToRoll in dices)
                            {
                                diceToRoll.RollDice();
                                // diceToRoll.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
                            }
                            doubletapTimer = false;
                            tapcounter = 0;
                            print("TapRoll");
                        }
                        //detect first tap
                        else
                        if (doubletapTimer==false)
                        {
                            doubletapTimer = true;
                            tapcounter = 50;
                            print("TapOnce");
                        }
                                      
                        startPos = Input.GetTouch(0).position;
                    }
                }
            }
            //move selected dice
            if (Input.GetTouch(0).phase==TouchPhase.Moved && sellectedDice!=null)
            {
                if (Vector2.Distance(startPos,Input.GetTouch(0).position)>5)
                {
                    sellectedDice.GetComponent<Dice>().Setkillable(true);
                    sellectedDice.GetComponent<Dice>().UnlockDice();
                    sellectedDice.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    sellectedDice.transform.position = (Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 10)));
                }                
            }
            //starting position of finger
            if (Input.GetTouch(0).phase == TouchPhase.Moved && sellectedDice == null)
            {
                //prender bandera swipe
                //Guardar posicion actual por inicio de swipe
                startPos = Input.GetTouch(0).position;
            }
            //swipe ended
            if (Input.GetTouch(0).phase == TouchPhase.Ended && sellectedDice == null)
            {
                //swipe
                //Revisar, con el if anterior, si el swipe fue suficientemente fuerte para que sea creado
                if (Vector2.Distance(startPos,Input.GetTouch(0).position)>5)
                {
                    //revisar la direccion
                    //Derecha: --> 
                    if(startPos.x < Input.GetTouch(0).position.x){
                        //Ver en la lista si existen boards anteriores al actual.
                        if(trayindex > 0){
                            //Si existe
                            //Cambiar el Currenttray al anterior
                            trayindex = trayindex - 1; 
                            currentTray = trays[trayindex];
                            cameraDirection = new Vector3(currentTray.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);


                            //Revisar si el tray actual no tenia dados
                            //GameObject.GetComponentsInChilden<Dice>()
                            if (trays[trayindex + 1].GetComponentsInChildren<Dice>().Length < 1)
                            {
                                
                                int borradoIndex = trayindex + 1; 
                                int finalIndex =  trays.Count - 1; 
                                if((borradoIndex) < (finalIndex)){
                                    //Todo Board excepo el ultimo
                                    for(int i = (borradoIndex + 1); i < (trays.Count); i++){
                                        int ant = i -1; 
                                        trays[ant] = trays[i]; 
                                    }
                                }

                            }
                            
                        }
                        //De no haber
                            //, quedarse en el mismo, 
                    }
                    else{
                        //Izquierda: <--
                        //Revisar si existe tray adelante de la lista
                        if(trayindex < 19){
                            trayindex = trayindex + 1;
                            currentTray = trays[trayindex];
                            cameraDirection = new Vector3(currentTray.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
                        }
                    }
                }
            }
            //let the sellected dice go
            if (Input.GetTouch(0).phase==TouchPhase.Ended && sellectedDice!=null)
            {                
                sellectedDice.GetComponent<Dice>().Setkillable(false);
                if (sellectedDice.GetComponent<Dice>().locked==false)
                {
                    sellectedDice.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                }
                TouchTimeFinish = Time.time;
                timeInterval = TouchTimeFinish - touchTimeStart;
                endPosf = Input.GetTouch(0).position;
                directionf = endPosf - startPosF;
                rb.AddForce(directionf.x*throwzx/timeInterval,0,directionf.y*throwzx/timeInterval);

                sellectedDice = null;
            }

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            dices = currentTray.GetComponentsInChildren<Dice>();
            print("rolling");
            foreach (Dice diceToRoll in dices)
            {
                
                diceToRoll.RollDice();
            }
        }
    }
}
    