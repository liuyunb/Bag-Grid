using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using UnityEngine;

public enum ClientEnum
{
    EMAIL_QQ,
    EMAIL_163,
    EMAIL_126
}

public class SendEmail : MonoBehaviour
{
    public ClientEnum clientEnum = ClientEnum.EMAIL_QQ;
    private string mEmailAcount = "845081507@qq.com";//发件人的邮箱
    private string mEmailPassword = "sactmmspdmwubbch";//发件人的邮箱第三方登录授权码

    /// <summary>
    /// 收件人邮箱
    /// </summary>
    private List<string> mReceiver = new List<string>()
    {
        "845081507@qq.com",
        "3066784317@qq.com",
        // "2390050866@163.com"
    };

    /// <summary>
    /// 邮件发送服务器
    /// </summary>
    private readonly List<string> smtpClient = new List<string>()
    {
        "smtp.qq.com",
        "smtp.163.com",
        "smtp.126.com"
    };
    void Start()
    {

        
    }
    [ContextMenu("SendEmail")]
    public void TestSendEmail()
    {
        SendMail("牛", "汪汪！牛！", "Assets/Scripts/Email/汪汪牛逼.docx", OnCall);
    }
    
    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="subject">邮件标题</param>
    /// <param name="body">邮件内容</param>
    /// <param name="attachment">附件（要发送附件的路径,包括后缀）</param>
    /// <param name="onSend">回调函数</param>
    public void SendMail(string subject, string body, string attachment, Action<bool> onSend)
    {
        if (mReceiver == null || mReceiver.Count < 1)
            return;
        MailMessage mail = new MailMessage();
        //发件人
        MailAddress fromAddr = new MailAddress(mEmailAcount);
        mail.From = fromAddr;
        //收件人,可以设置多个收件人，方法如下
        mReceiver.ForEach(r => mail.To.Add(r));
        //邮件标题
        mail.Subject = subject;
        mail.SubjectEncoding = Encoding.UTF8;//标题格式为UTF8  
        //邮件内容
        mail.Body = body;
        mail.BodyEncoding = Encoding.UTF8;//内容格式为UTF8  
        mail.Priority = MailPriority.High;//邮件级别 高级
        //附件，也就是要发送的文件
        mail.Attachments.Add(new Attachment(attachment));

        //邮件发送服务器，一般来说服务器可以到使用的邮箱后台查看，这里设置了QQ，163，126的
        SmtpClient client = new SmtpClient(smtpClient[(int)clientEnum]);
        //发送人的邮箱账号和密码(第三方授权码)
        client.Credentials = new NetworkCredential(mEmailAcount, mEmailPassword) as ICredentialsByHost;
        //是否启用ssl,也就是安全发送
        //client.EnableSsl = false;   
           
        //发送邮件
        client.SendCompleted += new SendCompletedEventHandler((sender, arg) =>
        {
            bool isResult = arg.Error == null;
            if (!isResult )
            {//失败
                Debug.LogError(arg.Error);
            }

            if (onSend != null)
            {
                onSend(isResult );
            }
        });

        client.SendAsync(mail, null);
    }

    private void OnCall(bool b)
    {
        Debug.Log(b);
        Debug.Log("发送成功");
    }

}
