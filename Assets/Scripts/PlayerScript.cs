using UnityEngine;

public class PlayerScript : MonoBehaviour{
    public float JumpForce;

    [SerializeField]
    bool isGrounded = false;
    int noOfJumps = 0;

    Rigidbody2D RB;

    private void Awake(){
        RB = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            if(isGrounded == true){
                RB.AddForce(Vector2.up * JumpForce);
                isGrounded = false;
                noOfJumps = 1;
            }else if(noOfJumps == 1){
                    RB.AddForce(Vector2.up * JumpForce);
                    noOfJumps = 2;
                    }
                
        }
                
    }
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Ground")){
            if(isGrounded == false){
                isGrounded = true;
            }
        }
    }
}
