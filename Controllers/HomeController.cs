using Microsoft.AspNetCore.Mvc;
using Npgsql;
using BazyDanych1Projekt.Models;
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

            string query = "SELECT * FROM Prawnik"; // zapytanie SQL
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
                            Nazwisko = reader.GetString(2)
                        });
                    }
                }
                _dbConnection.Close();
            }

            return View(prawnicy); // przekazanie danych do widoku
        }

        public IActionResult Prawnicy()
        {
            var prawnicy = new List<Prawnik>();

            string query = "SELECT * FROM Prawnik"; // zapytanie SQL
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
                            Nazwisko = reader.GetString(2)
                        });
                    }
                }
                _dbConnection.Close();
            }

            return View(prawnicy);
        }

        public IActionResult Sprawy()
        {
            var sprawy = new List<Sprawa>();

            string query = "SELECT * FROM sprawa where data_zakonczenia is not null"; // zapytanie SQL
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
                            Opis = reader.GetString(1),
                            DataZakonczenia = reader.GetDateTime(2),
                            StopienWynagrodzenia = reader.GetInt32(3)
                        });
                    }
                }
                _dbConnection.Close();
            }
            string query2 = "SELECT * FROM sprawa where data_zakonczenia is null"; // zapytanie SQL
            using (var cmd = new NpgsqlCommand(query2, _dbConnection))
            {
                _dbConnection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        sprawy.Add(new Sprawa
                        {
                            IdSprawy = reader.GetInt32(0),
                            Opis = reader.GetString(1),
                            StopienWynagrodzenia = reader.GetInt32(3)
                        });
                    }
                }
                _dbConnection.Close();
            }


            return View(sprawy); // przekazanie danych do widoku
        }

        public IActionResult Sprawy_Prawnicy()
        {
            var sprawy_prawnicy = new List<SprawyPrawnicy>();

            string query = "SELECT * FROM prawnicy_sprawy where data_zakonczenia is not null"; // zapytanie SQL
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
                _dbConnection.Close();
            }
            string query2 = "SELECT * FROM prawnicy_sprawy where data_zakonczenia is null"; // zapytanie SQL
            using (var cmd = new NpgsqlCommand(query2, _dbConnection))
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
                            StopienWynagrodzenia = reader.GetInt32(6)
                        });
                    }
                }
                _dbConnection.Close();
            }
            return View(sprawy_prawnicy);
        }

        public IActionResult Prawnicy_Zarobki(int OrderBy = 0)
        {
            var prawnicy_zarobki = new List<PrawnicyZarobki>();

            if (OrderBy == 0) { 
                string query = "SELECT * FROM prawnicy_zarobki";
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
                    _dbConnection.Close();
                }
            }
            else
            {
                string query = "SELECT * FROM prawnicy_zarobki ORDER BY zarobki ASC";
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
                    _dbConnection.Close();
                }
            }

            return View(prawnicy_zarobki);
        }

        public IActionResult Liczba_Spraw_Prawnika()
        {
            var liczba_spraw_prawnika = new List<LiczbaSprawPrawnika>();
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
                _dbConnection.Close();
            }

            return View(liczba_spraw_prawnika);
        }

        public IActionResult Sprawy_W_Toku()
        {
            var sprawy_w_toku = new List<SprawyWToku>();
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
                _dbConnection.Close();
            }

            return View(sprawy_w_toku);
        }


    }
}
