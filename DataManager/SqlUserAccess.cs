// Replaces Accounts\CSTL_Pro_to_Pay\DataManager\SqlUserAccess.cs, which held the SQL Server login and password
// as compile-time constants (security finding S9 / GAP-SYS-002). The connection string now comes from
// configuration (ConnectionStrings:Template, "{db}" = catalog); DataAccessManager/DashboardDAL call ConnectionString().
namespace New_Accounts.DataManager
{
    public static class SqlUserAccess
    {
        public static string Template = "Server=localhost;Initial Catalog={db};Integrated Security=true;TrustServerCertificate=true";

        public static string ConnectionString(string database) => Template.Replace("{db}", database);
    }
}
