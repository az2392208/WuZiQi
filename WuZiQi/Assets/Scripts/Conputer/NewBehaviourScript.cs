using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    private void Awake()
    {

    }
    private void Start()
    {
        Destroy(this.GetComponent<Button>());

    }

    private void Update()
    {
        if (gameObject.GetComponent<Button>() == null)
        {
            Button bt = this.gameObject.AddComponent<Button>();
            bt.onClick.AddListener(() => { Debug.Log(this.gameObject.name = "你好啊"); });
        }
    }
    public void ZZZZ()
    {
        Debug.Log("zzzz");
    }
}
