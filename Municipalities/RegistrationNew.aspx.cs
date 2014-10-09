using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Municipalities
{
    public partial class RegistrationNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void imagebutton1_Click(object sender, EventArgs e)
        {

            string username = txtUsername.Text;
            string password = txtPassword.Text;


            //proverka dali postoi korisnik so takov username vo bazata
            municipalitiesEntities db=new municipalitiesEntities();
            ObservableCollection<User> users=new ObservableCollection<User>(db.Users.Where(a=>a.Name==username));
            //korisnickoto ime e slobodno
            if (users.Count() == 0)
            {
                User u = new User();
                u.Name = username;
                u.Username = username;
                u.Password = password;


                db.Users.Add(u);
                db.SaveChanges();

                Label2.Visible = true;
            }
            else
            {

                //prikazi poraka za vnes na nov username
               Label1.Visible = true;
            }

        }




    }
}