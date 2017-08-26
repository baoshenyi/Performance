using System.Web;
using System.Web.Optimization;

namespace Performance
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //After css or javascript is bundled, 
            //send one http request instead of individual http requests; reduce network traffic
            CreateScriptBundles(bundles);
            CreateStyleBundles(bundles);
            //BundleTable will decide how to 
#if (DEBUG)
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }

        private static void CreateScriptBundles(BundleCollection bundles)
        {
            //@Scripts.Render to send one http request instead of individual http request
            //in View\Shared\_Layout.cshtml
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            bundles.Add(
                new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js"));
        }

        /// <summary>
        /// .tablesorter-blue .tablesorter-headerAsc {
        //background-color: #9fbfdf;
        ///* black asc arrow */
        //background-image: url(data:image/gif; base64,R0lGODlhFQAEAIAAACMtMP///yH5BAEAAAEALAAAAAAVAAQAAAINjI8Bya2wnINUMopZAQA7);
        ///* white asc arrow */
        ///* background-image: url(data:image/gif;base64,R0lGODlhFQAEAIAAAP///////yH5BAEAAAEALAAAAAAVAAQAAAINjI8Bya2wnINUMopZAQA7); */
        ///* image */
        ///* background-image: url(images/black-asc.gif); */
        /// </summary>
        /// <param name="bundles"></param>
        private static void CreateStyleBundles(BundleCollection bundles)
        {
            
            //@Styles.Render to send one http request instead of individual http request
            bundles.Add(
                new StyleBundle("~/Content/css")
                .Include(
                    //"~/Content/bootstrap.css",
                    "~/Content/site.css")
                .Include("~/Scripts/tablesorter/css/theme.blue.css"));
        }
    }
}
