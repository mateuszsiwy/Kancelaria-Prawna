﻿using Microsoft.AspNetCore.Mvc;
using Npgsql;
using BazyDanych1Projekt.Models;
using System.Collections.Generic;

namespace BazyDanych1Projekt.Controllers
{
    public class HomeController : Controller
    {
        private readonly NpgsqlConnection _dbConnection;

        public HomeController(NpgsqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IActionResult Index()
        {
            var prawnicy = new List<Prawnik>();

            try
            {
                string query = "SELECT * FROM Prawnik"; 
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            prawnicy.Add(new Prawnik
                            {
                                IdPrawnika = reader.GetInt32(0),
                                Imie = reader.GetString(1),
                                Nazwisko = reader.GetString(2),
                                Specjalizacja = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Stanowisko = reader.IsDBNull(4) ? null : reader.GetString(4),
                                StawkaGodzinowa = reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5),
                                DataZatrudnienia = reader.GetDateTime(6)
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

            return View(prawnicy); 
        }

        public IActionResult Prawnicy()
        {
            var prawnicy = new List<Prawnik>();

            try
            {
                string query = "SELECT * FROM Prawnik"; 
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            prawnicy.Add(new Prawnik
                            {
                                IdPrawnika = reader.GetInt32(0),
                                Imie = reader.GetString(1),
                                Nazwisko = reader.GetString(2),
                                Specjalizacja = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Stanowisko = reader.IsDBNull(4) ? null : reader.GetString(4),
                                StawkaGodzinowa = reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5),
                                DataZatrudnienia = reader.GetDateTime(6)
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

            return View(prawnicy);
        }

        public IActionResult Sprawy()
        {
            var sprawy = new List<Sprawa>();

            try
            {
                string query = "SELECT * FROM Sprawa"; 
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sprawy.Add(new Sprawa
                            {
                                IdSprawy = reader.GetInt32(0),
                                Opis = reader.IsDBNull(1) ? null : reader.GetString(1),
                                DataZakonczenia = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                                StopienWynagrodzenia = reader.GetInt32(3),
                                Tytul = reader.IsDBNull(4) ? null : reader.GetString(4),
                                DataRozpoczecia = reader.GetDateTime(5),
                                Status = reader.IsDBNull(6) ? null : reader.GetString(6),
                                Priorytet = reader.GetInt32(7),
                                Wynik = reader.IsDBNull(8) ? null : reader.GetString(8),
                            
                                IdKlienta = reader.GetInt32(9)
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

            return View(sprawy);
        }

        public IActionResult PrawnicySprawy()
        {
            var prawnicySprawyList = new List<PrawnicySprawy>();
        
            try
            {
                    string query = @"
                    SELECT 
                        id_przypisane, id_prawnika, imie, nazwisko, stanowisko,
                        id_sprawy, tytul, opis, data_rozpoczecia, data_zakonczenia, status, priorytet, wynik,
                        rola, data_przypisania
                    FROM PrawnicySprawyView";
        
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            prawnicySprawyList.Add(new PrawnicySprawy
                            {
                                IdPrzypisane = reader.GetInt32(0),
                                IdPrawnika = reader.GetInt32(1),
                                Imie = reader.GetString(2),
                                Nazwisko = reader.GetString(3),
                                Stanowisko = reader.GetString(4),
                                IdSprawy = reader.GetInt32(5),
                                Tytul = reader.GetString(6),
                                Opis = reader.GetString(7),
                                DataRozpoczecia = reader.GetDateTime(8),
                                DataZakonczenia = reader.IsDBNull(9) ? (DateTime?)null : reader.GetDateTime(9),
                                Status = reader.GetString(10),
                                Priorytet = reader.GetInt32(11),
                                Wynik = reader.IsDBNull(12) ? null : reader.GetString(12),
                                Rola = reader.GetString(13),
                                DataPrzypisania = reader.GetDateTime(14)
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
        
            return View(prawnicySprawyList);
        }

        public IActionResult Sprawy_Prawnicy()
        {
            var sprawy_prawnicy = new List<SprawyPrawnicy>();

            try
            {
                string query = "SELECT * FROM prawnicy_sprawy where data_zakonczenia is not null"; 
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sprawy_prawnicy.Add(new SprawyPrawnicy
                            {
                                IdPrawnika = reader.GetInt32(0),
                                Imie = reader.GetString(1),
                                Nazwisko = reader.GetString(2),
                                IdSprawy = reader.GetInt32(3),
                                Opis = reader.GetString(4),
                                DataZakonczenia = reader.GetDateTime(5),
                                StopienWynagrodzenia = reader.GetInt32(6)
                            });
                        }
                    }
                }

                string query2 = "SELECT * FROM prawnicy_sprawy where data_zakonczenia is null"; 
                using (var cmd = new NpgsqlCommand(query2, _dbConnection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sprawy_prawnicy.Add(new SprawyPrawnicy
                            {
                                IdPrawnika = reader.GetInt32(0),
                                Imie = reader.GetString(1),
                                Nazwisko = reader.GetString(2),
                                IdSprawy = reader.GetInt32(3),
                                Opis = reader.GetString(4),
                                StopienWynagrodzenia = reader.GetInt32(6)
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

            return View(sprawy_prawnicy);
        }

        public IActionResult Prawnicy_Zarobki(int OrderBy = 0)
        {
            var prawnicy_zarobki = new List<PrawnicyZarobki>();

            try
            {
                string query = OrderBy == 0 ? "SELECT * FROM prawnicy_zarobki" : "SELECT * FROM prawnicy_zarobki ORDER BY zarobki ASC";
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            prawnicy_zarobki.Add(new PrawnicyZarobki
                            {
                                Nazwisko = reader.GetString(0),
                                Zarobki = reader.GetDecimal(1)
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

            return View(prawnicy_zarobki);
        }

        public IActionResult Liczba_Spraw_Prawnika()
        {
            var liczba_spraw_prawnika = new List<LiczbaSprawPrawnika>();

            try
            {
                string query = "SELECT * FROM liczba_spraw_prawnika";
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            liczba_spraw_prawnika.Add(new LiczbaSprawPrawnika
                            {
                                Nazwisko = reader.GetString(0),
                                LiczbaSpraw = reader.GetInt32(1)
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

            return View(liczba_spraw_prawnika);
        }

        public IActionResult Sprawy_W_Toku()
        {
            var sprawy_w_toku = new List<SprawyWToku>();

            try
            {
                string query = "SELECT * FROM sprawy_w_toku";
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sprawy_w_toku.Add(new SprawyWToku
                            {
                                IdSprawy = reader.GetInt32(0),
                                Opis = reader.GetString(1),
                                NazwiskoPrawnika = reader.GetString(2)
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

            return View(sprawy_w_toku);
        }

        public IActionResult KlienciFakturyPlatnosci()
        {
            var klienciFakturyPlatnosci = new List<KlientFakturaPlatnosc>();

            try
            {
                string query = "SELECT * FROM KlienciFakturyPlatnosci";
                using (var cmd = new NpgsqlCommand(query, _dbConnection))
                {
                    _dbConnection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            klienciFakturyPlatnosci.Add(new KlientFakturaPlatnosc
                            {
                                IdKlienta = reader.GetInt32(0),
                                Imie = reader.GetString(1),
                                Nazwisko = reader.GetString(2),
                                IdFaktury = reader.GetInt32(3),
                                DataWystawienia = reader.GetDateTime(4),
                                KwotaFaktury = reader.GetDecimal(5),
                                Status = reader.GetString(6),
                                IdPlatnosci = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7),
                                DataPlatnosci = reader.IsDBNull(8) ? (DateTime?)null : reader.GetDateTime(8),
                                KwotaPlatnosci = reader.IsDBNull(9) ? (decimal?)null : reader.GetDecimal(9)
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

            return View(klienciFakturyPlatnosci);
        }

    }
}