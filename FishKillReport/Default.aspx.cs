using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FishKillReport
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //GetRequest(txtName.Text);
            byte[] imgBytes = fileCtrl.FileBytes;
            PostRequest(imgBytes);


            //Response.Redirect("https://fishkillapp.azurewebsites.net/api/ValidateFishKill?name=" + txtName.Text);

            //HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://fishkillapp.azurewebsites.net/api/ValidateFishKill?name=" + txtName.Text);
            //req.Method = "GET";
            //req.ContentType = "application/json";
            //Stream stream = req.GetRequestStream();
            //string json = "{\"name\": \"Azure\" }";
            //byte[] buffer = Encoding.UTF8.GetBytes(json);
            //stream.Write(buffer, 0, buffer.Length);
            //HttpWebResponse res = (HttpWebResponse)req.GetResponse();
        }

        async void GetRequest(string name)
        {
            
            using (HttpClient client = new HttpClient()) {
                using (HttpResponseMessage response = await client.GetAsync("https://fishkillapp.azurewebsites.net/api/ValidateFishKill?name=" + name))
                 {
                    using (HttpContent content = response.Content) {
                        string resContent = await content.ReadAsStringAsync();
                        HttpContentHeaders headers = content.Headers;
                        //System.Diagnostics.Trace.WriteLine(headers);
                        //System.Diagnostics.Trace.WriteLine(resContent);
                        
                    }   
                }
            }            
        }

        async void PostRequest(byte[] imgBytes)
        {
            HttpContent reqContent = new ByteArrayContent(imgBytes);
            
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsync("https://fishkillapp.azurewebsites.net/api/ValidateFishKill", reqContent))
                {
                    using (HttpContent content = response.Content)
                    {
                        string resContent = await content.ReadAsStringAsync();
                        HttpContentHeaders headers = content.Headers;
                        //System.Diagnostics.Trace.WriteLine(headers);
                        //System.Diagnostics.Trace.WriteLine(resContent);
                        ShowResponse(resContent);
                    }
                }
            }
        }

        private void ShowResponse(string resContent)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Response Message", "alert('" + resContent + "');", true);
        }
    }
}