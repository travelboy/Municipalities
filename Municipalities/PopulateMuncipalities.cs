using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Municipalities
{
    public class PopulateMuncipalities
    {
        public static void ddlPopulate(DropDownList d1)
        {
            SqlCommand cmd = new SqlCommand("SELECT MunId,Name FROM Municipalities", new SqlConnection(ConfigurationManager.AppSettings["ConnString"]));
            cmd.Connection.Open();

            SqlDataReader ddlValues;
            ddlValues = cmd.ExecuteReader();
            ListItem newItem = new ListItem();

            newItem.Text = "Изберете општина";
            newItem.Value = "0";
            d1.Items.Add(newItem);
            
            while (ddlValues.Read())
            {
                newItem = new ListItem();
                newItem.Text = ddlValues.GetString(1);
                newItem.Value = ddlValues.GetInt32(0).ToString();
                d1.Items.Add(newItem);
             
            }
            ddlValues.Close();


            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
    }
}