using Microsoft.AspNetCore.Mvc;
using Npgsql;
using BazyDanych1Projekt.Models;

namespace BazyDanych1Projekt.Controllers
{
    public class PrawnicySprawyController : Controller
    {
        private readonly NpgsqlConnection _dbConnection;

        public PrawnicySprawyController(NpgsqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Delete(int IdPrawnika, int IdSprawy)
        {
            try
            {
                string query = "DELETE FROM Przypisane WHERE id_sprawy = @id_sprawy AND id_prawnika = @id_prawnika";
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();

                    cmd.Parameters.AddWithValue("id_sprawy", IdSprawy);
                    cmd.Parameters.AddWithValue("id_prawnika", IdPrawnika);
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
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AddOrEdit(int IdPrawnika = 0, int IdSprawy = 0)
        {
            try
            {
                if (IdPrawnika == 0)
                {
                    return View(new Przypisane { IdPrzypisane = 0, IdPrawnika = IdPrawnika, IdSprawy = IdSprawy });
                }
                else
                {
                    string query = "SELECT * FROM przypisane WHERE id_sprawy = @id_sprawy AND id_prawnika = @id_prawnika";
                    using (var cmd = new NpgsqlCommand(query, _dbConnection))
                    {
                        _dbConnection.Open();
                        cmd.Parameters.AddWithValue("id_sprawy", IdSprawy);
                        cmd.Parameters.AddWithValue("id_prawnika", IdPrawnika);
                        var reader = cmd.ExecuteReader();
                        Przypisane przypisane = new Przypisane();
                        while (reader.Read())
                        {
                            przypisane.IdPrzypisane = reader.GetInt32(0);
                            przypisane.IdSprawy = reader.GetInt32(1);
                            przypisane.IdPrawnika = reader.GetInt32(2);
                            przypisane.Rola = reader.GetString(3);
                            przypisane.DataPrzypisania = reader.GetDateTime(4);
                        }
                        return View(przypisane);
                    }
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
        public IActionResult AddOrEdit(Przypisane przypisane)
        {
            try
            {
                if (przypisane.IdPrzypisane == 0)
                {
                    string query = "INSERT INTO przypisane(id_sprawy, id_prawnika) VALUES(@id_sprawy, @id_prawnika)";
                    using (var cmd = new NpgsqlCommand(query, _dbConnection))
                    {
                        _dbConnection.Open();
                        cmd.Parameters.AddWithValue("id_sprawy", przypisane.IdSprawy);
                        cmd.Parameters.AddWithValue("id_prawnika", przypisane.IdPrawnika);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string query = "UPDATE przypisane SET id_sprawy = @id_sprawy, id_prawnika = @id_prawnika, rola = @rola, data_przypisania = @data_przypisania WHERE id_przypisane = @id_przypisane";
                    using (var cmd = new NpgsqlCommand(query, _dbConnection))
                    {
                        _dbConnection.Open();
                        cmd.Parameters.AddWithValue("id_sprawy", przypisane.IdSprawy);
                        cmd.Parameters.AddWithValue("id_prawnika", przypisane.IdPrawnika);
                        cmd.Parameters.AddWithValue("id_przypisane", przypisane.IdPrzypisane);
                        cmd.Parameters.AddWithValue("rola", przypisane.Rola);
                        cmd.Parameters.AddWithValue("data_przypisania", przypisane.DataPrzypisania);
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
            return RedirectToAction("Index", "Home");
        }
    }
}