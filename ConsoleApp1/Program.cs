using WebApplication1.Models;
using WebApplication1.Utility;

IDAO p = DAOPersona.GetInstance();

Entity e = new Persona(1, "Giovanni", "Arnosti");

Console.WriteLine(p.Insert(e));
