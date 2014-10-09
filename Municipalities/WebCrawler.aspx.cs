using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;
using System.Text.RegularExpressions;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using LinqToTwitter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Municipalities
{
    public partial class WebCrawler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var socialmedia = Request.QueryString["id"];
            
               //IMPLEMENTACIJA NA WEB CRAWLER!!!
         
               #region authentication 

               ////FACEBOOK Settings////
               var client = new WebClient();
               string oauthUrl = string.Format("https://graph.facebook.com/oauth/access_token?type=client_cred&client_id={0}&client_secret={1}", "585761644868486", "4fb562bc94c58b8a92e10991cb0dcd7b");

               string accessToken = client.DownloadString(oauthUrl).Split('=')[1];
               ////End FACEBOOK Settings////

            

               ////TWITTER Settings////
               // postavuvanje na soodvetnite klucevi potrebni za twitter avtentikacija
               var oAuthConsumerKey = "61zzDpIh3WSrbcyBdKPES12bx";
               var oAuthConsumerSecret = "bvcNECRQOyAU4E74rxh9XxLmOLzdq6GarYdDGUzZBN1xrDGTWS";
               var oAuthUrl = "https://api.twitter.com/oauth2/token";
               // avtentikacija
               //var authHeaderFormat = "Basic {0}";

               var authHeader = string.Format("Basic {0}",
               Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(oAuthConsumerKey) + ":" +
               Uri.EscapeDataString((oAuthConsumerSecret)))
               ));

               var postBody = "grant_type=client_credentials";

               HttpWebRequest authRequest = (HttpWebRequest)WebRequest.Create(oAuthUrl);
               authRequest.Headers.Add("Authorization", authHeader);
               authRequest.Method = "POST";
               authRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
               authRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

               using (Stream stream = authRequest.GetRequestStream())
               {
                   byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
                   stream.Write(content, 0, content.Length);
               }

               authRequest.Headers.Add("Accept-Encoding", "gzip");

               WebResponse authResponse = authRequest.GetResponse();
               // deserialize into an object
               TwitAuthenticateResponse twitAuthResponse;
               using (authResponse)
               {
                   using (var reader = new StreamReader(authResponse.GetResponseStream()))
                   {
                       var objectText = reader.ReadToEnd();
                       twitAuthResponse = JsonConvert.DeserializeObject<TwitAuthenticateResponse>(objectText);
                   }

               }
            
            
               
               ////End TWITTER Settings////
            
               //kje se koristi za kirilcna poddrshka i za FAcebook i za Twitter
               Regex regex = new Regex(@"\\u([a-f0-9]{4})", RegexOptions.IgnoreCase);
               #endregion

               // zemi gi podatocite od bazata na Opstinite
               municipalitiesEntities db1 = new municipalitiesEntities();
      
              
               //zapishuvanje na novi podatoci vo bazata!
               //Brishi ja info tabelata za site zapisi-brojot na views i subscribers se menuva cesto 
               db1.Database.ExecuteSqlCommand("TRUNCATE TABLE [YTInfo]");

               municipalitiesEntities db = new municipalitiesEntities();

               ObservableCollection<Municipality> municipalities = new ObservableCollection<Municipality>(db.Municipalities);


               ObservableCollection<TwitterNew> twNews = new ObservableCollection<TwitterNew>(db.TwitterNews);

               ObservableCollection<FBInfo> fInfo = new ObservableCollection<FBInfo>(db.FBInfoes);
               ObservableCollection<TwitterInfo> tInfo = new ObservableCollection<TwitterInfo>(db.TwitterInfoes);

               ObservableCollection<YTNew> ytNews = new ObservableCollection<YTNew>(db.YTNews);

            foreach (var mun in municipalities)
            {

                
                      #region facebookData
               
                if (socialmedia == "facebook")
                {
                    string FBLinkMun = mun.FBId;


                    //samo ako postoi facebook page
                    if (FBLinkMun != "")
                    {
                        String Info = "";
                        String Posts = "";
                        if (FBLinkMun.Contains('/'))
                        {
                           
                            FBLinkMun = FBLinkMun.Substring(FBLinkMun.IndexOf('/')+1);
                           
                           

                        }
                        try
                        {
                            Info =
                                client.DownloadString(
                                    string.Format("https://graph.facebook.com/" + FBLinkMun + "?access_token={0}",
                                        accessToken));


                        }
                        catch (WebException greshka)
                        {
                            string a = greshka.Message;
                            
                        }
                        try
                        {
                            Posts =
                                client.DownloadString(
                                    string.Format(
                                        "https://graph.facebook.com/" + FBLinkMun +
                                        "/posts?since=2012-01-01&access_token={0}",
                                        accessToken));


                        }
                        catch (WebException greshka1)
                        {
                            string a = greshka1.Message;
                           
                        }

                        bool newMunFb = false;
                        if (Info != "" && Posts != "")
                        {

                            string pageInfo = regex.Replace(Info,
                                match =>
                                    ((Char) Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString());
                            string pagePosts = regex.Replace(Posts,
                                match =>
                                    ((Char) Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString());

                            dynamic resultInfo = Newtonsoft.Json.JsonConvert.DeserializeObject(pageInfo);
                            dynamic resultPosts = Newtonsoft.Json.JsonConvert.DeserializeObject(pagePosts);

                            //informacii koi ni se potrebni
                            //najprvo od resultInfo - generalni informacii za opstinite dostapni od stranata

                            var oldMun = fInfo.Any((aa => aa.MunId == mun.MunId));

                            //only if new municipality is here
                            if (oldMun == false)
                            {
                                newMunFb = true;
                                putFBInfoInDatabase(resultInfo, db, mun.MunId);

                            } //end new municipality add basic facebook info

                            var olderNewsExist = putFBNewsInDatabase(resultPosts, newMunFb, db, mun.MunId)[1];
                            bool state = (bool) olderNewsExist;
                            String Posts1 = "sledna strana";
                            //postoi naredna(postara) stranica!
                            if (state)
                                while (Posts1 != "")
                                {
                                    try
                                    {
                                        Posts1 =
                                            client.DownloadString(string.Format(
                                                resultPosts["paging"]["next"].ToString(),
                                                accessToken));
                                    }
                                    catch (WebException expct)
                                    {

                                        Console.WriteLine(expct.Message);
                                        throw expct;
                                    }


                                    string pagePosts1 = regex.Replace(Posts1,
                                        match =>
                                            ((Char) Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber))
                                                .ToString());
                                    dynamic resultPosts1 = Newtonsoft.Json.JsonConvert.DeserializeObject(pagePosts1);
                                    JArray items1 = (JArray) resultPosts1["data"];

                                    if (items1.Count != 0)
                                    {
                                        resultPosts = putFBNewsInDatabase(resultPosts1, newMunFb, db, mun.MunId)[0];

                                        //resultPosts = client.DownloadString(string.Format(resultPosts["paging"]["next"].ToString(), accessToken));
                                    }
                                    else
                                        Posts1 = "";


                                }

                        }
                    }
                }

                #endregion
        
                   
                      #region twitterData
                      //////TWITTER Data////
                      ////twitter imeto na opstinata
                     
                if (socialmedia == "twitter")
                {
                    var screenname = mun.TwitterID;


                    //ako postoi twitter profil za korisnikot
                    if (screenname != "")
                    {
                        bool newTwMun = false;
                       


                        string max_id = "";
                        var twitterInfo = getJson(screenname, twitAuthResponse, regex, max_id);
                        var newMTunCheck = tInfo.Where((aa => aa.MunId == mun.MunId)).Any();
                        if (newMTunCheck == false)
                        {
                            //its a new municipality!
                            newTwMun = true;

                            //basic user info!
                            var tweetDescription = "";
                            if (twitterInfo[0]["user"]["description"] != null)
                                tweetDescription = twitterInfo[0]["user"]["description"].ToString();

                            //zemi go posledniot zacuvan podatok za opstinata koja se razgleduva vo momentot


                            var tweetFollowers = "";
                            if (twitterInfo[0]["user"]["followers_count"] != null)
                                tweetFollowers = twitterInfo[0]["user"]["followers_count"].ToString();

                            var tweetFollowing = "";
                            if (twitterInfo[0]["user"]["friends_count"] != null)
                                tweetFollowing = twitterInfo[0]["user"]["friends_count"].ToString();

                            var tweetProfileCreated = "";
                            if (twitterInfo[0]["user"]["created_at"] != null)
                                tweetProfileCreated = twitterInfo[0]["user"]["created_at"].ToString();

                            var tweetNumTweets = "";
                            if (twitterInfo[0]["user"][" statuses_count"] != null)
                                tweetNumTweets = twitterInfo[0]["user"][" statuses_count"];

                            var tweetProfilePic = "";
                            if (twitterInfo[0]["user"][" profile_image_url"] != null)
                                tweetProfilePic = twitterInfo[0]["user"][" profile_image_url"].ToString();


                            TwitterInfo ti = new TwitterInfo();
                            ti.MunId = mun.MunId;
                            ti.TFollowers = tweetFollowers;
                            ti.TFollowing = tweetFollowing;
                            ti.TProfileCreated = tweetProfileCreated;
                            ti.TProfilePic = tweetProfilePic;
                            ti.Tdescription = tweetDescription;
                            ti.TNumTweers = tweetNumTweets;
                            db.TwitterInfoes.Add(ti);
                            db.SaveChanges();
                        } //end update new municipalities only 

                        String PostsT = "sledna stranica";
                        bool year2012 = false;
                        while (PostsT != "")
                        {

                            JArray itemsTwitter = (JArray) twitterInfo;
                            int numTweets = itemsTwitter.Count;

                            //ako nema informacii vo vrateniot string
                            if (numTweets != 0 && year2012 == false)
                            {
                                //  var compareT = "";

                                //  if (newTwMun == false)
                                //     compareT = twNews.Where((a => a.MunId == mun.MunId)).First().TCreated;

                                //Tweets from profile!!
                                for (int j = 0; j < numTweets; j++)
                                {

                                    var tweetCreated = "";
                                    if (twitterInfo[j]["created_at"] != null)
                                        tweetCreated = twitterInfo[j]["created_at"].ToString();

                                    bool anyT = twNews.Any(aaa => aaa.TCreated == tweetCreated && aaa.MunId == mun.MunId);
                                    //ne sakame podatocite da se povtoruvaat. compare go sodrzi posledniot procitan tweet od bazata
                                    //ako e ist so onoj noviot od json objektot znaci deka nemame novi procitani sodrzini i izleguvame od ciklusot
                              
                                    if (anyT)
                                        break;

                                    var tweetText = "";
                                    if (twitterInfo[j]["text"] != null)
                                        tweetText = twitterInfo[j]["text"].ToString();




                                    var tweetLink = "";
                                    if (twitterInfo[j]["entities"]["urls"].ToString() != "[]")
                                        tweetLink = twitterInfo[j]["entities"]["urls"][0]["url"].ToString();

                                    int Tmonth = convertMonth(tweetCreated.Substring(4, 3));
                                    int Tyear =
                                        Convert.ToInt32(tweetCreated.Substring(tweetCreated.LastIndexOf(' ') + 1, 4));
                                    int Tday = Convert.ToInt32(tweetCreated.Substring(8, 3));
                                    if (Tyear <= 2012)
                                    {
                                        year2012 = true;
                                        break;
                                    }
                                    var tweetMedia = "";

                                    try
                                    {
                                        if (twitterInfo[j]["entities"]["media"].ToString() != "[]")
                                            tweetMedia = twitterInfo[j]["entities"]["media"][0]["media_url"].ToString();
                                    }

                                    catch (Exception exception)
                                    {
                                        Console.WriteLine("{0} Exception caught.", exception);
                                    }

                                    TwitterNew tn = new TwitterNew();
                                    tn.MunId = mun.MunId;
                                    tn.TCreated = tweetCreated;
                                    tn.TLink = tweetLink;
                                    tn.TText = tweetText;
                                    tn.TMonth = Tmonth;
                                    tn.TDay = Tday;
                                    tn.TYear = Tyear;
                                    tn.TMedia = tweetMedia;
                                    db.TwitterNews.Add(tn);
                                    db.SaveChanges();




                                } //gi izvrte site twitovi dobieni od eden request

                            }
                                //prazen tweet
                            else
                            {
                                PostsT = "";
                            }
                            max_id = twitterInfo[numTweets - 1]["id"].ToString();
                            twitterInfo = getJson(screenname, twitAuthResponse, regex, max_id);
                        }
                    }
                }
                //////End TWITTER Data////
                      #endregion

                     

                #region youtubeData
                   
                if (socialmedia == "youtube")
                {
                    ////YOUTUBE////
                    string check = mun.YTId;
                    //enter yt part
                    
                    if (check != "")
                    {



                        var youtube = new YouTubeService(new BaseClientService.Initializer()
                        {
                            //HttpClientInitializer = credential,
                        
                            ApplicationName = this.GetType().ToString(),
                            ApiKey = "AIzaSyAUB1U07aL_zqnoj8T5FhRILtEA7HGzKpE"
                        });




                        //info za kanalot
                        var channelInfoReq = youtube.Channels.List("statistics");
                        // channelInfoReq.Id=

                        //znak deka treba da rabotime so ID, mesto so username
                        if (check.StartsWith("UC"))
                            channelInfoReq.Id = check;
                        else
                            channelInfoReq.ForUsername = check;

                        var channelInfo = channelInfoReq.Execute();
                        int subscribers = 0;
                        int totalViews = 0;
                        if (channelInfo.Items.Any())
                        {
                            if (channelInfo.Items[0].Statistics.SubscriberCount.HasValue)
                                subscribers = Convert.ToInt32(channelInfo.Items[0].Statistics.SubscriberCount.Value);
                            if (channelInfo.Items[0].Statistics.ViewCount.HasValue)
                                totalViews = Convert.ToInt32(channelInfo.Items[0].Statistics.ViewCount.Value);
                        }


                        YTInfo yt = new YTInfo();
                        yt.MunId = mun.MunId;
                        yt.subscrCount = subscribers;
                        yt.viewsTotal = totalViews;
                        db.YTInfoes.Add(yt);
                        db.SaveChanges();


                        //ottuka pocnuva delot so videata

                        var channelsListRequest = youtube.Channels.List("contentDetails");
                        //ova da se smeni, nekoi opstini se so drugi iminja??
                        if (check.StartsWith("UC"))
                            channelsListRequest.Id = check;
                        else
                            channelsListRequest.ForUsername = check;

                        var channelsListResponse = channelsListRequest.Execute();
                        //promenliva koja simbolizira rednudantni informacii vo bazata
                        bool old = false;

                        //odime niz kanalite na izbranata opstina
                        foreach (var channel in channelsListResponse.Items)
                        {
                            var uploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;

                            var nextPageToken = "";
                            while (nextPageToken != null && old == false)
                            {
                                var playlistItemsListRequest = youtube.PlaylistItems.List("snippet");
                                playlistItemsListRequest.PlaylistId = uploadsListId;
                                playlistItemsListRequest.MaxResults = 50;
                                playlistItemsListRequest.PageToken = nextPageToken;

                                //Tuka ja dobivame listata na videa za korisnikot
                                var playlistItemsListResponse = playlistItemsListRequest.Execute();

                                foreach (var playlistItem in playlistItemsListResponse.Items)
                                {
                                    // Print information about each video.
                                    //proverka dali vekje postoi toa video vo listata, t.e dali vnesuvame redundantni informacii vo bazata
                                    string vID = playlistItem.Snippet.ResourceId.VideoId;
                                    string t = playlistItem.Snippet.Title;
                                    bool any =
                                        ytNews.Any(
                                            a =>
                                                a.MunId == mun.MunId && a.YTVideo == vID &&
                                                a.YTTitle == playlistItem.Snippet.Title);

                                    //ima video so takov ID
                                    if (any)
                                    {
                                        old = true;
                                        break;

                                    }
                                    //novo video! dodadi go (ako e ponovo od 2012)
                                    if (playlistItem.Snippet.PublishedAt.Value.Year <= 2012)
                                    {
                                        old = true;
                                        break;
                                    }
                                    YTNew video = new YTNew();
                                    video.MunId = mun.MunId;
                                    video.YTVideo = vID;
                                    video.YTTitle = playlistItem.Snippet.Title;
                                    video.YTDay = playlistItem.Snippet.PublishedAt.Value.Day;
                                    video.YTMonth = playlistItem.Snippet.PublishedAt.Value.Month;
                                    video.YTYear = playlistItem.Snippet.PublishedAt.Value.Year;

                                    video.YTTitle = playlistItem.Snippet.Title;
                                    db.YTNews.Add(video);
                                    db.SaveChanges();

                                }

                                nextPageToken = playlistItemsListResponse.NextPageToken;
                            }
                        }




                    }

                } ////END YOUTUBE//// 


                #endregion


                }
            










            ////////end test

    

           }

           public static bool IsNullOrEmpty<T>( IEnumerable<T> enumerable)
           {
               return enumerable == null ;
           }
           //needed for twitter API
           public int convertMonth(string text)
           {
               int conText=0;

               switch (text)
               {
                   case "Jan":
                       conText = 1;
                       break;
                   case "Feb":
                       conText = 2;
                       break;
                   case "Mar":
                       conText = 3;
                       break;
                   case "Apr":
                       conText = 4;
                       break;
                   case "May":
                       conText = 5;
                       break;
                   case "Jun":
                       conText = 6;
                       break;
                   case "Jul":
                       conText = 7;
                       break;
                   case "Aug":
                       conText = 8;
                       break;
                   case "Sep":
                       conText = 9;
                       break;
                   case "Oct":
                       conText = 10;
                       break;
                   case "Noe":
                       conText = 11;
                       break;
                   case "Dec":
                       conText = 12;
                       break;
               }

               return conText;
                



        }

           public dynamic[] putFBNewsInDatabase(dynamic XresultPosts, bool XnewMunFb, municipalitiesEntities Xdb,int MunID)
        {
            ObservableCollection<FBNew> XfbNews = new ObservableCollection<FBNew>(Xdb.FBNews);
            //kolku postovi prikazuva stranicata
            JArray items = (JArray)XresultPosts["data"];
            int numFBPosts = items.Count;

               dynamic getback = true;

            //potoa od resultPosts - listanje na postovite od FB stranata 
            //sega za site postovi koi gi ima, zemi gi soodvetnite podatoci
            var FBname = "";
            var FBmessage = "";
            string oldInfo = "a";
            var compareFBData = "";

            //ako se raboti za opstina koja vekje postoi vo bazata 
           // if (XnewMunFb == false)
              ///  compareFBData = XfbNews.Where((a => a.MunId == MunID)).First().FBcreated;
               
            for (int i = 0; i < numFBPosts; i++)
            {

                var FBcreated = "";
                if (XresultPosts["data"][i]["created_time"] != null)
                    FBcreated = XresultPosts["data"][i]["created_time"].ToString();
                
                bool any = XfbNews.Any(a => a.FBcreated == FBcreated && a.MunId == MunID);
                if (any)
                {
                    getback = false;
                    return new [] {XresultPosts,getback};
                   
                    
                }
           

                //imame redundantni informacii vo ramkite na samoto zemanje na podatocite
                if (XresultPosts["data"][i]["name"] != null)
                    FBname = XresultPosts["data"][i]["name"].ToString();

                if (oldInfo != FBname)
                {


                    if (XresultPosts["data"][i]["message"] != null)
                        FBmessage = XresultPosts["data"][i]["message"].ToString();
                    if (FBmessage.Length > 3999)
                        FBmessage=FBmessage.Substring(0, 3999);

                    var FBpicture = "";
                    if (XresultPosts["data"][i]["picture"] != null)
                    {
                        FBpicture = XresultPosts["data"][i]["picture"].ToString();

                        if (FBpicture == "")
                            FBpicture = "images\facebook.pgn";
                    }



                    var year = FBcreated.Substring(FBcreated.LastIndexOf('/') + 1, 4);
                    var month = FBcreated.Substring(0, FBcreated.IndexOf('/'));

                    int q = FBcreated.LastIndexOf('/') - FBcreated.IndexOf('/')-1;
                    var day = FBcreated.Substring(FBcreated.IndexOf('/') + 1, q);
                    
                    if (Convert.ToInt32(year) <= 2012)
                        break;
                    //ako postot e link, daj pristap do linkot
                    var FBlink = "";
                    if (XresultPosts["data"][i]["type"].ToString() == "link")
                        if (XresultPosts["data"][i]["link"] != null)
                        FBlink = XresultPosts["data"][i]["link"].ToString();

                    FBNew news = new FBNew();
                    news.FBcreated = FBcreated;
                    news.FBlink = FBlink;
                    news.FBmessage = FBmessage;
                    news.FBname = FBname;
                    news.FBpicture = FBpicture;
                    news.MunId = MunID;
                    news.FByear = Convert.ToInt32(year);
                    news.FBmonth = Convert.ToInt32(month);
                    news.FBday = Convert.ToInt32(day);

                    Xdb.FBNews.Add(news);
                    try
                    {
                        Xdb.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var eve in ex.EntityValidationErrors)
                        {
                            Console.WriteLine(
                                "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                }
                //set done for the next municipality

                //set old info to the last read info
                oldInfo = FBname;

            }//end the FBposts cycle


            return new[] { XresultPosts, getback }; 


        }

        public void putFBInfoInDatabase(dynamic resultInfo,municipalitiesEntities db,int munID)
        {
            var FBdescription = "";
            if (resultInfo["description"] != null)
                FBdescription = resultInfo["description"].ToString();

            if (FBdescription.Length >= 4000)
                FBdescription = FBdescription.Substring(0, 3999);

            var FBgeneralInfo = "";
            if (resultInfo["general_info"] != null)
                FBgeneralInfo = resultInfo["general_info"].ToString();


            var FBstreet = "";
            try
            {
                if (resultInfo["location"]["street"].ToString() != "")
                    FBstreet = resultInfo["location"]["street"].ToString();
            }

            catch (Exception exception)
            {
                Console.WriteLine("{0} Exception caught.", exception);
            }


            var FBphone = "";
            if (resultInfo["phone"] != null)
                FBphone = resultInfo["phone"].ToString();

            var website = "";
            if (resultInfo["website"] != null)
                website = resultInfo["website"].ToString();


            try
            {

                //kreiraj objekt
                FBInfo fbinfo = new FBInfo();

                fbinfo.FBdescription = FBdescription;
                fbinfo.FBgeneralInfo = FBgeneralInfo;
                fbinfo.FBphoto = FBphone;
                fbinfo.FBstreet = FBstreet;
                fbinfo.FBwebsite = website;
                fbinfo.MunId = munID;
                db.FBInfoes.Add(fbinfo);
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine(
                        "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public dynamic getJson(string screenname,TwitAuthenticateResponse twitAuthResponse,Regex regex,string max_id)
        {

            string add = "&max_id=";
            if (max_id == "")
            {add = "";}
            else
            {
                add += max_id;
            }

            var timelineFormat = "https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={0}&include_rts=1&exclude_replies=1&count=200"+add;

            var timelineUrl = string.Format(timelineFormat, screenname);

            var timelineHeaderFormat = "{0} {1}";

            HttpWebRequest timeLineRequest;
            WebResponse timeLineResponse;

            // Create a web request 
            timeLineRequest = (HttpWebRequest)WebRequest.Create(timelineUrl);
            timeLineRequest.Headers.Add("Authorization",
                string.Format(timelineHeaderFormat, twitAuthResponse.token_type,
                    twitAuthResponse.access_token));
            timeLineRequest.Method = "Get";

            // Get the associated response for the above request.
            timeLineResponse = timeLineRequest.GetResponse();



            var timeLineJson = string.Empty;
            dynamic twitterInfo = string.Empty;

            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    timeLineJson = reader.ReadToEnd();
                    var tw = regex.Replace(timeLineJson,
                        match =>
                            ((Char)Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString());
                    // dynamic twitterInfo = JsonConvert.DeserializeObject(tw);
                    twitterInfo = Newtonsoft.Json.JsonConvert.DeserializeObject(tw);

                }
            }

            return twitterInfo;

        }

      

        


        //end page load
    }//end class web crawler


    public class TwitAuthenticateResponse
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
    }
}
