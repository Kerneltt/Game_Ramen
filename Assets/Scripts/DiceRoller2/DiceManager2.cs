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
    [SerializeField]
    GameObject sellectedDice;
    public Vector2 startPos;
    public Vector2 startPos2;
    public Vector2 direction;
    bool doubletapTimer = false;
    int tapcounter = 0;
    int trayindex = 0;
    Vector3 cameraDirection;
    public Color diceColor = Color.white;
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
    //[SerializeField]
    //Color currentTrackerColor = Color.white;
    [SerializeField]
    float scaleMultiplier = 2.5f;
    Vector3 trackerScale = new Vector3(1, 1, 1);
    bool displayShown;
    [SerializeField]
    GameObject collorPointer;
    [SerializeField]
    GameObject menuFader;
    private Animator menuAnim;
    [SerializeField]
    GameObject infoFader;
    private Animator infoAnim;
    [SerializeField]
    GameObject sideSweepArea;
    private Animator sidesweepAnim;
    [SerializeField]
    GameObject trayButtonL;
    [SerializeField]
    GameObject trayButtonR;
    // Use this for initialization

    private void Awake()
    {
        menuAnim = menuFader.GetComponent<Animator>();
        infoAnim = infoFader.GetComponent<Animator>();
        sidesweepAnim = sideSweepArea.GetComponent<Animator>();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Start () {
        for (int i = 0; i < 20; i++){
            float newx = i * 22f;
            Vector3 v = new Vector3(newx, 0, 0);
            locations.Add(v);  
        }   
        displayShown = false;
        trackerScale = new Vector3(1 * scaleMultiplier, 1 * scaleMultiplier, 1 * scaleMultiplier);
        menuAnim.SetTrigger("Start"); infoAnim.SetTrigger("Start"); sidesweepAnim.SetTrigger("Start");
        dicepicker.GetComponent<Animator>().Play("DicePickerStart");
        diceColor = Color.white;
        currentTray = trays[0];
        trayindex = 0;
        cameraDirection = new Vector3(currentTray.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        currentTracker = tracker[0];
        collorPointer.GetComponent<Image>().color = Color.grey;
    }

    public void TogglePicker()
    {
        CancelTap();
        if (displayShown)
        {
            dicepicker.GetComponent<Animator>().Play("hidePanel");
            displayShown = false;
            // Show bottom menu
            menuAnim.ResetTrigger("Start");
            menuAnim.ResetTrigger("Hide");
            menuAnim.SetTrigger("Show");
            // Show info button
            infoAnim.ResetTrigger("Start");
            infoAnim.ResetTrigger("Hide");
            infoAnim.SetTrigger("Show");
            // Enable border sweep areas
            sidesweepAnim.ResetTrigger("Start");
            sidesweepAnim.ResetTrigger("Hide");
            sidesweepAnim.SetTrigger("Show");
        }
        else
        {
            displayShown = true;
            dicepicker.GetComponent<Animator>().Play("ShowPanel");
            // Hide bottom menu
            menuAnim.ResetTrigger("Show");
            menuAnim.SetTrigger("Hide");
            // Hide Info button
            infoAnim.ResetTrigger("Show");
            infoAnim.SetTrigger("Hide");
            // Disable boder sweep areas
            sidesweepAnim.ResetTrigger("Show");
            sidesweepAnim.SetTrigger("Hide");
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
        CancelTap();
        diceColor =btn.GetComponent<Image>().color;
        collorPointer.transform.position = btn.transform.position;
        if (diceColor == Color.white)
        {
            collorPointer.GetComponent<Image>().color = Color.grey;

        }
        else
        {
            collorPointer.GetComponent<Image>().color = Color.white;
        }

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

            foreach (Dice dice in trays[i].GetComponentsInChildren<Dice>())
            {
                Destroy(dice.GetComponent<Rigidbody>());                
            }
            trays[i].transform.position = locations[i];
        }

        

    }
    public void SpawnDice(int dice)
    {
        CancelTap();
        if (currentTray.GetComponentsInChildren<Dice>().Length < 20)
        {
            //print("creatingDice");
            GameObject newdice = Instantiate(die[dice]);
            //newdice.GetComponentInChildren<Renderer>().material.color = collorpickerButton.GetComponent<Image>().color;
            if (newdice.GetComponentInChildren<Dice>().getIsCoin()==false)
            {
                newdice.GetComponentInChildren<Renderer>().material.color = diceColor;
            }            
            newdice.transform.SetParent(currentTray.transform);
            newdice.transform.localPosition = new Vector3(0, 5, 0);
            sellectedDice = newdice;
            touchTimeStart = Time.time;
            startPosF = Input.GetTouch(0).position;
            rb = sellectedDice.GetComponentInChildren<Rigidbody>();
            sellectedDice.GetComponentInChildren<Dice>().Setkillable(true);
            sellectedDice.GetComponentInChildren<Dice>().UnlockDice();
            sellectedDice.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            float fixz= (((Input.GetTouch(0).position.y * 100) / Screen.height) * (Mathf.Sin(Mathf.Deg2Rad * 60)) / 10);
            //print(fixz);
            Vector3 screenpoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x,Input.GetTouch(0).position.y,10+fixz));
            sellectedDice.transform.position = screenpoint;
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
        if (dir.sqrMagnitude > 3)
        {
            dices = currentTray.GetComponentsInChildren<Dice>();
            //dir.Normalize();
            foreach (Dice diceToRoll in dices)
            {
                diceToRoll.RollDice();
                if (!diceToRoll.locked)
                {
                    //diceToRoll.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
                }                
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
                if (Physics.Raycast(raycast, out raycastHit, 40f, mask))
                {
                   // print(raycastHit.transform.position);
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
                           // print("TapRoll");
                        }
                        //detect first tap
                        else
                        if (doubletapTimer==false)
                        {
                            doubletapTimer = true;
                            tapcounter = 50;
                           // print("TapOnce");
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
                    sellectedDice.GetComponentInChildren<Dice>().Setkillable(true);
                    sellectedDice.GetComponentInChildren<Dice>().UnlockDice();
                    sellectedDice.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    float fixz = (((Input.GetTouch(0).position.y * 100) / Screen.height) * (Mathf.Sin(Mathf.Deg2Rad * 60))/10);
                    sellectedDice.transform.position = (Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 10+fixz)));
                }                
            }
            //starting position of finger
            if (Input.GetTouch(0).phase == TouchPhase.Moved && sellectedDice == null)
            {
                //prender bandera swipe
                //Guardar posicion actual por inicio de swipe
                startPos = Input.GetTouch(0).position;
                //startPos2 = Input.GetTouch(1).position;
            }
            //swipe ended
            if (Input.touchCount>1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended && sellectedDice == null)
                {

                    //MODULAR FUERZA DEL SWIPE
                    if ((Vector2.Distance(startPos, Input.GetTouch(0).position) > 5)|| (Vector2.Distance(startPos2, Input.GetTouch(1).position) > 5))
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
                                currentTracker.transform.localScale = trackerScale;
                                //
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
                                    currentTracker.transform.localScale = trackerScale;
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
                                currentTracker.transform.localScale = trackerScale;
                                cameraDirection = new Vector3(currentTray.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
                            }
                        }
                    }
                }
            }
            
            //let the sellected dice go
            if (Input.GetTouch(0).phase==TouchPhase.Ended && sellectedDice!=null)
            {                
                sellectedDice.GetComponentInChildren<Dice>().Setkillable(false);
                if (sellectedDice.GetComponentInChildren<Dice>().locked==false)
                {
                    sellectedDice.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.None;
                    if (sellectedDice.GetComponentInChildren<Dice>().getIsCoin())
                    {
                        switch (Random.Range(0, 2))
                        {
                            case 0:

                                sellectedDice.GetComponentInChildren<Animator>().Play("Rotate_1", -1, 0f);
                                print("0");
                                //GetComponent<Animator>().SetBool("repeat", false);
                                break;

                            case 1:
                                sellectedDice.GetComponentInChildren<Animator>().Play("Rotate_2", -1, 0f);
                                print("1");
                                //GetComponent<Animator>().SetBool("repeat", false);
                                break;
                        }
                    }

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
           // print("rolling");
            foreach (Dice diceToRoll in dices)
            {
                diceToRoll.RollDice();
            }
        }

    //currentTracker.GetComponent<Image>().color = currentTrackerColor;


    }
    public void TrayLeft()
    {
        CancelTap();
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
            currentTracker.transform.localScale = trackerScale;

            if (trays.Count == 1)
            {
                trayButtonL.SetActive(false);
 
            }
            else
            {
                trayButtonL.SetActive(true);
                trayButtonR.SetActive(true);
            }
            if (trayindex==0)
            {
                trayButtonL.SetActive(false);
            }
        }
    }

    public void TrayRight()
    {
        CancelTap();
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
                currentTracker.transform.localScale = trackerScale;
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
            currentTracker.transform.localScale = trackerScale;
            cameraDirection = new Vector3(currentTray.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
            if (trays.Count==20)
            {
                trayButtonR.SetActive(false);
                
            }
            else
            {
                trayButtonR.SetActive(true);
                trayButtonL.SetActive(true);
            }
            if (trayindex == 19)
            {
                trayButtonR.SetActive(false);
            }
        }
    }
}
    