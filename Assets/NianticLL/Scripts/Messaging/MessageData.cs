using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageData : MonoBehaviour
{
    public static MessageData Inst;

    public int progression = 0;
    public List<Message> textToDisplayOnStart = new List<Message>();
    public bool displayed = false;
    public bool replied = true;
    public bool notif = true;


    // Start is called before the first frame update
    void Start()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!displayed) notif = true;

        if (VocabularySet.Instance.ongoingCategory.ContainsKey("OB") && progression == 1 && displayed)
        {
            progression++;
            displayed = false;
            //MessageData.Inst.notif = true;
        }
    }

    public class Message
    {
        public bool isPlayer;
        public string userName;
        public string textMsg;

        public Message(bool isPlayer_, string userName_, string textMsg_)
        {
            isPlayer = isPlayer_;
            userName = userName_;
            textMsg = textMsg_;
        }
    }
}
