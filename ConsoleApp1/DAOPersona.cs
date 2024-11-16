using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using WebApplication1.Utility;

namespace WebApplication1.Models
{
    public class DAOPersona : IDAO
    {
        private Database db;

        #region singleton
        private static DAOPersona instance = null;

        private DAOPersona()
        {
            db = Database.GetInstance();
        }

        public static DAOPersona GetInstance()
        {
            if (instance == null)
                instance = new DAOPersona();
            return instance;
        }
        #endregion

        public bool Delete(int id)
        {
            bool del = false;

            try
            {
                del = db.Update($"delete from Persona where id = {id}");
            } 
            catch
            {
                throw new ArgumentException("Errore rimozione id errato");
            }

            return del;
        }

        // per ora lascio id ma nel mio caso dovro' mettere codice fiscale
        public Entity Find(int id)
        {
            Dictionary<string,string> riga = null;
            try
            {
                riga = db.ReadOne($"select * from Persona where id = {id};");
            }
            catch
            {
                throw new ArgumentException("Errore la persona selezionata non esistente");
            }

            Persona persona = new Persona();

            if(riga != null)
                persona.FromDictionary(riga);

            return persona;
        }

        public Entity Find(string nome, string cognome)
        {
            Dictionary<string, string> riga = null;
            try
            {
                riga = db.ReadOne($"select * from Persona where nome = {nome} AND cognome = {cognome};");
            }
            catch
            {
                throw new ArgumentException("Errore la persona selezionata non esistente");
            }

            Persona persona = new Persona();

            if (riga != null)
                persona.FromDictionary(riga);

            return persona;
        }

        public bool Insert(Entity e)
        {
            try
            {
                string query = $"INSERT INTO Persona (nome, cognome) VALUES " +
                               $"('{((Persona)e).Nome}','{((Persona)e).Cognome}')";
                
                return db.Update(query);
            }
            catch
            {
                throw new ArgumentException("Errore nell'inserimento");
            }

        }
        public List<Entity> ReadAll()
        {
            List<Entity> lista = new();

            List<Dictionary<string,string>> righe = null;
            try
            {
                righe = db.Read("SELECT * FROM Persona");
            }
            catch 
            {
                throw new ArgumentException("errore nella lettura");
            }

            if (righe != null)
                foreach(var r in righe)
                {
                    Persona p = new Persona();
                    p.FromDictionary(r);
                    lista.Add(p);
                }

            return lista;
        }

        public bool Update(Entity e)
        {
            try
            {
                string query = $"UPDATE Persona SET" +
                               $"('{((Persona)e).Nome}','{((Persona)e).Cognome}')";

                return db.Update(query);
            }
            catch
            {
                throw new ArgumentException("Errore nella modifica");
            }
        }
    }
}
