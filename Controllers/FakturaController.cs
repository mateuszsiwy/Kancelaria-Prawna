using Microsoft.AspNetCore.Mvc;
using Npgsql;
using BazyDanych1Projekt.Models;
using System.Collections.Generic;

namespace BazyDanych1Projekt.Controllers
{
    public class FakturaController : Controller
    {
        private readonly NpgsqlConnection _dbConnection;

        public FakturaController(NpgsqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IActionResult Index()
        {
            var faktury = new List<Faktura>();

            try
            {
                string query = "SELECT * FROM Faktura";
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            faktury.Add(new Faktura
                            {
                                IdFaktury = reader.GetInt32(0),
                                IdKlienta = reader.GetInt32(1),
                                DataWystawienia = reader.GetDateTime(2),
                                Kwota = reader.GetDecimal(3),
                                Status = reader.GetString(4)
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

            return View(faktury);
        }

        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Faktura());
            else
            {
                Faktura faktura = null;
                try
                {
                    string query = "SELECT * FROM Faktura WHERE id_faktury = @id";
                    using (var cmd = new NpgsqlCommand(query, _dbConnection))
                    {
                        _dbConnection.Open();
                        cmd.Parameters.AddWithValue("id", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                faktura = new Faktura
                                {
                                    IdFaktury = reader.GetInt32(0),
                                    IdKlienta = reader.GetInt32(1),
                                    DataWystawienia = reader.GetDateTime(2),
                                    Kwota = reader.GetDecimal(3),
                                    Status = reader.GetString(4)
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
                return View(faktura);
            }
        }

        [HttpPost]
        public IActionResult AddOrEdit(Faktura faktura)
        {
            try
            {
                if (faktura.IdFaktury == 0)
                {
                    string query = "INSERT INTO Faktura(id_klienta, data_wystawienia, kwota, status) VALUES(@id_klienta, @data_wystawienia, @kwota, @status)";
                    using (var cmd = new NpgsqlCommand(query, _dbConnection))
                    {
                        _dbConnection.Open();
                        cmd.Parameters.AddWithValue("id_klienta", faktura.IdKlienta);
                        cmd.Parameters.AddWithValue("data_wystawienia", faktura.DataWystawienia);
                        cmd.Parameters.AddWithValue("kwota", faktura.Kwota);
                        cmd.Parameters.AddWithValue("status", faktura.Status);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string query = "UPDATE Faktura SET id_klienta = @id_klienta, data_wystawienia = @data_wystawienia, kwota = @kwota, status = @status WHERE id_faktury = @id";
                    using (var cmd = new NpgsqlCommand(query, _dbConnection))
                    {
                        _dbConnection.Open();
                        cmd.Parameters.AddWithValue("id_klienta", faktura.IdKlienta);
                        cmd.Parameters.AddWithValue("data_wystawienia", faktura.DataWystawienia);
                        cmd.Parameters.AddWithValue("kwota", faktura.Kwota);
                        cmd.Parameters.AddWithValue("status", faktura.Status);
                        cmd.Parameters.AddWithValue("id", faktura.IdFaktury);
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
                string query = "DELETE FROM Faktura WHERE id_faktury = @id";
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