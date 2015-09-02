using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


namespace SSOLogin
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 单点登录
            TokenService.CredentialSoapHeader header = new TokenService.CredentialSoapHeader();
            header.UserID = "admin";
            header.PassWord = "99it@2014";

            TokenService.TokenService tk = new TokenService.TokenService();
            tk.CredentialSoapHeaderValue = header;

            HttpCookie cookie = HttpContext.Current.Request.Cookies["token"];
            if (Session["UName"] != null)
            {
                if (cookie == null)
                {
                    cookie.Domain = ConfigurationManager.AppSettings["domain"].ToString();
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);

                    Request.Cookies.Remove("token");

                    Session["UName"] = null;
                }
                else
                {
                    Response.Redirect("Main.aspx");
                }
            }
            else
            {
                if (cookie != null && cookie.Value != null)
                {
                    string acount = tk.TokenGetCredence(cookie.Value);
                    if (!string.IsNullOrEmpty(acount))
                    {
                        if (acount == "18321887552")  //判断用户是否存在
                        {
                            Session["UName"] = "Admin";
                            Response.Redirect("Main.aspx");
                        }
                    }
                }
            }
            #endregion
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = this.txtUserName.Text.Trim();
            string pwd = this.txtPwd.Text.Trim();

            if (userName == "18321887552" && pwd == "123456")
            {
                #region 单点登录
                TokenService.CredentialSoapHeader header = new TokenService.CredentialSoapHeader();
                header.UserID = "admin";
                header.PassWord = "99it@2014";

                TokenService.TokenService tk = new TokenService.TokenService();
                tk.CredentialSoapHeaderValue = header;

                string token = tk.LoginCheck(userName);
                HttpCookie cookie = new HttpCookie("token")
                {
                    Value = token,
                    Expires = DateTime.Now.AddDays(1),
                    Domain = ConfigurationManager.AppSettings["domain"].ToString()
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
                #endregion

                Session["UName"] = "18321887552";
                Response.Redirect("Main.aspx");
            }
        }
    }
}