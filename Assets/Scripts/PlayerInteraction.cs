using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class PlayerInteraction : MonoBehaviour

{
    private Camera myCam;
    public float rayDistance = 2f;
    public float rotateSpeed;
    public UnityEvent onView;
    public UnityEvent onFinishView;
    private Interactable currentInteractable;
    private bool isViewing;
    private bool canFinish;
    public Transform objectViewer;
    private Vector3 originPosition;
    private Quaternion originRotation;
    private AudioPlayer audioPlayer;


    private void Awake()
    {
        audioPlayer = GetComponent<AudioPlayer>();
    }
    private void Start()
    {
        myCam = Camera.main;
    }

     void Update()
    {
       CheckInteractables();

    }

    void CheckInteractables()
      
    {

        if (isViewing)
        {
            if(currentInteractable.item.grabbable && Input.GetMouseButton(0))
            {
                RotateObject();
            }
            if (canFinish && Input.GetMouseButtonDown(1))
            {
                FinishView();
            }
            return;
        }
        RaycastHit hit;
        Vector3 rayOrign = myCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));
        
        if(Physics.Raycast(rayOrign,myCam.transform.forward,out hit, rayDistance))
        {
            Interactable interactable=hit.collider.GetComponent<Interactable>();

            if(interactable!=null)
            {
                UIManager.instance.SetHandCursor(true);
                if (Input.GetMouseButtonDown(0))
                {
                    if (interactable.isMoving)
                    {
                        return;
                    }
                    
                    currentInteractable = interactable;
                    currentInteractable.onInteract.Invoke();

                    if(currentInteractable.item != null)
                    {
                        onView.Invoke();
                        isViewing = true;
                        Interact(currentInteractable.item);


                        if (currentInteractable.item.grabbable)
                        {
                            originPosition = currentInteractable.transform.position;
                            originRotation = currentInteractable.transform.rotation;
                            StartCoroutine(MovingObject(currentInteractable, objectViewer.position));
                        }
                    }

                    
                }
            }
            else
            {
                UIManager.instance.SetHandCursor(false);
            }
        }
        else
        {
            UIManager.instance.SetHandCursor(false);

        }

    }

    void Interact(Item item)
    {
        if( item.image != null)
        {
            UIManager.instance.SetImage(item.image);

        }
        audioPlayer.PlayAudio(item.audioClip);
        UIManager.instance.SetCaptionText(item.text);
        Invoke("CanFinish",item.audioClip.length +0.5f);

    }
    void CanFinish()
    {
        canFinish=true;
        if(currentInteractable.item.image==null && !currentInteractable.item.grabbable)
        {
            FinishView();
        }
        else
        {
            UIManager.instance.SetBackImage(true);

        }

        UIManager.instance.SetCaptionText("");
    }

    void FinishView()
    {
        canFinish = false;
        isViewing =false;
        UIManager.instance.SetBackImage(false);
        if (currentInteractable.item.grabbable)
        {
            currentInteractable.transform.rotation = originRotation;
            StartCoroutine(MovingObject(currentInteractable, originPosition));
        }
        onFinishView.Invoke();
    }
    IEnumerator MovingObject(Interactable obj, Vector3 position)
    {
        obj.isMoving = true;
        float timer = 0;
        while (timer<1 ) 
        {
            obj.transform.position=Vector3.Lerp(obj.transform.position, position, Time.deltaTime*5);
            timer += Time.deltaTime;
            yield return null ;
        }
        obj.transform.position = position;
        obj.isMoving = false;
    }

    void RotateObject()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        currentInteractable.transform.Rotate(myCam.transform.right, -Mathf.Deg2Rad * y * rotateSpeed, Space.World);
        currentInteractable.transform.Rotate(myCam.transform.up, -Mathf.Deg2Rad * x * rotateSpeed, Space.World);


    }
}

