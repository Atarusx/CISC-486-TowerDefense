using UnityEngine;


public class PlayerMovement : MonoBehaviour
{


    //References
    Animator am;
    NewMonoBehaviourScript pm;
    SpriteRenderer sr;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        am = GetComponent<Animator>();
        pm = GetComponent<NewMonoBehaviourScript>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pm.moveDir.x != 0 || pm.moveDir.y != 0)
        {
            am.SetBool("Move", true);

            SpriteDirCheck();
        }
        else
        {
            am.SetBool("Move", false);
        }
    }

    void SpriteDirCheck()
    {
        if (pm.lastHorVector < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}
