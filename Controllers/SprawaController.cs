using Microsoft.AspNetCore.Mvc;
using Npgsql;
using BazyDanych1Projekt.Models;

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
            try
            {
                string query = "DELETE FROM sprawa WHERE id_sprawy = @id";
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (PostgresException ex)
            {
                Console.WriteLine(ex.Message);
                ViewData["ErrorMessage"] = "An error occurred while deleting the record: " + ex.Message;
                return View("Error");
            }
            finally
            {
                if (_dbConnection.State == System.Data.ConnectionState.Open)
                {
                    _dbConnection.Close();
                }
            }
            return RedirectToAction("Sprawy", "Home");
        }

        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            try
            {
                if (id == 0)
                    return View(new Sprawa { IdSprawy = id });
                else
                {
                    _dbConnection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM sprawa WHERE id_sprawy = @id", _dbConnection);
                    cmd.Parameters.AddWithValue("id", id);
                    var reader = cmd.ExecuteReader();
                    Sprawa sprawa = new Sprawa();
                    while (reader.Read())
                    {
                        sprawa.IdSprawy = reader.GetInt32(0);
                        sprawa.Opis = reader.GetString(1);
                        sprawa.DataZakonczenia = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2);
                        sprawa.StopienWynagrodzenia = reader.GetInt32(3);
                    }
                    return View(sprawa);
                }
            }
            catch (PostgresException ex)
            {
                Console.WriteLine(ex.Message);
                ViewData["ErrorMessage"] = "An error occurred while retrieving the record: " + ex.Message;
                return View("Error");
            }
            finally
            {
                if (_dbConnection.State == System.Data.ConnectionState.Open)
                {
                    _dbConnection.Close();
                }
            }
        }

        [HttpPost]
        public IActionResult AddOrEdit(Sprawa sprawa)
        {
            try
            {
                if (sprawa.IdSprawy == 0)
                {
                    string query = "INSERT INTO sprawa(opis, data_zakonczenia, stopien_wynagrodzenia) VALUES(@opis, @data_zakonczenia, @stopien_wynagrodzenia)";
                    using (var cmd = new NpgsqlCommand(query, _dbConnection))
                    {
                        _dbConnection.Open();
                        cmd.Parameters.AddWithValue("opis", sprawa.Opis);
                        cmd.Parameters.AddWithValue("data_zakonczenia", (object)sprawa.DataZakonczenia ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("stopien_wynagrodzenia", sprawa.StopienWynagrodzenia);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string query = "UPDATE sprawa SET opis = @opis, data_zakonczenia = @data_zakonczenia, stopien_wynagrodzenia = @stopien_wynagrodzenia WHERE id_sprawy = @id";
                    using (var cmd = new NpgsqlCommand(query, _dbConnection))
                    {
                        _dbConnection.Open();
                        cmd.Parameters.AddWithValue("opis", sprawa.Opis);
                        cmd.Parameters.AddWithValue("data_zakonczenia", (object)sprawa.DataZakonczenia ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("stopien_wynagrodzenia", sprawa.StopienWynagrodzenia);
                        cmd.Parameters.AddWithValue("id", sprawa.IdSprawy);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (PostgresException ex)
            {
                Console.WriteLine(ex.Message);
                ViewData["ErrorMessage"] = "An error occurred while saving the record: " + ex.Message;
                return View("Error");
            }
            finally
            {
                if (_dbConnection.State == System.Data.ConnectionState.Open)
                {
                    _dbConnection.Close();
                }
            }
            return RedirectToAction("Sprawy", "Home");
        }
    }
}