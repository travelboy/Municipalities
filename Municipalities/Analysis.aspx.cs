using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Municipalities
{
    public partial class Analysis : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PopulateMuncipalities.ddlPopulate(DropDownList1);

            PopulateMuncipalities.ddlPopulate(DropDownList2);
        }

 

       



        //zema statistika za FB aktivnostite na opstina za grafik
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string getStatsFB(dynamic munID, dynamic y)
        {

            string[] arrayMonths = 
            {
                "Јануари", "Фебруари", "Март", "Април", "Мај", "Јуни", "Јули", "Август", "Септември", "Октомври", "Ноември",
                "Декември"
            };

        
            municipalitiesEntities db = new municipalitiesEntities();

            ObservableCollection<FBNew> fbNews = new ObservableCollection<FBNew>(db.FBNews);


            int munID1 = Convert.ToInt32(munID);
            int year = Convert.ToInt32(y);
            int monthCount = 0;

            string json = "[";

            //sega gi zemame mesecnite podatoci!
            for (int x = 1; x <= 12; x++)
            {
                if (fbNews.Any(a => a.MunId == munID1 && a.FBmonth == x))
                {
                    monthCount = fbNews.Count(a => a.MunId == munID1 && a.FBmonth == x && a.FByear == year);

                    Chart f = new Chart()
                    {
                        label = arrayMonths[x - 1],
                        y = monthCount
                    };


                    json += JsonConvert.SerializeObject(f, Formatting.None);
                }
                //ako nema podatoci za toj mesec(na primer ima podatoci do septemvri od tekovnata godina), postavi gi ostanatite na 0
                else
                {

                    Chart f = new Chart()
                    {
                        label = arrayMonths[x - 1],
                        y = 0
                    };

                    json += JsonConvert.SerializeObject(f, Formatting.None);

                }

                json += ",";

            }
            //delete the last ,
            json = json.Remove(json.Length - 1, 1);
            json += "]";
           
    

            return json;
        }

        //statistika za twitter aktivnostite na opstina za grafik
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string getStatsTwitter(dynamic munID, dynamic y)
        {
            //empty json if no tweeter
            if (munID == "")
            
                return "";
            string[] arrayMonths = 
            {
                "Јануари", "Фебруари", "Март", "Април", "Мај", "Јуни", "Јули", "Август", "Септември", "Октомври", "Ноември",
                "Декември"
            };
        
            municipalitiesEntities db = new municipalitiesEntities();

            ObservableCollection<TwitterNew> twNews = new ObservableCollection<TwitterNew>(db.TwitterNews);
            int munID1 = Convert.ToInt32(munID);
            int year = Convert.ToInt32(y);
            int monthCount = 0;

            string json = "[";

            //sega gi zemame mesecnite podatoci!
            for (int x = 1; x <= 12; x++)
            {
                if (twNews.Any(a => a.MunId == munID1 && a.TMonth == x))
                {
                    monthCount = twNews.Count(a => a.MunId == munID1 && a.TMonth == x && a.TYear == year);

                   Chart f = new Chart()
                    {
                        label = arrayMonths[x - 1],
                        y = monthCount
                    };


                    json += JsonConvert.SerializeObject(f, Formatting.None);
                }
                //ako nema podatoci za toj mesec(na primer ima podatoci do septemvri od tekovnata godina), postavi gi ostanatite na 0
                else
                {

                    Chart f = new Chart()
                    {
                        label = arrayMonths[x - 1],
                        y = 0
                    };

                    json += JsonConvert.SerializeObject(f, Formatting.None);

                }

                json += ",";

            }
            //delete the last ,
            json = json.Remove(json.Length - 1, 1);
            json += "]";
       
            Console.WriteLine(json);
            return json;
        }



    

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string getStatsYT(dynamic munID, dynamic y)
        {
            //empty json if no tweeter
            if (munID == "")

                return "";



       
           
            string[] arrayMonths = 
            {
                "Јануари", "Фебруари", "Март", "Април", "Мај", "Јуни", "Јули", "Август", "Септември", "Октомври", "Ноември",
                "Декември"
            };
            //   string json = RemoveSpecialCharacters(");
            municipalitiesEntities db = new municipalitiesEntities();

            ObservableCollection<YTNew> yNews = new ObservableCollection<YTNew>(db.YTNews);
            int munID1 = Convert.ToInt32(munID);
            int year = Convert.ToInt32(y);
            int monthCount = 0;

            string json = "[";

            //sega gi zemame mesecnite podatoci!
            for (int x = 1; x <= 12; x++)
            {
                if (yNews.Any(a => a.MunId == munID1 && a.YTMonth == x))
                {
                    monthCount = yNews.Count(a => a.MunId == munID1 && a.YTMonth == x && a.YTYear == year);

                    Chart f = new Chart()
                    {
                        label = arrayMonths[x - 1],
                        y = monthCount
                    };


                    json += JsonConvert.SerializeObject(f, Formatting.None);
                }
                //ako nema podatoci za toj mesec(na primer ima podatoci do septemvri od tekovnata godina), postavi gi ostanatite na 0
                else
                {

                    Chart f = new Chart()
                    {
                        label = arrayMonths[x - 1],
                        y = 0
                    };

                    json += JsonConvert.SerializeObject(f, Formatting.None);

                }

                json += ",";

            }
            //delete the last ,
            json = json.Remove(json.Length - 1, 1);
            json += "]";
          
            return json;

        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string getYearlyInfo(dynamic munID, dynamic y)
        {

            int munID1 = Convert.ToInt32(munID);
            int year = Convert.ToInt32(y);

            municipalitiesEntities db = new municipalitiesEntities();

            ObservableCollection<FBNew> fbNews = new ObservableCollection<FBNew>(db.FBNews);
            ObservableCollection<TwitterNew> twNews = new ObservableCollection<TwitterNew>(db.TwitterNews);
           ObservableCollection<YTNew> ytNews = new ObservableCollection<YTNew>(db.YTNews);
            int fbAll = fbNews.Count(a => a.MunId == munID1 && a.FByear == year);
            int twAll = twNews.Count(a => a.MunId == munID1 && a.TYear == year);
            int ytAll = ytNews.Count(a => a.MunId == munID1 && a.YTYear == year);

            string json = "[ ";
            //some check if mun has fb
            Chart fb = new Chart()
            {
                label = "facebook",
                y = fbAll
            };
            json += JsonConvert.SerializeObject(fb, Formatting.None);

            json += ",";
            Chart tw = new Chart()
            {
                label = "twitter",
                y = twAll
            };
            json += JsonConvert.SerializeObject(tw, Formatting.None);

            json += ",";
            Chart yt = new Chart()
            {
                label = "youtube",
                y = ytAll
            };
            json += JsonConvert.SerializeObject(yt, Formatting.None);

            
            json += "]";

            return json;

        }



        internal class Chart
        {
            public string label { get; set; }
            public int y { get; set; }

        }
    }
   }
