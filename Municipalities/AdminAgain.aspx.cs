using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Municipalities
{
    public partial class AdminAgain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
   

        protected void imagebutton1_Click(object sender, ImageClickEventArgs e)
        {
            string Nickname = txtUsername.Text;
            string Password = txtPassword.Text;


            // proveri dali se validna kombinacija username i password-ot
            municipalitiesEntities db = new municipalitiesEntities();
            ObservableCollection<User> uName = new ObservableCollection<User>(db.Users.Where(a => a.Username == Nickname && a.Password == Password));


            if (uName.Count() == 1) 
            Response.Redirect("Input.aspx");
            // nevalidno
            else
            {
                Label1.Visible = true;

            }
        }
    }
}