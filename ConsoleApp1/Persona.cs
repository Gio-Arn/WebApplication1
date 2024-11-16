using System.Text.RegularExpressions;
using WebApplication1.Utility;

namespace WebApplication1.Models
{
    internal class Persona : Entity
    {
        public Persona() { }

        public Persona(int id ,string? nome,  string? cognome) : base(id)
        {
            Nome = nome;
            Cognome = cognome;
        }

        private string? _nome;

        public string? Nome { get => _nome;
            set{ 
                if (value != null)
                {
                    Regex userRGX = new Regex(@"^[a-zA-Z0-9_-]{4,15}$");
                    if (userRGX.Match(value).Success)
                        _nome = value;
                    else
                        throw new Exception("Nome non valido si prega di inserirne uno valido");
                }
                else
                {
                    _nome = null;
                }
            }
        }

        private string? _cognome;
        public string? Cognome
        {
            get => _cognome;
            set
            {
                if (value != null)
                {
                    Regex userRGX = new Regex(@"^[a-zA-Z0-9_-]{4,15}$");
                    if (userRGX.Match(value).Success)
                        _cognome = value;
                    else
                        throw new Exception("Cognome non valido si prega di inserirne uno valido");
                }
                else
                {
                    _cognome = null;
                }
            }
        }
    }
}
