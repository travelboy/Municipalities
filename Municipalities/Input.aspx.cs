using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Municipalities
{
    public partial class Input : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PopulateMuncipalities.ddlPopulate(DropDownList1);
        }

        protected void btnInput_Click(object sender, EventArgs e)
        {
            municipalitiesEntities db = new municipalitiesEntities();
            ObservableCollection<Municipality> munN =
                new ObservableCollection<Municipality>(db.Municipalities.Where(a => a.Name == txtNameMunicipalities.Text));
           
            //ne postoi opstina so toa ime
            if (munN.Count == 0)
            {
                Municipality m = new Municipality();
                m.Name = txtNameMunicipalities.Text;
                m.FBId = txtFBLink.Text;
                m.TwitterID = txtTwitterLink.Text;
                m.YTId = txtYTLink.Text;
                db.Municipalities.Add(m);
                db.SaveChanges();
            }
            else
            {
                lblExist.Visible = true;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if(cbFB.Checked)
                Response.Redirect("WebCrawler.aspx?id=facebook");
            if (cbTW.Checked)
                Response.Redirect("WebCrawler.aspx?id=twitter");
            if (cbYT.Checked)
                Response.Redirect("WebCrawler.aspx?id=youtube");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string nameMun = DropDownList1.SelectedItem.Text;

            municipalitiesEntities db = new municipalitiesEntities();

            ObservableCollection<Municipality> mun = new ObservableCollection<Municipality>(db.Municipalities);
            db.Database.ExecuteSqlCommand("DELETE FROM [Municipalities] WHERE Name = "+nameMun);
         
           
        }
    }
}