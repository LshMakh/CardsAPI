using AuthProjWebApi.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace AuthProjWebApi.Packages

{
    public interface IPKG_CARD
    {
        public List<Card> GetCards();

        public void UpdateCard(Card card);

        public Card GetCardbyId(int id);

        public void SaveCard(Card card);

        public void DeleteCard(int id);
    }

    public class PKG_CARD :PKG_BASE, IPKG_CARD
    {

        public List<Card> GetCards()
        {

            List<Card> cards = new List<Card>();
            string connstr = ConnStr;


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
                string connstr = ConnStr;


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
        public void SaveCard(Card card) {

            OracleConnection conn = new OracleConnection();
            string connstr = ConnStr;

            conn.ConnectionString = connstr;
            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_LM_CARD.save_card";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_name", OracleDbType.Varchar2).Value = card.Name;
            cmd.Parameters.Add("p_occupation", OracleDbType.Varchar2).Value = card.Occupation;
            cmd.Parameters.Add("p_image_url", OracleDbType.Varchar2).Value = card.ImageUrl;

            cmd.ExecuteNonQuery();
            conn.Close();




        }

        public Card GetCardbyId(int id) {

            Card card = null;

            OracleConnection conn = new OracleConnection();
            string connstr = ConnStr;

            conn.ConnectionString = connstr;
            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "olerning.PKG_LM_CARD.get_card_by_id";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_card_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            OracleDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                card = new Card {

                    Id = int.Parse(reader["cards_id"].ToString()),
                    Name = reader["name"].ToString(),
                    Occupation = reader["occupation"].ToString(),
                    ImageUrl = reader["image_url"].ToString()

                };
            }

            return card;
        }

        public void UpdateCard(Card card)
        {
            OracleConnection conn = new OracleConnection();
            string connstr = ConnStr;

            conn.ConnectionString = connstr;
            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = "olerning.PKG_LM_CARD.update_card";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_cards_id", OracleDbType.Int32).Value = card.Id;
            cmd.Parameters.Add("p_name", OracleDbType.Varchar2).Value = card.Name;
            cmd.Parameters.Add("p_occupation", OracleDbType.Varchar2).Value = card.Occupation;
            cmd.Parameters.Add("p_image_url", OracleDbType.Varchar2).Value = card.ImageUrl;
            cmd.ExecuteNonQuery();
            conn.Close();

        }
    }
}
