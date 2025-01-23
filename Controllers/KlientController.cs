using Microsoft.AspNetCore.Mvc;
using Npgsql;
using BazyDanych1Projekt.Models;
using System.Collections.Generic;

namespace BazyDanych1Projekt.Controllers
{
    public class KlientController : Controller
    {
        private readonly NpgsqlConnection _dbConnection;

        public KlientController(NpgsqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IActionResult Index()
        {
            var klienci = new List<Klient>();

            try
            {
                string query = "SELECT * FROM Klient";
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            klienci.Add(new Klient
                            {
                                IdKlienta = reader.GetInt32(0),
                                Imie = reader.GetString(1),
                                Nazwisko = reader.GetString(2),
                                Email = reader.GetString(3),
                                Telefon = reader.GetString(4)
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

            return View(klienci);
        }

        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            try
            {
                if (id == 0)
                    return View(new Klient { IdKlienta = id });
                else
                {
                    _dbConnection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM klient WHERE id_klienta = @id", _dbConnection);
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters["id"].NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer;

                    var reader = cmd.ExecuteReader();
                    Klient klient = new Klient();
                    while (reader.Read())
                    {
                        klient.IdKlienta = reader.GetInt32(0);
                        klient.Imie = reader.GetString(1);
                        klient.Nazwisko = reader.GetString(2);
                        klient.Email = reader.GetString(3);
                        klient.Telefon = reader.GetString(4);
                    }
                    return View(klient);
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
        public IActionResult AddOrEdit(Klient klient)
        {
            try
            {
                if (klient.IdKlienta == 0)
                {
                    string query = "INSERT INTO Klient(imie, nazwisko, email, telefon) VALUES(@imie, @nazwisko, @email, @telefon)";
                    using (var cmd = new NpgsqlCommand(query, _dbConnection))
                    {
                        _dbConnection.Open();
                        cmd.Parameters.AddWithValue("imie", klient.Imie);
                        cmd.Parameters.AddWithValue("nazwisko", klient.Nazwisko);
                        cmd.Parameters.AddWithValue("email", klient.Email);
                        cmd.Parameters.AddWithValue("telefon", klient.Telefon);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string query = "UPDATE Klient SET imie = @imie, nazwisko = @nazwisko, email = @email, telefon = @telefon WHERE id_klienta = @id";
                    using (var cmd = new NpgsqlCommand(query, _dbConnection))
                    {
                        _dbConnection.Open();
                        cmd.Parameters.AddWithValue("imie", klient.Imie);
                        cmd.Parameters.AddWithValue("nazwisko", klient.Nazwisko);
                        cmd.Parameters.AddWithValue("email", klient.Email);
                        cmd.Parameters.AddWithValue("telefon", klient.Telefon);
                        cmd.Parameters.AddWithValue("id", klient.IdKlienta);
                        cmd.Parameters["id"].NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer;

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
                string query = "DELETE FROM Klient WHERE id_klienta = @id";
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