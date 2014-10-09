using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Municipalities
{
    public partial class News : System.Web.UI.Page
    {
        public municipalitiesEntities model;
        public ObservableCollection<FBNew> facebook;
        //public ObservableCollection<TwitterNew> twitter;


        public PagedDataSource pd = new PagedDataSource();


        protected void Page_Load(object sender, EventArgs e)
        {
            FillNews();
        }


        protected void FillNews()
        {
            var fbNewsId = Convert.ToInt32(Request.QueryString["id"]);

            model = new municipalitiesEntities();
            
            facebook = new ObservableCollection<FBNew>(model.FBNews.Where(a => a.FBNewsId==fbNewsId));
            rptNews.DataSource = facebook;
            rptNews.DataBind();


         

        }
    }
}