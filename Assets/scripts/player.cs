using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    //預設是private ， public -> unity API 會出現
    //想要讓private 出現在API中 ，加上 [serializeField]
    //[SerializeField] float moveSpeed = 10 ;
    [SerializeField] float moveSpeed = 5f ;
    [SerializeField] int hp ;
    [SerializeField] GameObject hpbar ;
    [SerializeField] GameObject currentfloor ;
    [SerializeField] Text scoretext ;
    int score;
    Animator anim;
    SpriteRenderer render;
    float scoretime ;
    AudioSource deathsound ; 
    [SerializeField] GameObject replybutton;
    // Start is called before the first frame update
    void Start()
    {    
        Debug.Log("check player ...");   
        hp = 10;
        score = 0;
        scoretime =0f;
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        deathsound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    
    void Update()
    {
        // update to next pudate's time interval
        // 解決每台電腦運作速度不同的問題
        // Time.deltaTime
        UpdateScore();
        
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed*Time.deltaTime,0,0);
            render.flipX = false;
            anim.SetBool("run",true);
        }
        else if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed*Time.deltaTime,0,0);
            render.flipX = true;
            anim.SetBool("run",true);
        }else
        {
            anim.SetBool("run",false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "nails")
        {   
            
            if(other.contacts[0].normal == new Vector2(0f,1f))
            {                
                currentfloor = other.gameObject;
                ModifyHP(-3);
                anim.SetTrigger("hurt");
                other.gameObject.GetComponent<AudioSource>().Play();
            }            
        }else if(other.gameObject.tag == "normal")
        { 
            if(other.contacts[0].normal == new Vector2(0f,1f))
            {                
                currentfloor = other.gameObject;
                ModifyHP(1);
                other.gameObject.GetComponent<AudioSource>().Play();
            }   
        }else if(other.gameObject.tag == "ceiling"){
            currentfloor.GetComponent<BoxCollider2D>().enabled = false;
            ModifyHP(-3);
            anim.SetTrigger("hurt");
            other.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "dead")
        {            
            Die();
        }
    }

    void ModifyHP(int num){        
        hp+=num ;
        
        if(hp>10){
            hp = 10;
        }else if (hp<=0){
            hp = 0;
            Die();
        }
        UpdateHpBar();
    }

    void UpdateHpBar(){
        for(int i=0; i<hpbar.transform.childCount; i++)
        {
            if(hp>i){
                hpbar.transform.GetChild(i).gameObject.SetActive(true);
            }else{
                hpbar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void UpdateScore()
    {
        scoretime+= Time.deltaTime;
        if(scoretime > 2f)
        {   
            score++;
            scoretime=0f;
            scoretext.text = "地下"+ score.ToString() +"層";
        }
    }
    
    void Die()
    {
        deathsound.Play();
        //unity 裡面時間的縮放比例 預設為1
        // 0 遊戲凍結 暫停
        Time.timeScale = 0f;
        replybutton.SetActive(true);
    }

    public void reply()
    {
        Time.timeScale = 1f;
        //重新載入scene
        SceneManager.LoadScene("SampleScene");
    }
}
