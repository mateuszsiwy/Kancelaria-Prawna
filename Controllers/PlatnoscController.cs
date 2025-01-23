using Microsoft.AspNetCore.Mvc;
using Npgsql;
using BazyDanych1Projekt.Models;
using System.Collections.Generic;

namespace BazyDanych1Projekt.Controllers
{
    public class PlatnoscController : Controller
    {
        private readonly NpgsqlConnection _dbConnection;

        public PlatnoscController(NpgsqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IActionResult Index()
        {
            var platnosci = new List<Platnosc>();

            try
            {
                string query = "SELECT * FROM Platnosc";
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            platnosci.Add(new Platnosc
                            {
                                IdPlatnosci = reader.GetInt32(0),
                                IdFaktury = reader.GetInt32(1),
                                DataPlatnosci = reader.GetDateTime(2),
                                Kwota = reader.GetDecimal(3)
                            });
                        }
                    }
                }
            }
            catch (PostgresException ex)
            {
                Console.WriteLine(ex.Message);
                ViewData["ErrorMessage"] = "An error occurred while retrieving the records: " + ex.Message;
                return View("Error");
            }
            finally
            {
                if (_dbConnection.State == System.Data.ConnectionState.Open)
                {
                    _dbConnection.Close();
                }
            }

            return View(platnosci);
        }

        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Platnosc());
            else
            {
                Platnosc platnosc = null;
                try
                {
                    string query = "SELECT * FROM Platnosc WHERE id_platnosci = @id";
                    using (var cmd = new NpgsqlCommand(query, _dbConnection))
                    {
                        _dbConnection.Open();
                        cmd.Parameters.AddWithValue("id", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                platnosc = new Platnosc
                                {
                                    IdPlatnosci = reader.GetInt32(0),
                                    IdFaktury = reader.GetInt32(1),
                                    DataPlatnosci = reader.GetDateTime(2),
                                    Kwota = reader.GetDecimal(3)
                                };
                            }
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
                return View(platnosc);
            }
        }

        [HttpPost]
        public IActionResult AddOrEdit(Platnosc platnosc)
        {
            try
            {
                if (platnosc.IdPlatnosci == 0)
                {
                    string query = "INSERT INTO Platnosc(id_faktury, data_platnosci, kwota) VALUES(@id_faktury, @data_platnosci, @kwota)";
                    using (var cmd = new NpgsqlCommand(query, _dbConnection))
                    {
                        _dbConnection.Open();
                        cmd.Parameters.AddWithValue("id_faktury", platnosc.IdFaktury);
                        cmd.Parameters.AddWithValue("data_platnosci", platnosc.DataPlatnosci);
                        cmd.Parameters.AddWithValue("kwota", platnosc.Kwota);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string query = "UPDATE Platnosc SET id_faktury = @id_faktury, data_platnosci = @data_platnosci, kwota = @kwota WHERE id_platnosci = @id";
                    using (var cmd = new NpgsqlCommand(query, _dbConnection))
                    {
                        _dbConnection.Open();
                        cmd.Parameters.AddWithValue("id_faktury", platnosc.IdFaktury);
                        cmd.Parameters.AddWithValue("data_platnosci", platnosc.DataPlatnosci);
                        cmd.Parameters.AddWithValue("kwota", platnosc.Kwota);
                        cmd.Parameters.AddWithValue("id", platnosc.IdPlatnosci);
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
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            try
            {
                string query = "DELETE FROM Platnosc WHERE id_platnosci = @id";
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
            return RedirectToAction("Index");
        }
    }
}