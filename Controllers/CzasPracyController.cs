using Microsoft.AspNetCore.Mvc;
using Npgsql;
using BazyDanych1Projekt.Models;
using System.Collections.Generic;

namespace BazyDanych1Projekt.Controllers
{
    public class CzasPracyController : Controller
    {
        private readonly NpgsqlConnection _dbConnection;

        public CzasPracyController(NpgsqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IActionResult Index()
        {
            var czasPracyList = new List<CzasPracy>();

            try
            {
                string query = "SELECT * FROM CzasPracy";
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            czasPracyList.Add(new CzasPracy
                            {
                                IdCzasPracy = reader.GetInt32(0),
                                IdPrzypisane = reader.GetInt32(1),
                                Data = reader.GetDateTime(2),
                                LiczbaGodzin = reader.GetDecimal(3),
                                OpisCzynnosci = reader.GetString(4)
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

            return View(czasPracyList);
        }

        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new CzasPracy());
            else
            {
                try
                {
                    string query = "SELECT * FROM CzasPracy WHERE id_czasu = @id";
                    using (var cmd = new NpgsqlCommand(query, _dbConnection))
                    {
                        _dbConnection.Open();
                        cmd.Parameters.AddWithValue("id", id);
                        var reader = cmd.ExecuteReader();
                        CzasPracy czasPracy = new CzasPracy();
                        while (reader.Read())
                        {
                            czasPracy.IdCzasPracy = reader.GetInt32(0);
                            czasPracy.IdPrzypisane = reader.GetInt32(1);
                            czasPracy.Data = reader.GetDateTime(2);
                            czasPracy.LiczbaGodzin = reader.GetDecimal(3);
                            czasPracy.OpisCzynnosci = reader.GetString(4);
                        }
                        return View(czasPracy);
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
        }

        [HttpPost]
        public IActionResult AddOrEdit(CzasPracy czasPracy)
        {
            try
            {
                string query;
                if (czasPracy.IdCzasPracy == 0)
                {
                    query = "INSERT INTO CzasPracy (id_przypisane, data, liczba_godzin, opis_czynnosci) VALUES (@id_przypisane, @data, @liczba_godzin, @opis_czynnosci)";
                }
                else
                {
                    query = "UPDATE CzasPracy SET id_przypisane = @id_przypisane, data = @data, liczba_godzin = @liczba_godzin, opis_czynnosci = @opis_czynnosci WHERE id_czasu = @id_czasu";
                }

                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    cmd.Parameters.AddWithValue("id_przypisane", czasPracy.IdPrzypisane);
                    cmd.Parameters.AddWithValue("data", czasPracy.Data);
                    cmd.Parameters.AddWithValue("liczba_godzin", czasPracy.LiczbaGodzin);
                    cmd.Parameters.AddWithValue("opis_czynnosci", czasPracy.OpisCzynnosci);
                    if (czasPracy.IdCzasPracy != 0)
                    {
                        cmd.Parameters.AddWithValue("id_czasu", czasPracy.IdCzasPracy);
                    }
                    cmd.ExecuteNonQuery();
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
    }
}