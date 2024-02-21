
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(LineRenderer))]

public class LineRendrerHandler : MonoBehaviour
{
    private bool canDrag
    {
        get; set;
    }
    public LineRenderer _lineRenderer;
    private bool _isDraging;
    private Vector3 _endPoint;
    [SerializeField]private bool _verif ;
    private RaycastHit2D hit1;
    public void reset()
    {
        _endPoint = Vector3.zero;
        _verif = false;


    }

    public bool verif
    {
        get => _verif; 
        set => _verif = value;
    }
    public void toggleCanDrag(bool param)
    {
        canDrag = param;
        Debug.Log($"canDrag value is : {canDrag}");
    }


    private void Awake()
    {
        canDrag = true;
    }
    private void Start()
    {
        _verif = false;
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
    }
    private void Update()
    {   
        if (!canDrag)
        {
            //Debug.Log("you cant drag ");
            return;
        }
            
            
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("mouse down");
            hit1 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
            //Debug.Log(hit.collider);
            //Debug.Log(hit.collider.gameObject);
            if (hit1.collider != null && hit1.collider.gameObject==gameObject) 
            {
                _isDraging = true;
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                Vector2 ObjecctPosition = hit1.collider.gameObject.transform.position;
                Debug.Log(ObjecctPosition);
                Vector3 fixedPosition =new Vector3 (ObjecctPosition.x, ObjecctPosition.y, 0);
                Debug.Log(fixedPosition);
                _lineRenderer.SetPosition(0, fixedPosition);
            }
        }
        if (_isDraging) 
        {
            
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            _lineRenderer.SetPosition(1,mousePosition);
            _endPoint = mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            //Debug.Log("mouse up");
            _isDraging = false;
            RaycastHit2D hit2 = Physics2D.Raycast(_endPoint, Vector2.zero);
            _endPoint = Vector3.zero;
            //Debug.Log(hit2.collider);
            //Debug.Log(hit2.collider.gameObject.transform.parent.transform.parent.name);
            if (hit2.collider != null && hit2.collider.gameObject.transform.parent.transform.parent.transform.parent.name == "NamesContainer" )
            {
                //Debug.Log(hit2.collider);
                //Debug.Log(hit2.collider.gameObject.transform.parent.transform.parent.name);
                toggleCanDrag(false);
                if (hit2.collider.gameObject.transform.parent.name == hit1.collider.gameObject.transform.parent.name)
                {
                    _verif = true;
                    Debug.Log("shiiiiha");
                }
            }
            else
            {
                _lineRenderer.positionCount = 0;
            }
            _lineRenderer.positionCount = 2;

        }
        
    }

}
