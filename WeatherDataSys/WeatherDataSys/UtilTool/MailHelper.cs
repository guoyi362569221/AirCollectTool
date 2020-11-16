using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDataSys.UtilTool
{
    public class MailHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="fromMail">邮件发送人地址：如：362569221@qq.com</param>
        /// <param name="toMails">邮件接收人地址，数组</param>
        /// <param name="ccMails">邮件抄送人地址，数组</param>
        /// <param name="mailTitle">邮件主题</param>
        /// <param name="mailContent">邮件内容</param>
        /// <param name="sendCredentials">邮件发送人授权码</param>
        /// <param name="attaFilePaths">附件路径</param>
        /// <param name="senderName">发件人昵称</param>
        public static void SendMail(string fromMail, string[] toMails, string[] ccMails, string mailTitle, string mailContent, string sendCredentials, string[] attaFilePaths = null, string senderName = "邮件机器人")
        {
            MailMessage mailMessage = new MailMessage
            {
                //发件人
                From = new MailAddress(fromMail, senderName)
            };

            //收件人 可以添加多个收件人
            for (int i = 0; i < toMails.Length; i++)
            {
                mailMessage.To.Add(new MailAddress(toMails[i]));
            }
            if (ccMails != null)
            {
                //mailMessage.CC 获取包含此电子邮件的抄送(CC)收件人的地址集合
                for (int i = 0; i < ccMails.Length; i++)
                {
                    mailMessage.CC.Add(ccMails[i]);
                }
            }

            //邮件主题
            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.Subject = mailTitle;

            //邮件正文
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.Body = mailContent;

            //如果要发送html格式的消息，需要设置这个属性
            mailMessage.IsBodyHtml = true;

            //邮件内容即消息正文中中显示图片 
            //需要为图片指明src='cid:idname(资源id)'
            //AlternateView htmlBody = AlternateView.CreateAlternateViewFromString("<img src='cid:zfp'/>", null, "text/html");

            //然后在LinkedResource加入文件的绝对地址，和ContentType 例如image/gif，text/html...与http请求的响应报文中的ContentType一致
            //LinkedResource lr = new LinkedResource("1.gif", "image/gif");

            //绑定上文中指定的idname
            //lr.ContentId = "zfp";

            //添加链接资源
            //htmlBody.LinkedResources.Add(lr);

            //mailMessage.AlternateViews.Add(htmlBody);
            //mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(mailContent, null, MediaTypeNames.Text.Plain));
            //mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(mailContent, null, MediaTypeNames.Text.Html));

            //发送附件 指明附件的绝对地址
            if (attaFilePaths != null && attaFilePaths.Length > 0)
            {
                for (int i = 0; i < attaFilePaths.Length; i++)
                {
                    Attachment attachment = new Attachment(attaFilePaths[i]);
                    mailMessage.Attachments.Add(attachment);
                }
            }

            //创建邮件发送客户端
            try
            {
                //这里使用qq邮箱 需要在设置->账户下开启POP3/SMTP服务 和 IMAP / SMTP服务
                //qq邮箱的发件服务器smtp.qq.com  端口25
                SmtpClient sendClient = new SmtpClient("smtp.qq.com", 25)
                {
                    //指定邮箱账号和密码
                    //在第三方客户端登陆qq邮箱时，密码是授权码
                    //登陆qq邮箱在设置->账户下可以生成授权码
                    Credentials = new NetworkCredential(fromMail, sendCredentials)
                };

                //指定如何发送电子邮件
                sendClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                //指定使用使用安全套接字ssl加密的链接

                sendClient.EnableSsl = true;
                sendClient.Send(mailMessage);
            }
            catch (Exception e)
            {
                Loghelper.WriteErrorLog("发送邮件失败", e);
                throw e;
            }
        }
    }
}
