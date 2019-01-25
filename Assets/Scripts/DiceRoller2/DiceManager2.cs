using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager2 : MonoBehaviour {
    Dice[] dices;
    [SerializeField]
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
    public Color diceColor=Color.white;
    [SerializeField]
    List<GameObject> trays;
    List<Vector3> locations = new List<Vector3>(); 
    [SerializeField]
    GameObject currentTray;
    [SerializeField]
    GameObject trayfab;
    bool boolDirection;
    [SerializeField]
    GameObject dicepicker;
    //fling
    Vector2 startPosF, endPosf, directionf;
    float touchTimeStart, TouchTimeFinish, timeInterval;
    [SerializeField]
    float throwzx = 50;
    Rigidbody rb;
    [SerializeField]
    List<GameObject> tracker;
    [SerializeField]
    GameObject trackerParent;
    [SerializeField]
    GameObject trackerPoint;
    GameObject currentTracker;
    // Use this for initialization
    void Start () {
        for (int i = 0; i < 20; i++){
            float newx = i * 17f;
            Vector3 v = new Vector3(newx, 0, 0);
            locations.Add(v);  
        }
        diceColor = Color.white;
        currentTray = trays[0];
        trayindex = 0;
        cameraDirection = new Vector3(currentTray.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        currentTracker = tracker[0];
    }

    public void TogglePicker()
    {
        if (dicepicker.activeInHierarchy)
        {
            dicepicker.SetActive(false);
        }
        else
        {
            dicepicker.SetActive(true);
        }
    }

    public void ClearTray()
    {
        foreach (Dice dice in currentTray.GetComponentsInChildren<Dice>())
        {
            Destroy(dice.gameObject.transform.parent.gameObject);
        }
    }

    public void Setcollor(GameObject btn)
    {
        /*
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
        */
        diceColor =btn.GetComponent<Image>().color;


    }
    public void CancelTap()
    {
        doubletapTimer = false;
        tapcounter = 0;
    }
    public void UpdateLocations(int startIndex){
        int end = trays.Count;
        for (int i = startIndex; i < end; i++)
        {
            trays[i].transform.position = locations[i];  
        }

    }
    public void SpawnDice(int dice)
    {
        CancelTap();
        if (currentTray.GetComponentsInChildren<Dice>().Length<20)
        {
            print("creatingDice");
            GameObject newdice = Instantiate(die[dice]);
            //newdice.GetComponentInChildren<Renderer>().material.color = collorpickerButton.GetComponent<Image>().color;
            newdice.GetComponentInChildren<Renderer>().material.color = diceColor;
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
    void Update() {

        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, cameraDirection, 50 * Time.deltaTime);
        Vector3 dir = Vector3.zero;
        dir.x = -Input.acceleration.x;
        dir.z = Input.acceleration.z;
        dir.y = Input.acceleration.y;
        //print(dir);
        //Unlock all dice
        if (tapcounter > 0)
        {
            tapcounter--;
            if (tapcounter == 0 && doubletapTimer == true)
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
            dices = currentTray.GetComponentsInChildren<Dice>();
            dir.Normalize();
            foreach (Dice diceToRoll in dices)
            {
                diceToRoll.RollDice();
                diceToRoll.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
            }
        }

        //tilt dice on tray

        if (dir.x > 0.5)
        {

            foreach (Dice dice in currentTray.GetComponentsInChildren<Dice>())
            {
                if (!dice.locked)
                {
                    //dice.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * 1);
                    dice.gameObject.transform.parent.Translate(Vector3.left * Time.deltaTime);
                }
            }

        }
        if (dir.x < -0.5)
        {

            foreach (Dice dice in currentTray.GetComponentsInChildren<Dice>())
            {
                if (!dice.locked)
                {
                    //dice.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * 1);
                    dice.gameObject.transform.parent.Translate(Vector3.right * Time.deltaTime);
                }
            }

        }
        /*
        if (dir.z > -0.7)
        {

            foreach (Dice dice in currentTray.GetComponentsInChildren<Dice>())
            {
                //dice.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * 1);
                dice.gameObject.transform.parent.Translate(Vector3.forward * Time.deltaTime);

            }

        }
        if (dir.z < -1)
        {

            foreach (Dice dice in currentTray.GetComponentsInChildren<Dice>())
            {
                //dice.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * 1);
                dice.gameObject.transform.parent.Translate(Vector3.back * Time.deltaTime);
            }

        }
        */
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
                if (Vector2.Distance(startPos,Input.GetTouch(0).position)>50)
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
            if (Input.touchCount>1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended && sellectedDice == null)
                {

                    //MODULAR FUERZA DEL SWIPE
                    if (Vector2.Distance(startPos, Input.GetTouch(0).position) > 5)
                    {
                        //REVISAR LA DIRECCION DEL SWIPE
                        //DERECHA: "-->" 
                        if (startPos.x < Input.GetTouch(0).position.x)
                        {

                            if (trayindex > 0)
                            {
                                //MOVIMIENTO DE TRAYS
                                trayindex = trayindex - 1;
                                currentTray = trays[trayindex];
                                cameraDirection = new Vector3(currentTray.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
                                //ELIMINACION DE TRAYS
                                //Revisar si el anterior tiene 0 dados. Si es asi, borrarlo. 

                                if (trays[trayindex + 1].GetComponentsInChildren<Dice>().Length < 1)
                                {
                                    //Eliminar el tray de la derecha
                                    Destroy(trays[trayindex + 1]);
                                    trays.RemoveAt(trayindex + 1);
                                    UpdateLocations(trayindex + 1);
                                    print("Removed");
                                    Destroy(tracker[trayindex]);
                                    tracker.RemoveAt(trayindex);

                                }
                                currentTracker.transform.localScale = new Vector3(1, 1, 1);
                                currentTracker = tracker[trayindex];
                                currentTracker.transform.localScale = new Vector3(2, 2, 2);
                            }
                        }
                        else
                        {
                            //IZQUIERDa: "<--"
                            //Revisar si existe tray adelante de la lista
                            if (trayindex < 19)
                            {

                                //MOVIMIENTO Y CREACION DE TRAYS
                                trayindex = trayindex + 1;
                                if (trayindex > (trays.Count - 1))
                                {
                                    //Crear el objeto, moverse al objeto recien creado
                                    GameObject newBoard = Instantiate(trayfab);
                                    GameObject newTracker = Instantiate(trackerPoint);
                                    tracker.Add(newTracker);
                                    newTracker.transform.SetParent(trackerParent.transform);
                                    newBoard.transform.position = locations[trayindex];
                                    trays.Add(newBoard);
                                    currentTracker.transform.localScale = new Vector3(1, 1, 1);
                                    currentTracker = tracker[trayindex];
                                    currentTracker.transform.localScale = new Vector3(2, 2, 2);
                                }

                                //ELIMINACION DE TRAYS
                                //Revisar si el anterior tiene 0 dados. Si es asi, borrarlo. 
                                if (trays[trayindex - 1].GetComponentsInChildren<Dice>().Length < 1 && (trayindex - 1) > 0)
                                {
                                    trayindex = trayindex - 1;
                                    Destroy(trays[trayindex]);
                                    trays.RemoveAt(trayindex);
                                    UpdateLocations(trayindex);
                                    print("Removed");
                                    Destroy(tracker[trayindex]);
                                    tracker.RemoveAt(trayindex);
                                }

                                currentTray = trays[trayindex];
                                currentTracker.transform.localScale = new Vector3(1, 1, 1);
                                currentTracker = tracker[trayindex];
                                currentTracker.transform.localScale = new Vector3(2, 2, 2);
                                cameraDirection = new Vector3(currentTray.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
                            }
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
    