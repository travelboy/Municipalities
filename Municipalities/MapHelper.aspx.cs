using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Municipalities
{
    public partial class MapHelper : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var munId = Convert.ToInt32(Request.QueryString["id"]);
            municipalitiesEntities model = new municipalitiesEntities();
            //prikazi go podatocite samo za baranata opstina
            ObservableCollection<FBInfo> info = new ObservableCollection<FBInfo>(model.FBInfoes.Where(a=>a.MunId==munId));
            rptMap.DataSource = info;
            rptMap.DataBind();

          


        }

        public string calcFB()
        {
            string result = "";
            var munId = Convert.ToInt32(Request.QueryString["id"]);
            municipalitiesEntities model = new municipalitiesEntities();
            ObservableCollection<FBNew> fbNews = new ObservableCollection<FBNew>(model.FBNews);
           

            int fbAll = fbNews.Count(a => a.MunId == munId);
         
            result = fbAll.ToString() + " постови од 01 Јануари 2013...";
            return result;
        }
        public string calcTW()
        {
            string result = "";
            var munId = Convert.ToInt32(Request.QueryString["id"]);
            municipalitiesEntities model = new municipalitiesEntities();
           
            ObservableCollection<TwitterNew> twNews = new ObservableCollection<TwitterNew>(model.TwitterNews);
         

     
            int twAll = twNews.Count(a => a.MunId == munId);

            result = twAll.ToString() + " tweet-ови од 01 Јануари 2013... ";
            return result;
        }
        public string calcYT()
        {
            string result = "";
            var munId = Convert.ToInt32(Request.QueryString["id"]);
            municipalitiesEntities model = new municipalitiesEntities();
            
            ObservableCollection<YTNew> ytNews = new ObservableCollection<YTNew>(model.YTNews);

            int ytAll = ytNews.Count(a => a.MunId == munId);
            result = ytAll.ToString() + "  видеа од 01 Јануари 2013...";
            return result;
        }
    }
}