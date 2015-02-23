using System.Configuration;

namespace Sofia.Data.Logic
{
    public class AdoHelper
    {
        //public static SqlConnection WebConnection
        //{
        //    get
        //    {
        //        string connectionString = ConfigurationManager.ConnectionStrings["Sofa"].ConnectionString;
        //        var myConnection = new SqlConnection(connectionString);
        //        return myConnection;
        //    }
        //}

        public static string WebConnectionNameAxapta
        {
            get { return ConfigurationManager.ConnectionStrings["Sofa"].Name; }
        }

        public static string WebConnectionNameFactelligence
        {
            get { return ConfigurationManager.ConnectionStrings["Factelligence"].Name; }
        }

        //public static string WebConnectionString
        //{
        //    get { return ConfigurationManager.ConnectionStrings["Sofa"].ConnectionString; }
        //}

        //public static SqlConnection GetConnection(string connectionName)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        //    var myConnection = new SqlConnection(connectionString);
        //    return myConnection;
        //}
    }
}