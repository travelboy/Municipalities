using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Municipalities
{
    public partial class YTActivities : System.Web.UI.Page
    {
        public municipalitiesEntities model;
        public ObservableCollection<YTNew> yt;



        public PagedDataSource pd = new PagedDataSource();

        // private int pos;
        int findex, lindex;
        DataRow dr;


        private int CurrentPage
        {
            get
            {   //Check view state is null if null then return current index value as "0" else return specific page viewstate value
                if (ViewState["CurrentPage"] == null)
                {
                    return 0;
                }
                else
                {
                    return ((int)ViewState["CurrentPage"]);
                }
            }
            set
            {
                //Set View statevalue when page is changed through Paging "RepeaterPaging" DataList
                ViewState["CurrentPage"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            FillYt();
        }


        protected void FillYt()
        {


            model = new municipalitiesEntities();
        
            yt = new ObservableCollection<YTNew>(model.YTNews.OrderBy(a => a.YTDay & a.YTMonth & a.YTYear));
            pd.DataSource = yt;
            rptYT.DataSource = pd;
            pd.AllowPaging = true;
            pd.PageSize = 15;
            pd.CurrentPageIndex = CurrentPage;
            // pd.DataSource = dt.DefaultView;
            lblpage.Text = "Страница  " + (CurrentPage + 1) + " од " + pd.PageCount;
            ViewState["totpage"] = pd.Count;
            lnkPrevious.Enabled = !pd.IsFirstPage;
            lnkNext.Enabled = !pd.IsLastPage;
            lnkFirst.Enabled = !pd.IsFirstPage;
            lnkLast.Enabled = !pd.IsLastPage;
            rptYT.DataBind();
            rptYT1.DataSource = pd;
            rptYT1.DataBind();
            doPaging();
        }
        private void doPaging()
        {
            DataTable dt = new DataTable();
            //Add two column into the DataTable "dt" 
            //First Column store page index default it start from "0"
            //Second Column store page index default it start from "1"
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");

            //Assign First Index starts from which number in paging data list
            findex = CurrentPage - 5;

            //Set Last index value if current page less than 5 then last index added "5" values to the Current page else it set "10" for last page number
            if (CurrentPage > 5)
            {
                lindex = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(yt.Count / pd.PageSize)));
            }
            else
            {
                lindex = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(yt.Count / pd.PageSize)));
            }

            //Check last page is greater than total page then reduced it to total no. of page is last index
            if (lindex > Convert.ToInt32(ViewState["totpage"]))
            {
                lindex = Convert.ToInt32(ViewState["totpage"]);
                findex = lindex - 5;
            }

            if (findex < 0)
            {
                findex = 0;
            }

            //Now creating page number based on above first and last page index
            for (int i = findex; i < lindex; i++)
            {
                dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

         
        }

        protected void lnkLeftClicked(object sender, EventArgs e)
        {
          
            CurrentPage -= 1;
            FillYt();

        }


        protected void lnkRightClicked(object sender, EventArgs e)
        {
            CurrentPage += 1;
         
            FillYt();

        }
        protected void firstClicked(object sender, EventArgs e)
        {
            CurrentPage = 0;
            FillYt();

        }


        protected void lastClicked(object sender, EventArgs e)
        {
            CurrentPage = Convert.ToInt32(ViewState["totpage"]);
            FillYt();

        }


        protected void RepeaterPaging_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName.Equals("newpage"))
            {
                //Assign CurrentPage number when user click on the page number in the Paging "RepeaterPaging" DataList
                CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
                //Refresh "Repeater1" control Data once user change page
                FillYt();
            }
        }
        protected void RepeaterPaging_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            //Enabled False for current selected Page index
            LinkButton lnkPage = (LinkButton)e.Item.FindControl("Pagingbtn");
            if (lnkPage.CommandArgument.ToString() == CurrentPage.ToString())
            {
                lnkPage.Enabled = false;
                lnkPage.BackColor = System.Drawing.Color.FromName("#686093");
                lnkPage.BorderColor = System.Drawing.Color.FromName("#000");
            }
        }

    }
}