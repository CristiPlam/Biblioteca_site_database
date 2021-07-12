using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Atestat2._0.Models.Modele
{
    // Tabelul din baza de date pentru genurile cărților. Un gen are un id (număr de identificare; generat automat de baza de date) și un nume pentru genul respectiv
    public class Gen
    {
        [Key] // cheia primară din tabel
        public int Gen_id { get; set; } // get și set sunt constructori de inițializare pentru fiecare variabilă din clasă

        [MinLength(3, ErrorMessage = "Numele genului nu poate să fie mai scurt de 3 caractere"), // validări pentru numele genului
        MaxLength(30, ErrorMessage = "Numele genului nu poate să aibă mai mult de 30 de caractere")]
        public string Nume { get; set; }

        //relatie many-to-many cu tabelul Carti
        public virtual ICollection<Carti> Carti { get; set; } //creez o colecție de tipul cărți. Relația e many-to-many pentru că o carte poate să aibă mai multe genuri
                                                              // iar un gen poate să aparțină de mai multe cărți
    }
}