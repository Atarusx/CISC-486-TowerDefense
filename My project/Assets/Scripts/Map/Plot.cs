using UnityEngine;

public class Plot : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hovorColor;

    private GameObject tower;
    private Color placeColor;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placeColor = sr.color;   
    }


    private void OnMouseEnter()
    {
        sr.color = hovorColor;
    }


    private void OnMouseExit()
    {
        sr.color = placeColor;
    }


    private void OnMouseDown()
    {
        if (tower != null) return;

        GameObject buildedTower = BuildManager.main.GetSelectedTower();
        tower = Instantiate(buildedTower, transform.position, Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
