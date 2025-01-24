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
                        sprawa.Opis = reader.IsDBNull(1) ? null : reader.GetString(1);
                        sprawa.DataZakonczenia = reader.IsDBNull(2) ? null : reader.GetDateTime(2);
                        sprawa.StopienWynagrodzenia = reader.GetInt32(3);
                        sprawa.Tytul = reader.IsDBNull(4) ? null : reader.GetString(4);
                        sprawa.DataRozpoczecia = reader.GetDateTime(5);
                        sprawa.Status = reader.IsDBNull(6) ? null : reader.GetString(6);
                        sprawa. Priorytet = reader.GetInt32(7);
                        sprawa.Wynik = reader.IsDBNull(8) ? null : reader.GetString(8);
                            
                        sprawa.IdKlienta = reader.GetInt32(9);
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
                _dbConnection.Open();
                if (sprawa.IdSprawy == 0)
                {
                    string query = @"
                        INSERT INTO sprawa (tytul, opis, data_rozpoczecia, data_zakonczenia, status, priorytet, wynik, stopien_wynagrodzenia, id_klienta) 
                        VALUES (@tytul, @opis, @data_rozpoczecia, @data_zakonczenia, @status, @priorytet, @wynik, @stopien_wynagrodzenia, @id_klienta)";
                    using (var cmd = new NpgsqlCommand(query, _dbConnection))
                    {
                        cmd.Parameters.AddWithValue("tytul", sprawa.Tytul);
                        cmd.Parameters.AddWithValue("opis", sprawa.Opis ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("data_rozpoczecia", sprawa.DataRozpoczecia);
                        cmd.Parameters.AddWithValue("data_zakonczenia", (object)sprawa.DataZakonczenia ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("status", sprawa.Status ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("priorytet", sprawa.Priorytet);
                        cmd.Parameters.AddWithValue("wynik", sprawa.Wynik ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("stopien_wynagrodzenia", sprawa.StopienWynagrodzenia);
                        cmd.Parameters.AddWithValue("id_klienta", sprawa.IdKlienta);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string query = @"
                        UPDATE sprawa 
                        SET tytul = @tytul, opis = @opis, data_rozpoczecia = @data_rozpoczecia, data_zakonczenia = @data_zakonczenia, status = @status, priorytet = @priorytet, wynik = @wynik, stopien_wynagrodzenia = @stopien_wynagrodzenia, id_klienta = @id_klienta 
                        WHERE id_sprawy = @id";
                    using (var cmd = new NpgsqlCommand(query, _dbConnection))
                    {
                        cmd.Parameters.AddWithValue("id", sprawa.IdSprawy);
                        cmd.Parameters.AddWithValue("tytul", sprawa.Tytul);
                        cmd.Parameters.AddWithValue("opis", sprawa.Opis ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("data_rozpoczecia", sprawa.DataRozpoczecia);
                        cmd.Parameters.AddWithValue("data_zakonczenia", (object)sprawa.DataZakonczenia ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("status", sprawa.Status ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("priorytet", sprawa.Priorytet);
                        cmd.Parameters.AddWithValue("wynik", sprawa.Wynik ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("stopien_wynagrodzenia", sprawa.StopienWynagrodzenia);
                        cmd.Parameters.AddWithValue("id_klienta", sprawa.IdKlienta);
                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Sprawy", "Home");
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
        }
        [HttpGet]
        public IActionResult SprawaKoszt(int IdSprawy)
        {
            SprawaKoszt sprawaKoszt = new SprawaKoszt { IdSprawy = IdSprawy };
            try
            {
                _dbConnection.Open();
                string query = "SELECT oblicz_koszt_sprawy(@id_sprawy)";
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    cmd.Parameters.AddWithValue("id_sprawy", IdSprawy);
                    sprawaKoszt.TotalCost = (decimal)cmd.ExecuteScalar();
                }
            }
            catch (PostgresException ex)
            {
                Console.WriteLine(ex.Message);
                ViewData["ErrorMessage"] = "An error occurred while calculating the cost: " + ex.Message;
                return View("Error");
            }
            finally
            {
                if (_dbConnection.State == System.Data.ConnectionState.Open)
                {
                    _dbConnection.Close();
                }
            }

            return View(sprawaKoszt);
        }

        [HttpPost]
        public IActionResult CalculateCost(int IdSprawy)
        {
            SprawaKoszt sprawaKoszt = new SprawaKoszt { IdSprawy = IdSprawy };
            try
            {
                _dbConnection.Open();
                string query = "SELECT oblicz_koszt_sprawy(@id_sprawy)";
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    cmd.Parameters.AddWithValue("id_sprawy", IdSprawy);
                    sprawaKoszt.TotalCost = (decimal)cmd.ExecuteScalar();
                }
            }
            catch (PostgresException ex)
            {
                Console.WriteLine(ex.Message);
                ViewData["ErrorMessage"] = "An error occurred while calculating the cost: " + ex.Message;
                return View("Error");
            }
            finally
            {
                if (_dbConnection.State == System.Data.ConnectionState.Open)
                {
                    _dbConnection.Close();
                }
            }

            return View("SprawaKoszt", sprawaKoszt);
        }

    }
}