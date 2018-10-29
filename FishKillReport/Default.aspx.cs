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

        
        async  void PostRequest(byte[] imgBytes)
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