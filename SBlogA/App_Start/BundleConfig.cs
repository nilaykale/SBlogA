using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;


namespace SBlogA.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles) {

            bundles.Add(new StyleBundle("~/admin/styles")
                    .Include("~/content/css/bootstrap.css")
                    .Include("~/content/css/admin.css")
                );

            bundles.Add(new StyleBundle("~/styles")
                .Include("~/content/css/bootstrap.css")
                .Include("~/content/css/site.css")
            );


            bundles.Add(new ScriptBundle("~/admin")
                .Include("~/Areas/Admin/Scripts/Forms.js")
                .Include("~/content/js/jquery-3.4.1.min.js")
            );

        }


    }
}