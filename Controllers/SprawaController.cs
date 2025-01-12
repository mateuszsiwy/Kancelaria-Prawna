using BazyDanych1Projekt.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace BazyDanych1Projekt.Controllers
{
    public class SprawaController : Controller
    {
        private readonly NpgsqlConnection _dbConnection;

        public SprawaController(NpgsqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Delete(int id)
        {
            string query = "DELETE FROM sprawa WHERE id_sprawy = @id";
            using (var cmd = new NpgsqlCommand(query, _dbConnection))
            {
                _dbConnection.Open();

                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
                _dbConnection.Close();
            }
            return RedirectToAction("Sprawy", "Home");
        }


        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Sprawa { IdSprawy = id, DataZakonczenia = null });
            else
            {
                bool isFound = false;
                _dbConnection.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Sprawa WHERE id_sprawy = @id and data_zakonczenia is not null", _dbConnection);
                cmd.Parameters.AddWithValue("id", id);
                Sprawa sprawa = new Sprawa();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sprawa.IdSprawy = reader.GetInt32(0);
                        sprawa.Opis = reader.GetString(1);
                        sprawa.DataZakonczenia = reader.GetDateTime(2);
                        sprawa.StopienWynagrodzenia = reader.GetInt32(3);
                        isFound = true;
                    }
                }


                if (!isFound)
                {
                    cmd = new NpgsqlCommand("SELECT * FROM Sprawa WHERE id_sprawy = @id and data_zakonczenia is null", _dbConnection);
                    cmd.Parameters.AddWithValue("id", id);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        sprawa.IdSprawy = reader.GetInt32(0);
                        sprawa.Opis = reader.GetString(1);
                        sprawa.DataZakonczenia = null;
                        sprawa.StopienWynagrodzenia = reader.GetInt32(3);
                    }
                }

                _dbConnection.Close();
                return View(sprawa);
            }
        }

        [HttpPost]
        public IActionResult AddOrEdit(Sprawa sprawa)
        {
            if (sprawa.IdSprawy == 0)
            {
                string query = "INSERT INTO Sprawa(opis, data_zakonczenia, stopien_wynagrodzenia) Values(@opis, @data_zakonczenia, @stopien_wynagrodzenia)";
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    cmd.Parameters.AddWithValue("opis", sprawa.Opis);
                    if(sprawa.DataZakonczenia == null)
                    {
                        cmd.Parameters.AddWithValue("data_zakonczenia", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("data_zakonczenia", sprawa.DataZakonczenia);
                    }
                    cmd.Parameters.AddWithValue("stopien_wynagrodzenia", sprawa.StopienWynagrodzenia);
                    cmd.ExecuteNonQuery();
                    _dbConnection.Close();
                }
            }
            else
            {
                string query = "UPDATE Sprawa SET opis = @opis, data_zakonczenia = @data_zakonczenia, stopien_wynagrodzenia = @stopien_wynagrodzenia WHERE id_sprawy = @id";
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    cmd.Parameters.AddWithValue("opis", sprawa.Opis);
                    if (sprawa.DataZakonczenia == null)
                    {
                        cmd.Parameters.AddWithValue("data_zakonczenia", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("data_zakonczenia", sprawa.DataZakonczenia);
                    }
                    cmd.Parameters.AddWithValue("stopien_wynagrodzenia", sprawa.StopienWynagrodzenia);
                    cmd.Parameters.AddWithValue("id", sprawa.IdSprawy);
                    cmd.ExecuteNonQuery();
                    _dbConnection.Close();
                }
            }
            return RedirectToAction("Sprawy", "Home");
        }
    }
}
