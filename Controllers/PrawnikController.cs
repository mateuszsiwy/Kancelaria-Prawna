using Microsoft.AspNetCore.Mvc;
using Npgsql;
using BazyDanych1Projekt.Models;
namespace BazyDanych1Projekt.Controllers
{
    public class PrawnikController : Controller
    {
        private readonly NpgsqlConnection _dbConnection;

        public PrawnikController(NpgsqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            string query = "DELETE FROM Prawnik WHERE id_prawnika = @id";
            using (var cmd = new NpgsqlCommand(query, _dbConnection))
            {
                _dbConnection.Open();

                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
                _dbConnection.Close();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AddOrEdit(int id=0)
        {
            if (id == 0)
                return View(new Prawnik { IdPrawnika = id});
            else
            {
                _dbConnection.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Prawnik WHERE id_prawnika = @id", _dbConnection);
                cmd.Parameters.AddWithValue("id", id);
                var reader = cmd.ExecuteReader();
                Prawnik prawnik = new Prawnik();
                while (reader.Read())
                {
                    prawnik.IdPrawnika = reader.GetInt32(0);
                    prawnik.Imie = reader.GetString(1);
                    prawnik.Nazwisko = reader.GetString(2);
                }
                _dbConnection.Close();
                return View(prawnik);
            }
        }

        [HttpPost]
        public IActionResult AddOrEdit(Prawnik prawnik)
        {
            if (prawnik.IdPrawnika == 0)
            {
                string query = "INSERT INTO Prawnik(imie, nazwisko) VALUES(@imie, @nazwisko)";
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    cmd.Parameters.AddWithValue("imie", prawnik.Imie);
                    cmd.Parameters.AddWithValue("nazwisko", prawnik.Nazwisko);
                    cmd.ExecuteNonQuery();
                    _dbConnection.Close();
                }
            }
            else
            {
                string query = "UPDATE Prawnik SET imie = @imie, nazwisko = @nazwisko WHERE id_prawnika = @id";
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    cmd.Parameters.AddWithValue("imie", prawnik.Imie);
                    cmd.Parameters.AddWithValue("nazwisko", prawnik.Nazwisko);
                    cmd.Parameters.AddWithValue("id", prawnik.IdPrawnika);
                    cmd.ExecuteNonQuery();
                    _dbConnection.Close();
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
