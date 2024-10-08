using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace DFProject.Controllers
{
    public class SiteBaseController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            else
            {
                var userLanguage = Request.UserLanguages;
                var userLang = userLanguage != null ? userLanguage[0] : "";
                if (userLang != "")
                {
                    lang = userLang;
                }
                else
                {
                    lang = "tr";
                }
            }
            setLanguage(lang);
            return base.BeginExecuteCore(callback, state);
        }

        public void setLanguage(string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = "tr";
                }
                else
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
                    HttpCookie langCookie = new HttpCookie("culture", lang);
                    Response.Cookies.Add(langCookie);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

            
    }
}