using AuthProjWebApi.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace AuthProjWebApi.Packages

{
    public interface IPKG_CARD
    {
        List<Card> GetCards();

        public void DeleteCard(int id);
    }

    public class PKG_CARD : IPKG_CARD
    {

        public List<Card> GetCards()
        {

            List<Card> cards = new List<Card>();
            string connstr = @"Data Source=(DESCRIPTION =  (ADDRESS = (PROTOCOL = TCP)(HOST = 172.20.0.188)
                            (PORT = 1521)) (CONNECT_DATA =   (SERVER = DEDICATED)
                            (SID = orcl)));User Id=olerning;Password=olerning";

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = connstr;
            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_LM_CARD.get_cards";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Card card = new Card();
                card.Id = int.Parse(reader["cards_id"].ToString());
                card.Name = reader["name"].ToString();
                card.Occupation = reader["occupation"].ToString();
                card.ImageUrl = reader["image_url"].ToString();

                cards.Add(card);
            }
            conn.Close();
            return cards;


        }

        public void DeleteCard(int cards_id)
        {
            {
                string connstr = @"Data Source=(DESCRIPTION =  (ADDRESS = (PROTOCOL = TCP)(HOST = 172.20.0.188)
                        (PORT = 1521)) (CONNECT_DATA =   (SERVER = DEDICATED)
                        (SID = orcl)));User Id=olerning;Password=olerning";

                using (OracleConnection conn = new OracleConnection(connstr))
                {
                    conn.Open();
                    OracleCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "olerning.PKG_LM_CARD.delete_card";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_cards_id", OracleDbType.Int32).Value = cards_id;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error deleting card: " + ex.Message);
                        throw;
                    }
                }
            }
        }
    }
}
