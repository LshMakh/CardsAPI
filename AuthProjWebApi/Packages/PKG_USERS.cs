using AuthProjWebApi.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
namespace AuthProjWebApi.Packages
{

    public interface IPKG_USERS
    {
        public void add_user(User user);
        public User? authenticate(Login loginData);
        public List<User> get_users();
        public User get_user_name(string name);
    }


    public class PKG_USERS :PKG_BASE, IPKG_USERS
    {


        public void add_user(User user)
        {
            string connstr = ConnStr;

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = connstr;
            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_LM_USERS.add_user";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_name", OracleDbType.Varchar2).Value = user.Username;
            cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = user.Email;
            cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = user.Password;

            cmd.ExecuteNonQuery();
            conn.Close();

        }
        public List<User> get_users()
        {
            List<User> users = new List<User>();
            string connstr = ConnStr;


            OracleConnection con = new OracleConnection();
            con.ConnectionString = connstr;
            con.Open();
            OracleCommand cmd = con.CreateCommand();
            cmd.Connection = con;
            cmd.CommandText = "olerning.PKG_LM_USERS.get_users";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                User user = new User();
                user.Id  = int.Parse(reader["id"].ToString());
                user.Username = reader["name"].ToString();
                user.Email = reader["email"].ToString();
                user.Password = reader["password"].ToString();
                user.Role = reader["role"].ToString();

                users.Add(user);
            }
            con.Close();
            return users;
        }

        public User? authenticate(Login loginData)
        {
            string connstr = ConnStr;

            OracleConnection con = new OracleConnection();
            con.ConnectionString = connstr;
            con.Open();
            OracleCommand cmd = con.CreateCommand();
            cmd.Connection = con;
            cmd.CommandText = "olerning.PKG_LM_USERS.authenticate";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_name",OracleDbType.Varchar2).Value = loginData.Username;
            cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value=loginData.Email;
            cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = loginData.Password;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();
            User? user = null;
            if (reader.Read())
            {
                user = new User();
                user.Id = int.Parse(reader["id"].ToString());
                user.Username = reader["name"].ToString();
                user.Email = reader["email"].ToString();
                user.Role = reader["role"].ToString();
            }
            con.Close();
            return user;
        }
        public User get_user_name(string name)
        {
            User user = null;
            string connstr = ConnStr;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = connstr;
            conn.Open();
            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText="olerning.PKG_LM_USERS.get_user_name";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_name", OracleDbType.Varchar2).Value = name;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            OracleDataReader reader = cmd.ExecuteReader();

            if(reader.Read())
            {
                user = new User()
                {
                    Id = int.Parse(reader["id"].ToString()),
                    Password = reader["password"].ToString(),
                    Username = reader["name"].ToString(),
                    Email = reader["email"].ToString(),
                    Role = reader["role"].ToString()
                };
                
                
            }


            return user;
        }


    }
}
