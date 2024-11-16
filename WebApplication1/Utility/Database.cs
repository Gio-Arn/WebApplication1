using Microsoft.Data.SqlClient;

namespace WebApplication1.Utility
{
    public class Database
    {
        private bool _creato = false;

        private bool Creato {  get => _creato; }

        private SqlConnection Connection { get; set; }

        private static Database instance = null;

        private string nomeDB = "master";

        private Database(string nomeDB, string server = "localhost")
        {
            this.nomeDB = nomeDB;

            // connette con il master DB
            Connection = new SqlConnection($"Server={server};Integrated security=true;");

            try
            {
                // prova a connettersi al database che noi li passiamo
                Connection.Open();
                SqlCommand cmd = new SqlCommand($"USE {nomeDB}", Connection);
                cmd.ExecuteNonQuery();
                _creato = true;
            }
            catch (Exception ex)
            {
                // se non ci riesce, genera un'eccezione, allora andiamo a creare il DB
                Console.WriteLine("Sto creando il database attendere...");
                string query = $"CREATE DATABASE {nomeDB}";
                SqlCommand cmd = new SqlCommand(query , Connection);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand($"USE {nomeDB}", Connection);
                cmd.ExecuteNonQuery();
                Connection.Close();
                //connessione al nostro db
                Connection = new SqlConnection($"Server={server};Database={nomeDB};Integrated security=true;");
                try
                { //creazione delle tabelle
                    CreaTabelle();
                }
                catch
                {
                    Console.WriteLine("Errore durante la creazione delle tabelle: " + ex.Message);
                }
                //il db non era già creato in precedenza
                _creato = false;
            }
            finally
            {
                Connection.Close();//precauzione
                                   //aggiorna la connessione da master a db corrente se necessario
                Connection = new SqlConnection($"Server=localhost;Database={nomeDB};Integrated security=true;");
            }

        }
        public static Database GetInstance()
        {
            if (instance == null)
                instance = new Database("Prova_Codice_Fiscale");

            return instance;
        }

        public List<Dictionary<string,string>> Read(string query)
        {
            // per importare un record di una tabella sql lo salvo in un dizionario dove come chiave uso il nome della colonna
            // e come valore uso il contenuto della stessa nel record corrente
            // poi pero' per avere tutti i record in una singla locazione li salvo in una lista "ris"
            List<Dictionary<string, string>> ris = new List<Dictionary<string, string>>();

            // apro la connessione con il db
            using (SqlConnection conn = new SqlConnection(Connection.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query,conn);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    // read è un metodo del pacchetto sql.data... che ritorna true finchè vede un record successivo a quello corrente
                    while (dr.Read())
                    {
                        // mi salvo i dati del record in un dizionario che ha come chiave il nome della colonna
                        // e come valore il suo contenuto
                        Dictionary<string,string> riga = new Dictionary<string,string>();

                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            // GetName è un metodo di Sql.data... che si prende il nome della colonna corrente
                            var columnName = dr.GetName(i).ToLower();
                            // GetValue è un metodo di Sql.data... che si prende il valore della colonna corrente
                            object Columnvalue = dr.GetValue(i);

                            if (Columnvalue is byte[] byteArray)
                            {
                                string byteValues = BitConverter.ToString(byteArray);
                                riga.Add(columnName, byteValues);
                                
                            }
                            else
                            {
                                riga.Add(columnName, Columnvalue.ToString());
                            }
                        }

                        ris.Add(riga);
                    }
                }
            }

            return ris;

        }

        public Dictionary<string, string> ReadOne(string query)
        {
            try
            {
                return Read(query)[0];
            }
            catch
            {
                return null;
            }
        }

        public bool Update(string query)
        {
            try
            {
                Connection.Open();
                //Console.WriteLine("connessione a:" + Connection.Database);

                SqlCommand cmd = new SqlCommand(query, Connection);
                //Statistiche(Connection,query);

                int affette = cmd.ExecuteNonQuery();

                //Console.WriteLine("Righe affette: " + affette);

                return affette > 0;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"\nQUERY ERRATA:\n{query}");

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Errore generico" + "\n" + e.Message);
                return false;
            }
            finally
            {
                Connection.Close();
            }
        }

        public void CreaTabelle()
        {
            Update("CREATE TABLE Persona\r\n(\r\n\tid int primary key identity (1,1),\r\n\tnome varchar(100),\r\n\tcognome varchar(100),\r\n);");
        }
    }
}
