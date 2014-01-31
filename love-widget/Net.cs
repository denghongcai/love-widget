using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace love_widget
{
    class Net
    {
        private Form form;
        private string User_Agent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";

        public Net(Form form)
        {
            this.form = form;
        }

        public void UploadData(string data)
        {
            WebClient wc = new WebClient();
            wc.OpenReadCompleted += new OpenReadCompletedEventHandler(this.wc_OpenReadCompleted);
            wc.UploadDataCompleted += new UploadDataCompletedEventHandler(this.wc_UploadDataCompleted);
            wc.Headers.Add(HttpRequestHeader.UserAgent, this.User_Agent);
            try
            {
                wc.OpenReadAsync(new Uri("http://www.dhchouse.com"));
            }
            catch
            {
            }
        }

        private void wc_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string returnMessage = Encoding.UTF8.GetString(e.Result);
                Debug.WriteLine(returnMessage);
                JavaScriptSerializer json = new JavaScriptSerializer();
                List<Message> list = json.Deserialize<List<Message>>(returnMessage);
                foreach (Message item in list)
                {
                    Debug.WriteLine(item.MessageID + "\t" + item.MessageContent + "\t" + item.MessageShowed);    
                }
            }
        }

        private void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string jsondata = new StreamReader(e.Result, Encoding.UTF8).ReadToEnd();
                /*
                ((WebClient)sender).Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                ((WebClient)sender).Headers.Add("Accept-Language", "zh-cn");
                string postData = "";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                ((WebClient)sender).UploadDataAsync(new Uri("http://www.dhchouse.com/"), "POST", byteArray);
                */
            }
        }
    }

    class Message
    {
        public int MessageID { get; set; }
        public string MessageContent { get; set; }
        public bool MessageShowed { get; set; }
    }
}
